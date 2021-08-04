using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Base class of all random.
	/// </summary>
	public abstract class Random : IRNG, IDistribution
	{
		#region Member

		protected double _NextGaussian = 0.0;

		#endregion Member

		#region Public Method

		/// <inheritdoc/>
		public virtual string AlgorithmName()
		{
			return "Random";
		}

		/// <inheritdoc/>
		public abstract void Reseed();

		/// <inheritdoc/>
		public override string ToString()
		{
			return this.AlgorithmName();
		}

		#endregion Public Method

		#region Basic

		/// <inheritdoc/>
		public abstract bool NextBoolean();

		/// <inheritdoc/>
		public abstract byte NextByte();

		/// <inheritdoc/>
		public virtual byte NextByte(byte lower, byte upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = (byte)(upper - lower + 1);
			return (byte)(lower + (this.NextByte() % diff));
		}

		/// <inheritdoc/>
		public abstract byte[] NextBytes(int length);

		/// <inheritdoc/>
		public abstract void Fill(byte[] bytes);

#if NET5_0_OR_GREATER

		/// <inheritdoc/>
		public abstract void Fill(Span<byte> bytes);

#endif

		/// <inheritdoc/>
		public abstract uint NextInt();

		/// <inheritdoc/>
		public virtual uint NextInt(uint lower, uint upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = upper - lower + 1;
			return lower + (this.NextInt() % diff);
		}

		/// <summary>
		///		Lemire algorithm to generate <see cref="uint"/> value between 
		///		lower bound and upper bound from generator.
		/// </summary>
		/// <remarks>
		///		https://lemire.me/blog/2016/06/27/a-fast-alternative-to-the-modulo-reduction/
		/// </remarks>
		/// <param name="lower">
		///		Lower bound or expected minimum value.
		/// </param>
		/// <param name="upper">
		///		Upper bound or ecpected maximum value.
		/// </param>
		/// <param name="unbias">
		///		Determine using division for reduce bias.
		/// </param>
		/// <returns>
		///		<see cref="uint"/> value between lower bound and upper bound.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		Lower bound is greater than or equal to upper bound.
		/// </exception>
		public virtual uint NextIntFast(uint lower, uint upper, bool unbias = false)
		{
			if (lower >= upper)
			{
				throw new ArgumentException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
			}

			uint range = upper - lower;
			ulong random32bit = this.NextInt();
			ulong multiresult = random32bit * range;
			uint leftover = (uint)multiresult;

			if (unbias)
			{
				if (leftover < range)
				{
					uint threshold = (uint)((int)-range % range);
					while (leftover < threshold)
					{
						random32bit = this.NextInt();
						multiresult = random32bit * range;
						leftover = (uint)multiresult;
					}
				}
			}

			return (uint)(multiresult >> 32) + lower;
		}

		/// <inheritdoc/>
		public abstract ulong NextLong();

		/// <inheritdoc/>
		public virtual ulong NextLong(ulong lower, ulong upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = upper - lower + 1;
			return lower + (this.NextLong() % diff);
		}

		/// <inheritdoc/>
		public virtual double NextDouble()
		{
			return (this.NextLong() >> 11) * (1.0 / (1L << 53));
		}

		/// <inheritdoc/>
		public virtual double NextDouble(double lower, double upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = upper - lower + 1;
			return lower + (this.NextDouble() % diff);
		}

		#endregion Basic

		#region Sequence

		/// <inheritdoc/>
		public virtual T Choice<T>(T[] items)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), "The items is empty or null.");
			}

			if (items.Length > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(items), "The items length or size can't be greater than int.MaxValue or { int.MaxValue }.");
			}

			return items[(int)this.NextInt(0, (uint)(items.Length - 1))];
		}

		/// <inheritdoc/>
		public virtual T[] Choice<T>(T[] items, int select)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), "The items is empty or null.");
			}

			if (select < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(select), "The number of elements to be retrieved is negative or less than 1.");
			}
			else if (select > items.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(select), "The number of elements to be retrieved exceeds the items size.");
			}

			var selected = new List<T>();

			while (selected.Count < select)
			{
				var index = this.NextInt(0, (uint)(items.Length - 1));
				selected.Add(items[index]);
			}

			return selected.ToArray();
		}

		/// <inheritdoc/>
		public virtual T Choice<T>(ICollection<T> items)
		{
			return this.Choice(items.ToArray());
		}

		/// <inheritdoc/>
		public virtual T[] Choice<T>(ICollection<T> items, int select)
		{
			return this.Choice(items.ToArray(), select);
		}

		/// <inheritdoc/>
		public virtual T[] Sample<T>(T[] items, int k)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), "The items is empty or null.");
			}

			if (k <= 0)
			{
				throw new ArgumentException(nameof(k), "The number of elements to be retrieved is negative or less than 1.");
			}
			else if (k > items.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(k), "The number of elements to be retrieved exceeds the items size.");
			}

			if (k == items.Length)
			{
				return items;
			}

			T[] reservoir = new T[k];

			for (var i = 0; i < k; i++)
			{
				reservoir[i] = items[i];
			}

			for (var i = k; i < items.Length; i++)
			{
				var index = (int)this.NextInt(0, (uint)i);

				if (index < k)
				{
					reservoir[index] = items[i];
				}
			}

			return reservoir;
		}

		/// <inheritdoc/>
		public virtual T[] Sample<T>(ICollection<T> items, int k)
		{
			return this.Sample(items.ToArray(), k);
		}

		/// <inheritdoc/>
		public virtual async Task<T[]> SampleAsync<T>(T[] items, int k)
		{
			return await Task.Run(() =>
			{
				return Task.FromResult(this.Sample(items, k));
			});
		}

		/// <inheritdoc/>
		public virtual void Shuffle<T>(T[] items)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), "The items is empty or null.");
			}

			T temp;

			for (var i = items.Length - 1; i > 1; i--)
			{
				var index = this.NextInt(0, (uint)i);
				temp = items[i];
				items[i] = items[index];
				items[index] = temp;
			}
		}

		/// <inheritdoc/>
		public async Task ShuffleAsync<T>(T[] items)
		{
			await Task.Run(() =>
			{
				this.Shuffle(items);
			});
		}

		#endregion Sequence

		#region Distribution

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
			//TODO some prng algo infinite loop			
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

		#endregion Distribution
	}
}
