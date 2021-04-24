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
		#region Member

		private ulong _W, _X, _Y, _Z;

		#endregion Member

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
		public RomuQuad(uint seed1 = 0, uint seed2 = 0, uint seed3 = 0, uint seed4 = 0)
		{
			this.SetSeed(seed1, seed2, seed3, seed4);
		}

		/// <summary>
		///		Create an instance of <see cref="RomuQuad"/> object.
		/// </summary>
		/// <param name="seed">
		///		A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Seed need 4 numbers.
		/// </exception>
		public RomuQuad(uint[] seed)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Clear all seed.
		/// </summary>
		~RomuQuad()
		{
			this._W = this._X = this._Y = this._Z = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			ulong wp = this._W;
			ulong xp = this._X;
			ulong yp = this._Y;
			ulong zp = this._Z;

			this._W = 15241094284759029579u * zp; // a-mult
			this._X = zp + this.ROTL(wp, 52); // b-rotl, c-add
			this._Y = yp - xp; // d-sub
			this._Z = yp + wp; // e-add
			this._Z = this.ROTL(this._Z, 19); // f-rotl
			return xp;
		}

		protected ulong ROTL(ulong d, int lrot)
		{
			return (d << (lrot)) | (d >> (64 - lrot));
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Romu Quad 64 bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[32];
				rng.GetBytes(bytes);
				this._W = BitConverter.ToUInt32(bytes, 0);
				this._X = BitConverter.ToUInt32(bytes, 8);
				this._Y = BitConverter.ToUInt32(bytes, 16);
				this._Z = BitConverter.ToUInt32(bytes, 24);
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
		public void SetSeed(uint seed1, uint seed2, uint seed3, uint seed4)
		{
			this._W = seed1;
			this._X = seed2;
			this._Y = seed3;
			this._Z = seed4;
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		Array of seed is null or empty.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Seed need 4 numbers.
		/// </exception>
		public void SetSeed(uint[] seed)
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
		}

		#endregion Public Method
	}
}
