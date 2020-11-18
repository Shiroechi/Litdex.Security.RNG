using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG
{
	/// <summary>
	/// Great for general purpose work, including huge jobs.
	/// Est. capacity = 2^75 bytes. Register pressure = 6. State size = 192 bits.
	/// </summary>
	public class RomuTrio : Random64
	{
		#region Member

		private ulong _X, _Y, _Z;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		/// Create <see cref="RomuTrio"/> instance.
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
		public RomuTrio(uint seed1 = 0, uint seed2 = 0, uint seed3 = 0)
		{
			this._X = seed1;
			this._Y = seed2;
			this._Z = seed3;
		}

		/// <summary>
		/// Create <see cref="RomuTrio"/> instance.
		/// </summary>
		/// <param name="seed">
		/// A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Seed need 3 numbers.
		/// </exception>
		public RomuTrio(uint[] seed)
		{
			if (seed.Length < 3)
			{
				throw new ArgumentOutOfRangeException(nameof(seed), $"Seed need 3 numbers.");
			}

			this._X = seed[0];
			this._Y = seed[1];
			this._Z = seed[2];
		}

		/// <summary>
		/// Clear all seed.
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
			this._Y = ROTL(this._Y, 12);
			this._Z = zp - yp; // e-add
			this._Z = ROTL(this._Z, 44); // f-rotl
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
