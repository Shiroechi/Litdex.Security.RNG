﻿using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// RNG from xoroshiro family.
	/// http://vigna.di.unimi.it/xorshift/xoshiro256plus.c
	/// </summary>
	public class Xoshiro256plus : Random64
    {
        private ulong[] _State = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		public Xoshiro256plus()
        {
			this._State = new ulong[4];
			this.Reseed();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Xoshiro256plus(ulong[] seed)
        {
			this._State = new ulong[4];
			if (seed.Length < 4)
            {
				throw new ArgumentException("The generator need 4 seed, your seed " + seed.Length);
            }

			for (var i = 0; i < 4; i++)
			{
				this._State[i] = seed[i];
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~Xoshiro256plus()
		{
			this._State = null;
		}

		#region Protected Method

		/// <summary>
		/// Generate next random number.
		/// </summary>
		/// <returns></returns>
		protected override ulong Next()
		{
			var result = this._State[0] + this._State[3];

			var t = this._State[1] << 17;

			this._State[2] ^= this._State[0];
			this._State[3] ^= this._State[1];
			this._State[1] ^= this._State[2];
			this._State[0] ^= this._State[3];

			this._State[2] ^= t;

			this._State[3] = this.RotateLeft(this._State[3], 45);

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
			return "Xoshiro 256+";
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
				this._State[0] = BitConverter.ToUInt64(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._State[1] = BitConverter.ToUInt64(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._State[2] = BitConverter.ToUInt64(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._State[3] = BitConverter.ToUInt64(bytes, 0);
			}
		}

		/// <summary>
		/// This is the jump function for the generator. It is equivalent
		/// to 2^128 calls to next(); it can be used to generate 2^128
		/// non-overlapping subsequences for parallel computations.
		/// </summary>
		public void NextJump()
        {
            ulong[] JUMP = { 0x180ec6d33cfd0aba, 0xd5a61266f0c9392c, 0xa9582618e03fc9aa, 0x39abdc4529b1661c };

            var s0 = 0UL;
            var s1 = 0UL;
            var s2 = 0UL;
            var s3 = 0UL;

            for (var i = 0; i < 4; i++)
            {
                for (var b = 0; b < 64; b++)
                {
                    if ((JUMP[i] & ((1UL) << b)) != 0 )
                    {
                        s0 ^= this._State[0];
                        s1 ^= this._State[1];
                        s2 ^= this._State[2];
                        s3 ^= this._State[3];
                    }
                    this.NextLong();
                }
            }
			this._State[0] = s0;
			this._State[1] = s1;
			this._State[2] = s2;
			this._State[3] = s3;
        }

		#endregion Public
	}
}