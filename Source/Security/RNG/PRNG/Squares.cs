using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Counter-based RNG based on <see cref="MiddleSquareWeylSequence"/>.
	/// </summary>
	/// <remarks>
	///		Source: https://arxiv.org/pdf/2004.06278.pdf
	/// </remarks>
	public class Squares : Random32
	{
		#region Member

		private ulong _Key = 0xc58efd154ce32f6d; // first key in key.h.
		private ulong _Counter = 0;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Squares"/> object.
		/// </summary>
		/// <param name="ctr">
		///		Counter start number.
		/// </param>
		/// <param name="key">
		///		RNG seed.
		/// </param>
		public Squares(ulong ctr = 0, ulong key = 0)
		{
			this.SetSeed(ctr, key);
		}

		~Squares()
		{
			this._Counter = 0;
			this._Key = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			this._Counter++;
			return this.Next(this._Counter, this._Key);
		}

		/// <summary>
		///		Generate next random number.
		/// </summary>
		/// <param name="ctr">
		///		Counter-based number.
		///	</param>
		/// <param name="key">
		///		RNG seed.
		///	</param>
		/// <returns>
		///		
		/// </returns>
		protected uint Next(ulong ctr, ulong key)
		{
			ulong x, y, z;
			y = x = ctr * key;
			z = y + key;

			// round 1
			x = (x * x) + y;
			x = (x >> 32) | (x << 32);

			// round 2
			x = (x * x) + z;
			x = (x >> 32) | (x << 32);

			// round 3
			x = (x * x) + y;
			x = (x >> 32) | (x << 32);

			// round 4
			return (uint)((x * x) + z) >> 32;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Squares";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[8];
				rng.GetBytes(bytes);
				this._Key = BitConverter.ToUInt64(bytes, 0);
			}
			this._Counter = 0;
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="ctr">
		///		Counter-based number.
		///	</param>
		/// <param name="key">
		///		RNG seed.
		///	</param>
		public void SetSeed(ulong ctr, ulong key)
		{
			this._Counter = ctr;
			if (key != 0)
			{
				this._Key = key;
			}
			else
			{
				this._Key = 0xc58efd154ce32f6d;
			}
		}

		#endregion Public Method
	}
}