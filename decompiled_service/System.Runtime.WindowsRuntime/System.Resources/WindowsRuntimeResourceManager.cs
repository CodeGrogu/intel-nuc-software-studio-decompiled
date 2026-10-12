using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using Microsoft.Win32;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace System.Resources;

[FriendAccessAllowed]
[SecurityCritical]
internal sealed class WindowsRuntimeResourceManager : WindowsRuntimeResourceManagerBase
{
	private struct PackageInfo
	{
		public string Path;

		public string Name;
	}

	private ResourceMap m_resourceMap;

	private ResourceContext m_clonedResourceContext;

	private string m_clonedResourceContextFallBackList;

	private static char[] s_charCultureSeparator;

	private static PackageInfo s_currentPackageInfo;

	private static List<PackageInfo> s_dependentPackageInfoList;

	private static ResourceContext s_globalResourceContext;

	private static volatile string s_globalResourceContextFallBackList;

	private static volatile CultureInfo s_globalResourceContextBestFitCultureInfo;

	private static volatile ResourceManager s_globalResourceManager;

	private static object s_objectForLock = new object();

	public override CultureInfo GlobalResourceContextBestFitCultureInfo
	{
		get
		{
			InitializeStaticGlobalResourceContext(null);
			return s_globalResourceContextBestFitCultureInfo;
		}
	}

	private static bool InitializeStatics()
	{
		ResourceManager val = null;
		if (s_globalResourceManager == null)
		{
			lock (s_objectForLock)
			{
				if (s_globalResourceManager == null)
				{
					val = ResourceManager.Current;
					InitializeStaticGlobalResourceContext(val);
					s_charCultureSeparator = new char[1] { ';' };
					Package current = Package.Current;
					if (current != null)
					{
						StorageFolder installedLocation = current.InstalledLocation;
						s_currentPackageInfo.Path = null;
						s_currentPackageInfo.Name = null;
						if (installedLocation != null)
						{
							s_currentPackageInfo.Path = installedLocation.Path;
						}
						PackageId id = current.Id;
						if (id != null)
						{
							s_currentPackageInfo.Name = id.Name;
						}
					}
					if (val != null && s_globalResourceContext != null && s_globalResourceContextFallBackList != null && s_globalResourceContextFallBackList.Length > 0 && s_charCultureSeparator != null && s_currentPackageInfo.Path != null)
					{
						s_globalResourceManager = val;
					}
				}
			}
		}
		return s_globalResourceManager != null;
	}

	private static void InitializeStaticGlobalResourceContext(ResourceManager resourceManager)
	{
		// ILSpy could not decompile this. Please report the exception below,
		// along with the assembly it came from, at https://github.com/icsharpcode/ILSpy/issues/new
		// System.ArgumentNullException: Value cannot be null. (Parameter 'source')
		//    at System.Linq.ThrowHelper.ThrowArgumentNullException(ExceptionArgument argument)
		//    at System.Linq.Enumerable.Skip[TSource](IEnumerable`1 source, Int32 count)
		//    at ICSharpCode.Decompiler.CSharp.ArgumentList.GetArgumentResolveResultsDirect(Int32 skipCount) in /_/ICSharpCode.Decompiler/CSharp/CallBuilder.cs:line 124
		//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Probe() in /_/ICSharpCode.Decompiler/CSharp/Disambiguator.cs:line 493
		//    at ICSharpCode.Decompiler.CSharp.Disambiguator.Run() in /_/ICSharpCode.Decompiler/CSharp/Disambiguator.cs:line 438
		//    at ICSharpCode.Decompiler.CSharp.Disambiguator.ForMethodReference(ExpressionBuilder expressionBuilder, IMethod method, IType targetType, TranslatedExpression target, Boolean requireTarget, ExpectedTargetDetails expectedTargetDetails, Boolean isExtensionMethodReference, Boolean castTargetUpFront) in /_/ICSharpCode.Decompiler/CSharp/Disambiguator.cs:line 131
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.DisambiguateDelegateReference(IMethod method, IMethod invokeMethod, ExpectedTargetDetails expectedTargetDetails, ILInstruction thisArg) in /_/ICSharpCode.Decompiler/CSharp/CallBuilder.cs:line 1718
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildDelegateReference(IMethod method, IMethod invokeMethod, ExpectedTargetDetails expectedTargetDetails, ILInstruction thisArg) in /_/ICSharpCode.Decompiler/CSharp/CallBuilder.cs:line 1648
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.HandleDelegateConstruction(IType delegateType, IMethod method, ExpectedTargetDetails expectedTargetDetails, ILInstruction thisArg, ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/CallBuilder.cs:line 1735
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(LdVirtDelegate inst) in /_/ICSharpCode.Decompiler/CSharp/CallBuilder.cs:line 1634
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitLdVirtDelegate(LdVirtDelegate inst, TranslationContext context) in /_/ICSharpCode.Decompiler/CSharp/ExpressionBuilder.cs:line 543
		//    at ICSharpCode.Decompiler.IL.LdVirtDelegate.AcceptVisitor[C,T](ILVisitor`2 visitor, C context) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 3294
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction inst, IType typeHint) in /_/ICSharpCode.Decompiler/CSharp/ExpressionBuilder.cs:line 173
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.BuildArgumentList(ExpectedTargetDetails expectedTargetDetails, ResolveResult target, IMethod method, Int32 firstParamIndex, IReadOnlyList`1 callArguments, IReadOnlyList`1 argumentToParameterMap, Boolean writtenAsAssignment) in /_/ICSharpCode.Decompiler/CSharp/CallBuilder.cs:line 1070
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(OpCode callOpCode, IMethod method, IReadOnlyList`1 callArguments, IReadOnlyList`1 argumentToParameterMap, IType constrainedTo) in /_/ICSharpCode.Decompiler/CSharp/CallBuilder.cs:line 461
		//    at ICSharpCode.Decompiler.CSharp.CallBuilder.Build(CallInstruction inst, IType typeHint) in /_/ICSharpCode.Decompiler/CSharp/CallBuilder.cs:line 255
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.VisitCall(Call inst, TranslationContext context) in /_/ICSharpCode.Decompiler/CSharp/ExpressionBuilder.cs:line 2530
		//    at ICSharpCode.Decompiler.IL.Call.AcceptVisitor[C,T](ILVisitor`2 visitor, C context) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 2192
		//    at ICSharpCode.Decompiler.CSharp.ExpressionBuilder.Translate(ILInstruction inst, IType typeHint) in /_/ICSharpCode.Decompiler/CSharp/ExpressionBuilder.cs:line 173
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Default(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 93
		//    at ICSharpCode.Decompiler.IL.ILVisitor`1.VisitCall(Call inst) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 7663
		//    at ICSharpCode.Decompiler.IL.Call.AcceptVisitor[T](ILVisitor`1 visitor) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 2188
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 82
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlock(Block block) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 1311
		//    at ICSharpCode.Decompiler.IL.Block.AcceptVisitor[T](ILVisitor`1 visitor) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 882
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 82
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitIfInstruction(IfInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 162
		//    at ICSharpCode.Decompiler.IL.IfInstruction.AcceptVisitor[T](ILVisitor`1 visitor) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 1440
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 82
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlock(Block block) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 1311
		//    at ICSharpCode.Decompiler.IL.Block.AcceptVisitor[T](ILVisitor`1 visitor) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 882
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 82
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitIfInstruction(IfInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 162
		//    at ICSharpCode.Decompiler.IL.IfInstruction.AcceptVisitor[T](ILVisitor`1 visitor) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 1440
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 82
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement blockStatement, BlockContainer container, IEnumerable`1 blocks, Boolean isLoop) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 1565
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer container, Boolean isLoop) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 1452
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer container) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 1338
		//    at ICSharpCode.Decompiler.IL.BlockContainer.AcceptVisitor[T](ILVisitor`1 visitor) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 857
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 82
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 87
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitLockInstruction(LockInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 520
		//    at ICSharpCode.Decompiler.IL.LockInstruction.AcceptVisitor[T](ILVisitor`1 visitor) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 1945
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 82
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockStatement blockStatement, BlockContainer container, IEnumerable`1 blocks, Boolean isLoop) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 1565
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertBlockContainer(BlockContainer container, Boolean isLoop) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 1452
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.VisitBlockContainer(BlockContainer container) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 1338
		//    at ICSharpCode.Decompiler.IL.BlockContainer.AcceptVisitor[T](ILVisitor`1 visitor) in /_/ICSharpCode.Decompiler/IL/Instructions.cs:line 857
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.Convert(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 82
		//    at ICSharpCode.Decompiler.CSharp.StatementBuilder.ConvertAsBlock(ILInstruction inst) in /_/ICSharpCode.Decompiler/CSharp/StatementBuilder.cs:line 87
		//    at ICSharpCode.Decompiler.CSharp.CSharpDecompiler.DecompileBody(IMethod method, EntityDeclaration entityDecl, DecompileRun decompileRun, ITypeResolveContext decompilationContext, ExtensionInfo extensionInfo) in /_/ICSharpCode.Decompiler/CSharp/CSharpDecompiler.cs:line 2435
	}

	private static CultureInfo GetBestFitCultureFromLanguageList(List<string> languages)
	{
		StringBuilder stringBuilder = new StringBuilder(85);
		for (int i = 0; i < languages.Count; i++)
		{
			if (CultureData.GetCultureData(languages[i], useUserOverride: true) != null)
			{
				return new CultureInfo(languages[i]);
			}
			if (SafeNativeMethods.ResolveLocaleName(languages[i], stringBuilder, stringBuilder.MaxCapacity) != 0)
			{
				string text = stringBuilder.ToString();
				if (CultureData.GetCultureData(text, useUserOverride: true) != null)
				{
					return new CultureInfo(text);
				}
			}
		}
		return null;
	}

	private static void InitializeStaticsForDependentPackages()
	{
		if (s_dependentPackageInfoList != null)
		{
			return;
		}
		List<PackageInfo> list = new List<PackageInfo>();
		Package current = Package.Current;
		if (current != null)
		{
			IReadOnlyList<Package> dependencies = current.Dependencies;
			if (dependencies != null)
			{
				int count = dependencies.Count;
				if (count > 0)
				{
					list = new List<PackageInfo>(count);
					PackageInfo item = default;
					foreach (Package item2 in dependencies)
					{
						if (item2 != null)
						{
							StorageFolder installedLocation = item2.InstalledLocation;
							item.Path = null;
							item.Name = null;
							if (installedLocation != null)
							{
								item.Path = installedLocation.Path;
							}
							PackageId id = item2.Id;
							if (id != null)
							{
								item.Name = id.Name;
							}
							list.Add(item);
						}
					}
				}
			}
		}
		s_dependentPackageInfoList = list;
	}

	private static void GlobalResourceContextChanged(object sender, IMapChangedEventArgs<string> e)
	{
		List<string> list = new List<string>(s_globalResourceContext.Languages);
		s_globalResourceContextBestFitCultureInfo = GetBestFitCultureFromLanguageList(list);
		s_globalResourceContextFallBackList = ReadOnlyListToString(list);
	}

	private static bool LibpathMatchesPackagepath(string libpath, string packagepath)
	{
		if (packagepath.Length < libpath.Length && string.Compare(packagepath, 0, libpath, 0, packagepath.Length, StringComparison.OrdinalIgnoreCase) == 0)
		{
			if (libpath[packagepath.Length] != '\\')
			{
				return packagepath.EndsWith("\\");
			}
			return true;
		}
		return false;
	}

	private static string FindPackageSimpleNameForFilename(string libpath)
	{
		if (LibpathMatchesPackagepath(libpath, s_currentPackageInfo.Path))
		{
			return s_currentPackageInfo.Name;
		}
		InitializeStaticsForDependentPackages();
		foreach (PackageInfo s_dependentPackageInfo in s_dependentPackageInfoList)
		{
			if (LibpathMatchesPackagepath(libpath, s_dependentPackageInfo.Path))
			{
				return s_dependentPackageInfo.Name;
			}
		}
		return null;
	}

	[SecurityCritical]
	public override bool Initialize(string libpath, string reswFilename, out PRIExceptionInfo exceptionInfo)
	{
		exceptionInfo = null;
		if (InitializeStatics())
		{
			IReadOnlyDictionary<string, ResourceMap> allResourceMaps = s_globalResourceManager.AllResourceMaps;
			if (allResourceMaps != null)
			{
				string text = FindPackageSimpleNameForFilename(libpath);
				if (text != null)
				{
					ResourceMap value = null;
					if (allResourceMaps.TryGetValue(text, out value) && value != null)
					{
						reswFilename = UriUtility.UriEncode(reswFilename);
						m_resourceMap = value.GetSubtree(reswFilename);
						if (m_resourceMap == null)
						{
							exceptionInfo = new PRIExceptionInfo();
							exceptionInfo._PackageSimpleName = text;
							exceptionInfo._ResWFile = reswFilename;
						}
						else
						{
							m_clonedResourceContext = s_globalResourceContext.Clone();
							if (m_clonedResourceContext != null)
							{
								m_clonedResourceContextFallBackList = ReadOnlyListToString(m_clonedResourceContext.Languages);
								if (m_clonedResourceContextFallBackList != null)
								{
									return true;
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	private static IReadOnlyList<string> StringToReadOnlyList(string s)
	{
		List<string> list = new List<string>(s.Split(s_charCultureSeparator));
		return list.AsReadOnly();
	}

	private static string ReadOnlyListToString(IReadOnlyList<string> list)
	{
		return string.Join(";", list);
	}

	public override bool SetGlobalResourceContextDefaultCulture(CultureInfo ci)
	{
		InitializeStaticGlobalResourceContext(null);
		if (s_globalResourceContext == null)
		{
			return false;
		}
		if (s_globalResourceContextBestFitCultureInfo != null && s_globalResourceContextBestFitCultureInfo.Name.Equals(ci.Name, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		List<string> list = new List<string>(s_globalResourceContext.Languages);
		list.Insert(0, ci.Name);
		for (int num = list.Count - 1; num > 0; num--)
		{
			if (list[num].Equals(ci.Name, StringComparison.OrdinalIgnoreCase))
			{
				list.RemoveAt(num);
			}
		}
		s_globalResourceContext.put_Languages((IReadOnlyList<string>)list);
		s_globalResourceContextBestFitCultureInfo = ci;
		return true;
	}

	[SecurityCritical]
	public override string GetString(string stringName, string startingCulture, string neutralResourcesCulture)
	{
		ResourceCandidate val = null;
		stringName = UriUtility.UriEncode(stringName);
		if (startingCulture == null && neutralResourcesCulture == null)
		{
			val = m_resourceMap.GetValue(stringName);
		}
		else
		{
			string text = null;
			text = ((startingCulture == null) ? "" : (startingCulture + ";")) + s_globalResourceContextFallBackList + ((neutralResourcesCulture == null) ? "" : (";" + neutralResourcesCulture));
			lock (m_clonedResourceContext)
			{
				if (!string.Equals(text, m_clonedResourceContextFallBackList, StringComparison.Ordinal))
				{
					m_clonedResourceContext.put_Languages(StringToReadOnlyList(text));
					m_clonedResourceContextFallBackList = text;
				}
				val = m_resourceMap.GetValue(stringName, m_clonedResourceContext);
			}
		}
		if (val != null)
		{
			return val.ValueAsString;
		}
		return null;
	}
}
