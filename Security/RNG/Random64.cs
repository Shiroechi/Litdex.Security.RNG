using System;
using System.Collections.Generic;

namespace Litdex.Security.RNG
{
	/// <summary>
	/// Base class for 64 bit RNG.
	/// </summary>
	public abstract class Random64 : IRNG
	{
		#region Constructor & Destructor

		public Random64()
		{

		}

		~Random64()
		{

		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <summary>
		/// Generate next random number.
		/// </summary>
		/// <returns>64 bit random integer.</returns>
		protected abstract ulong Next();

		#endregion Protected Method

		#region Public Method
		
		public virtual string AlgorithmName()
		{
			return "Random64";
		}

		public virtual void Reseed()
		{
			//do nothing.
		}

		public virtual bool NextBoolean()
		{
			return this.NextInt() % 2 == 0;
		}

		public virtual byte NextByte()
		{
			return this.NextBytes(1)[0];
		}

		public virtual byte[] NextBytes(int length)
		{
			ulong sample = 0;
			var data = new byte[length];

			for (var i = 0; i < length; i++)
			{
				if (i % 8 == 0)
				{
					sample = this.Next();
				}
				data[i - 1] = (byte)sample;
				sample >>= 8;
			}
			return data;
		}

		public virtual byte NextByte(byte lower, byte upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentException("The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = (byte)(upper - lower + 1);
			return (byte)(lower + (this.NextByte() % diff));
		}

		public virtual uint NextInt()
		{
			return (uint)this.Next();
		}

		public virtual uint NextInt(uint lower, uint upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentException("The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = upper - lower + 1;
			return lower + (this.NextInt() % diff);
		}

		public virtual ulong NextLong()
		{
			return this.Next();
		}

		public virtual ulong NextLong(ulong lower, ulong upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentException("The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = upper - lower + 1;
			return lower + (this.Next() % diff);
		}

		public virtual double NextDouble()
		{
			return NextLong() * (1.0 / (1L << 53)); //java conversion method
			//return (double)(NextLong() >> 11) * (1.0 / long.MaxValue);
		}

		public virtual double NextDouble(double lower, double upper)
		{
			if (lower >= upper)
			{
				throw new ArgumentException("The lower bound must not be greater than or equal to the upper bound.");
			}

			var diff = upper - lower + 1;
			return lower + (this.NextDouble() % diff);
		}

		/// <summary>
		/// Select one element randomly.
		/// </summary>
		/// <typeparam name="T">Data type</typeparam>
		/// <param name="items">Set of items to choose.</param>
		/// <returns></returns>
		public virtual T Choice<T>(T[] items)
		{
			return items[(int)this.NextInt(0, (uint)(items.Length - 1))];
		}

		/// <summary>
		/// Select abritary element randomly.
		/// </summary>
		/// <typeparam name="T">Data type</typeparam>
		/// <param name="items">Set of items to choose.</param>
		/// <param name="count">The desired amount to select.</param>
		/// <returns></returns>
		public virtual T[] Choice<T>(T[] items, int count)
		{
			var temp = new List<T>(items);
			return this.Choice<T>(temp, count).ToArray();
		}

		/// <summary>
		/// Select one element randomly.
		/// </summary>
		/// <remarks>
		/// More slower because need boxing/unboxing.
		/// </remarks>
		/// <param name="items">Set of items to choose.</param>
		/// <returns></returns>
		public virtual object Choice(object[] items)
		{
			return items[(int)this.NextInt(0, (uint)(items.Length - 1))];
		}

		/// <summary>
		/// Select abritary element randomly.
		/// </summary>
		/// <remarks>
		/// More slower because need boxing/unboxing.
		/// </remarks>
		/// <typeparam name="T">Data type</typeparam>
		/// <param name="items">Set of items to choose.</param>
		/// <param name="count">The desired amount to select.</param>
		/// <returns></returns>
		/// <returns></returns>
		public virtual object[] Choice(object[] items, int count)
		{
			var temp = new List<object>(items);
			return this.Choice(temp, count).ToArray();
		}

		/// <summary>
		/// Select one element randomly.
		/// </summary>
		/// <typeparam name="T">Data type</typeparam>
		/// <param name="items">Set of items to choose.</param>
		/// <returns></returns>
		public virtual T Choice<T>(List<T> items)
		{
			return items[(int)this.NextInt(0, (uint)(items.Count - 1))];
		}

		/// <summary>
		/// Select abritary element randomly.
		/// </summary>
		/// <typeparam name="T">Data type</typeparam>
		/// <param name="items">Set of items to choose.</param>
		/// <param name="count">The desired amount to select.</param>
		/// <returns></returns>
		public virtual List<T> Choice<T>(List<T> items, int count)
		{
			if (count > items.Count)
			{
				throw new Exception("Count can't greater than items length(" + items.Count + ")");
			}
			else if (count < 1)
			{
				throw new Exception("Count can't lower than 1.");
			}

			var selected = new List<T>();

			int index = (int)this.NextInt(0, (uint)(items.Count - 1));

			while (selected.Count < count)
			{
				if (selected.Contains(items[index]))
				{
					index = (int)this.NextInt(0, (uint)(items.Count - 1));
					continue;
				}

				selected.Add(items[index]);
			}

			return selected;
		}

		/// <summary>
		/// Select one element randomly.
		/// </summary>
		/// <remarks>
		/// More slower because need boxing/unboxing.
		/// </remarks>
		/// <param name="items">Set of items to choose.</param>
		/// <returns></returns>
		public virtual object Choice(List<object> items)
		{
			return items[(int)this.NextInt(0, (uint)(items.Count - 1))];
		}

		/// <summary>
		/// Select abritary element randomly.
		/// </summary>
		/// <remarks>
		/// More slower because need boxing/unboxing.
		/// </remarks>
		/// <typeparam name="T">Data type</typeparam>
		/// <param name="items">Set of items to choose.</param>
		/// <param name="count">The desired amount to select.</param>
		/// <returns></returns>
		/// <returns></returns>
		public virtual List<object> Choice(List<object> items, int count)
		{
			if (count > items.Count)
			{
				throw new Exception("Count can't greater than items length(" + items.Count + ")");
			}
			else if (count < 1)
			{
				throw new Exception("Count can't lower than 1.");
			}

			var selected = new List<object>();

			var index = (int)this.NextInt(0, (uint)items.Count - 1);

			while (selected.Count < count)
			{
				if (selected.Contains(items[index]))
				{
					index = (int)this.NextInt(0, (uint)(items.Count - 1));
					continue;
				}

				selected.Add(items[index]);
			}

			return selected;
		}

		#endregion Public Method

	}
}
