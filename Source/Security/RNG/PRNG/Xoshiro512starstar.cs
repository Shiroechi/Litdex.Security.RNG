namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Vartions of <see cref="Xoshiro512plus"/>.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoshiro512starstar.c
	/// </remarks>
	public class Xoshiro512starstar : Xoshiro512plus
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Xoshiro512starstar"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		/// </param>
		/// <param name="seed3">
		///		Third RNG seed.
		/// </param>
		/// <param name="seed4">
		///		Fourth RNG seed.
		/// </param>
		/// <param name="seed5">
		///		Fifth RNG seed.
		/// </param>
		/// <param name="seed6">
		///		Sixth RNG seed.
		/// </param>
		/// <param name="seed7">
		///		Seventh RNG seed.
		/// </param>
		/// <param name="seed8">
		///		Eighth RNG seed.
		/// </param>
		public Xoshiro512starstar(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0, ulong seed5 = 0, ulong seed6 = 0, ulong seed7 = 0, ulong seed8 = 0)
		{
			this.SetSeed(seed1, seed2, seed3, seed4, seed5, seed6, seed7, seed8);
		}

		/// <summary>
		///		Create an instance of <see cref="Xoshiro512starstar"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public Xoshiro512starstar(ulong[] seed)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor
		/// </summary>
		~Xoshiro512starstar()
		{
			this._State = null;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var result = this.RotateLeft(this._State[1] * 5, 7) * 9;

			var t = this._State[1] << 11;

			this._State[2] ^= this._State[0];
			this._State[5] ^= this._State[1];
			this._State[1] ^= this._State[2];
			this._State[7] ^= this._State[3];
			this._State[3] ^= this._State[4];
			this._State[4] ^= this._State[5];
			this._State[0] ^= this._State[6];
			this._State[6] ^= this._State[7];

			this._State[6] ^= t;

			this._State[7] = this.RotateLeft(this._State[7], 21);

			return result;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Xoshiro 512**";
		}

		#endregion Public
	}
}