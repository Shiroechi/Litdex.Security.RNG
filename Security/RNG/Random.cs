using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Litdex.Security.RNG
{
	/// <summary>
	/// Base class of all random.
	/// </summary>
	public abstract class Random : IRNG
	{
		#region Public Method

		/// <inheritdoc/>
		public virtual string AlgorithmName()
		{
			return "Random";
		}

		/// <inheritdoc/>
		public abstract void Reseed();

		#region Basic

		/// <inheritdoc/>
		public abstract bool NextBoolean();

		/// <inheritdoc/>
		public virtual byte NextByte()
		{
			return this.NextBytes(1)[0];
		}

		/// <inheritdoc/>
		public virtual byte NextByte(byte lower, byte upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentOutOfRangeException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = (byte)(upper - lower + 1);
			return (byte)(lower + (this.NextByte() % diff));
		}

		/// <inheritdoc/>
		public abstract byte[] NextBytes(int length);

		/// <inheritdoc/>
		public abstract uint NextInt();

		/// <inheritdoc/>
		public virtual uint NextInt(uint lower, uint upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentOutOfRangeException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = upper - lower + 1;
			return lower + (this.NextInt() % diff);
		}

		/// <summary>
		/// Lemire algorithm to generate <see cref="uint"/> value between 
		/// lower bound and upper bound from generator.
		/// </summary>
		/// <remarks>
		/// https://lemire.me/blog/2016/06/27/a-fast-alternative-to-the-modulo-reduction/
		/// </remarks>
		/// <param name="lower">
		/// Lower bound or expected minimum value.
		/// </param>
		/// <param name="upper">
		/// Upper bound or ecpected maximum value.
		/// </param>
		/// <param name="unbias">
		/// Determine using division for reduce bias.
		/// </param>
		/// <returns>
		/// <see cref="uint"/> value between lower bound and upper bound.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Lower bound is greater than or equal to upper bound.
		/// </exception>
		public virtual uint NextIntFast(uint lower, uint upper, bool unbias = false)
		{
			if (lower >= upper)
			{
				throw new ArgumentOutOfRangeException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
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
				throw new ArgumentOutOfRangeException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
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
				throw new ArgumentOutOfRangeException(nameof(lower), "The lower bound must not be greater than or equal to the upper bound.");
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
				throw new ArgumentNullException(nameof(items), $"The items is empty or null.");
			}

			if (items.Length > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(items), $"The items length or size can't be greater than int.MaxValue or { int.MaxValue }.");
			}

			return items[(int)this.NextInt(0, (uint)(items.Length - 1))];
		}

		/// <inheritdoc/>
		public virtual T[] Choice<T>(T[] items, int select)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), $"The items is empty or null.");
			}

			if (select < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(select), $"The number of elements to be retrieved is negative or less than 1.");
			}
			else if (select > items.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(select), $"The number of elements to be retrieved exceeds the items size.");
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
		public virtual T Choice<T>(IList<T> items)
		{
			return this.Choice(items.ToArray());
		}

		/// <inheritdoc/>
		public virtual T[] Choice<T>(IList<T> items, int select)
		{
			return this.Choice(items.ToArray(), select);
		}

		/// <inheritdoc/>
		public virtual T[] Sample<T>(T[] items, int k)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), $"The items is empty or null.");
			}

			if (k <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(k), $"The number of elements to be retrieved is negative or less than 1.");
			}
			else if (k > items.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(k), $"The number of elements to be retrieved exceeds the items size.");
			}

			T[] reservoir = new T[k];

			for (var i = 0; i < k; i++)
			{
				reservoir[i] = items[i];
			}

			if (k == items.Length)
			{
				return reservoir;
			}

			for (var i = k; i < items.Length; i++)
			{
				int index = (int)this.NextInt(0, (uint)i);

				if (index < k)
				{
					reservoir[index] = items[i];
				}
			}

			return reservoir;
		}

		/// <inheritdoc/>
		public virtual Task<T[]> SampleAsync<T>(T[] items, int k)
		{
			return Task.FromResult(this.Sample(items, k));
		}

		/// <inheritdoc/>
		public virtual void Shuffle<T>(T[] items)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), $"The items is empty or null.");
			}

			T temp;

			for (var i = items.Length - 1; i > 1; i--)
			{
				var index = this.NextLong(0, (ulong)i);
				temp = items[i];
				items[i] = items[index];
				items[index] = temp;
			}
		}

		/// <inheritdoc/>
		public Task ShuffleAsync<T>(T[] items)
		{
			this.Shuffle(items);

			return Task.CompletedTask;
		}

		#endregion Sequence

		/// <inheritdoc/>
		public override string ToString()
		{
			return this.AlgorithmName();
		}

		#endregion Public Method
	}
}
