using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Based on Wyhash from  https://github.com/wangyi-fudan/wyhash
	/// </summary>
	public class Wyrand : Random64
	{
		#region Member

		protected ulong _State;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Wyrand"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public Wyrand(ulong seed = 0)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~Wyrand()
		{
			this._State = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			this._State += 0xa0761d6478bd642f;
			var result = this.MUM(this._State ^ 0xe7037ed1a0b428db, this._State);
			return result;
		}

		protected ulong MUM(ulong x, ulong y)
		{
			// from source code
			// uint64_t ha=*A>>32, hb=*B>>32, la=(uint32_t)*A, lb=(uint32_t)*B, hi, lo;
			// uint64_t rh = ha * hb, rm0 = ha * lb, rm1 = hb * la, rl = la * lb, t = rl + (rm0 << 32), c = t < rl;
			// lo = t + (rm1 << 32);
			// c += lo < t;
			// hi = rh + (rm0 >> 32) + (rm1 >> 32) + c;
			// #if (WYHASH_CONDOM>1)
			// *A^=lo;  *B^=hi;
			// #else
			// *A = lo;
			// *B = hi;
			// #endif

			ulong hi, lo;

			lo = x * y;

			ulong x0 = (uint)x;
			var x1 = x >> 32;

			ulong y0 = (uint)y;
			var y1 = y >> 32;

			var p11 = x1 * y1;
			var p10 = x1 * y0;
			var p01 = x0 * y1;
			var p00 = x0 * y0;

			// 64-bit product + two 32-bit values
			var middle = p10 + (p00 >> 32) + (uint)p01;

			// 64-bit product + two 32-bit values
			hi = p11 + (middle >> 32) + (p01 >> 32);

			return hi ^ lo;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Wyrand";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[8];
				rng.GetNonZeroBytes(bytes);
				this._State = BitConverter.ToUInt64(bytes, 0);
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
			this._State = seed;
		}

		#endregion Public Method
	}
}
