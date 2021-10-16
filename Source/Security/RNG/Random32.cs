using System;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Base class for Random Number Generator that the internal state produces 32 bit output.
	/// </summary>
	public abstract class Random32 : Random
	{
		#region Member

		/// <summary>
		///		The internal state of RNG.
		/// </summary>
		protected uint[] _State;

		/// <summary>
		///		<see cref="int"/> and <see cref="uint"/> is 4 bytes.
		/// </summary>
		protected const byte _Size = 4;

		#endregion Member

		#region Protected Method

		/// <summary>
		///		Generate next random number.
		/// </summary>
		/// <returns>
		///		A 32-bit unsigned integer.
		///	</returns>
		protected abstract uint Next();

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Random32";
		}

		/// <summary>
		///		Set <see cref="RNG"/> internal state manually.
		/// </summary>
		/// <param name="seed">
		///		Number to generate the random numbers.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		Array of seed is null or empty.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Seed amount must same as the internal state amount.
		/// </exception>
		public virtual void SetSeed(params uint[] seed)
		{
			if (seed == null || seed.Length == 0)
			{
				throw new ArgumentNullException(nameof(seed), "Seed can't null or empty.");
			}

			if (seed.Length < this._State.Length)
			{
				throw new ArgumentException(nameof(seed), $"Seed need at least { this._State.Length } numbers.");
			}

			var length = seed.Length > this._State.Length ? this._State.Length : seed.Length;
			Array.Copy(seed, 0, this._State, 0, length);
		}

		/// <inheritdoc/>
		public override bool NextBoolean()
		{
			return this.NextInt() >> 31 == 0;
		}

		/// <inheritdoc/>
		public override byte NextByte()
		{
			return (byte)(this.Next() >> 24);
		}

		/// <inheritdoc/>
		public override byte[] NextBytes(int length)
		{
			if (BitConverter.IsLittleEndian)
			{
				return this.NextBytesLittleEndian(length);
			}
			else
			{
				return this.NextBytesBigEndian(length);
			}
		}

		/// <inheritdoc/>
		public override byte[] NextBytesLittleEndian(int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), "The requested output size can't lower than 1.");
			}

			var bytes = new byte[length];

#if NET5_0_OR_GREATER

			var span = new Span<byte>(bytes);

			this.FillLittleEndian(span);

#else

			this.FillLittleEndian(bytes);

#endif

			return bytes;
		}

		/// <inheritdoc/>
		public override byte[] NextBytesBigEndian(int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), "The requested output size can't lower than 1.");
			}

			var bytes = new byte[length];

#if NET5_0_OR_GREATER

			var span = new Span<byte>(bytes);

			this.FillBigEndian(span);

#else

			this.FillBigEndian(bytes);

#endif

			return bytes;
		}

		/// <inheritdoc/>
		public override void Fill(byte[] bytes)
		{
			if (BitConverter.IsLittleEndian)
			{
				this.FillLittleEndian(bytes);
			}
			else
			{
				this.FillBigEndian(bytes);
			}
		}

		/// <inheritdoc/>
		public override void FillLittleEndian(byte[] bytes)
		{

#if NET5_0_OR_GREATER

			var span = new Span<byte>(bytes);
			this.FillLittleEndian(span);

#else

			if (bytes.Length <= 0 || bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes), "Array length can't be lower than 1 or null.");
			}

			uint sample = 0;
			var idx = 0;
			var length = bytes.Length;

			while (length >= _Size)
			{
				sample = this.Next();

				bytes[idx] = (byte)sample;
				bytes[idx + 1] = (byte)(sample >> 8);
				bytes[idx + 2] = (byte)(sample >> 16);
				bytes[idx + 3] = (byte)(sample >> 24);

				length -= _Size;
				idx += _Size;
			}

			if (length != 0)
			{
				sample = this.Next();

				for (var i = 0; i < length; i++)
				{
					bytes[idx] = (byte)sample;
					sample >>= 8;
				}
			}
#endif
		}

		/// <inheritdoc/>
		public override void FillBigEndian(byte[] bytes)
		{

#if NET5_0_OR_GREATER

			var span = new Span<byte>(bytes);
			this.FillBigEndian(span);

#else

			if (bytes.Length <= 0 || bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes), "Array length can't be lower than 1 or null.");
			}

			uint sample = 0;
			var idx = 0;
			var length = bytes.Length;

			while (length >= _Size)
			{
				sample = this.Next();

				bytes[idx + 3] = (byte)sample;
				bytes[idx + 2] = (byte)(sample >> 8);
				bytes[idx + 1] = (byte)(sample >> 16);
				bytes[idx] = (byte)(sample >> 24);

				length -= _Size;
				idx += _Size;
			}

			if (length != 0)
			{
				sample = this.Next();

				for (var i = 0; i < length; i++)
				{
					bytes[idx] = (byte)(sample >> (24 - (i * 8)));
					idx++;
				}
			}
#endif
		}

#if NET5_0_OR_GREATER

		/// <inheritdoc/>
		public override void Fill(Span<byte> bytes)
		{
			if (BitConverter.IsLittleEndian)
			{
				this.FillLittleEndian(bytes);
			}
			else
			{
				this.FillBigEndian(bytes);
			}
		}

		/// <inheritdoc/>
		public override void FillLittleEndian(Span<byte> bytes)
		{
			if (bytes.Length <= 0 || bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes), "Array length can't be lower than 1 or null.");
			}

			while (bytes.Length >= _Size)
			{
				System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(bytes, this.Next());
				bytes = bytes.Slice(_Size);
			}

			if (bytes.Length != 0)
			{
				var chunk = new byte[_Size];
				System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(chunk, this.Next());

				for (var i = 0; i < bytes.Length; i++)
				{
					bytes[i] = chunk[i];
				}
			}
		}

		/// <inheritdoc/>
		public override void FillBigEndian(Span<byte> bytes)
		{
			if (bytes.Length <= 0 || bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes), "Array length can't be lower than 1 or null.");
			}

			while (bytes.Length >= _Size)
			{
				System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(bytes, this.Next());
				bytes = bytes.Slice(_Size);
			}

			if (bytes.Length != 0)
			{
				var chunk = new byte[_Size];
				System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(chunk, this.Next());

				for (var i = 0; i < bytes.Length; i++)
				{
					bytes[i] = chunk[i];
				}
			}
		}

#endif

		/// <inheritdoc/>
		public override uint NextInt()
		{
			return this.Next();
		}

		/// <inheritdoc/>
		public override ulong NextLong()
		{
			ulong high = this.NextInt();
			ulong low = this.NextInt();
			return high << 32 | low;
		}

		#endregion Public Method
	}
}
