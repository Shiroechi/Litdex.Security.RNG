using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// The fastest generator using 64-bit arith., but not suited for huge jobs.
	/// Est. capacity = 2^51 bytes. Register pressure = 4. State size = 128 bits.
	/// </summary>
	public class RomuDuoJr : Random64
	{
		#region Member

		private ulong _X;
		private ulong _Y;

		#endregion Member

		#region Constructor & Destructor

		public RomuDuoJr(ulong seed1 = 0, ulong seed2 = 0)
		{
			this.SetSeed(seed1, seed2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seed"></param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Seed need 2 numbers.
		/// </exception>
		public RomuDuoJr(ulong[] seed)
		{
			this.SetSeed(seed);
		}

		~RomuDuoJr()
		{
			this._X = this._Y = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			ulong xp = this._X;
			this._X = 15241094284759029579u * this._Y;
			this._Y -= xp;
			this._Y = this.ROTL(this._Y, 27);
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
			return "Romu Duo Jr 64 bit";
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
			}
		}

		/// <summary>
		/// Set <see cref="RNG"/> seed manually.
		/// </summary>
		public void SetSeed(ulong seed1 = 0, ulong seed2 = 0)
		{
			this._X = seed1;
			this._Y = seed2;
		}

		/// <summary>
		/// Set <see cref="RNG"/> seed manually.
		/// </summary>
		public void SetSeed(ulong[] seed)
		{
			if (seed.Length < 2)
			{
				throw new ArgumentOutOfRangeException(nameof(seed), "Seed need 2 numbers.");
			}

			this._X = seed[0];
			this._Y = seed[1];
		}

		#endregion Public Method
	}
}
