using System;
using Prism.Mvvm;

namespace SystemMonitorMgr.Models;

public class OrderedKeyValuePair : BindableBase, IComparable
{
	private int _order;

	private string _key;

	private string _value;

	public int Order
	{
		get
		{
			return _order;
		}
		set
		{
			SetProperty(ref _order, value, "Order");
		}
	}

	public string Key
	{
		get
		{
			return _key;
		}
		set
		{
			SetProperty(ref _key, value, "Key");
		}
	}

	public string Value
	{
		get
		{
			return _value;
		}
		set
		{
			SetProperty(ref _value, value, "Value");
		}
	}

	public OrderedKeyValuePair(int order, string key, string value)
	{
		_order = order;
		_key = key;
		_value = value;
	}

	public OrderedKeyValuePair(string key, string value)
	{
		_order = 0;
		_key = key;
		_value = value;
	}

	public int CompareTo(object obj)
	{
		if (obj.GetType() == typeof(OrderedKeyValuePair))
		{
			if (!(obj is OrderedKeyValuePair orderedKeyValuePair))
			{
				return -1;
			}
			if (_order < orderedKeyValuePair.Order)
			{
				return -1;
			}
			if (_order == orderedKeyValuePair.Order)
			{
				return 0;
			}
			if (_order > orderedKeyValuePair.Order)
			{
				return 1;
			}
		}
		return -1;
	}
}
