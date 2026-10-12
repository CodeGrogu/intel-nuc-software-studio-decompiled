using NucLedMgr.ENE.Enums;

namespace NucLedMgr.ENE.Models;

public static class EneLedHelper
{
	public static EneLedZone ConvertChannelToZone(byte Channel)
	{
		return (char)Channel switch
		{
			'A' => EneLedZone.LogoHeader, 
			'B' => EneLedZone.LeftHeader, 
			'C' => EneLedZone.RightHeader, 
			'D' => EneLedZone.FrontHeader, 
			_ => EneLedZone.Error, 
		};
	}

	public static byte ConvertZoneToChannel(EneLedZone Index)
	{
		return Index switch
		{
			EneLedZone.LogoHeader => (byte)65, 
			EneLedZone.LeftHeader => (byte)66, 
			EneLedZone.RightHeader => (byte)67, 
			EneLedZone.FrontHeader => (byte)68, 
			_ => (byte)0, 
		};
	}
}
