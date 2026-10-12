namespace System.Threading.Tasks;

internal static class AsyncInfoIdGenerator
{
	internal const uint InvalidId = 2147483647u;

	private static Random idGenerator = new Random(19830118);

	internal static uint CreateNext()
	{
		lock (idGenerator)
		{
			return (uint)idGenerator.Next(1, int.MaxValue);
		}
	}

	internal unsafe static uint EnsureInitializedThreadsafe(ref uint id)
	{
		if (id != int.MaxValue)
		{
			return id;
		}
		uint num = CreateNext();
		fixed (uint* ptr = &id)
		{
			uint num2 = (uint)Interlocked.CompareExchange(ref *(int*)ptr, (int)num, int.MaxValue);
			if (num2 == int.MaxValue)
			{
				return num;
			}
			return num2;
		}
	}
}
