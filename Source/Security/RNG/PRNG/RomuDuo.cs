using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Romu random variations, might be faster than <see cref="RomuTrio"/> due to using fewer registers, but might struggle with massive jobs.
	///		Est. capacity = 2^61 bytes. Register pressure = 5. State size = 128 bits.
	/// </summary>
	/// <remarks>
	///		Source: https://www.romu-random.org/
	/// </remarks>
	public class RomuDuo : Random64
	{
		#region Member

		protected ulong _X;
		protected ulong _Y;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="RomuDuo"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		/// </param>
		public RomuDuo(ulong seed1 = 0, ulong seed2 = 0)
		{
			this.SetSeed(seed1, seed2);
		}

		/// <summary>
		///		Create an instance of <see cref="RomuDuo"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed numbers.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Seed need 2 numbers.
		/// </exception>
		public RomuDuo(ulong[] seed)
		{
			this.SetSeed(seed);
		}

		~RomuDuo()
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
			this._Y = this.ROTL(this._Y, 27) + this.ROTL(this._Y, 15) - xp;
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
			return "Romu Duo 64 bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[16];
				rng.GetNonZeroBytes(bytes);
				this.SetSeed(
					seed1: BitConverter.ToUInt64(bytes, 0),
					seed2: BitConverter.ToUInt64(bytes, 8));
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
		public void SetSeed(ulong seed1, ulong seed2)
		{
			this._X = seed1;
			this._Y = seed2;
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seeds.
		/// </param>
		/// <exception cref="ArgumentException">
		///		Seed need at least 2 numbers.
		/// </exception>
		public void SetSeed(ulong[] seed)
		{
			if (seed.Length < 2)
			{
				throw new ArgumentException(nameof(seed), "Seed need 2 numbers.");
			}

			this.SetSeed(seed[0], seed[1]);
		}

		#endregion Public Method
	}
}
