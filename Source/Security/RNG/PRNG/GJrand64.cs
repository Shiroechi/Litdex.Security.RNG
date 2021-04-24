using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Implementation of David Blackman's GJrand PRNG(s)
	/// </summary>
	public class GJrand64 : Random64
	{
		#region Member

		private ulong _A, _B, _C, _D;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="GJrand64"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First seed.
		/// </param>
		/// <param name="seed2">
		///		Second seed.
		/// </param>
		/// <param name="seed3">
		///		Third seed.
		/// </param>
		/// <param name="seed4">
		///		Fourth seed.
		/// </param>
		public GJrand64(ulong seed1 = 0xCAFEF00DBEEF5EED, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0)
		{
			this.SetSeed(seed1, seed2, seed3, seed4);
		}

		/// <summary>
		///		Create an instance of <see cref="GJrand64"/> object.
		/// </summary>
		/// <param name="seed">
		///		Array of 64 bit unsigned integer with minimum length of 4. 
		/// </param>
		public GJrand64(ulong[] seed)
		{
			this.SetSeed(seed);
		}

		~GJrand64()
		{
			this._A = this._B = this._C = this._D = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			this.Advance();
			return this._A;
		}

		protected void Advance()
		{
			this._B += this._C;
			this._A = this.Rotate(this._A, 32);
			this._C ^= this._B;
			this._D += 0x55AA96A5;
			this._A += this._B;
			this._C = this.Rotate(this._C, 23);
			this._B ^= this._A;
			this._A += this._C;
			this._B = this.Rotate(this._B, 19);
			this._C += this._A;
			this._B += this._D;
		}

		protected ulong Rotate(ulong value, int bit)
		{
			return (value << bit) | (value >> (64 - bit));
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Gjrand 64";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				// get all bytes for 4 ulong seed
				// 4 ulong x 8 bytes = 32 bytes
				var bytes = new byte[32];
				rng.GetBytes(bytes);
				this._A = BitConverter.ToUInt64(bytes, 0);
				this._B = BitConverter.ToUInt64(bytes, 8);
				this._C = BitConverter.ToUInt64(bytes, 16);
				this._D = BitConverter.ToUInt64(bytes, 24);
			}

			for (var i = 0; i < 15; i++)
			{
				this.Advance();
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		public void SetSeed(ulong seed1, ulong seed2, ulong seed3, ulong seed4)
		{
			this._A = seed1;
			this._B = seed2;
			this._C = seed3;
			this._D = seed4;

			for (var i = 0; i < 15; i++)
			{
				this.Advance();
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		public void SetSeed(ulong[] seed)
		{
			if (seed.Length < 4)
			{
				throw new ArgumentOutOfRangeException(nameof(seed), $"Seed need at least 4 numbers.");
			}

			this.SetSeed(seed[0], seed[1], seed[2], seed[3]);
		}

		#endregion Public Method
	}
}
