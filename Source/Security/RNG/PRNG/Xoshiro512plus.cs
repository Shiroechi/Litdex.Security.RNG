using System;
using System.Security.Cryptography;

using Litdex.Utilities.Extension;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		(XOR/shift/rotate) all-purpose generators.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoshiro512plus.c
	/// </remarks>
	public class Xoshiro512Plus : Random64
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Xoshiro512Plus"/> object.
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
		/// <param name="seed5">
		///		Fifth RNG seed.
		/// </param>
		/// <param name="seed6">
		///		Sixth RNG seed.
		/// </param>
		/// <param name="seed7">
		///		Seventh RNG seed.
		/// </param>
		/// <param name="seed8">
		///		Eighth RNG seed.
		/// </param>
		public Xoshiro512Plus(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0, ulong seed5 = 0, ulong seed6 = 0, ulong seed7 = 0, ulong seed8 = 0)
		{
			this._State = new ulong[8];
			this.SetSeed(seed1, seed2, seed3, seed4, seed5, seed6, seed7, seed8);
		}

		/// <summary>
		///		Create an instance of <see cref="Xoshiro512Plus"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public Xoshiro512Plus(ulong[] seed)
		{
			this._State = new ulong[8];
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor
		/// </summary>
		~Xoshiro512Plus()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var result = this._State[0] + this._State[2];

			var t = this._State[1] << 11;

			this._State[2] ^= this._State[0];
			this._State[5] ^= this._State[1];
			this._State[1] ^= this._State[2];
			this._State[7] ^= this._State[3];
			this._State[3] ^= this._State[4];
			this._State[4] ^= this._State[5];
			this._State[0] ^= this._State[6];
			this._State[6] ^= this._State[7];

			this._State[6] ^= t;

			this._State[7] = this._State[7].RotateLeft(21);

			return result;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Xoshiro 512+";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[64];
				rng.GetNonZeroBytes(bytes);
#if NET5_0_OR_GREATER
				var span = bytes.AsSpan();
				this.SetSeed(
					seed1: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span),
					seed2: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(8)),
					seed3: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(16)),
					seed4: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(24)),
					seed5: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(32)),
					seed6: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(40)),
					seed7: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(48)),
					seed8: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span.Slice(56)));
#else
				this.SetSeed(
					seed1: BitConverter.ToUInt64(bytes, 0),
					seed2: BitConverter.ToUInt64(bytes, 8),
					seed3: BitConverter.ToUInt64(bytes, 16),
					seed4: BitConverter.ToUInt64(bytes, 24),
					seed5: BitConverter.ToUInt64(bytes, 32),
					seed6: BitConverter.ToUInt64(bytes, 40),
					seed7: BitConverter.ToUInt64(bytes, 48),
					seed8: BitConverter.ToUInt64(bytes, 56));
#endif
			}
		}

		/// <summary>
		///		This is the jump function for the generator. It is equivalent
		///		to 2^256 calls to next(); it can be used to generate 2^256
		///		non-overlapping subsequences for parallel computations.
		/// </summary>
		public virtual void NextJump()
		{
			ulong[] JUMP = { 0x33ed89b6e7a353f9, 0x760083d7955323be,
							 0x2837f2fbb5f22fae, 0x4b8c5674d309511c,
							 0xb11ac47a7ba28c25, 0xf1be7667092bcc1c,
							 0x53851efdb6df0aaf, 0x1ebbc8b23eaf25db };

			var s = new ulong[8];

			for (var i = 0; i < 8; i++)
			{
				for (var b = 0; b < 64; b++)
				{
					if ((JUMP[i] & ((1UL) << b)) != 0)
					{
						for (var w = 0; w < 8; w++)
						{
							s[w] ^= this._State[w];
						}
					}
					this.Next();
				}
			}

			for (var i = 0; i < 8; i++)
			{
				this._State[i] = s[i];
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
		/// <param name="seed3">
		///		Third RNG seed.
		/// </param>
		/// <param name="seed4">
		///		Fourth RNG seed.
		/// </param>
		/// <param name="seed5">
		///		Fifth RNG seed.
		/// </param>
		/// <param name="seed6">
		///		Sixth RNG seed.
		/// </param>
		/// <param name="seed7">
		///		Seventh RNG seed.
		/// </param>
		/// <param name="seed8">
		///		Eighth RNG seed.
		/// </param>
		public virtual void SetSeed(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0, ulong seed5 = 0, ulong seed6 = 0, ulong seed7 = 0, ulong seed8 = 0)
		{
			this._State[0] = seed1;
			this._State[1] = seed2;
			this._State[2] = seed3;
			this._State[3] = seed4;
			this._State[4] = seed5;
			this._State[5] = seed6;
			this._State[6] = seed7;
			this._State[7] = seed8;
		}

		#endregion Public Method
	}
}