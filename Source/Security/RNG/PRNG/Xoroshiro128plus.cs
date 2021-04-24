using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Xoroshiro128plus PRNG is improved from Xoroshift128.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoroshiro128plus.c
	/// </remarks>
	public class Xoroshiro128plus : Random64
	{
		#region Member

		protected ulong _State1, _State2;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Xoroshiro128plus"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		///	</param>
		public Xoroshiro128plus(ulong seed1 = 0, ulong seed2 = 0)
		{
			this.SetSeed(seed1, seed2);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var s0 = this._State1;
			var s1 = this._State2;
			var result = this._State1 + this._State2;

			s1 ^= s0;
			this._State1 = this.RotateLeft(s0, 24) ^ s1 ^ (s1 << 16); // a, b
			this._State2 = this.RotateLeft(s1, 37); // c

			return result;
		}

		protected ulong RotateLeft(ulong val, int shift)
		{
			return (val << shift) | (val >> (64 - shift));
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Xoroshiro 128+";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[16];
				rng.GetBytes(bytes);
				this._State1 = BitConverter.ToUInt64(bytes, 0);
				this._State2 = BitConverter.ToUInt64(bytes, 8);
			}
		}

		/// <summary>
		///		2^64 calls to NextLong(), it can be used to generate 2^64
		///		non-overlapping subsequences for parallel computations.
		/// </summary>
		public void NextJump()
		{
			ulong[] JUMP = { 0xDF900294D8F554A5, 0x170865DF4B3201FC };
			ulong seed1 = 0, seed2 = 0;

			for (var i = 0; i < 2; i++)
			{
				for (var b = 0; b < 64; b++)
				{
					if ((JUMP[i] & (1UL << b)) != 0)
					{
						seed1 ^= JUMP[0];
						seed2 ^= JUMP[1];
					}
					this.NextLong();
				}
			}

			this._State1 = seed1;
			this._State2 = seed2;
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		///	</param>
		public void SetSeed(ulong seed1, ulong seed2)
		{
			this._State1 = seed1;
			this._State2 = seed2;
		}

		#endregion Public
	}
}