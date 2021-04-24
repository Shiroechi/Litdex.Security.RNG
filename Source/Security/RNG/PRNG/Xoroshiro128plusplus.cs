namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Variations of <see cref="Xoroshiro128plus"/>.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoroshiro128plusplus.c
	/// </remarks>
	public class Xoroshiro128plusplus : Xoroshiro128plus
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Xoroshiro128plusplus"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		/// </param>
		public Xoroshiro128plusplus(ulong seed1 = 0, ulong seed2 = 0)
		{
			this.SetSeed(seed1, seed2);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var s0 = this._State1;
			var s1 = this._State2;
			var result = this.RotateLeft(s0 + s1, 17) + s0;

			s1 ^= s0;
			this._State1 = this.RotateLeft(s0, 49) ^ s1 ^ (s1 << 21); // a, b
			this._State2 = this.RotateLeft(s1, 28); // c

			return result;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Xoroshiro 128++";
		}

		#endregion Public Method
	}
}
