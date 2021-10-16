using System;
using System.Security.Cryptography;

using Litdex.Utilities.Extension;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Romu random variations, 32-bit arithmetic: Suitable only up to 2^26 output-values. Outputs 16-bit numbers.
	///		Fixed period of (2^32)-47. Must be seeded using the romuMono32_init function.
	///		Capacity = 2^27 bytes. Register pressure = 2. State size = 32 bits.
	/// </summary>
	/// <remarks>
	///		Source: https://www.romu-random.org/
	/// </remarks>
	public class RomuMono32 : Random32
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="RomuMono32"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public RomuMono32(uint seed = 0)
		{
			this._State = new uint[1];
			this.SetSeed(seed);
		}

		~RomuMono32()
		{
			this._State[0] = 0;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			uint result = this._State[0] >> 16;
			this._State[0] *= 3611795771u;
			this._State[0] = this._State[0].RotateLeft(12);
			return result;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Romu Mono 32-bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[4];
				rng.GetBytes(bytes);
#if NET5_0_OR_GREATER
				this.SetSeed(System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(bytes));
#else
				this.SetSeed(BitConverter.ToUInt32(bytes, 0));
#endif
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		public void SetSeed(uint seed)
		{
			this._State[0] = (seed & 0x1fffffffu) + 1156979152u;  // Accepts 29 seed-bits.;
		}

		/// <inheritdoc/>
		public override void SetSeed(params uint[] seed)
		{
			if (seed == null || seed.Length == 0)
			{
				throw new ArgumentNullException(nameof(seed), "Seed can't null or empty.");
			}

			if (seed.Length < this._State.Length)
			{
				throw new ArgumentException(nameof(seed), $"Seed need at least { this._State.Length } numbers.");
			}

			this.SetSeed(seed[0]);
		}

		#endregion Public Method
	}
}
