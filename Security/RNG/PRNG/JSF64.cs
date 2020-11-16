using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// Implementation of a Bob Jenkins Small Fast (Noncryptographic) PRNGs.
	/// 
	/// http://burtleburtle.net/bob/rand/smallprng.html
	/// </summary>
	public class JSF64 : Random64
    {
		private ulong[] _Seed = new ulong[8];
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="seed">Your seed.</param>
		public JSF64(ulong seed = 0)
		{
			if (seed == 0) 
			{
				this.Reseed();
			}
			else
			{
				this._Seed[0] = 0xF1EA5EED;
				this._Seed[1] = this._Seed[2] = this._Seed[3] = seed;

				for (var i = 0; i < 20; i++)
				{
					this.Next();
				}
			}
			
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~JSF64()
		{
			this._Seed = null;
		}

		#region	Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			var e = this._Seed[0] - this.Rotate(this._Seed[1], 7);
			this._Seed[0] = this._Seed[1] ^ this.Rotate(this._Seed[2], 13);
			this._Seed[1] = this._Seed[2] + this.Rotate(this._Seed[3], 37);
			this._Seed[2] = this._Seed[3] + e;
			this._Seed[3] = e + this._Seed[0];
			return this._Seed[3];
		}

		/// <summary>
		/// Rotate the bit.
		/// </summary>
		/// <param name="value">Number to rotate.</param>
		/// <param name="shift">Bit to rotate.</param>
		/// <returns></returns>
		protected ulong Rotate(ulong value, int shift)
		{
			return ((value) << (shift)) | ((value) >> (64 - (shift)));
		}

		#endregion Protected Method

		#region	Public Method


		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "JSF 64 bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			ulong seed;
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[8];
				rng.GetNonZeroBytes(bytes);
				seed = BitConverter.ToUInt64(bytes, 0);
			}

			this._Seed[0] = 0xF1EA5EED;
			this._Seed[1] = this._Seed[2] = this._Seed[3] = seed;

			for (var i = 0; i < 20; i++)
			{
				this.Next();
			}
		}

		#endregion	Public
	}
}