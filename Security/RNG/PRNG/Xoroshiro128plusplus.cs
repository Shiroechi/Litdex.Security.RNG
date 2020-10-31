﻿using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// http://prng.di.unimi.it/xoroshiro128plusplus.c
	/// </summary>
	public class Xoroshiro128plusplus : Random64
	{
		private ulong _State1, _State2;

		/// <summary>
		/// Constructor.
		/// </summary>
		public Xoroshiro128plusplus()
		{
			this.Reseed();
		}

		/// <summary>
		/// Constructor with defined seed.
		/// </summary>
		/// <param name="seed1"></param>
		/// <param name="seed2"></param>
		public Xoroshiro128plusplus(ulong seed1, ulong seed2)
		{
			this._State1 = seed1;
			this._State2 = seed2;
		}

		#region Protected Method

		/// <summary>
		/// Generate next random number.
		/// </summary>
		/// <returns></returns>
		protected override ulong Next()
		{
			var s0 = this._State1;
			var s1 = this._State2;
			var result = this.RotateLeft(s0 + s1, 17) + s0;

			s1 ^= s0;
			this._State1 = this.RotateLeft(s0, 49) ^ s1 ^ (s1 << 21); // a, b
			this._State2 = this.RotateLeft(s1, 28); // c

			return result;
		}

		protected ulong RotateLeft(ulong val, int shift)
		{
			return (val << shift) | (val >> (64 - shift));
		}

		#endregion Protected Method

		#region Public Method

		/// <summary>
		/// The name of the algorithm this generator implements.
		/// </summary>
		/// <returns></returns>
		public override string AlgorithmName()
		{
			return "Xoroshiro 128++";
		}

		/// <summary>
		/// Seed with RNGCryptoServiceProvider.
		/// </summary>
		public override void Reseed()
		{
			var bytes = new byte[8];
			using (var rng = new RNGCryptoServiceProvider())
			{
				rng.GetNonZeroBytes(bytes);
				this._State1 = BitConverter.ToUInt64(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._State2 = BitConverter.ToUInt64(bytes, 0);
			}
		}

		/// <summary>
		/// 2^64 calls to NextLong(), it can be used to generate 2^64
		/// non-overlapping subsequences for parallel computations.
		/// </summary>
		public void NextJump()
		{
			ulong[] JUMP = { 0x2bd7a6a6e99c2ddc, 0x0992ccaf6a6fca05 };
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
			Array.Clear(JUMP, 0, JUMP.Length);
		}

		#endregion Public Method
	}
}
