using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Implementation of Small, Fast, Counting (SFC) 64-bit generator of Chris Doty-Humphrey.
	///		The original source is the PractRand test suite
	/// 
	/// </summary>
	/// <remarks>
	///		<para>
	///			Source: http://pracrand.sourceforge.net/
	///		</para>
	///		<para>
	///			https://github.com/bashtage/randomgen/blob/main/randomgen/src/sfc/
	///		</para>
	///	</remarks>
	public class SFC64 : Random64
	{
		#region Member

		private ulong[] _Seed = new ulong[3];
		private ulong _Counter;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="SFC64"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		///	</param>
		public SFC64(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong counter = 0)
		{
			this.SetSeed(seed1, seed2, seed3, counter);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~SFC64()
		{
			this._Seed = null;
		}

		#endregion Constructor & Destructor

		#region	Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			ulong result = this._Seed[0] + this._Seed[1] + this._Counter;
			this._Counter++;
			this._Seed[0] = this._Seed[1] ^ (this._Seed[1] >> 11);
			this._Seed[1] = this._Seed[2] + (this._Seed[2] << 3);
			this._Seed[2] = (this._Seed[2] << 24) | (this._Seed[2] >> 40 /* 64 - 24 */);
			this._Seed[2] += result;
			return result;
		}

		#endregion Protected Method

		#region	Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "SFC 64 bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[24];
				rng.GetBytes(bytes);
				this.SetSeed(
					seed1: BitConverter.ToUInt64(bytes, 0),
					seed2: BitConverter.ToUInt64(bytes, 8),
					seed3: BitConverter.ToUInt64(bytes, 16),
					1
					);
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public void SetSeed(ulong seed1 = 0, ulong seed2 = 0, ulong seed3 = 0, ulong counter = 0)
		{
			this._Seed[0] = seed1;
			this._Seed[1] = seed2;
			this._Seed[2] = seed3;
			this._Counter = counter;

			// todo: max loop 18?
			for (var i = 0; i < 12; i++)
			{
				this.Next();
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public void SetSeed(ulong[] seed, ulong counter = 0)
		{
			this.SetSeed(seed[0], seed[1], seed[2], 0);
		}

		#endregion	Public
	}
}