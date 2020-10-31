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
		/// Seed with RNGCryptoServiceProvider.
		/// </summary>
		void Reseed();

		/// <summary>
		/// Generate <see cref="bool"/> value from generator.
		/// </summary>
		/// <returns></returns>
		bool NextBoolean();

		/// <summary>
		/// Generate <see cref="byte"/> value from generator.
		/// </summary>
		/// <returns></returns>
		byte NextByte();

		/// <summary>
		/// Generate <see cref="byte"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <param name="lower">Lower bound.</param>
		/// <param name="upper">Upper bound.</param>
		/// <returns></returns>
		byte NextByte(byte lower, byte upper);

		/// <summary>
		/// Generate random byte[] value from generator.
		/// </summary>
		/// <param name="length">Output length.</param>
		/// <returns></returns>
		byte[] NextBytes(int length);

		/// <summary>
		/// Generate <see cref="uint"/> value from generator.
		/// </summary>
		/// <returns></returns>
		uint NextInt();

		/// <summary>
		/// Generate <see cref="uint"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <param name="lower">Lower bound.</param>
		/// <param name="upper">Upper bound.</param>
		/// <returns></returns>
		uint NextInt(uint lower, uint upper);

		/// <summary>
		/// Generate <see cref="ulong"/> value from generator. 
		/// </summary>
		/// <returns></returns>
		ulong NextLong();

		/// <summary>
		/// Generate <see cref="ulong"/> value between 
		/// lower bound and upper bound from generator. 
		/// </summary>
		/// <param name="lower">Lower bound.</param>
		/// <param name="upper">Upper bound.</param>
		/// <returns></returns>
		ulong NextLong(ulong lower, ulong upper);

		/// <summary>
		/// Generate <see cref="double"/> value from generator.
		/// </summary>
		/// <returns></returns>
		double NextDouble();

		/// <summary>
		/// Generate <see cref="double"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <param name="lower">Lower bound.</param>
		/// <param name="upper">Upper bound.</param>
		/// <returns></returns>
		double NextDouble(double lower, double upper);
	}
}