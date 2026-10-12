using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using OpenHardwareMonitor.Collections;

namespace OpenHardwareMonitor.Hardware;

internal static class PInvokeDelegateFactory
{
	private static readonly ModuleBuilder moduleBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("PInvokeDelegateFactoryInternalAssembly"), AssemblyBuilderAccess.Run).DefineDynamicModule("PInvokeDelegateFactoryInternalModule");

	private static readonly IDictionary<Pair<DllImportAttribute, Type>, Type> wrapperTypes = new Dictionary<Pair<DllImportAttribute, Type>, Type>();

	public static void CreateDelegate<T>(DllImportAttribute dllImportAttribute, out T newDelegate, DllImportSearchPath dllImportSearchPath = DllImportSearchPath.System32) where T : class
	{
		Pair<DllImportAttribute, Type> key = new Pair<DllImportAttribute, Type>(dllImportAttribute, typeof(T));
		wrapperTypes.TryGetValue(key, out var value);
		if (value == null)
		{
			value = CreateWrapperType(typeof(T), dllImportAttribute, dllImportSearchPath);
			wrapperTypes.Add(key, value);
		}
		newDelegate = Delegate.CreateDelegate(typeof(T), value, dllImportAttribute.EntryPoint) as T;
	}

	private static Type CreateWrapperType(Type delegateType, DllImportAttribute dllImportAttribute, DllImportSearchPath dllImportSearchPath)
	{
		TypeBuilder typeBuilder = moduleBuilder.DefineType("PInvokeDelegateFactoryInternalWrapperType" + wrapperTypes.Count);
		MethodInfo method = delegateType.GetMethod("Invoke");
		ParameterInfo[] parameters = method.GetParameters();
		int length = parameters.GetLength(0);
		Type[] array = new Type[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = parameters[i].ParameterType;
		}
		MethodBuilder methodBuilder = typeBuilder.DefinePInvokeMethod(dllImportAttribute.EntryPoint, dllImportAttribute.Value, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl, CallingConventions.Standard, method.ReturnType, array, dllImportAttribute.CallingConvention, dllImportAttribute.CharSet);
		methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(DefaultDllImportSearchPathsAttribute).GetConstructor(new Type[1] { typeof(DllImportSearchPath) }), new object[1] { dllImportSearchPath }));
		ParameterInfo[] array2 = parameters;
		foreach (ParameterInfo parameterInfo in array2)
		{
			methodBuilder.DefineParameter(parameterInfo.Position + 1, parameterInfo.Attributes, parameterInfo.Name);
		}
		if (dllImportAttribute.PreserveSig)
		{
			methodBuilder.SetImplementationFlags(MethodImplAttributes.PreserveSig);
		}
		return typeBuilder.CreateType();
	}
}
