using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// Improved version from Middle Square Method
	/// invented by John Von Neumann. 
	/// <list type="bullet">
	/// <item>https://arxiv.org/abs/1704.00358</item>
	/// </list>
	/// </summary>
	public class MiddleSquareWeylSequence : Random64
	{
		private ulong _Output = 0; //random output
		private ulong _Sequence = 0; //Weyl sequence
		private ulong _Increment = 0xB5AD4ECEDA1CE2A9; //odd constant

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="seed">Your seed.</param>
		public MiddleSquareWeylSequence(ulong seed = 0)
		{
			if (seed == 0) 
			{
				this.Reseed();
			}
			else
			{
				this._Output = seed;
				this._Sequence = seed;
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~MiddleSquareWeylSequence()
		{
			this._Output = 0;
			this._Sequence = 0;
			this._Increment = 0;
		}

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			this._Output *= this._Output;
			this._Output += (this._Sequence += this._Increment);
			return this._Output = (this._Output >> 32) | (this._Output << 32);
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
				var bytes = new byte[8];
				rng.GetNonZeroBytes(bytes);
				this._Sequence = BitConverter.ToUInt64(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._Output = BitConverter.ToUInt64(bytes, 0);
			}
		}

		#endregion Public Method
	}
}