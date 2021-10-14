using System;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Base class of all random.
	/// </summary>
	public abstract partial class Random : IRNG
	{
		#region Member

		/// <summary>
		///		Amount of roll after the state is initialized or seeded.
		/// </summary>
		protected const byte _InitialRoll = 20;

		#endregion Member

		#region Protected Method

		/// <summary>
		///		Do multiplication of 2 <see cref="ulong"/>.
		/// </summary>
		/// <param name="x">
		///		Number to multiply.
		/// </param>
		/// <param name="y">
		///		Number to multiply.
		/// </param>
		/// <returns>
		///		128-bit number, split into 2 <see cref="ulong"/>.
		///		The first one is the high bit, the second one is low bit. 
		/// </returns>
		protected (ulong, ulong) Multiply128(ulong x, ulong y)
		{
			ulong hi, lo;

			lo = x * y;

			ulong x0 = (uint)x;
			var x1 = x >> 32;

			ulong y0 = (uint)y;
			var y1 = y >> 32;

			var p11 = x1 * y1;
			var p10 = x1 * y0;
			var p01 = x0 * y1;
			var p00 = x0 * y0;

			// 64-bit product + two 32-bit values
			var middle = p10 + (p00 >> 32) + (uint)p01;

			// 64-bit product + two 32-bit values
			hi = p11 + (middle >> 32) + (p01 >> 32);

			return (hi, lo);
		}

		#endregion Protected Method

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

		#region IRNG Method

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

			return (byte)this.NextInt(lower, upper);
		}

		/// <inheritdoc/>
		public abstract byte[] NextBytes(int length);

		/// <summary>
		///		Generate array of random bytes from generator.
		/// </summary>
		/// <remarks>
		///		<see cref="byte"/> order in Little Endian.
		/// </remarks>
		/// <param name="length">
		///		Requested output length.
		/// </param>
		/// <returns>
		///		Array of bytes.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The requested output size can't lower than 1.
		/// </exception>
		public abstract byte[] NextBytesLittleEndian(int length);

		/// <summary>
		///		Generate array of random bytes from generator.
		/// </summary>
		/// <remarks>
		///		<see cref="byte"/> order in Big Endian.
		/// </remarks>
		/// <param name="length">
		///		Requested output length.
		/// </param>
		/// <returns>
		///		Array of bytes.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		The requested output size can't lower than 1.
		/// </exception>
		public abstract byte[] NextBytesBigEndian(int length);

		/// <inheritdoc/>
		public abstract void Fill(byte[] bytes);

		/// <summary>
		///		Fill the array with random bytes.
		/// </summary>
		/// <remarks>
		///		<see cref="byte"/> order in Little Endian.
		/// </remarks>
		/// <param name="bytes">
		///		Array to fill with random bytes.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		Array length can't be lower than 1 or null.
		/// </exception>
		public abstract void FillLittleEndian(byte[] bytes);

		/// <summary>
		///		Fill the array with random bytes.
		/// </summary>
		/// <remarks>
		///		<see cref="byte"/> order in Big Endian.
		/// </remarks>
		/// <param name="bytes">
		///		Array to fill with random bytes.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		Array length can't be lower than 1 or null.
		/// </exception>
		public abstract void FillBigEndian(byte[] bytes);

#if NET5_0_OR_GREATER

		/// <inheritdoc/>
		public abstract void Fill(Span<byte> bytes);

		/// <summary>
		///		Fill the array with random bytes.
		/// </summary>
		/// <remarks>
		///		<see cref="byte"/> order in Little Endian.
		/// </remarks>
		/// <param name="bytes">
		///		Array to fill with random bytes.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		Array length can't be lower than 1 or null.
		/// </exception>
		public abstract void FillLittleEndian(Span<byte> bytes);

		/// <summary>
		///		Fill the array with random bytes.
		/// </summary>
		/// <remarks>
		///		<see cref="byte"/> order in Big Endian.
		/// </remarks>
		/// <param name="bytes">
		///		Array to fill with random bytes.
		///	</param>
		/// <exception cref="ArgumentNullException">
		///		Array length can't be lower than 1 or null.
		/// </exception>
		public abstract void FillBigEndian(Span<byte> bytes);

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

			// using unbiased lemire method
			// from https://www.pcg-random.org/posts/bounded-rands.html

			var range = upper - lower;
			uint x = this.NextInt();
			ulong m = (ulong)x * range;
			uint l = (uint)m;
			if (l < range)
			{
				uint t = uint.MaxValue - range;
				if (t >= range)
				{
					t -= range;
					if (t >= range)
					{
						t %= range;
					}
				}
				while (l < t)
				{
					x = this.NextInt();
					m = (ulong)x * range;
					l = (uint)m;
				}
			}
			return (uint)(m >> 32) + lower;
			//var diff = upper - lower + 1;
			//return lower + (this.NextInt() % diff);
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

			// using unbiased lemire method
			// from https://www.pcg-random.org/posts/bounded-rands.html

			var range = upper - lower;
			ulong x = this.NextLong();
			var (m, l) = this.Multiply128(x, range);
			if (l < range)
			{
				ulong t = ulong.MaxValue - range;
				if (t >= range)
				{
					t -= range;
					if (t >= range)
					{
						t %= range;
					}
				}
				while (l < t)
				{
					x = this.NextLong();
					(m, l) = this.Multiply128(x, range);
				}
			}
			return m;

			//var diff = upper - lower + 1;
			//return lower + (this.NextLong() % diff);
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

		#endregion IRNG Method
	}
}
