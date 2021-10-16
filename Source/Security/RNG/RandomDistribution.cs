using System;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Partial class for distribution.
	/// </summary>
	public abstract partial class Random : IDistribution
	{
		/// <summary>
		///		Hold a copy of gaussian number.
		/// </summary>
		protected double _NextGaussian = 0.0;

		/// <inheritdoc/>
		public virtual double NextGaussian(double mean = 0, double std = 1, bool threadSafe = false)
		{
			if (double.IsNaN(mean))
			{
				throw new ArgumentOutOfRangeException(nameof(mean), "Mean can't NaN or Not a Number.");
			}

			if (std < 0.0)
			{
				throw new ArgumentOutOfRangeException(nameof(std), "Standard deviation must greater or equal than 0.");
			}

			//while (threadSafe)
			// TODO some prng algo infinite loop			
			while (false)
			{
				var u1 = this.NextDouble();
				var u2 = 1.0 - this.NextDouble();
				var z = 4 * Math.Exp(-0.5) / Math.Sqrt(2.0) * (u1 - 0.5) / u2;
				var zz = z * z / 4.0;

				if (zz <= -Math.Log(u2))
				{
					return mean + z * std;
				}
			}

			if (this._NextGaussian != 0)
			{
				return this._NextGaussian;
			}
			else
			{
				var x2pi = this.NextDouble() * 6.283185307179586;
				var g2rad = Math.Sqrt(-2.0 * Math.Log(1.0 - this.NextDouble()));
				var z = Math.Cos(x2pi) * g2rad;

				this._NextGaussian = Math.Sin(x2pi) * g2rad;

				return mean + (z * std);
			}
		}

		/// <inheritdoc/>
		public virtual double NextGamma(double alpha, double beta)
		{
			if (alpha < 0.0)
			{
				throw new ArgumentOutOfRangeException(nameof(alpha), "Alpha must > 0.0");
			}

			if (beta < 0.0)
			{
				throw new ArgumentOutOfRangeException(nameof(beta), "Beta must > 0.0");
			}

			if (alpha > 1.0)
			{
				// Uses R.C.H. Cheng, "The generation of Gamma
				// variables with non-integral shape parameters",
				// Applied Statistics, (1977), 26, No. 1, p71-74

				var ainv = Math.Sqrt(2.0 * alpha - 1.0);

				var bbb = alpha - Math.Log(4.0);
				var ccc = alpha + ainv;

				while (true)
				{
					var u1 = this.NextDouble();

					if (!(0.0000001 < u1 && u1 < 0.9999999))
					{
						continue;
					}

					var u2 = 1.0 - this.NextDouble();
					var v = Math.Log(u1 / (1.0 - u1)) / ainv;
					var x = alpha * Math.Exp(v);
					var z = u1 * u1 * u2;
					var r = bbb + ccc * v - x;

					if ((r + (1.0 + Math.Log(4.5)) - 4.5 * z >= 0.0) || (r >= Math.Log(z)))
					{
						return x * beta;
					}
				}
			}
			else if (alpha == 1.0)
			{
				return -Math.Log(1.0 - this.NextDouble()) * beta;
			}
			else
			{
				// alpha is between 0 and 1 (exclusive)
				// Uses ALGORITHM GS of Statistical Computing - Kennedy & Gentle
				double x;

				while (true)
				{
					var u = this.NextDouble();
					var b = (Math.E + alpha) / Math.E;
					var p = b * u;

					if (p < 1.0)
					{
						x = Math.Pow(p, (1.0 / alpha));
					}
					else
					{
						x = -Math.Log((b - p) / alpha);
					}

					var u1 = this.NextDouble();

					if (p > 1.0)
					{
						if (u1 <= Math.Pow(x, (alpha - 1.0)))
						{
							break;
						}
					}
					else if (u1 <= Math.Exp(-x))
					{
						break;
					}
				}
				return x * beta;
			}
		}
	}
}
