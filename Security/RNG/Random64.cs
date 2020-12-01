using System;
using System.Collections.Generic;

namespace Litdex.Security.RNG
{
	/// <summary>
	/// Base class for 64 bit RNG.
	/// </summary>
	public abstract class Random64 : Random
	{
		#region Protected Method

		/// <summary>
		/// Generate next random number.
		/// </summary>
		/// <returns>A 64-bit unsigned integer.</returns>
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
		public override byte[] NextBytes(int length)
		{
			if (length <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), $"The requested output size can't lower than 1.");
			}
#if NET5_0
			var chunk = new System.Span<byte>(new byte[8]);
			List<byte> output = new List<byte>(length);

			while (length >= 8)
			{
				System.Buffers.Binary.BinaryPrimitives.WriteUInt64LittleEndian(chunk, this.rng.NextLong());
				output.AddRange(chunk.ToArray());
				length -= 8;
			}

			if (length != 0)
			{
				System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(chunk, this.rng.NextInt());
				output.AddRange(chunk.Slice(0, length).ToArray());
			}

			return output.ToArray();
#else
			ulong sample = 0;
			var chunk = new byte[8];
			List<byte> output = new List<byte>(length);

			while (length >= 8)
			{
				sample = this.Next();

				chunk[0] = (byte)sample;
				chunk[1] = (byte)(sample >> 8);
				chunk[2] = (byte)(sample >> 16);
				chunk[3] = (byte)(sample >> 24);
				chunk[4] = (byte)(sample >> 32);
				chunk[5] = (byte)(sample >> 40);
				chunk[6] = (byte)(sample >> 48);
				chunk[7] = (byte)(sample >> 56);

				output.AddRange(chunk);

				length -= 8;
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
