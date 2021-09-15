using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Implementation of a Bob Jenkins Small Fast (Noncryptographic) PRNGs.
	/// </summary>
	/// <remarks>
	///		Source: http://burtleburtle.net/bob/rand/smallprng.html
	/// </remarks>
	public class JSF32 : Random32
	{
		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="JSF32"/> object.
		/// </summary>
		/// <param name="seed">
		///		RNG seed.
		///	</param>
		public JSF32(uint seed = 0)
		{
			this._State = new uint[4];
			this.SetSeed(seed);
		}

		/// <summary>
		///		Destructor.
		/// </summary>
		~JSF32()
		{
			Array.Clear(this._State, 0, this._State.Length);
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override uint Next()
		{
			var e = this._State[0] - this.RotateLeft(this._State[1], 27);
			this._State[0] = this._State[1] ^ this.RotateLeft(this._State[2], 17);
			this._State[1] = this._State[2] + this._State[3];
			this._State[2] = this._State[3] + e;
			this._State[3] = e + this._State[0];
			return this._State[3];
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "JSF 32-bit";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[4];
				rng.GetNonZeroBytes(bytes);
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
			this._State[0] = 0xF1EA5EED;
			this._State[1] = this._State[2] = this._State[3] = seed;

			for (var i = 0; i < _InitialRoll; i++)
			{
				this.Next();
			}
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