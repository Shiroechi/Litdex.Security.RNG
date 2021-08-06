using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Implementation of a Bob Jenkins Small Fast (Noncryptographic) PRNGs.
	/// </summary>
	/// <remarks>
	///		Source: http://burtleburtle.net/bob/rand/smallprng.html
	/// </remarks>
	public class JSF32 : Random32
	{
		#region Member

		protected uint[] _Seed = new uint[4];

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="JSF32"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		///	</param>
		public JSF32(uint seed = 0)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~JSF32()
		{
			this._Seed = null;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			var e = this._Seed[0] - this.Rotate(this._Seed[1], 27);
			this._Seed[0] = this._Seed[1] ^ this.Rotate(this._Seed[2], 17);
			this._Seed[1] = this._Seed[2] + this._Seed[3];
			this._Seed[2] = this._Seed[3] + e;
			this._Seed[3] = e + this._Seed[0];
			return this._Seed[3];
		}

		/// <summary>
		///		Rotate the bit.
		/// </summary>
		/// <param name="value">
		///		Number to rotate.</param>
		/// <param name="shift">
		///		Bit to rotate.
		///	</param>
		/// <returns>
		///		Left rotate of the <paramref name="value"/>.
		/// </returns>
		protected uint Rotate(uint value, int shift)
		{
			// sizeof(uint) = 32
			return ((value) << (shift)) | ((value) >> (sizeof(uint) - shift));
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "JSF 32 bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[4];
				rng.GetBytes(bytes);
				this.SetSeed(BitConverter.ToUInt32(bytes, 0));
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public void SetSeed(uint seed)
		{
			this._Seed[0] = 0xF1EA5EED;
			this._Seed[1] = this._Seed[2] = this._Seed[3] = seed;

			for (var i = 0; i < 20; i++)
			{
				this.Next();
			}
		}

		#endregion Public Method
	}
}