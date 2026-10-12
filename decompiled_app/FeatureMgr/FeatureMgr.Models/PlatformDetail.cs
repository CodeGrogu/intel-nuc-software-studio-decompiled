using System;
using FeatureMgr.Enums;
using Infrastructure.Utilities;

namespace FeatureMgr.Models;

public static class PlatformDetail
{
	public static NucType GetNucType()
	{
		string empty = string.Empty;
		try
		{
			empty = SysInfo.GetItem(SysItemName.BIOS).Substring(0, 3);
		}
		catch (Exception)
		{
			return NucType.Error;
		}
		if (empty == null)
		{
			return NucType.Error;
		}
		if (empty.Contains("HVK"))
		{
			return NucType.Hades;
		}
		if (empty.Contains("PH"))
		{
			return NucType.Phantom;
		}
		if (empty.Contains("PA"))
		{
			return NucType.Panther;
		}
		if (empty.Contains("FR"))
		{
			return NucType.Frost;
		}
		if (empty.Contains("BT"))
		{
			return NucType.Beast;
		}
		if (empty.Contains("ED") || empty.Contains("DC"))
		{
			return NucType.Dragon;
		}
		if (empty.Contains("SN"))
		{
			return NucType.Serpent;
		}
		if (empty.Contains("SBR"))
		{
			return NucType.Raptor;
		}
		return NucType.Error;
	}
}
