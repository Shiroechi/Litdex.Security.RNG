using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		LFSR-based pseudorandom number generators.
	/// </summary>
	/// <remarks>
	///		Source: https://github.com/andanteyk/prng-seiran
	/// </remarks>
	public class Seiran : Random64
	{
		#region Constructor & Destructor
		
		/// <summary>
		///		Create an instance of <see cref="Seiran"/> object.
		/// </summary>
		public Seiran()
		{
			this._State = new ulong[2];
		}

		/// <summary>
		///		Create an instance of <see cref="Seiran"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed numbers.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Seed need 2 numbers.
		/// </exception>
		public Seiran(ulong[] seed) : this()
		{
			this.SetSeed(seed);
		}

		~Seiran()
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

			ulong result = this.RotateLeft((s0 + s1) * 9, 29) + s0;

			this._State[0] = s0 ^ this.RotateLeft(s1, 29);
			this._State[1] = s0 ^ s1 << 9;

			return result;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Seiran";
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
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		/// </param>
		public void SetSeed(ulong seed1, ulong seed2)
		{
			this._State[0] = seed1;
			this._State[1] = seed2;
		}

		private void Jump(ulong[] jumpPolynomial)
		{
			ulong t0 = 0, t1 = 0;

			for (var i = 0; i < 2; i++)
			{
				for (var b = 0; b < 64; b++)
				{
					if (((jumpPolynomial[i] >> b) & 1) != 0)
					{
						t0 ^= this._State[0];
						t1 ^= this._State[1];
					}
					this.Next();
				}
			}
			this._State[0] = t0;
			this._State[1] = t1;
		}

		/// <summary>
		///     Advance the internal state by 2^32 steps.
		/// </summary>
		/// <remarks>
		///     This method is equivalent to 2^32 <see cref="Next"/> calls.
		///     It can be executed in the same amount of time as 128 <see cref="Next"/> calls.
		/// </remarks>
		public void Jump32() => this.Jump(new ulong[] { 0x40165CBAE9CA6DEB, 0x688E6BFC19485AB1 });

		/// <summary>
		///     Advance the internal state by 2^64 steps.
		/// </summary>
		/// <remarks>
		///     This method is equivalent to 2^64 <see cref="Next"/> calls.
		///     It can be executed in the same amount of time as 128 <see cref="Next"/> calls.
		/// </remarks>
		public void Jump64() => this.Jump(new ulong[] { 0xF4DF34E424CA5C56, 0x2FE2DE5C2E12F601 });

		/// <summary>
		///     Advance the internal state by 2^96 steps.
		/// </summary>
		/// <remarks>
		///     This method is equivalent to 2^96 <see cref="Next"/> calls.
		///     It can be executed in the same amount of time as 128 <see cref="Next"/> calls.
		/// </remarks>
		public void Jump96() => this.Jump(new ulong[] { 0x185F4DF8B7634607, 0x95A98C7025F908B2 });

		/// <summary>
		///		Rewinds the internal state to the previous state.
		/// </summary>
		public void Previous()
		{
			ulong t1 = this.RotateLeft(this._State[0] ^ this._State[1], 64 - 29);
			t1 ^= t1 << 44 ^ (t1 & ~0xffffful) << 24;
			t1 ^= (t1 & (0x7ffful << 40)) << 4;
			t1 ^= (t1 & (0x7fful << 40)) << 8;
			t1 ^= (t1 & (0x7ul << 40)) << 16;
			t1 ^= (t1 & (0xffffful << 35)) >> 20;
			t1 ^= (t1 & (0x7ffful << 20)) >> 20;

			this._State[0] ^= this.RotateLeft(t1, 29);
			this._State[1] = t1;
		}

		#endregion Public Method
	}
}
