using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Romu random variations, more robust than anyone could need, but uses more registers than RomuTrio.
	///		Est. capacity >= 2^90 bytes. Register pressure = 8 (high). State size = 256 bits.
	/// </summary>
	/// <remarks>
	///		Source: https://www.romu-random.org/
	/// </remarks>
	public class RomuQuad : Random64
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="RomuQuad"/> object.
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
		public RomuQuad(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0)
		{
			this._State = new ulong[4];
			this.SetSeed(seed1, seed2, seed3, seed4);
		}

		/// <summary>
		///		Create an instance of <see cref="RomuQuad"/> object.
		/// </summary>
		/// <param name="seed">
		///		A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		Array of <paramref name="seed"/> is null or empty.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Seed need 4 numbers.
		/// </exception>
		public RomuQuad(ulong[] seed)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Clear all seed.
		/// </summary>
		~RomuQuad()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			ulong wp = this._State[0];
			ulong xp = this._State[1];
			ulong yp = this._State[2];
			ulong zp = this._State[3];

			this._State[0] = 15241094284759029579u * zp; // a-mult
			this._State[1] = zp + this.RotateLeft(wp, 52); // b-rotl, c-add
			this._State[2] = yp - xp; // d-sub
			this._State[3] = yp + wp; // e-add
			this._State[3] = this.RotateLeft(this._State[3], 19); // f-rotl
			return xp;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Romu Quad 64-bit";
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
		///		Set <see cref="RNG"/> seed manually.
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
		public void SetSeed(ulong seed1, ulong seed2, ulong seed3, ulong seed4)
		{
			this._State[0] = seed1;
			this._State[1] = seed2;
			this._State[2] = seed3;
			this._State[3] = seed4;
		}

		#endregion Public Method
	}
}
