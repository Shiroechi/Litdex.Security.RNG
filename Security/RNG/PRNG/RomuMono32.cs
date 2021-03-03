using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	/// 32-bit arithmetic: Suitable only up to 2^26 output-values. Outputs 16-bit numbers.
	/// Fixed period of (2^32)-47. Must be seeded using the romuMono32_init function.
	/// Capacity = 2^27 bytes. Register pressure = 2. State size = 32 bits.
	/// </summary>
	public class RomuMono32 : Random32
	{
		#region Member

		private uint _Seed;

		#endregion Member

		#region Constructor & Destructor

		public RomuMono32(uint seed = 0)
		{
			this.SetSeed(seed);
		}

		~RomuMono32()
		{
			this._Seed = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			uint result = this._Seed >> 16;
			this._Seed *= 3611795771u;
			this._Seed = this.ROTL(this._Seed, 12);
			return result;
		}

		protected uint ROTL(uint d, int lrot)
		{
			return (d << (lrot)) | (d >> (32 - lrot));
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Romu Mono 32 bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[4];
				rng.GetNonZeroBytes(bytes);
				this._Seed = BitConverter.ToUInt32(bytes, 0);
			}
		}

		/// <summary>
		/// Set <see cref="RNG"/> seed manually.
		/// </summary>
		public void SetSeed(uint seed = 0)
		{
			this._Seed = (seed & 0x1fffffffu) + 1156979152u;  // Accepts 29 seed-bits.;
		}

		#endregion Public Method
	}
}
