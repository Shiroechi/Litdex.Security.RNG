using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Romu random variations, 32-bit arithmetic: Good for general purpose use.
	///		Est. capacity >= 2^62 bytes. Register pressure = 7. State size = 128 bits.
	/// </summary>
	/// <remarks>
	///		Source: https://www.romu-random.org/
	/// </remarks>
	public class RomuQuad32 : Random32
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="RomuQuad32"/> object.
		/// </summary>
		/// <param name="seed1">
		///		W state.
		/// </param>
		/// <param name="seed2">
		///		X state.
		/// </param>
		/// <param name="seed3">
		///		Y state.
		/// </param>
		/// <param name="seed4">
		///		Z state.
		/// </param>
		public RomuQuad32(uint seed1 = 0, uint seed2 = 0, uint seed3 = 0, uint seed4 = 0)
		{
			this._State = new uint[4];
			this.SetSeed(seed1, seed2, seed3, seed4);
		}

		/// <summary>
		///		Create an instance of <see cref="RomuQuad"/> object.
		/// </summary>
		/// <param name="seed">
		///		A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentException">
		///		Seed need 4 numbers.
		/// </exception>
		public RomuQuad32(uint[] seed)
		{
			this._State = new uint[4];
			this.SetSeed(seed);
		}

		/// <summary>
		///		Clear all seed.
		/// </summary>
		~RomuQuad32()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			uint wp = this._State[0];
			uint xp = this._State[1];
			uint yp = this._State[2];
			uint zp = this._State[3];

			this._State[0] = 3323815723 * zp; // a-mult
			this._State[1] = zp + this.RotateLeft(wp, 26); // b-rotl, c-add
			this._State[2] = yp - xp; // d-sub
			this._State[3] = yp + wp; // e-add
			this._State[3] = this.RotateLeft(this._State[3], 9); // f-rotl
			return xp;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Romu Quad 32-bit";
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
					seed1: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span),
					seed2: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(4)),
					seed3: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(8)),
					seed4: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(12)));
#else
				this.SetSeed(
					seed1: BitConverter.ToUInt32(bytes, 0),
					seed2: BitConverter.ToUInt32(bytes, 4),
					seed3: BitConverter.ToUInt32(bytes, 8),
					seed4: BitConverter.ToUInt32(bytes, 12));
#endif
			}
		}

		/// <summary>
		/// Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed1">
		///		W state.
		/// </param>
		/// <param name="seed2">
		///		X state.
		/// </param>
		/// <param name="seed3">
		///		Y state.
		/// </param>
		/// <param name="seed4">
		///		Z state.
		/// </param>
		public void SetSeed(uint seed1, uint seed2, uint seed3, uint seed4)
		{
			this._State[0] = seed1;
			this._State[1] = seed2;
			this._State[2] = seed3;
			this._State[3] = seed4;
		}

		#endregion Public Method
	}
}
