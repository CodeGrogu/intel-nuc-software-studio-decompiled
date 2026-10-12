using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MobileKeyboardLedMgr.Events;
using MobileKeyboardLedMgr.Interfaces;
using MobileKeyboardLedMgr.Models;
using Prism.Events;

namespace MobileKeyboardLedMgr.Views;

public partial class KeyboardView : UserControl, IComponentConnector
{
	private const double DRAG_TOLERANCE = 5.0;

	private readonly IEventAggregator _events;

	private List<ToggleButton> mainKeyToggleButtons;

	private bool _isMouseBeingTracked;

	private Point mouseDownPos;

	public KeyboardView(IKeyboardProfileManager keyboardProfileMgr, IEventAggregator events)
	{
		InitializeComponent();
		DataContext = keyboardProfileMgr;
		_events = events;
		AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(KeyboardViewGrid_MouseDown), handledEventsToo: true);
	}

	private void KeyboardViewGrid_Loaded(object sender, RoutedEventArgs e)
	{
		mainKeyToggleButtons = FindKeyboardToggleButtonsAndRectsOnUi(KeyboardControlUi.KeyboardItemControl);
	}

	private void KeyboardViewGrid_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if ((IsMainKeyboardEditable() || IsKeyGroupKeyboardEditable()) && IsOnAndNotDefaultProfile())
		{
			if (!e.Handled)
			{
				BeginCapturingMouseDragEvent(e);
			}
			else if (e.Handled && e.OriginalSource is FrameworkElement child && IsAncestorAKeyToggleButton(child))
			{
				BeginCapturingMouseDragEvent(e);
			}
		}
	}

	private void KeyboardViewGrid_MouseMove(object sender, MouseEventArgs e)
	{
		if ((IsMainKeyboardEditable() || IsKeyGroupKeyboardEditable()) && IsOnAndNotDefaultProfile() && _isMouseBeingTracked)
		{
			Point position = e.GetPosition(KeyboardViewGrid);
			double num = Math.Abs(mouseDownPos.X - position.X);
			double num2 = Math.Abs(mouseDownPos.Y - position.Y);
			if (num < 5.0 && num2 < 5.0)
			{
				DragSelectionBox.Visibility = Visibility.Collapsed;
			}
			else
			{
				DragSelectionBox.Visibility = Visibility.Visible;
			}
			if (mouseDownPos.X < position.X)
			{
				Canvas.SetLeft(DragSelectionBox, mouseDownPos.X);
				DragSelectionBox.Width = position.X - mouseDownPos.X;
			}
			else
			{
				Canvas.SetLeft(DragSelectionBox, position.X);
				DragSelectionBox.Width = mouseDownPos.X - position.X;
			}
			if (mouseDownPos.Y < position.Y)
			{
				Canvas.SetTop(DragSelectionBox, mouseDownPos.Y);
				DragSelectionBox.Height = position.Y - mouseDownPos.Y;
			}
			else
			{
				Canvas.SetTop(DragSelectionBox, position.Y);
				DragSelectionBox.Height = mouseDownPos.Y - position.Y;
			}
		}
	}

	private void KeyboardViewGrid_MouseUp(object sender, MouseButtonEventArgs e)
	{
		if ((IsMainKeyboardEditable() || IsKeyGroupKeyboardEditable()) && IsOnAndNotDefaultProfile())
		{
			bool isMouseBeingTracked = _isMouseBeingTracked;
			_isMouseBeingTracked = false;
			KeyboardViewGrid.ReleaseMouseCapture();
			DragSelectionBox.Visibility = Visibility.Collapsed;
			if (isMouseBeingTracked)
			{
				ApplyDragSelection();
			}
		}
	}

	private void BeginCapturingMouseDragEvent(MouseButtonEventArgs e)
	{
		_isMouseBeingTracked = true;
		mouseDownPos = e.GetPosition(KeyboardViewGrid);
		KeyboardViewGrid.CaptureMouse();
		Canvas.SetLeft(DragSelectionBox, mouseDownPos.X);
		Canvas.SetTop(DragSelectionBox, mouseDownPos.Y);
		DragSelectionBox.Width = 0.0;
		DragSelectionBox.Height = 0.0;
	}

	private bool IsAncestorAKeyToggleButton(DependencyObject child)
	{
		DependencyObject parent = VisualTreeHelper.GetParent(child);
		if (parent == null)
		{
			return false;
		}
		if (parent is ContentControl contentControl && contentControl.DataContext is KeyUiData)
		{
			return true;
		}
		return IsAncestorAKeyToggleButton(parent);
	}

	private void ApplyDragSelection()
	{
		List<KeyUiData> list = new List<KeyUiData>();
		double left = Canvas.GetLeft(DragSelectionBox);
		double top = Canvas.GetTop(DragSelectionBox);
		double width = DragSelectionBox.Width;
		double height = DragSelectionBox.Height;
		Rect rect = new Rect(left, top, width, height);
		Rect rect2;
		List<ToggleButton> list2;
		if (IsMainKeyboardEditable())
		{
			rect2 = CalculateRectRelativeToParent(KeyboardControlViewBox, KeyboardViewGrid);
			list2 = mainKeyToggleButtons;
		}
		else
		{
			if (!IsKeyGroupKeyboardEditable())
			{
				return;
			}
			rect2 = CalculateRectRelativeToParent(KeyGroupEditor.KeyGroupKeyboardControlViewBox, KeyboardViewGrid);
			list2 = FindKeyboardToggleButtonsAndRectsOnUi(KeyGroupEditor.KeyGroupKeyboardControlUi.KeyboardItemControl);
		}
		if (rect.IntersectsWith(rect2))
		{
			foreach (ToggleButton item in list2)
			{
				Rect rect3 = CalculateRectRelativeToParent(item, KeyboardViewGrid);
				if (rect.IntersectsWith(rect3))
				{
					list.Add((KeyUiData)item.DataContext);
				}
			}
		}
		if (list.Count > 0)
		{
			if (IsMainKeyboardEditable())
			{
				_events.GetEvent<UpdateMultipleKeys>().Publish(list);
			}
			else if (IsKeyGroupKeyboardEditable())
			{
				((KeyboardProfileManager)DataContext).KeyGroupToEdit.KeyboardUiData.UpdateMultipleKeys(list);
			}
		}
	}

	private List<ToggleButton> FindKeyboardToggleButtonsAndRectsOnUi(ItemsControl targetKeyboardItemControl)
	{
		List<ToggleButton> list = new List<ToggleButton>();
		if (targetKeyboardItemControl != null)
		{
			for (int i = 0; i < targetKeyboardItemControl.Items.Count; i++)
			{
				DependencyObject parent = targetKeyboardItemControl.ItemContainerGenerator.ContainerFromIndex(i);
				ItemsControl itemsControl = FindChildInVisualTree<ItemsControl>(parent);
				if (itemsControl == null)
				{
					continue;
				}
				for (int j = 0; j < itemsControl.Items.Count; j++)
				{
					DependencyObject parent2 = itemsControl.ItemContainerGenerator.ContainerFromIndex(j);
					ToggleButton toggleButton = FindChildInVisualTree<ToggleButton>(parent2);
					if (toggleButton != null)
					{
						list.Add(toggleButton);
					}
				}
			}
		}
		return list;
	}

	private T FindChildInVisualTree<T>(DependencyObject parent) where T : DependencyObject
	{
		if (parent == null)
		{
			return null;
		}
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(parent, i);
			T val = (child as T) ?? FindChildInVisualTree<T>(child);
			if (val != null)
			{
				return val;
			}
		}
		return null;
	}

	private Rect CalculateRectRelativeToParent(FrameworkElement child, Visual parent)
	{
		return child.TransformToAncestor(parent).TransformBounds(new Rect(0.0, 0.0, child.ActualWidth, child.ActualHeight));
	}

	private bool IsMainKeyboardEditable()
	{
		return ((KeyboardProfileManager)DataContext).KeyboardUiData.Editable;
	}

	private bool IsKeyGroupKeyboardEditable()
	{
		return ((KeyboardProfileManager)DataContext).KeyGroupToEdit?.KeyboardUiData.Editable ?? false;
	}

	private bool IsOnAndNotDefaultProfile()
	{
		return ((KeyboardProfileManager)DataContext).OnAndNotDefault;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
