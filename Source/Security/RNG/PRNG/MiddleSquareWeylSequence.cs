using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Improved version from Middle Square Method, invented by John Von Neumann.
	/// </summary>
	/// <remarks>
	///		Source: https://arxiv.org/abs/1704.00358
	/// </remarks>
	public class MiddleSquareWeylSequence : Random64
	{
		#region Member

		private ulong _Output = 0; // random output
		private ulong _Sequence = 0; // Weyl sequence
		private ulong _Increment = 0xB5AD4ECEDA1CE2A9; // odd constant

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="MiddleSquareWeylSequence"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		///	</param>
		public MiddleSquareWeylSequence(ulong seed = 0)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~MiddleSquareWeylSequence()
		{
			this._Output = 0;
			this._Sequence = 0;
			this._Increment = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			this._Output *= this._Output;
			this._Output += this._Sequence += this._Increment;
			this._Output = (this._Output >> 32) | (this._Output << 32);
			return this._Output;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Middle Square Weyl Sequence";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[16];
				rng.GetNonZeroBytes(bytes);
				this._Sequence = BitConverter.ToUInt64(bytes, 0);
				this._Output = BitConverter.ToUInt64(bytes, 8);
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public void SetSeed(ulong seed)
		{
			this._Output = seed;
			this._Sequence = seed;
		}

		#endregion Public Method
	}
}