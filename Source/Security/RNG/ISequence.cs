using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Interface structure for Sequence.
	/// </summary>
	public interface ISequence
	{
		#region Sequence

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
		///		Select abritary element randomly.
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
		T Choice<T>(ICollection<T> items);

		/// <summary>
		///		Select abritary element randomly.
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
		T[] Choice<T>(ICollection<T> items, int select);

		/// <summary>
		///		Select abritary distinct element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		/// 	A set of items to select.
		/// </param>
		/// <param name="k">
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
		T[] Sample<T>(T[] items, int k);

		/// <summary>
		///		Select abritary distinct element randomly.
		/// </summary>
		/// <typeparam name="T">
		///		The type of objects in array.
		/// </typeparam>
		/// <param name="items">
		///		A set of items to select.
		/// </param>
		/// <param name="k">
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
		T[] Sample<T>(ICollection<T> items, int k);

		/// <summary>
		///		Select abritary distinct element randomly.
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
		/// <param name="k">
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
		Task<T[]> SampleAsync<T>(T[] items, int k);

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
		void Shuffle<T>(T[] items);

		/// <summary>
		///		Shuffle items with Fisher-Yates shuffle.
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
		///		Shuffled items.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		Task ShuffleAsync<T>(T[] items);

		#endregion Sequence
	}
}