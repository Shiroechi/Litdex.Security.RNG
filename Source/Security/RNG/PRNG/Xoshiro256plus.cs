using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		(XOR/shift/rotate) all-purpose generators.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoshiro256plus.c
	/// </remarks>
	public class Xoshiro256plus : Random64
	{
		#region Member

		protected ulong[] _State = new ulong[4];

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Xoshiro256plus"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		/// </param>
		/// <param name="seed3">
		///		Third RNG seed.
		/// </param>
		/// <param name="seed4">
		///		Fourth RNG seed.
		/// </param>
		public Xoshiro256plus(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0)
		{
			this.SetSeed(seed1, seed2, seed3, seed4);
		}

		/// <summary>
		///		Create an instance of <see cref="Xoshiro256plus"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public Xoshiro256plus(ulong[] seed)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~Xoshiro256plus()
		{
			this._State = null;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Xoshiro 256+";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[32];
				rng.GetBytes(bytes);
				this._State[0] = BitConverter.ToUInt64(bytes, 0);
				this._State[1] = BitConverter.ToUInt64(bytes, 8);
				this._State[2] = BitConverter.ToUInt64(bytes, 16);
				this._State[3] = BitConverter.ToUInt64(bytes, 24);
			}
		}

		/// <summary>
		///		This is the jump function for the generator. It is equivalent
		///		to 2^128 calls to next(); it can be used to generate 2^128
		///		non-overlapping subsequences for parallel computations.
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
					if ((JUMP[i] & ((1UL) << b)) != 0)
					{
						s0 ^= this._State[0];
						s1 ^= this._State[1];
						s2 ^= this._State[2];
						s3 ^= this._State[3];
					}
					this.Next();
				}
			}
			this._State[0] = s0;
			this._State[1] = s1;
			this._State[2] = s2;
			this._State[3] = s3;
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		/// </param>
		/// <param name="seed3">
		///		Third RNG seed.
		/// </param>
		/// <param name="seed4">
		///		Fourth RNG seed.
		/// </param>
		public void SetSeed(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0)
		{
			this._State[0] = seed1;
			this._State[1] = seed2;
			this._State[2] = seed3;
			this._State[3] = seed4;
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seed numbers.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		Array of seed is null or empty.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Seed length lower than 4.
		/// </exception>
		public void SetSeed(ulong[] seed)
		{
			if (seed == null || seed.Length == 0)
			{
				throw new ArgumentNullException(nameof(seed), "Seed can't null or empty.");
			}

			if (seed.Length < 4)
			{
				throw new ArgumentException(nameof(seed), "Seed numbers must have at least 4 numbers.");
			}

			this.SetSeed(seed[0], seed[1], seed[2], seed[3]);

			//for (var i = 0; i < 4; i++)
			//{
			//	this._State[i] = seed[i];
			//}
		}

		#endregion Public
	}
}