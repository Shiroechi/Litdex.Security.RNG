using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG
{
	/// <summary>
	/// 32-bit arithmetic: Good for general purpose use, except for huge jobs.
	/// Est. capacity >= 2^53 bytes. Register pressure = 5. State size = 96 bits.
	/// </summary>
	public class RomuTrio32 : Random32
	{
		#region Member

		private uint _X;
		private uint _Y;
		private uint _Z;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		/// Create <see cref="RomuTrio32"/> instance.
		/// </summary>
		/// <param name="seed1">
		/// X state.
		/// </param>
		/// <param name="seed2">
		/// Y state.
		/// </param>
		/// <param name="seed3">
		/// Z state.
		/// </param>
		public RomuTrio32(uint seed1 = 0, uint seed2 = 0, uint seed3 = 0)
		{
			this._X = seed1;
			this._Y = seed2;
			this._Z = seed3;
		}

		/// <summary>
		/// Create <see cref="RomuTrio32"/> instance.
		/// </summary>
		/// <param name="seed">
		/// Rng seed.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Seed nedd 3 numbers.
		/// </exception>
		public RomuTrio32(uint[] seed)
		{
			if (seed.Length < 3)
			{
				throw new ArgumentOutOfRangeException(nameof(seed), "Seed nedd 3 numbers.");
			}

			this._X = seed[0];
			this._Y = seed[1];
			this._Z = seed[2];
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
				var bytes = new byte[4];
				rng.GetNonZeroBytes(bytes);
				this._X = BitConverter.ToUInt32(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._Y = BitConverter.ToUInt32(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._Z = BitConverter.ToUInt32(bytes, 0);
			}
		}

		#endregion Public Method
	}
}
