using System;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Improved version of <see cref="JSF32"/> with 3 rotate.
	/// </summary>
	public class JSF32t : JSF32
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="JSF32t"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		///	</param>
		public JSF32t(uint seed = 0)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~JSF32t()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			var e = this._State[0] - this.RotateLeft(this._State[1], 23);
			this._State[0] = this._State[1] ^ this.RotateLeft(this._State[2], 16);
			this._State[1] = this._State[2] + this.RotateLeft(this._State[3], 11);
			this._State[2] = this._State[3] + e;
			this._State[3] = e + this._State[0];
			return this._State[3];
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "JSF 32-bit with 3-rotate";
		}

		#endregion Public Method
	}
}
