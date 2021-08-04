﻿using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Based on Wyhash https://github.com/wangyi-fudan/wyhash
	/// </summary>
	public class WyRng : Random64
	{
		#region Member

		private ulong _Seed;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="WyRng"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public WyRng(ulong seed = 0)
		{
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~WyRng()
		{
			this._Seed = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			this._Seed += 0xa0761d6478bd642f;
			var result = this.MUM(this._Seed ^ 0xe7037ed1a0b428db, this._Seed);
			return result;
		}

		protected ulong MUM(ulong x, ulong y)
		{
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
			return "WyRng";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[8];
				rng.GetBytes(bytes);
				this._Seed = BitConverter.ToUInt64(bytes, 0);
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
			this._Seed = seed;
		}

		#endregion Public Method
	}
}
