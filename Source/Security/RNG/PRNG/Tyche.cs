using System;
using System.Security.Cryptography;

// http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.714.1893&rep=rep1&type=pdf

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		<see cref="Tyche"/> is based on ChaCha's quarter-round.
	/// </summary>
	public class Tyche : Random32
	{
		#region Member

		protected uint _A, _B, _C, _D;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Tyche"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		/// <param name="idx">
		///		Unique key.
		/// </param>
		public Tyche(ulong seed = 0, uint idx = 0)
		{
			this.SetSeed(seed, idx);
		}

		~Tyche()
		{
			this._A = this._B = this._C = this._D = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			this.Mix();
			return this._B;
		}

		/// <summary>
		///		Initialzied internal state.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		/// <param name="idx">
		///		Unique key.
		/// </param>
		protected void Init(ulong seed, uint idx)
		{
			this._A = (uint)(seed / uint.MaxValue);
			this._B = (uint)(seed % uint.MaxValue);
			this._C = 2654435769;
			this._D = idx ^ 1367130551;

			for (var i = 0; i < 20; i++)
			{
				this.Mix();
			}
		}

		/// <summary>
		///		Update internal state based on quater round function of ChaCha stream chiper.
		/// </summary>
		protected virtual void Mix()
		{
			this._A += this._B;
			this._D ^= this._A;
			this._D = this._D << 16 | this._D >> 16;

			this._C += this._D;
			this._B ^= this._C;
			this._B = this._B << 12 | this._B >> 20;

			this._A += this._B;
			this._D ^= this._A;
			this._D = this._D << 8 | this._D >> 24;

			this._C += this._D;
			this._B ^= this._C;
			this._B = this._B << 7 | this._B >> 25;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Tyche";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[12];
				rng.GetBytes(bytes);
				this.Init(BitConverter.ToUInt64(bytes, 0), BitConverter.ToUInt32(bytes, 8));
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		/// <param name="idx">
		///		Unique key.
		/// </param>
		public void SetSeed(ulong seed, uint idx)
		{
			this.Init(seed, idx);
		}

		#endregion Public Method
	}
}