using System;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Base class for 64 bit RNG.
	/// </summary>
	public abstract class Random64 : Random
	{
		#region Member

		/// <summary>
		///		<see cref="long"/> and <see cref="ulong"/> is 8 bytes.
		/// </summary>
		protected const byte _Size = 8;

		#endregion Member

		#region Protected Method

		/// <summary>
		///		Generate next random number.
		/// </summary>
		/// <returns>
		///		A 64-bit unsigned integer.
		///	</returns>
		protected abstract ulong Next();

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Random64";
		}

		/// <inheritdoc/>
		public override bool NextBoolean()
		{
			return this.Next() >> 63 == 0;
		}

		/// <inheritdoc/>
		public override byte NextByte()
		{
			return (byte)(this.Next() >> 56);
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

#if NET5_0_OR_GREATER

			var bytes = new byte[length];
			var span = new Span<byte>(bytes);

			this.FillLittleEndian(span);

			return bytes;
#else

			var bytes = new byte[length];

			this.FillLittleEndian(bytes);

			return bytes;
#endif
		}

		/// <inheritdoc/>
		public override byte[] NextBytesBigEndian(int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), "The requested output size can't lower than 1.");
			}


#if NET5_0_OR_GREATER

			var bytes = new byte[length];
			var span = new Span<byte>(bytes);

			this.FillBigEndian(span);

			return bytes;
#else

			var bytes = new byte[length];

			this.FillBigEndian(bytes);

			return bytes;
#endif
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

			ulong sample = 0;
			var idx = 0;
			var length = bytes.Length;

			while (length >= _Size)
			{
				sample = this.Next();

				bytes[idx] = (byte)sample;
				bytes[idx + 1] = (byte)(sample >> 8);
				bytes[idx + 2] = (byte)(sample >> 16);
				bytes[idx + 3] = (byte)(sample >> 24);
				bytes[idx + 4] = (byte)(sample >> 32);
				bytes[idx + 5] = (byte)(sample >> 40);
				bytes[idx + 6] = (byte)(sample >> 48);
				bytes[idx + 7] = (byte)(sample >> 56);

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
					idx++;
				}
			}
#endif
		}

		/// <inheritdoc/>
		public override void FillBigEndian(byte[] bytes)
		{

#if NET5_0_OR_GREATER

			var span = new Span<byte>(bytes);
			this.FillLittleEndian(span);

#else

			if (bytes.Length <= 0 || bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes), "Array length can't be lower than 1 or null.");
			}

			ulong sample = 0;
			var idx = 0;
			var length = bytes.Length;

			while (length >= _Size)
			{
				sample = this.Next();

				bytes[idx + 7] = (byte)sample;
				bytes[idx + 6] = (byte)(sample >> 8);
				bytes[idx + 5] = (byte)(sample >> 16);
				bytes[idx + 4] = (byte)(sample >> 24);
				bytes[idx + 3] = (byte)(sample >> 32);
				bytes[idx + 2] = (byte)(sample >> 40);
				bytes[idx + 1] = (byte)(sample >> 48);
				bytes[idx] = (byte)(sample >> 56);

				length -= _Size;
				idx += _Size;
			}

			if (length != 0)
			{
				sample = this.Next();

				for (var i = 0; i < length; i++)
				{
					bytes[idx] = (byte)(sample >> (56 - (i * 8)));
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
				System.Buffers.Binary.BinaryPrimitives.WriteUInt64LittleEndian(bytes, this.Next());
				bytes = bytes.Slice(_Size);
			}

			if (bytes.Length != 0)
			{
				var chunk = new byte[_Size];
				System.Buffers.Binary.BinaryPrimitives.WriteUInt64LittleEndian(chunk, this.Next());

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
				System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(bytes, this.Next());
				bytes = bytes.Slice(_Size);
			}

			if (bytes.Length != 0)
			{
				var chunk = new byte[_Size];
				System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(bytes, this.Next());

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
			return (uint)(this.Next() >> 32);
		}

		/// <inheritdoc/>
		public override ulong NextLong()
		{
			return this.Next();
		}

		#endregion Public Method
	}
}
