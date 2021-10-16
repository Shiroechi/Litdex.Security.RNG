using System;
using System.Security.Cryptography;

using Litdex.Utilities.Extension;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		(XOR/shift/rotate) all-purpose generators.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoshiro256plus.c
	/// </remarks>
	public class Xoshiro256Plus : Random64
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Xoshiro256Plus"/> object.
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
		public Xoshiro256Plus(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0)
		{
			this._State = new ulong[4];
			this.SetSeed(seed1, seed2, seed3, seed4);
		}

		/// <summary>
		///		Create an instance of <see cref="Xoshiro256Plus"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		Array of seed is null or empty.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Seed length lower than 4.
		/// </exception>
		public Xoshiro256Plus(ulong[] seed)
		{
			this._State = new ulong[4];
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~Xoshiro256Plus()
		{
			Array.Clear(this._State, 0, this._State.Length);
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

			this._State[3] = this._State[3].RotateLeft(45);

			return result;
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
				rng.GetNonZeroBytes(bytes);
#if NET5_0_OR_GREATER
				var span = bytes.AsSpan();
				this.SetSeed(
					seed1: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span),
					seed2: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(8)),
					seed3: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(16)),
					seed4: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(24)));
#else
				this.SetSeed(
					seed1: BitConverter.ToUInt64(bytes, 0),
					seed2: BitConverter.ToUInt64(bytes, 8),
					seed3: BitConverter.ToUInt64(bytes, 16),
					seed4: BitConverter.ToUInt64(bytes, 24));
#endif
			}
		}

		/// <summary>
		///		This is the jump function for the generator. It is equivalent
		///		to 2^128 calls to next(); it can be used to generate 2^128
		///		non-overlapping subsequences for parallel computations.
		/// </summary>
		public virtual void NextJump()
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
		public virtual void SetSeed(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0)
		{
			this._State[0] = seed1;
			this._State[1] = seed2;
			this._State[2] = seed3;
			this._State[3] = seed4;
		}

		#endregion Public
	}
}