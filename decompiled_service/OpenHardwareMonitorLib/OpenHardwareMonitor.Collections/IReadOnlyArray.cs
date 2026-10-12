using System.Collections;
using System.Collections.Generic;

namespace OpenHardwareMonitor.Collections;

public interface IReadOnlyArray<T> : IEnumerable<T>, IEnumerable
{
	T this[int index] { get; }

	int Length { get; }
}
