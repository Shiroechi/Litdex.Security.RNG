using System;

using Litdex.Utilities.Extension;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Variations of <see cref="Xoshiro256Plus"/>.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoshiro256starstar.c
	/// </remarks>
	public class Xoshiro256StarStar : Xoshiro256Plus
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Xoshiro256StarStar"/> object.
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
		public Xoshiro256StarStar(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0)
		{
			this._State = new ulong[4];
			this.SetSeed(seed1, seed2, seed3, seed4);
		}

		/// <summary>
		///		Create an instance of <see cref="Xoshiro256StarStar"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public Xoshiro256StarStar(ulong[] seed)
		{
			this._State = new ulong[4];
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~Xoshiro256StarStar()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var result = (this._State[1] * 5).RotateLeft(7) * 9;

			var t = this._State[1] << 17;

			this._State[2] ^= this._State[0];
			this._State[3] ^= this._State[1];
			this._State[1] ^= this._State[2];
			this._State[0] ^= this._State[3];

			this._State[2] ^= t;

			this._State[3] = this._State[3].RotateLeft(45);

			return result;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Xoshiro 256**";
		}

		#endregion Public Method
	}
}