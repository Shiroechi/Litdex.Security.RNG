namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// Improved version of <see cref="JSF32"/> with 3 rotate.
	/// </summary>
	public class JSF32t : JSF32
	{
		#region Constructor & Destructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="seed">Your seed.</param>
		public JSF32t(uint seed = 0)
		{
			if (seed == 0)
			{
				this.Reseed();
			}
			else
			{
				this.SetSeed(seed);
			}
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~JSF32t()
		{
			this._Seed = null;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			var e = this._Seed[0] - this.Rotate(this._Seed[1], 23);
			this._Seed[0] = this._Seed[1] ^ this.Rotate(this._Seed[2], 16);
			this._Seed[1] = this._Seed[2] + this.Rotate(this._Seed[3], 11);
			this._Seed[2] = this._Seed[3] + e;
			this._Seed[3] = e + this._Seed[0];
			return this._Seed[3];
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "JSF 32 bit 3-rotate.";
		}

		#endregion Public Method
	}
}
