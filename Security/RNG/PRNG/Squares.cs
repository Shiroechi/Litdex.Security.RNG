using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// Counter-based RNG based on <see cref="MiddleSquareWeylSequence"/>
	/// 
	/// <list type="bullet">
	///		<item>https://arxiv.org/pdf/2004.06278v2.pdf</item>
	///		<item>https://arxiv.org/pdf/1704.00358v5.pdf</item>
	/// </list>
	/// </summary>
	public class Squares : Random32
	{
		private ulong _Key = 0xc58efd154ce32f6d; //first key in key.h.
		private ulong _Counter = 0;

		/// <summary>
		/// Constructor.
		/// </summary>
		public Squares(ulong ctr = 0, ulong key = 0)
		{
			this._Counter = ctr;
			if (key != 0)
			{
				this._Key = key;
			}
		}

		~Squares()
		{
			this._Counter = 0;
			this._Key = 0;
		}

		#region Protected Method

		protected override uint Next()
		{
			this._Counter++;
			return this.Next(this._Counter, this._Key);
		}

		/// <summary>
		/// Generate next random number.
		/// </summary>
		/// <param name="ctr">Counter-based.</param>
		/// <param name="key">Spesific key.</param>
		/// <returns></returns>
		protected uint Next(ulong ctr, ulong key)
		{
			ulong x, y, z;
			y = x = ctr * key; 
			z = y + key;
			
			x = x * x + y; 
			x = (x >> 32) | (x << 32);
			
			x = x * x + z; 
			x = (x >> 32) | (x << 32);
			
			return (uint)((x * x + y) >> 32);
		}

		#endregion Protected Method

		#region Public Method

		public override string AlgorithmName()
		{
			return "Squares";
		}

		public override void Reseed()
		{
			ulong key;
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[8];
				rng.GetNonZeroBytes(bytes);
				key = BitConverter.ToUInt64(bytes, 0);
			}
			this._Key = key;
			this._Counter = 0;
		}

		#endregion Public Method
	}
}