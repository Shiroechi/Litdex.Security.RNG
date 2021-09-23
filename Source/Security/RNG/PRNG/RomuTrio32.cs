using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Romu random variations, 32-bit arithmetic: Good for general purpose use, except for huge jobs.
	///		Est. capacity >= 2^53 bytes. Register pressure = 5. State size = 96 bits.
	/// </summary>
	/// <remarks>
	///		Source: https://www.romu-random.org/
	/// </remarks>
	public class RomuTrio32 : Random32
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="RomuTrio32"/> object.
		/// </summary>
		/// <param name="seed1">
		///		X state.
		/// </param>
		/// <param name="seed2">
		///		Y state.
		/// </param>
		/// <param name="seed3">
		///		Z state.
		/// </param>
		public RomuTrio32(uint seed1 = 0, uint seed2 = 0, uint seed3 = 0)
		{
			this._State = new uint[3];
			this.SetSeed(seed1, seed2, seed3);
		}

		/// <summary>
		///		Create an instance of <see cref="RomuTrio32"/> object.
		/// </summary>
		/// <param name="seed">
		///		A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentException">
		///		Seed need 3 numbers.
		/// </exception>
		public RomuTrio32(uint[] seed)
		{
			this._State = new uint[3];
			this.SetSeed(seed);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			uint xp = this._State[0];
			uint yp = this._State[1];
			uint zp = this._State[2];
			this._State[0] = 3323815723u * zp;
			this._State[1] = yp - xp;
			this._State[1] = this.RotateLeft(this._State[1], 6);
			this._State[2] = zp - yp;
			this._State[2] = this.RotateLeft(this._State[2], 22);
			return xp;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Romu Trio 32-bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[12];
				rng.GetNonZeroBytes(bytes);
#if NET5_0_OR_GREATER
				var span = bytes.AsSpan();
				this.SetSeed(
					seed1: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span),
					seed2: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(4)),
					seed3: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(8)));
#else
				this.SetSeed(
					seed1: BitConverter.ToUInt32(bytes, 0),
					seed2: BitConverter.ToUInt32(bytes, 4),
					seed3: BitConverter.ToUInt32(bytes, 8));
#endif
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed1">
		///		X state.
		/// </param>
		/// <param name="seed2">
		///		Y state.
		/// </param>
		/// <param name="seed3">
		///		Z state.
		/// </param>
		public void SetSeed(uint seed1, uint seed2, uint seed3)
		{
			this._State[0] = seed1;
			this._State[1] = seed2;
			this._State[2] = seed3;
		}

		#endregion Public Method
	}
}
