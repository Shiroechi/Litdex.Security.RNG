using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Romu random variations, great for general purpose work, including huge jobs.
	///		Est. capacity = 2^75 bytes. Register pressure = 6. State size = 192 bits.
	/// </summary>
	/// <remarks>
	///		Source: https://www.romu-random.org/
	/// </remarks>
	public class RomuTrio : Random64
	{
		#region Member

		private ulong _X, _Y, _Z;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="RomuTrio"/> object.
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
		public RomuTrio(uint seed1 = 0, uint seed2 = 0, uint seed3 = 0)
		{
			this.SetSeed(seed1, seed2, seed3);
		}

		/// <summary>
		///		Create an instance of <see cref="RomuTrio"/> object.
		/// </summary>
		/// <param name="seed">
		///		A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Seed need 3 numbers.
		/// </exception>
		public RomuTrio(uint[] seed)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Clear all seed.
		/// </summary>
		~RomuTrio()
		{
			this._X = this._Y = this._Z = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			ulong xp = this._X;
			ulong yp = this._Y;
			ulong zp = this._Z;

			this._X = 15241094284759029579u * zp; // a-mult
			this._Y = yp - xp; // d-sub
			this._Y = this.ROTL(this._Y, 12);
			this._Z = zp - yp; // e-add
			this._Z = this.ROTL(this._Z, 44); // f-rotl
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
			return "Romu Trio 64 bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[24];
				rng.GetBytes(bytes);
				this._X = BitConverter.ToUInt32(bytes, 0);
				this._Y = BitConverter.ToUInt32(bytes, 8);
				this._Z = BitConverter.ToUInt32(bytes, 16);
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
			this._X = seed1;
			this._Y = seed2;
			this._Z = seed3;
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Seed need 3 numbers.
		/// </exception>
		public void SetSeed(uint[] seed)
		{
			if (seed.Length < 3)
			{
				throw new ArgumentOutOfRangeException(nameof(seed), $"Seed need 3 numbers.");
			}

			this.SetSeed(seed[0], seed[1], seed[2]);
		}

		#endregion Public Method
	}
}
