using System;
using System.Security.Cryptography;

// http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.714.1893&rep=rep1&type=pdf

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		<see cref="Tyche"/> is based on ChaCha's quarter-round.
	/// </summary>
	public class Tyche : Random32
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Tyche"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		/// <param name="idx">
		///		Unique key.
		/// </param>
		public Tyche(ulong seed = 0, uint idx = 0)
		{
			this._State = new uint[4];
			this.SetSeed(seed, idx);
		}

		~Tyche()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			this.Mix();
			return this._State[1];
		}

		/// <summary>
		///		Initialzied internal state.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		/// <param name="idx">
		///		Unique key.
		/// </param>
		protected void Init(ulong seed, uint idx)
		{
			this._State[0] = (uint)(seed / uint.MaxValue);
			this._State[1] = (uint)(seed % uint.MaxValue);
			this._State[2] = 2654435769;
			this._State[3] = idx ^ 1367130551;

			for (var i = 0; i < _InitialRoll; i++)
			{
				this.Mix();
			}
		}

		/// <summary>
		///		Update internal state based on quater round function of ChaCha stream chiper.
		/// </summary>
		protected virtual void Mix()
		{
			this._State[0] += this._State[1];
			this._State[3] ^= this._State[0];
			this._State[3] = this._State[3] << 16 | this._State[3] >> 16;

			this._State[2] += this._State[3];
			this._State[1] ^= this._State[2];
			this._State[1] = this._State[1] << 12 | this._State[1] >> 20;

			this._State[0] += this._State[1];
			this._State[3] ^= this._State[0];
			this._State[3] = this._State[3] << 8 | this._State[3] >> 24;

			this._State[2] += this._State[3];
			this._State[1] ^= this._State[2];
			this._State[1] = this._State[1] << 7 | this._State[1] >> 25;
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Tyche";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[12];
				rng.GetNonZeroBytes(bytes);
#if NET5_0_OR_GREATER
				var span = bytes.AsSpan();
				this.Init(
					seed: System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(span),
					idx: System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(8)));
#else
				this.Init(
					seed: BitConverter.ToUInt64(bytes, 0),
					idx: BitConverter.ToUInt32(bytes, 8));
#endif
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		/// </param>
		/// <param name="idx">
		///		Unique key.
		/// </param>
		public virtual void SetSeed(ulong seed, uint idx)
		{
			this.Init(seed, idx);
		}

		#endregion Public Method
	}
}