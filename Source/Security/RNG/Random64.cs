using System;
using System.Collections.Generic;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Base class for 64 bit RNG.
	/// </summary>
	public abstract class Random64 : Random
	{
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
			return (byte)this.Next();
		}

		/// <inheritdoc/>
		public override byte[] NextBytes(int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), "The requested output size can't lower than 1.");
			}

			ulong sample = 0;
			var output = new List<byte>(length);
			var chunk = new byte[8];

			while (length >= 8)
			{

#if NET5_0_OR_GREATER

				if (BitConverter.IsLittleEndian)
				{
					System.Buffers.Binary.BinaryPrimitives.WriteUInt64LittleEndian(chunk, this.Next());
				}
				else
				{
					System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(chunk, this.Next());
				}

#else

				sample = this.Next();

				if (BitConverter.IsLittleEndian)
				{
					chunk[0] = (byte)sample;
					chunk[1] = (byte)(sample >> 8);
					chunk[2] = (byte)(sample >> 16);
					chunk[3] = (byte)(sample >> 24);
					chunk[4] = (byte)(sample >> 32);
					chunk[5] = (byte)(sample >> 40);
					chunk[6] = (byte)(sample >> 48);
					chunk[7] = (byte)(sample >> 56);
				}
				else
				{
					chunk[7] = (byte)sample;
					chunk[6] = (byte)(sample >> 8);
					chunk[5] = (byte)(sample >> 16);
					chunk[4] = (byte)(sample >> 24);
					chunk[3] = (byte)(sample >> 32);
					chunk[2] = (byte)(sample >> 40);
					chunk[1] = (byte)(sample >> 48);
					chunk[0] = (byte)(sample >> 56);
				}
				
#endif

				output.AddRange(chunk);
				length -= 8;
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
						output.Add((byte)(sample >> (56 - (i * 8))));
					}
				}
			}
			
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

			while (length > 8)
			{
				sample = this.Next();

				if (BitConverter.IsLittleEndian)
				{
					bytes[fill_idx] = (byte)sample;
					bytes[fill_idx + 1] = (byte)(sample >> 8);
					bytes[fill_idx + 2] = (byte)(sample >> 16);
					bytes[fill_idx + 3] = (byte)(sample >> 24);
					bytes[fill_idx + 4] = (byte)(sample >> 32);
					bytes[fill_idx + 5] = (byte)(sample >> 40);
					bytes[fill_idx + 6] = (byte)(sample >> 48);
					bytes[fill_idx + 7] = (byte)(sample >> 56);
				}
				else
				{
					bytes[fill_idx + 7] = (byte)sample;
					bytes[fill_idx + 6] = (byte)(sample >> 8);
					bytes[fill_idx + 5] = (byte)(sample >> 16);
					bytes[fill_idx + 4] = (byte)(sample >> 24);
					bytes[fill_idx + 3] = (byte)(sample >> 32);
					bytes[fill_idx + 2] = (byte)(sample >> 40);
					bytes[fill_idx + 1] = (byte)(sample >> 48);
					bytes[fill_idx] = (byte)(sample >> 56);
				}

				length -= 8;
				fill_idx += 8;
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
						bytes[fill_idx] = (byte)(sample >> (56 - (i * 8)));
					}
					fill_idx++;
				}
			}
		}

		/// <inheritdoc/>
		public override uint NextInt()
		{
			return unchecked((uint)this.Next());
		}

		/// <inheritdoc/>
		public override ulong NextLong()
		{
			return this.Next();
		}

		#endregion Public Method
	}
}
