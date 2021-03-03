using System;
using System.Dynamic;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// Implementation of David Blackman's GJrand PRNG(s)
	/// </summary>
	public class GJrand64 : Random64
	{
		#region Member

		private ulong _A, _B, _C, _D;

		#endregion Member

		#region Constructor & Destructor

		public GJrand64(ulong seed1 = 0xCAFEF00DBEEF5EED, ulong seed2 = 0, ulong seed3 = 0, ulong seed4 = 0)
		{
			this.SetSeed(seed1, seed2, seed3, seed4);
		}

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
				var bytes = new byte[8];
				rng.GetNonZeroBytes(bytes);
				this._A = BitConverter.ToUInt64(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._B = BitConverter.ToUInt64(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._C = BitConverter.ToUInt64(bytes, 0);
				rng.GetNonZeroBytes(bytes);
				this._D = BitConverter.ToUInt64(bytes, 0);
			}

			for (var i = 0; i < 15; i++)
			{
				this.Advance();
			}
		}

		/// <summary>
		/// Set <see cref="RNG"/> seed manually.
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
		/// Set <see cref="RNG"/> seed manually.
		/// </summary>
		public void SetSeed(ulong[] seed)
		{
			if (seed.Length < 4)
			{
				throw new ArgumentOutOfRangeException(nameof(seed), "Seed length ");
			}

			if (seed.Length < 4)
			{
				throw new ArgumentOutOfRangeException(nameof(seed), $"Seed need 4 numbers.");
			}

			this._A = seed[0];
			this._B = seed[1];
			this._C = seed[2];
			this._D = seed[3];

			for (var i = 0; i < 15; i++)
			{
				this.Advance();
			}
		}

		#endregion Public Method
	}
}
