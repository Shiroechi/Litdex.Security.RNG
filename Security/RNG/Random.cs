using System;
using System.Collections.Generic;
using System.Linq;

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
			// java conversion method
			return NextLong() * (1.0 / (1L << 53));
			//return (double)(NextLong() >> 11) * (1.0 / long.MaxValue);
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

		/// <inheritdoc/>
		public virtual T Choice<T>(T[] items)
		{
			if (items.Length > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(items), $"The items length or size can't be greater than int.MaxValue or { int.MaxValue }.");
			}

			return items[(int)this.NextInt(0, (uint)(items.Length - 1))];
		}

		/// <inheritdoc/>
		public virtual T[] Choice<T>(T[] items, int select)
		{
			if (select < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(select), $"The number of elements to be retrieved is negative or less than 1.");
			}

			if (select > items.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(select), $"The number of elements to be retrieved exceeds the items size.");
			}
			
			var selected = new List<T>();

			while (selected.Count < select)
			{
				var index = this.NextInt(0, (uint)(items.Length - 1));

				if (selected.Contains(items[index]) == false)
				{
					selected.Add(items[index]);
				}
			}

			return selected.ToArray();
		}

		/// <inheritdoc/>
		public virtual T Choice<T>(IList<T> items)
		{
			return this.Choice(items.ToArray());
		}

		/// <inheritdoc/>
		public virtual IList<T> Choice<T>(IList<T> items, int select)
		{
			return this.Choice(items.ToArray(), select);
		}

		#endregion Public Method
	}
}
