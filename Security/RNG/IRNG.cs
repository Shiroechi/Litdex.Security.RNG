using System;
using System.Collections.Generic;

namespace Litdex.Security.RNG {
  /// <summary>
  /// Interface structure for Random Number Generator (RNG).
  /// </summary>
  public interface IRNG {
    /// <summary>
    /// The name of the algorithm this generator implements.
    /// </summary>
    /// <returns>
    /// The name of this RNG.
    /// </returns>
    string AlgorithmName();

    /// <summary>
    /// Seed with <see
    /// cref="System.Security.Cryptography.RNGCryptoServiceProvider"/>.
    /// </summary>
    void Reseed();

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
    byte[] NextBytes(int length);

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
    /// <exception cref="ArgumentOutOfRangeException">
    /// <list type="bullet">
    ///		<item>
    ///		The number of elements to be retrieved is negative or less
    ///than 1.
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
    /// <exception cref="ArgumentOutOfRangeException">
    /// <list type="bullet">
    ///		<item>
    ///		The number of elements to be retrieved is negative or less
    ///than 1.
    ///		</item>
    ///		<item>
    ///		The number of elements to be retrieved exceeds the items size.
    ///		</item>
    /// </list>
    /// </exception>
    T[] Choice<T>(IList<T> items, int select);
  }
}