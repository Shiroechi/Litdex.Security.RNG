using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		A Permuted Congruential Generator (PCG) that is composed of 
	///		a 64-bit Linear Congruential Generator (LCG) combined with 
	///		the XSH-RR (xorshift; random rotate) output transformation 
	///		to create 32-bit output.
	/// </summary>
	/// <remarks>
	///		Source: https://www.pcg-random.org/
	/// </remarks>
	public class PcgXshRr32 : Random32
	{
		#region Member

		protected ulong _State0;
		protected ulong _Increment;
		protected const ulong _Multiplier = 6364136223846793005;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="PcgXshRr32"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		///	</param>
		/// <param name="increment">
		///		Increment step.
		///	</param>
		public PcgXshRr32(ulong seed = 0, ulong increment = 0)
		{
			this.SetSeed(seed, increment);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~PcgXshRr32()
		{
			this._State0 = 0;
			this._Increment = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			var oldseed = this._State0;
			this._State0 = (oldseed * _Multiplier) + (this._Increment | 1);
			var xorshifted = (uint)(((oldseed >> 18) ^ oldseed) >> 27);
			var rot = (uint)(oldseed >> 59);
			return (xorshifted >> (int)rot) | (xorshifted << (int)((-rot) & 31));
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "PCG XSH-RR 32-bit";
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
			this._State0 = (seed + increment) * _Multiplier + increment;
			this._Increment = increment;
		}

		#endregion Public Method
	}
}