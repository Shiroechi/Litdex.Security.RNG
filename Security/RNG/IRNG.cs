using System;
using System.Collections.Generic;

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
		/// <returns>The name of this RNG.</returns>
		string AlgorithmName();

		/// <summary>
		/// Seed with <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/>.
		/// </summary>
		void Reseed();

		/// <summary>
		/// Generate <see cref="bool"/> value from generator.
		/// </summary>
		/// <returns><see langword="true"/> or <see langword="false"/></returns>
		bool NextBoolean();

		/// <summary>
		/// Generate <see cref="byte"/> value from generator.
		/// </summary>
		/// <returns>Random <see cref="byte"/></returns>
		byte NextByte();

		/// <summary>
		/// Generate <see cref="byte"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <param name="lower">Lower bound.</param>
		/// <param name="upper">Upper bound.</param>
		/// <returns><see cref="byte"/> value between 
		/// lower bound and upper bound.</returns>
		/// <exception cref="ArgumentException">Lower bound is greater than or equal to upper bound.</exception>
		byte NextByte(byte lower, byte upper);

		/// <summary>
		/// Generate random byte[] value from generator.
		/// </summary>
		/// <param name="length">Output length.</param>
		/// <returns>Array of bytes.</returns>
		byte[] NextBytes(int length);

		/// <summary>
		/// Generate <see cref="uint"/> value from generator.
		/// </summary>
		/// <returns>A 32-bit unsigned integer.</returns>
		uint NextInt();

		/// <summary>
		/// Generate <see cref="uint"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <param name="lower">Lower bound.</param>
		/// <param name="upper">Upper bound.</param>
		/// <returns><see cref="uint"/> value between 
		/// lower bound and upper bound.</returns>
		/// <exception cref="ArgumentException">Lower bound is greater than or equal to upper bound.</exception>
		uint NextInt(uint lower, uint upper);

		/// <summary>
		/// Generate <see cref="ulong"/> value from generator. 
		/// </summary>
		/// <returns>A 64-bit unsigned integer.</returns>
		ulong NextLong();

		/// <summary>
		/// Generate <see cref="ulong"/> value between 
		/// lower bound and upper bound from generator. 
		/// </summary>
		/// <param name="lower">Lower bound.</param>
		/// <param name="upper">Upper bound.</param>
		/// <returns><see cref="ulong"/> value lower bound and upper bound.</returns>
		/// <exception cref="ArgumentException">Lower bound is greater than or equal to upper bound.</exception>
		ulong NextLong(ulong lower, ulong upper);

		/// <summary>
		/// Generate <see cref="double"/> value from generator.
		/// </summary>
		/// <returns>A 64-bit floating point.</returns>
		double NextDouble();

		/// <summary>
		/// Generate <see cref="double"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <param name="lower">Lower bound.</param>
		/// <param name="upper">Upper bound.</param>
		/// <returns><see cref="double"/> value between lower bound and upper bound.</returns>
		/// <exception cref="ArgumentException">Lower bound is greater than or equal to upper bound.</exception>
		double NextDouble(double lower, double upper);

		T Choice<T>(T[] items);

		T[] Choice<T>(T[] items, int select);

		T Choice<T>(IList<T> items);

		IList<T> Choice<T>(IList<T> items, int select);
	}
}