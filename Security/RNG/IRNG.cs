using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Litdex.Security.RNG
{
	/// <summary>
	/// Interface structure for Random Number Generator (RNG).
	/// </summary>
	public interface IRNG
	{
		/// <summary>
		/// The name of the algorithm this generator implements.
		/// </summary>
		/// <returns>
		/// The name of this RNG.
		/// </returns>
		string AlgorithmName();

		/// <summary>
		/// Seed with <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/>.
		/// </summary>
		void Reseed();

		#region Basic

		/// <summary>
		/// Generate <see cref="bool"/> value from generator.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> or <see langword="false"/>.
		/// </returns>
		bool NextBoolean();

		/// <summary>
		/// Generate <see cref="byte"/> value from generator.
		/// </summary>
		/// <returns>
		/// Random <see cref="byte"/>.
		/// </returns>
		byte NextByte();

		/// <summary>
		/// Generate <see cref="byte"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <param name="lower">
		/// Lower bound or expected minimum value.
		/// </param>
		/// <param name="upper">
		/// Upper bound or ecpected maximum value.
		/// </param>
		/// <returns>
		/// <see cref="byte"/> value between lower bound and upper bound.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Lower bound is greater than or equal to upper bound.
		/// </exception>
		byte NextByte(byte lower, byte upper);

		/// <summary>
		/// Generate random byte[] value from generator.
		/// </summary>
		/// <param name="length">
		/// Requested output length.
		/// </param>
		/// <returns>
		/// Array of bytes.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The requested output size can't lower than 1.
		/// </exception>
		byte[] NextBytes(int length);

		/// <summary>
		/// Fill the array with random bytes.
		/// </summary>
		/// <param name="bytes">Array to fill with random bytes.</param>		/// 
		/// <exception cref="ArgumentNullException">
		/// Array length can't be lower than 1 or null.
		/// </exception>
		void Fill(byte[] bytes);

		/// <summary>
		/// Generate <see cref="uint"/> value from generator.
		/// </summary>
		/// <returns>
		/// A 32-bit unsigned integer.
		/// </returns>
		uint NextInt();

		/// <summary>
		/// Generate <see cref="uint"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <param name="lower">
		/// Lower bound or expected minimum value.
		/// </param>
		/// <param name="upper">
		/// Upper bound or ecpected maximum value.
		/// </param>
		/// <returns>
		/// <see cref="uint"/> value between lower bound and upper bound.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Lower bound is greater than or equal to upper bound.
		/// </exception>
		uint NextInt(uint lower, uint upper);

		/// <summary>
		/// Generate <see cref="ulong"/> value from generator. 
		/// </summary>
		/// <returns>
		/// A 64-bit unsigned integer.
		/// </returns>
		ulong NextLong();

		/// <summary>
		///	Generate <see cref="ulong"/> value between 
		/// lower bound and upper bound from generator. 
		/// </summary>
		/// <param name="lower">
		/// Lower bound or expected minimum value.
		/// </param>
		/// <param name="upper">
		/// Upper bound or ecpected maximum value.
		/// </param>
		/// <returns>
		/// <see cref="ulong"/> value lower bound and upper bound.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Lower bound is greater than or equal to upper bound.
		/// </exception>
		ulong NextLong(ulong lower, ulong upper);

		/// <summary>
		/// Generate <see cref="double"/> value from generator.
		/// </summary>
		/// <returns>
		/// A 64-bit floating point.
		/// </returns>
		double NextDouble();

		/// <summary>
		/// Generate <see cref="double"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <param name="lower">
		/// Lower bound or expected minimum value.
		/// </param>
		/// <param name="upper">
		/// Upper bound or ecpected maximum value.
		/// </param>
		/// <returns>
		/// <see cref="double"/> value between lower bound and upper bound.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Lower bound is greater than or equal to upper bound.
		/// </exception>
		double NextDouble(double lower, double upper);

		#endregion Basic

		#region Sequence

		/// <summary>
		/// Select one element randomly.
		/// </summary>
		/// <typeparam name="T">
		/// Data type
		/// </typeparam>
		/// <param name="items">
		/// Set of items to choose.
		/// </param>
		/// <returns>
		/// Random element from the given sets.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The items length or size can't be greater than int.MaxValue.
		/// </exception>
		T Choice<T>(T[] items);

		/// <summary>
		/// Select abritary element randomly.
		/// </summary>
		/// <typeparam name="T">
		/// Data type
		/// </typeparam>
		/// <param name="items">
		/// Set of items to choose.
		/// </param>
		/// <param name="select">
		/// The desired amount to select.
		/// </param>
		/// <returns>
		/// Multiple random elements from the given sets.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <list type="bullet">
		///		<item>
		///		The number of elements to be retrieved is negative or less than 1.
		///		</item>
		///		<item>
		///		The number of elements to be retrieved exceeds the items size.
		///		</item>
		/// </list>
		/// </exception>
		T[] Choice<T>(T[] items, int select);

		/// <summary>
		/// Select one element randomly.
		/// </summary>
		/// <typeparam name="T">
		/// Data type
		/// </typeparam>
		/// <param name="items">
		/// Set of items to choose.
		/// </param>
		/// <returns>
		/// Random element from the given sets.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The items length or size can't be greater than int.MaxValue.
		/// </exception>
		T Choice<T>(IList<T> items);

		/// <summary>
		/// Select abritary element randomly.
		/// </summary>
		/// <typeparam name="T">
		/// Data type
		/// </typeparam>
		/// <param name="items">
		/// Set of items to choose.
		/// </param>
		/// <param name="select">
		/// The desired amount to select.
		/// </param>
		/// <returns>
		/// Multiple random elements from the given sets.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <list type="bullet">
		///		<item>
		///		The number of elements to be retrieved is negative or less than 1.
		///		</item>
		///		<item>
		///		The number of elements to be retrieved exceeds the items size.
		///		</item>
		/// </list>
		/// </exception>
		T[] Choice<T>(IList<T> items, int select);

		/// <summary>
		/// Select abritary distinct element randomly.
		/// </summary>
		/// <typeparam name="T">
		/// Data type
		/// </typeparam>
		/// <param name="items">
		/// Set of items to choose.
		/// </param>
		/// <param name="k">
		/// The desired amount to select.
		/// </param>
		/// <returns>
		/// Multiple random elements from the given sets.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <list type="bullet">
		///		<item>
		///		The number of elements to be retrieved is negative or less than 1.
		///		</item>
		///		<item>
		///		The number of elements to be retrieved exceeds the items size.
		///		</item>
		/// </list>
		/// </exception>
		T[] Sample<T>(T[] items, int k);

		/// <summary>
		/// Select abritary distinct element randomly.
		/// </summary>
		/// <remarks>
		/// Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		/// Data type
		/// </typeparam>
		/// <param name="items">
		/// Set of items to choose.
		/// </param>
		/// <param name="k">
		/// The desired amount to select.
		/// </param>
		/// <returns>
		/// Multiple random elements from the given sets.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <list type="bullet">
		///		<item>
		///		The number of elements to be retrieved is negative or less than 1.
		///		</item>
		///		<item>
		///		The number of elements to be retrieved exceeds the items size.
		///		</item>
		/// </list>
		/// </exception>
		Task<T[]> SampleAsync<T>(T[] items, int k);

		/// <summary>
		/// Shuffle items with Fisher-Yates shuffle.
		/// </summary>
		/// <typeparam name="T">
		/// Data type
		/// </typeparam>
		/// <param name="items">
		/// Set of items to shuffle.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		void Shuffle<T>(T[] items);

		/// <summary>
		/// Shuffle items with Fisher-Yates shuffle.
		/// </summary>
		/// <remarks>
		/// Used for large data, objects or arrays.
		/// </remarks>
		/// <typeparam name="T">
		/// Data type
		/// </typeparam>
		/// <param name="items">
		/// Set of items to shuffle.
		/// </param>
		/// <returns>
		/// Shuffled items.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		The items is null, empty or not initialized. 
		/// </exception>
		Task ShuffleAsync<T>(T[] items);

		#endregion Sequence
	}
}