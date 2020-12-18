namespace Litdex.Security.RNG
{
	/// <summary>
	/// Interface structure for distribution.
	/// </summary>
	public interface IDistribution
	{
		/// <summary>
		/// Return gaussian distribution.
		/// </summary>
		/// <param name="mean">
		///		Mean value.
		/// </param>
		/// <param name="std">
		///		Standard deviation value.
		/// </param>
		/// <returns>
		///		Normal distribution.
		/// </returns>
		double NextGaussian(double mean = 0, double std = 1);
	}
}
