using System.Collections.ObjectModel;

namespace System.ComponentModel;

/// <summary>Defines properties that enables sorting, grouping, and filtering on a <see cref="T:System.Windows.Data.CollectionView" /> in real time.</summary>
public interface ICollectionViewLiveShaping
{
	/// <summary>Gets a value that indicates whether the collection view supports turning sorting data in real time on or off.</summary>
	/// <returns>
	///     <see langword="true" /> if the collection view supports turning live sorting on or off; otherwise, <see langword="false" />.</returns>
	bool CanChangeLiveSorting { get; }

	/// <summary>Gets a value that indicates whether the collection view supports turning filtering data in real time on or off.</summary>
	/// <returns>
	///     <see langword="true" /> if the collection view supports turning live filtering on or off; otherwise, <see langword="false" />.</returns>
	bool CanChangeLiveFiltering { get; }

	/// <summary>Gets a value that indicates whether the collection view supports turning grouping data in real time on or off.</summary>
	/// <returns>
	///     <see langword="true" /> if the collection view supports turning live grouping on or off; otherwise, <see langword="false" />.</returns>
	bool CanChangeLiveGrouping { get; }

	/// <summary>Gets or sets a value that indicates whether sorting in real time is enabled.</summary>
	/// <returns>
	///     <see langword="true" /> if sorting data in real time is enable; <see langword="false" /> if live sorting is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live sorting.</returns>
	bool? IsLiveSorting { get; set; }

	/// <summary>Gets or sets a value that indicates whether filtering data in real time is enabled.</summary>
	/// <returns>
	///     <see langword="true" /> if filtering data in real time is enabled; <see langword="false" /> if live filtering is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live filtering.</returns>
	bool? IsLiveFiltering { get; set; }

	/// <summary>Gets or sets a value that indicates whether grouping data in real time is enabled.</summary>
	/// <returns>
	///     <see langword="true" /> if grouping data in real time is enable; <see langword="false" /> if live grouping is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live grouping.</returns>
	bool? IsLiveGrouping { get; set; }

	/// <summary>Gets a collection of strings that specify the properties that participate in sorting data in real time.</summary>
	/// <returns>A collection of strings that specify the properties that participate in sorting data in real time.</returns>
	ObservableCollection<string> LiveSortingProperties { get; }

	/// <summary>Gets a collection of strings that specify the properties that participate in filtering data in real time.</summary>
	/// <returns>A collection of strings that specify the properties that participate in filtering data in real time.</returns>
	ObservableCollection<string> LiveFilteringProperties { get; }

	/// <summary>Gets a collection of strings that specify the properties that participate in grouping data in real time.</summary>
	/// <returns>A collection of strings that specify the properties that participate in grouping data in real time.</returns>
	ObservableCollection<string> LiveGroupingProperties { get; }
}
