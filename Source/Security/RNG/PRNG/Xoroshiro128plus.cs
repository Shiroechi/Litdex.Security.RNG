using System;
using System.Security.Cryptography;

using Litdex.Utilities.Extension;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Xoroshiro128+ PRNG is improved from Xoroshift128.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoroshiro128plus.c
	/// </remarks>
	public class Xoroshiro128Plus : Random64
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Xoroshiro128Plus"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		///	</param>
		public Xoroshiro128Plus(ulong seed1 = 0, ulong seed2 = 0)
		{
			this._State = new ulong[2];
			this.SetSeed(seed1, seed2);
		}

		~Xoroshiro128Plus()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var s0 = this._State[0];
			var s1 = this._State[1];
			var result = this._State[0] + this._State[1];

			s1 ^= s0;
			this._State[0] = s0.RotateLeft(24) ^ s1 ^ (s1 << 16); // a, b
			this._State[1] = s1.RotateLeft(37); // c

			return result;
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
				rng.GetNonZeroBytes(bytes);
#if NET5_0_OR_GREATER
				var span = bytes.AsSpan();
				this.SetSeed(
					seed1: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span),
					seed2: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(8)));
#else
				this.SetSeed(
					seed1: BitConverter.ToUInt64(bytes, 0),
					seed2: BitConverter.ToUInt64(bytes, 8));
#endif
			}
		}

		/// <summary>
		///		2^64 calls to NextLong(), it can be used to generate 2^64
		///		non-overlapping subsequences for parallel computations.
		/// </summary>
		public virtual void NextJump()
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

			this._State[0] = seed1;
			this._State[1] = seed2;
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
		public virtual void SetSeed(ulong seed1, ulong seed2)
		{
			this._State[0] = seed1;
			this._State[1] = seed2;
		}

		#endregion Public
	}
}