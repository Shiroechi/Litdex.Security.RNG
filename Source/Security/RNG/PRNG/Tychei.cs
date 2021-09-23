// http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.714.1893&rep=rep1&type=pdf
// https://github.com/lemire/testingRNG/blob/master/cpp-prng-bench/tychei.hpp

using System;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Inverse <see cref="Tyche"/>.
	/// </summary>
	public class Tychei : Tyche
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Tychei"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		/// <param name="idx">
		///		unique key.
		/// </param>
		public Tychei(ulong seed = 0xFEEDFACECAFEF00D, uint idx = 0)
		{
			this._State = new uint[4];
			this.SetSeed(seed, idx);
		}

		~Tychei()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			this.Mix();
			return this._State[0];
		}

		/// <summary>
		///		Update internal state based on quater round function of ChaCha stream chiper.
		/// </summary>
		protected override void Mix()
		{
			this._State[1] = (this._State[1] << 25 | this._State[1] >> 7) ^ this._State[2];
			this._State[2] -= this._State[3];

			this._State[3] = (this._State[3] << 24 | this._State[3] >> 8) ^ this._State[0];
			this._State[0] -= this._State[1];

			this._State[1] = (this._State[1] << 20 | this._State[1] >> 12) ^ this._State[2];
			this._State[2] -= this._State[3];

			this._State[3] = (this._State[3] << 16 | this._State[3] >> 16) ^ this._State[0];
			this._State[0] -= this._State[1];
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Tyche-i";
		}

		#endregion Public Method
	}
}
