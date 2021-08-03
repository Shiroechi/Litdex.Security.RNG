using System;
using System.Collections.Generic;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Base class for 32 bit RNG.
	/// </summary>
	public abstract class Random32 : Random
	{
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

		/// <inheritdoc/>
		public override bool NextBoolean()
		{
			return this.NextInt() >> 31 == 0;
		}

		/// <inheritdoc/>
		public override byte NextByte()
		{
			return (byte)this.Next();
		}

		/// <inheritdoc/>
		public override byte[] NextBytes(int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), "The requested output size can't lower than 1.");
			}

			var output = new List<byte>(length);
#if NET5_0_OR_GREATER
			var chunk = new System.Span<byte>(new byte[4]);

			while (length >= 4)
			{
				if (BitConverter.IsLittleEndian)
				{
					System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(chunk, this.Next());
				}
				else
				{
					System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(chunk, this.Next());
				}
				output.AddRange(chunk.ToArray());
				length -= 4;
			}

			if (length != 0)
			{
				if (BitConverter.IsLittleEndian)
				{
					System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(chunk, this.Next());
				}
				else
				{
					System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(chunk, this.Next());
				}
				output.AddRange(chunk.Slice(0, length).ToArray());
			}
//#elif NETSTANDARD2_0
#else
			uint sample = 0;
			var chunk = new byte[4];

			while (length >= 4)
			{
				sample = this.Next();

				if (BitConverter.IsLittleEndian)
				{
					chunk[0] = (byte)sample;
					chunk[1] = (byte)(sample >> 8);
					chunk[2] = (byte)(sample >> 16);
					chunk[3] = (byte)(sample >> 24);
				}
				else
				{
					chunk[3] = (byte)sample;
					chunk[2] = (byte)(sample >> 8);
					chunk[1] = (byte)(sample >> 16);
					chunk[0] = (byte)(sample >> 24);
				}

				output.AddRange(chunk);

				length -= 4;
			}

			if (length != 0)
			{
				sample = this.Next();

				for (var i = 0; i < length; i++)
				{
					if (BitConverter.IsLittleEndian)
					{
						output.Add((byte)sample);
						sample >>= 8;
					}
					else
					{
						output.Add((byte)(sample >> (24 - (i * 8))));
					}
				}
			}
#endif
			return output.ToArray();
		}

		/// <inheritdoc/>
		public override void Fill(byte[] bytes)
		{
			if (bytes.Length <= 0 || bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes), "Array length can't be lower than 1 or null.");
			}

			ulong sample = 0;
			var fill_idx = 0;
			var length = bytes.Length;

			while (length > 4)
			{
				sample = this.Next();

				if (BitConverter.IsLittleEndian)
				{
					bytes[fill_idx] = (byte)sample;
					bytes[fill_idx + 1] = (byte)(sample >> 8);
					bytes[fill_idx + 2] = (byte)(sample >> 16);
					bytes[fill_idx + 3] = (byte)(sample >> 24);
				}
				else
				{
					bytes[fill_idx + 3] = (byte)sample;
					bytes[fill_idx + 2] = (byte)(sample >> 8);
					bytes[fill_idx + 1] = (byte)(sample >> 16);
					bytes[fill_idx] = (byte)(sample >> 24);
				}

				length -= 4;
				fill_idx += 4;
			}

			if (length != 0)
			{
				sample = this.Next();

				for (var i = 0; i < length; i++)
				{
					if (BitConverter.IsLittleEndian)
					{
						bytes[fill_idx] = (byte)sample;
						sample >>= 8;
					}
					else
					{
						bytes[fill_idx] = (byte)(sample >> (24 - (i * 8)));
					}
					fill_idx++;
				}
			}
		}

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
