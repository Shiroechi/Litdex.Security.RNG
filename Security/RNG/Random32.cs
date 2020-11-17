using System;

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
			uint sample = 0;
			var data = new byte[length];

			for (var i = 0; i < length; i++)
			{
				if (i % 4 == 0)
				{
					sample = this.Next();
				}
				data[i] = (byte)sample;
				sample >>= 8;
			}
			return data;
		}

		/// <inheritdoc/>
		public override uint NextInt()
		{
			return this.Next();
		}

		/// <inheritdoc/>
		public override ulong NextLong()
		{
			return BitConverter.ToUInt64(this.NextBytes(8), 0);
		}

		#endregion Public Method
	}
}
