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
		#region Member

		private uint _X;
		private uint _Y;
		private uint _Z;

		#endregion Member

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
			this.SetSeed(seed1, seed2, seed3);
		}

		/// <summary>
		///		Create an instance of <see cref="RomuTrio32"/> object.
		/// </summary>
		/// <param name="seed">
		///		A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Seed need 3 numbers.
		/// </exception>
		public RomuTrio32(uint[] seed)
		{
			this.SetSeed(seed);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			uint xp = this._X;
			uint yp = this._Y;
			uint zp = this._Z;
			this._X = 3323815723u * zp;
			this._Y = yp - xp;
			this._Y = this.ROTL(this._Y, 6);
			this._Z = zp - yp;
			this._Z = this.ROTL(this._Z, 22);
			return xp;
		}

		protected uint ROTL(uint d, int lrot)
		{
			return (d << (lrot)) | (d >> (32 - lrot));
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Romu Trio 32 bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[12];
				rng.GetBytes(bytes);
				this._X = BitConverter.ToUInt32(bytes, 0);
				this._Y = BitConverter.ToUInt32(bytes, 4);
				this._Z = BitConverter.ToUInt32(bytes, 8);
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
				throw new ArgumentOutOfRangeException(nameof(seed), "Seed nedd 3 numbers.");
			}

			this.SetSeed(seed[0], seed[1], seed[2]);
		}

		#endregion Public Method
	}
}
