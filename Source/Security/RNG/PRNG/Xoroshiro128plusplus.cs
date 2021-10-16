using System;

using Litdex.Utilities.Extension;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Variations of <see cref="Xoroshiro128Plus"/>.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoroshiro128plusplus.c
	/// </remarks>
	public class Xoroshiro128PlusPlus : Xoroshiro128Plus
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Xoroshiro128PlusPlus"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First RNG seed.
		/// </param>
		/// <param name="seed2">
		///		Second RNG seed.
		/// </param>
		public Xoroshiro128PlusPlus(ulong seed1 = 0, ulong seed2 = 0)
		{
			this._State = new ulong[2];
			this.SetSeed(seed1, seed2);
		}

		~Xoroshiro128PlusPlus()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var s0 = this._State[0];
			var s1 = this._State[1];
			var result = (s0 + s1).RotateLeft(17) + s0;

			s1 ^= s0;
			this._State[0] = s0.RotateLeft(49) ^ s1 ^ (s1 << 21); // a, b
			this._State[1] = s1.RotateLeft(28); // c

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
