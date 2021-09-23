namespace Litdex.Security.RNG
{
	/// <summary>
	///		Interface structure for distribution.
	/// </summary>
	public interface IDistribution
	{
		/// <summary>
		///		Generate gaussian distribution.
		/// </summary>
		/// <param name="mean">
		///		Mean value.
		/// </param>
		/// <param name="std">
		///		Standard deviation value.
		/// </param>
		/// <param name="threadSafe">
		///		By default <see langword="false"/>. Set <see langword="true"/> for thread safe but more slower.
		/// </param>
		/// <returns>
		///		A 64 bit floating point number normal distribution.
		/// </returns>
		double NextGaussian(double mean = 0, double std = 1, bool threadSafe = false);

		/// <summary>
		///		Generate gamma distribution from 2 numbers.
		/// </summary>
		/// <param name="alpha">
		///		Alpha uniform number.
		/// </param>
		/// <param name="beta">
		///		Beta uniform number.
		/// </param>
		/// <returns>
		///		Gamma distribution.
		/// </returns>
		double NextGamma(double alpha, double beta);
	}
}
