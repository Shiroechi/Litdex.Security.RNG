// http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.714.1893&rep=rep1&type=pdf
// https://github.com/lemire/testingRNG/blob/master/cpp-prng-bench/tychei.hpp

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
			this.SetSeed(seed, idx);
		}

		~Tychei()
		{
			this._A = this._B = this._C = this._D = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			this.Mix();
			return this._A;
		}

		/// <summary>
		///		Update internal state based on quater round function of ChaCha stream chiper.
		/// </summary>
		protected override void Mix()
		{
			this._B = (this._B << 25 | this._B >> 7) ^ this._C;
			this._C -= this._D;

			this._D = (this._D << 24 | this._D >> 8) ^ this._A;
			this._A -= this._B;

			this._B = (this._B << 20 | this._B >> 12) ^ this._C;
			this._C -= this._D;

			this._D = (this._D << 16 | this._D >> 16) ^ this._A;
			this._A -= this._B;
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
