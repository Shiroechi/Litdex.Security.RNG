﻿namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		A Permuted Congruential Generator (PCG) that is composed of 
	///		a 64-bit Linear Congruential Generator (LCG) combined with 
	///		the XSH-RS (xorshift; random shift) output transformation
	///		to create 32-bit output.
	/// </summary>
	/// <remarks>
	///		Source: https://www.pcg-random.org/
	/// </remarks>
	public class PcgXshRs32 : PcgXshRr32
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="PcgXshRs32"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		///	</param>
		/// <param name="increment">
		///		Increment step.
		///	</param>
		public PcgXshRs32(ulong seed = 0, ulong increment = 0)
		{
			if (seed == 0)
			{
				this.Reseed();
			}
			else
			{
				this.SetSeed(seed, increment);
			}
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~PcgXshRs32()
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
			this._State0 = (oldseed * 6364136223846793005) + (this._Increment | 1);
			var rot = (int)(oldseed >> 61);
			return (uint)(oldseed ^ (oldseed >> 22)) >> (22 + rot);
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "PCG XSH-RS 32-bit";
		}

		#endregion Public Method
	}
}