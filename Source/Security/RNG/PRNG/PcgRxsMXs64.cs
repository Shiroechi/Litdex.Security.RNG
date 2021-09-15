using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		A Permuted Congruential Generator (PCG) that is composed of a 64-bit 
	///		Linear Congruential Generator(LCG) combined with the RXS-M-XS (random xorshift; multiply; xorshift) output
	///		transformation to create 64-bit output.
	/// </summary>
	/// <remarks>
	///		https://www.pcg-random.org/
	/// </remarks>
	public class PcgRxsMXs64 : Random64
	{
		#region Member

		protected ulong _Increment;
		protected const ulong _Multiplier = 6364136223846793005;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="PcgRxsMXs64"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		///	</param>
		/// <param name="increment">
		///		Increment step.
		///	</param>
		public PcgRxsMXs64(ulong seed = 0, ulong increment = 0)
		{
			this._State = new ulong[1];
			this.SetSeed(seed, increment);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~PcgRxsMXs64()
		{
			Array.Clear(this._State, 0, this._State.Length);
			this._Increment = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var oldseed = this._State[0];
			this._State[0] = (oldseed * _Multiplier) + (this._Increment | 1);
			ulong word = ((oldseed >> ((int)(oldseed >> 59) + 5)) ^ oldseed) * 12605985483714917081;
			return (word >> 43) ^ word;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "PCG RXS-M-XS 64-bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[16];
				rng.GetNonZeroBytes(bytes);
#if NET5_0_OR_GREATER
				var span = bytes.AsSpan();
				this.SetSeed(
					seed: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span),
					increment: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(8)));
#else
				this.SetSeed(
					seed: BitConverter.ToUInt64(bytes, 0),
					increment: BitConverter.ToUInt64(bytes, 8));
#endif
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		/// <param name="increment">
		///		Increment step.
		/// </param>
		public void SetSeed(ulong seed, ulong increment)
		{
			this._State[0] = (seed + increment) * _Multiplier + increment;
			this._Increment = increment;
		}

		#endregion Public Method
	}
}
