using System;
using System.Collections.Generic;

#if NET5_0
using System.Buffers.Binary;
#endif

namespace Litdex.Security.RNG
{
	/// <summary>
	/// Base class for 32 bit RNG.
	/// </summary>
	public abstract class Random32 : Random
	{
		#region Protected Method

		/// <summary>
		/// Generate next random number.
		/// </summary>
		/// <returns>A 32-bit unsigned integer.</returns>
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
		public override byte[] NextBytes(int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), $"The requested output size can't lower than 1.");
			}

#if NET5_0
			var chunk = new Span<byte>(new byte[4]);
			List<byte> output = new List<byte>(length);

			while (length >= 4)
			{
				BinaryPrimitives.WriteUInt32LittleEndian(chunk, this.Next());
				output.AddRange(chunk.ToArray());
				length -= 4;
			}

			if (data != 0)
			{
				BinaryPrimitives.WriteUInt32LittleEndian(chunk, this.Next());
				output.AddRange(chunk.Slice(0, length).ToArray());
			}

			return output.ToArray();
#else
			uint sample = 0;
			var chunk = new byte[4];
			List<byte> output = new List<byte>(length);

			while (length >= 4)
			{
				sample = this.Next();

				chunk[0] = (byte)sample;
				chunk[1] = (byte)(sample >> 8);
				chunk[2] = (byte)(sample >> 16);
				chunk[3] = (byte)(sample >> 24);
				
				output.AddRange(chunk);
				
				length -= 4;
			}

			if (length != 0)
			{
				sample = this.Next();

				for (int i = 0; i < length; i++)
				{
					output.Add((byte)sample);
					sample >>= 8;
				}
			}

			return output.ToArray();
#endif
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
