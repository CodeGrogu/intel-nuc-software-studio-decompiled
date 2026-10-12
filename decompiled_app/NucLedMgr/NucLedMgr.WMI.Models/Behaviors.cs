namespace NucLedMgr.WMI.Models;

public static class Behaviors
{
	private static readonly byte SOLID = 0;

	private static readonly byte BREATHING = 1;

	private static readonly byte PULSING = 2;

	private static readonly byte STROBING = 3;

	private static readonly byte MINIMUM = 0;

	private static readonly byte MAXIMUM = 10;

	private static readonly byte S0_BEHAVIOR = 1;

	private static readonly byte S0_FREQUENCY = 2;

	private static readonly byte S3_BEHAVIOR = 7;

	private static readonly byte S3_FREQUENCY = 8;

	private static readonly byte RM_BEHAVIOR = 13;

	private static readonly byte RM_FREQUENCY = 14;

	private static readonly byte S5_BEHAVIOR = 19;

	private static readonly byte S5_FREQUENCY = 20;

	private static readonly byte HDD_BEHAVIOR = 4;

	private static readonly byte ETHERNET_BEHAVIOR = 0;

	private static readonly byte POWERLIMIT_BEHAVIOR = 0;

	public static readonly ControlItem S0Solid = new ControlItem(S0_BEHAVIOR, SOLID, supported: true);

	public static readonly ControlItem S0Breathing = new ControlItem(S0_BEHAVIOR, BREATHING, supported: true);

	public static readonly ControlItem S0Pulsing = new ControlItem(S0_BEHAVIOR, PULSING, supported: true);

	public static readonly ControlItem S0Strobing = new ControlItem(S0_BEHAVIOR, STROBING, supported: true);

	public static readonly ControlItem S0FreqMin = new ControlItem(S0_FREQUENCY, MINIMUM, supported: true);

	public static readonly ControlItem S0FreqMax = new ControlItem(S0_FREQUENCY, MAXIMUM, supported: true);

	public static readonly ControlItem S3Solid = new ControlItem(S3_BEHAVIOR, SOLID, supported: true);

	public static readonly ControlItem S3Breathing = new ControlItem(S3_BEHAVIOR, BREATHING, supported: true);

	public static readonly ControlItem S3Pulsing = new ControlItem(S3_BEHAVIOR, PULSING, supported: true);

	public static readonly ControlItem S3Strobing = new ControlItem(S3_BEHAVIOR, STROBING, supported: true);

	public static readonly ControlItem S3FreqMin = new ControlItem(S3_FREQUENCY, MINIMUM, supported: true);

	public static readonly ControlItem S3FreqMax = new ControlItem(S3_FREQUENCY, MAXIMUM, supported: true);

	public static readonly ControlItem RMSolid = new ControlItem(RM_BEHAVIOR, SOLID, supported: true);

	public static readonly ControlItem RMBreathing = new ControlItem(RM_BEHAVIOR, BREATHING, supported: true);

	public static readonly ControlItem RMPulsing = new ControlItem(RM_BEHAVIOR, PULSING, supported: true);

	public static readonly ControlItem RMStrobing = new ControlItem(RM_BEHAVIOR, STROBING, supported: true);

	public static readonly ControlItem RMFreqMin = new ControlItem(RM_FREQUENCY, MINIMUM, supported: true);

	public static readonly ControlItem RMFreqMax = new ControlItem(RM_FREQUENCY, MAXIMUM, supported: true);

	public static readonly ControlItem S5Solid = new ControlItem(S5_BEHAVIOR, SOLID, supported: true);

	public static readonly ControlItem S5Breathing = new ControlItem(S5_BEHAVIOR, BREATHING, supported: true);

	public static readonly ControlItem S5Pulsing = new ControlItem(S5_BEHAVIOR, PULSING, supported: true);

	public static readonly ControlItem S5Strobing = new ControlItem(S5_BEHAVIOR, STROBING, supported: true);

	public static readonly ControlItem S5FreqMin = new ControlItem(S5_FREQUENCY, MINIMUM, supported: true);

	public static readonly ControlItem S5FreqMax = new ControlItem(S5_FREQUENCY, MAXIMUM, supported: true);

	public static readonly ControlItem OffToOn = new ControlItem(HDD_BEHAVIOR, 0, supported: true);

	public static readonly ControlItem OnToOff = new ControlItem(HDD_BEHAVIOR, 1, supported: true);

	public static readonly ControlItem LAN1 = new ControlItem(ETHERNET_BEHAVIOR, 0, supported: true);

	public static readonly ControlItem LAN2 = new ControlItem(ETHERNET_BEHAVIOR, 1, supported: true);

	public static readonly ControlItem LAN1and2 = new ControlItem(ETHERNET_BEHAVIOR, 2, supported: true);

	public static readonly ControlItem PowerLimitGreenToRed = new ControlItem(POWERLIMIT_BEHAVIOR, 0, supported: true);

	public static readonly ControlItem PowerLimitCustom = new ControlItem(POWERLIMIT_BEHAVIOR, 1, supported: true);
}
