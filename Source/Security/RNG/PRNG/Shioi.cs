using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		LFSR-based pseudorandom number generators. It have interesting jump characteristics.
	/// </summary>
	/// <remarks>
	///		Source: https://github.com/andanteyk/prng-shioi
	/// </remarks>
	public class Shioi : Random64
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Shioi"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed numbers.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Seed need 2 numbers.
		/// </exception>
		public Shioi(ulong[] seed)
		{
			this._State = new ulong[2];
			this.SetSeed(seed);
		}

		~Shioi()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		///	<inheritdoc/>
		protected override ulong Next()
		{
			var s0 = this._State[0];
			var s1 = this._State[1];

			var result = this.RotateLeft(s0 * 0xD2B74407B1CE6E93, 29) + s1;

			// note: MUST use arithmetic right shift
			this._State[0] = s1;
			this._State[1] = (s0 << 2) ^ (s0 >> 19) ^ s1;

			return result;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Shioi";
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

		private void Jump(ulong[] jumppoly)
		{
			ulong[] t = { 0, 0 };

			for (var i = 0; i < 2; i++)
			{
				for (var b = 0; b < 64; b++)
				{
					if (((jumppoly[i] >> b) & 1) == 1)
					{
						t[0] ^= this._State[0];
						t[1] ^= this._State[1];
					}
					this.Next();
				}
			}

			this._State[0] = t[0];
			this._State[1] = t[1];
		}

		/// <summary>
		///     Advance the internal state by 2^32 steps.
		/// </summary>
		/// <remarks>
		///     This method is equivalent to 2^32 <see cref="Next"/> calls.
		///     It can be executed in the same amount of time as 128 <see cref="Next"/> calls.
		/// </remarks>
		public void Jump32()
		{
			this.Jump(new ulong[] { 0x8003A4B944F009D0, 0x7FFE925EEBD5615B });
		}

		/// <summary>
		///     Advance the internal state by 2^64 steps.
		/// </summary>
		/// <remarks>
		///     This method is equivalent to 2^64 <see cref="Next"/> calls.
		///     It can be executed in the same amount of time as 128 <see cref="Next"/> calls.
		/// </remarks>
		private void Jump64()
		{
			// It is equivalent to jump({ 0x3, 0 })
			var s0 = this._State[0];
			var s1 = this._State[1];

			this._State[0] = s0 ^ s1;
			this._State[1] = (s0 << 2) ^ (s0 >> 19);
		}

		/// <summary>
		///     Advance the internal state by 2^96 steps.
		/// </summary>
		/// <remarks>
		///     This method is equivalent to 2^96 <see cref="Next"/> calls.
		///     It can be executed in the same amount of time as 128 <see cref="Next"/> calls.
		/// </remarks>
		public void Jump96()
		{
			this.Jump(new ulong[] { 0x8003A4B944F009D1, 0x7FFE925EEBD5615B });
		}

		#endregion Public Method
	}
}
