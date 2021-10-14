using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Interface structure for Sequence.
	/// </summary>
	public interface ISequence
	{
		/// <summary>
		///		Select one element randomly from the given set.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <returns>
		///		Random element from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The items length or size can't be greater than int.MaxValue.
		/// </exception>
		T Choice<T>(T[] items);

		/// <summary>
		///		Select arbitrary element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved is negative or less than 1.
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		T[] Choice<T>(T[] items, int select);

		/// <summary>
		///		Select arbitrary element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved is negative or less than 1.
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		Task<T[]> ChoiceAsync<T>(T[] items, int select);

		/// <summary>
		///		Select arbitrary element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <param name="cancellationToken">
		///		Token to cancel the operations.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved is negative or less than 1.
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		Task<T[]> ChoiceAsync<T>(T[] items, int select, CancellationToken cancellationToken);

		/// <summary>
		///		Select one element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <returns>
		///		Random element from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The items length or size can't be greater than int.MaxValue.
		/// </exception>
		T Choice<T>(IEnumerable<T> items);

		/// <summary>
		///		Select arbitrary element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved is negative or less than 1.
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		T[] Choice<T>(IEnumerable<T> items, int select);

		/// <summary>
		///		Select arbitrary element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved is negative or less than 1.
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		Task<T[]> ChoiceAsync<T>(IEnumerable<T> items, int select);

		/// <summary>
		///		Select arbitrary element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <param name="cancellationToken">
		///		Token to cancel the operations.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved is negative or less than 1.
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		Task<T[]> ChoiceAsync<T>(IEnumerable<T> items, int select, CancellationToken cancellationToken);

		/// <summary>
		///		Select arbitrary distinct element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		/// 	A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The number of elements to be retrieved is negative or less than 1.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		T[] Sample<T>(T[] items, int select);

		/// <summary>
		///		Select arbitrary distinct element randomly.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		/// 	A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The number of elements to be retrieved is negative or less than 1.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		Task<T[]> SampleAsync<T>(T[] items, int select);

		/// <summary>
		///		Select arbitrary distinct element randomly.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		/// 	A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <param name="cancellationToken">
		///		Token to cancel the operations.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The number of elements to be retrieved is negative or less than 1.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		Task<T[]> SampleAsync<T>(T[] items, int select, CancellationToken cancellationToken);

		/// <summary>
		///		Select arbitrary distinct element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The number of elements to be retrieved is negative or less than 1.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		T[] Sample<T>(IEnumerable<T> items, int select);

		/// <summary>
		///		Select arbitrary distinct element randomly.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		/// 	A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The number of elements to be retrieved is negative or less than 1.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		Task<T[]> SampleAsync<T>(IEnumerable<T> items, int select);

		/// <summary>
		///		Select arbitrary distinct element randomly.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		/// 	A set of items to select.
		/// </param>
		/// <param name="select">
		///		The desired amount to select.
		/// </param>
		/// <param name="cancellationToken">
		///		Token to cancel the operations.
		/// </param>
		/// <returns>
		///		Multiple random elements from the given set.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The number of elements to be retrieved is negative or less than 1.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The number of elements to be retrieved exceeds the items size.
		/// </exception>
		Task<T[]> SampleAsync<T>(IEnumerable<T> items, int select, CancellationToken cancellationToken);

		/// <summary>
		///		Shuffle items with Fisher-Yates shuffle then return the shuffled item in new array.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to shuffle.
		/// </param>
		/// <returns>
		///		Array of shuffled items.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		T[] Shuffle<T>(T[] items);

		/// <summary>
		///		Shuffle items with Fisher-Yates shuffle then return the shuffled item in new array.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to shuffle.
		/// </param>
		/// <returns>
		///		Array of shuffled items.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		Task<T[]> ShuffleAsync<T>(T[] items);

		/// <summary>
		///		Shuffle items with Fisher-Yates shuffle then return the shuffled item in new array.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to shuffle.
		/// </param>
		/// <param name="cancellationToken">
		///		Token to cancel the operations.
		/// </param>
		/// <returns>
		///		Array of shuffled items.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		Task<T[]> ShuffleAsync<T>(T[] items, CancellationToken cancellationToken);

		/// <summary>
		///		Shuffle items with Fisher-Yates shuffle then return the shuffled item in new array.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to shuffle.
		/// </param>
		/// <returns>
		///		Array of shuffled items.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		T[] Shuffle<T>(IEnumerable<T> items);

		/// <summary>
		///		Shuffle items with Fisher-Yates shuffle then return the shuffled item in new array.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to shuffle.
		/// </param>
		/// <returns>
		///		Array of shuffled items.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		Task<T[]> ShuffleAsync<T>(IEnumerable<T> items);

		/// <summary>
		///		Shuffle items with Fisher-Yates shuffle then return the shuffled item in new array.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to shuffle.
		/// </param>
		/// <param name="cancellationToken">
		///		Token to cancel the operations.
		/// </param>
		/// <returns>
		///		Array of shuffled items.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		Task<T[]> ShuffleAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken);

		/// <summary>
		///		Shuffle items in place with Fisher-Yates shuffle.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to shuffle.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		void ShuffleInPlace<T>(T[] items);

		/// <summary>
		///		Shuffle items in place with Fisher-Yates shuffle.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to shuffle.
		/// </param>
		/// <returns>
		///		Task based operations.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		Task ShuffleInPlaceAsync<T>(T[] items);

		/// <summary>
		///		Shuffle items in place with Fisher-Yates shuffle.
		/// </summary>
		/// <remarks>
		///		Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to shuffle.
		/// </param>
		/// <param name="cancellationToken">
		///		Token to cancel the operations.
		/// </param>
		/// <returns>
		///		Task based operations.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		Task ShuffleInPlaceAsync<T>(T[] items, CancellationToken cancellationToken);
	}
}