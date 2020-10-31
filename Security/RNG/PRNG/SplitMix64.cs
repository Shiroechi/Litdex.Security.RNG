using System;
using System.Security.Cryptography;

//http://grepcode.com/file/repository.grepcode.com/java/root/jdk/openjdk/8-b132/java/util/SplittableRandom.java#SplittableRandom.0gamma

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// SplitMix64 PRNG Algorithm.
	/// </summary>
	public class SplitMix64 : Random64
    {
		#region Member

		private ulong _Seed;

		#endregion Member

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="seed"></param>
		public SplitMix64(ulong seed = 0)
        {
            if (seed <= 0)
            {
				this.Reseed();
            }
            else
            {
                this._Seed = seed;
            }
        }
		
		/// <summary>
		/// Destructor.
		/// </summary>
		~SplitMix64()
		{
			this._Seed = 0;
		}

		#region Protected Method

		/// <summary>
		/// Generate next random number.
		/// </summary>
		/// <returns></returns>
		protected override ulong Next()
		{
			this._Seed = this._Seed + 0x9E3779B97F4A7C15UL;
			var result = this._Seed;
			result = (result ^ (result >> 30)) * 0xBF58476D1CE4E5B9UL;
			result = (result ^ (result >> 27)) * 0x94D049BB133111EBUL;
			return result ^ (result >> 31);
		}

		#endregion Protected Method

		#region Public Method

		/// <summary>
		/// The name of the algorithm this generator implements.
		/// </summary>
		/// <returns></returns>
		public override string AlgorithmName()
		{
			return "SplitMix64";
		}

		/// <summary>
		/// Seed with RNGCryptoServiceProvider.
		/// </summary>
		public override void Reseed()
		{
			var bytes = new byte[8];
			using (var rng = new RNGCryptoServiceProvider())
			{
				rng.GetNonZeroBytes(bytes);
			}
			this._Seed = BitConverter.ToUInt64(bytes, 0);
		}

		#endregion Public Method
	}
}