using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Implemetation of Permuted Congruential Generator by O'Neill.
	/// </summary>
	/// <remarks>
	///		Source: http://www.pcg-random.org/download.html
	/// </remarks>
	public class PCG32 : Random64
	{
		#region Member

		private ulong _Seed; //state in original code
		private ulong _Increment;

		#endregion Member

		/// <summary>
		///		Create an instance of <see cref="PCG32"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		///	</param>
		/// <param name="increment">
		///		Increment step.
		///	</param>
		public PCG32(ulong seed = 0, ulong increment = 0)
		{
			this.SetSeed(seed, increment);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~PCG32()
		{
			this._Seed = 0;
			this._Increment = 0;
		}

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var oldseed = this._Seed;
			this._Seed = (oldseed * 6364136223846793005) + (this._Increment | 1);
			var xorshifted = (uint)((oldseed >> 18) ^ oldseed) >> 27;
			var rot = (uint)(oldseed >> 59);
			return (xorshifted >> (int)rot) | (xorshifted << (int)((-rot) & 31));
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "PCG32";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			ulong seed, increment;

			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[16];
				rng.GetBytes(bytes);
				seed = BitConverter.ToUInt64(bytes, 0);
				increment = BitConverter.ToUInt64(bytes, 8);
			}

			this.SetSeed(seed, increment);
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
			this._Seed = seed + increment;
			this._Increment = increment;
			this.Next();
		}

		#endregion Public Method
	}
}