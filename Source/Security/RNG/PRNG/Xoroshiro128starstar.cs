using System;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Variations of <see cref="Xoroshiro128Plus"/>.
	/// </summary>
	/// <remarks>
	///		Source: https://prng.di.unimi.it/xoroshiro128starstar.c
	/// </remarks>
	public class Xoroshiro128StarStar : Xoroshiro128Plus
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
		public Xoroshiro128StarStar(ulong seed1 = 0, ulong seed2 = 0)
		{
			this._State = new ulong[2];
			this.SetSeed(seed1, seed2);
		}

		~Xoroshiro128StarStar()
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
			var result = this.RotateLeft(this._State[0] * 5, 7) * 9;

			s1 ^= s0;
			this._State[0] = this.RotateLeft(s0, 24) ^ s1 ^ (s1 << 16); // a, b
			this._State[1] = this.RotateLeft(s1, 37); // c

			return result;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Xoroshiro 128**";
		}

		#endregion Public Method
	}
}