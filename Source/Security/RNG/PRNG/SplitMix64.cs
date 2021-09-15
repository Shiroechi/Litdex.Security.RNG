using System;
using System.Security.Cryptography;

//http://grepcode.com/file/repository.grepcode.com/java/root/jdk/openjdk/8-b132/java/util/SplittableRandom.java

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		SplitMix64 PRNG Algorithm.
	/// </summary>
	public class SplitMix64 : Random64
	{
		/// <summary>
		///		Create an instance of <see cref="SplitMix64"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public SplitMix64(ulong seed = 0)
		{
			this._State = new ulong[1];
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~SplitMix64()
		{
			this._State[0] = 0;
		}

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			this._State[0] += 0x9E3779B97F4A7C15UL;
			var result = this._State[0];
			result = (result ^ (result >> 30)) * 0xBF58476D1CE4E5B9UL;
			result = (result ^ (result >> 27)) * 0x94D049BB133111EBUL;
			return result ^ (result >> 31);
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "SplitMix64";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[8];
				rng.GetNonZeroBytes(bytes);
#if NET5_0_OR_GREATER
				this.SetSeed(System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(bytes));
#else
				this.SetSeed(BitConverter.ToUInt64(bytes, 0));
#endif
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
			this._State[0] = seed;
		}

		#endregion Public Method
	}
}