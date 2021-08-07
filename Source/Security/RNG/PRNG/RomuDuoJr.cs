using System;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Romu random variations, the fastest generator using 64-bit arith., but not suited for huge jobs.
	///		Est. capacity = 2^51 bytes. Register pressure = 4. State size = 128 bits.
	/// </summary>
	/// <remarks>
	///		Source: https://www.romu-random.org/
	/// </remarks>
	public class RomuDuoJr : RomuDuo
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="RomuDuoJr"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		/// </param>
		public RomuDuoJr(ulong seed1 = 0, ulong seed2 = 0)
		{
			this.SetSeed(seed1, seed2);
		}

		/// <summary>
		///		Create an instance of <see cref="RomuDuoJr"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed numbers.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Seed need 2 numbers.
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

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Romu Duo Jr 64 bit";
		}

		#endregion Public Method
	}
}
