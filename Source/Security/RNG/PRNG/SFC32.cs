using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Implementation of Small, Fast, Counting (SFC) 32-bit generator of Chris Doty-Humphrey.
	///		The original source is the PractRand test suite
	/// </summary>
	/// <remarks>
	///		<para>
	///			Source: http://pracrand.sourceforge.net/
	///		</para>
	///		<para>
	///			https://github.com/bashtage/randomgen/blob/main/randomgen/src/sfc/
	///		</para>
	///	</remarks>
	public class SFC32 : Random32
	{
		#region Member

		private uint _Counter;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="SFC64"/> object.
		/// </summary>
		/// <param name="seed1">
		///		First seed.
		/// </param>
		/// <param name="seed2">
		///		Second seed.
		/// </param>
		/// <param name="seed3">
		///		Third seed.
		/// </param>
		/// <param name="counter">
		///		Counter number.
		/// </param>
		public SFC32(uint seed1 = 0, uint seed2 = 0, uint seed3 = 0, uint counter = 0)
		{
			this._State = new uint[3];
			this.SetSeed(seed1, seed2, seed3, counter);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~SFC32()
		{
			Array.Clear(this._State, 0, this._State.Length);
			this._Counter = 0;
		}

		#endregion Constructor & Destructor

		#region	Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			uint result = this._State[0] + this._State[1] + this._Counter;
			this._Counter++;
			this._State[0] = this._State[1] ^ (this._State[1] >> 9);
			this._State[1] = this._State[2] + (this._State[2] << 3);
			this._State[2] = this.RotateLeft(this._State[2], 21);
			this._State[2] += result;
			return result;
		}

		#endregion Protected Method

		#region	Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "SFC 32-bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[12];
				rng.GetNonZeroBytes(bytes);
#if NET5_0_OR_GREATER
				var span = new Span<byte>(bytes);
				this.SetSeed(
					seed1: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span),
					seed2: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(4)),
					seed3: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(8)),
					1);
#else
				this.SetSeed(
					seed1: BitConverter.ToUInt32(bytes, 0),
					seed2: BitConverter.ToUInt32(bytes, 4),
					seed3: BitConverter.ToUInt32(bytes, 8),
					1);
#endif
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed1">
		///		First seed.
		/// </param>
		/// <param name="seed2">
		///		Second seed.
		/// </param>
		/// <param name="seed3">
		///		Third seed.
		/// </param>
		/// <param name="counter">
		///		Counter number.
		/// </param>
		public void SetSeed(uint seed1 = 0, uint seed2 = 0, uint seed3 = 0, uint counter = 0)
		{
			this._State[0] = seed1;
			this._State[1] = seed2;
			this._State[2] = seed3;
			this._Counter = counter;

			for (var i = 0; i < _InitialRoll; i++)
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
		/// <param name="counter">
		///		Counter number.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		Array of <paramref name="seed"/> is null or empty.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Seed need 3 numbers.
		/// </exception>
		public void SetSeed(uint[] seed, uint counter = 0)
		{
			if (seed == null || seed.Length == 0)
			{
				throw new ArgumentNullException(nameof(seed), "Seed can't null or empty.");
			}

			if (seed.Length < 3)
			{
				throw new ArgumentException(nameof(seed), "Seed need 3 numbers.");
			}

			this.SetSeed(seed[0], seed[1], seed[2], 0);
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

			this.SetSeed(seed[0], seed[1], seed[2], 0);
		}

		#endregion	Public
	}
}