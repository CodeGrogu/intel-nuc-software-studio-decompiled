using System;

namespace MobileKeyboardLedMgr.Models;

public static class KeyboardIdleTimer
{
	private const int IDLE_TIMER_VALUE_0 = 0;

	private static readonly int IDLE_TIMER_VALUE_1 = (int)TimeSpan.FromSeconds(5.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_2 = (int)TimeSpan.FromSeconds(10.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_3 = (int)TimeSpan.FromSeconds(15.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_4 = (int)TimeSpan.FromSeconds(30.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_5 = (int)TimeSpan.FromSeconds(45.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_6 = (int)TimeSpan.FromMinutes(1.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_7 = (int)TimeSpan.FromMinutes(2.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_8 = (int)TimeSpan.FromMinutes(5.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_9 = (int)TimeSpan.FromMinutes(10.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_10 = (int)TimeSpan.FromMinutes(30.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_11 = (int)TimeSpan.FromHours(1.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_12 = (int)TimeSpan.FromHours(2.0).TotalSeconds;

	private static readonly int IDLE_TIMER_VALUE_13 = (int)TimeSpan.FromHours(3.0).TotalSeconds;

	public static int ConvertIdleTimerToSeconds(int Value)
	{
		return Value switch
		{
			0 => 0, 
			1 => IDLE_TIMER_VALUE_1, 
			2 => IDLE_TIMER_VALUE_2, 
			3 => IDLE_TIMER_VALUE_3, 
			4 => IDLE_TIMER_VALUE_4, 
			5 => IDLE_TIMER_VALUE_5, 
			6 => IDLE_TIMER_VALUE_6, 
			7 => IDLE_TIMER_VALUE_7, 
			8 => IDLE_TIMER_VALUE_8, 
			9 => IDLE_TIMER_VALUE_9, 
			10 => IDLE_TIMER_VALUE_10, 
			11 => IDLE_TIMER_VALUE_11, 
			12 => IDLE_TIMER_VALUE_12, 
			13 => IDLE_TIMER_VALUE_13, 
			_ => 0, 
		};
	}

	public static int ConvertSecondsToIdleTImer(int Seconds)
	{
		if (Seconds <= 0)
		{
			return 0;
		}
		if (Seconds > 0 && Seconds <= IDLE_TIMER_VALUE_1)
		{
			return 1;
		}
		if (Seconds > IDLE_TIMER_VALUE_1 && Seconds <= IDLE_TIMER_VALUE_2)
		{
			return 2;
		}
		if (Seconds > IDLE_TIMER_VALUE_2 && Seconds <= IDLE_TIMER_VALUE_3)
		{
			return 3;
		}
		if (Seconds > IDLE_TIMER_VALUE_3 && Seconds <= IDLE_TIMER_VALUE_4)
		{
			return 4;
		}
		if (Seconds > IDLE_TIMER_VALUE_4 && Seconds <= IDLE_TIMER_VALUE_5)
		{
			return 5;
		}
		if (Seconds > IDLE_TIMER_VALUE_5 && Seconds <= IDLE_TIMER_VALUE_6)
		{
			return 6;
		}
		if (Seconds > IDLE_TIMER_VALUE_6 && Seconds <= IDLE_TIMER_VALUE_7)
		{
			return 7;
		}
		if (Seconds > IDLE_TIMER_VALUE_7 && Seconds <= IDLE_TIMER_VALUE_8)
		{
			return 8;
		}
		if (Seconds > IDLE_TIMER_VALUE_8 && Seconds <= IDLE_TIMER_VALUE_9)
		{
			return 9;
		}
		if (Seconds > IDLE_TIMER_VALUE_9 && Seconds <= IDLE_TIMER_VALUE_10)
		{
			return 10;
		}
		if (Seconds > IDLE_TIMER_VALUE_10 && Seconds <= IDLE_TIMER_VALUE_11)
		{
			return 11;
		}
		if (Seconds > IDLE_TIMER_VALUE_11 && Seconds <= IDLE_TIMER_VALUE_12)
		{
			return 12;
		}
		if (Seconds > IDLE_TIMER_VALUE_12 && Seconds <= IDLE_TIMER_VALUE_13)
		{
			return 13;
		}
		return 0;
	}
}
