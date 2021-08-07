using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
	/// <summary>
	///		Shift, Shuffle, Add PRNG. 
	/// </summary>
	/// <remarks>
	///		Still in experimental. Use with your own risk.
	///		
	///		<para>https://github.com/espadrine/shishua</para>
	/// </remarks>
	public class Shishua : Random64
	{
		#region Member

		internal static readonly ulong[] PHI =
		{
			0x9E3779B97F4A7C15, 0xF39CC0605CEDC834, 0x1082276BF3A27251, 0xF86C6A11D0C18E95,
			0x2767F0B153D27B7F, 0x0347045B5BF1827F, 0x01886F0928403002, 0xC1D64BA40F335E36,
			0xF06AD7AE9717877E, 0x85839D6EFFBD7DC6, 0x64D325D1C5371682, 0xCADD0CCCFDFFBBE1,
			0x626E33B8D04B4331, 0xBBF73C790D94F79D, 0x471C4AB3ED3D82A5, 0xFEC507705E4AE6E5,
		};

		// Note: While it is an array, a "lane" refers to 4 consecutive ulong.
		// RNG state.
		private ulong[] _State = new ulong[16]; // 4 lanes
		private ulong[] _Output = new ulong[16]; // 4 lanes, 2 parts
		private ulong[] _Counter = new ulong[4]; // 1 lane

		// register the current index of output
		private byte _OutputIndex = 0;

		#endregion Member

		#region Constructor & Destructor

		/// <summary>
		///		Create an instance of <see cref="Shishua"/> object.
		/// </summary>
		/// <param name="seed">
		///		W state.
		/// </param>
		public Shishua(ulong[] seed = null)
		{
			if (seed.Length < 4)
			{
				throw new ArgumentException("Seed must contain at least 4 numbers.", nameof(seed));
			}
			else if (seed == null)
			{
				this.Reseed();
			}
			else
			{
				this.SetSeed(seed);
			}
		}

		~Shishua()
		{
			this._State = null;
			this._Output = null;
			this._Counter = null;
		}

		#endregion Constructor & Destructor

		#region Protected Method

		/// <inheritdoc/>
		protected override ulong Next()
		{
			if (this._OutputIndex < this._Output.Length)
			{
				var result = this._Output[this._OutputIndex];
				this._OutputIndex++;
				return result;
			}
			else
			{
				this.Mix(128);
				this._OutputIndex = 1;
				return this._Output[0];
			}
		}

		/// <summary>
		///		Mix the internal state.
		/// </summary>
		protected void Mix(int size)
		{
			for (var i = 0; i < size; i += 128)
			{
				// Write the current output block to state if it is not NULL
				//if (buf != NULL)
				//{
				//	for (size_t j = 0; j < 16; j++)
				//	{
				//		prng_write_le64(b, state->output[j]);
				//		b += 8;
				//	}
				//}

				for (var j = 0; j < 2; j++)
				{
					int stateCounter = j * 8;
					int outputCounter = j * 4;

					ulong[] t = new ulong[8]; // temp buffer

					for (var k = 0; k < 4; k++)
					{
						this._State[stateCounter + k + 4] += this._Counter[k];
					}

					var shuf_offsets = new byte[]
					{
						2,3,0,1, 5,6,7,4,// left
						3,0,1,2, 6,7,4,5 // right

					};

					for (var k = 0; k < 8; k++)
					{
						t[k] = (this._State[stateCounter + shuf_offsets[k]] >> 32) | (this._State[stateCounter + shuf_offsets[k + 8]] << 32);
					}

					for (var k = 0; k < 4; k++)
					{
						ulong u_lo = this._State[stateCounter + k + 0] >> 1;
						ulong u_hi = this._State[stateCounter + k + 4] >> 3;

						this._State[stateCounter + k + 0] = u_lo + t[k + 0];
						this._State[stateCounter + k + 4] = u_hi + t[k + 4];

						this._Output[outputCounter + k] = u_lo ^ t[k + 4];
					}
				}

				// Merge together.
				for (var j = 0; j < 4; j++)
				{

					this._Output[j + 8] = this._State[j + 0] ^ this._State[j + 12];
					this._Output[j + 12] = this._State[j + 8] ^ this._State[j + 4];

					this._Counter[j] += (ulong)(7 - (j * 2)); // 7, 5, 3, 1
				}
			}
		}

		#endregion Protected Method

		#region Public Method

		/// <inheritdoc/>
		public override string AlgorithmName()
		{
			return "Shishua";
		}

		/// <inheritdoc/>
		public override void Reseed()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var bytes = new byte[64];
				rng.GetNonZeroBytes(bytes);

				this.SetSeed(new ulong[] {
					BitConverter.ToUInt64(bytes, 0),
					BitConverter.ToUInt64(bytes, 8),
					BitConverter.ToUInt64(bytes, 16),
					BitConverter.ToUInt64(bytes, 24)
				});
			}
		}

		/// <summary>
		///		Set <see cref="RNG"/> seed manually.
		/// </summary>
		/// <param name="seed">
		///		A array of seed numbers.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///		Array of seed is null or empty.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Seed need 4 numbers.
		/// </exception>
		public void SetSeed(ulong[] seed)
		{
			if (seed.Length < 4 || seed == null)
			{
				throw new ArgumentException("Seed can't null and at least need 4 seed.", nameof(seed));
			}

			Array.Copy(PHI, 0, this._State, 0, PHI.Length);

			for (var i = 0; i < 4; i++)
			{
				this._State[i * 2 + 0] ^= seed[i];           // { s0,0,s1,0,s2,0,s3,0 }
				this._State[i * 2 + 8] ^= seed[(i + 2) % 4]; // { s2,0,s3,0,s0,0,s1,0 }
			}

			for (var i = 0; i < 13; i++)
			{
				this.Mix(128);
				for (var j = 0; j < 4; j++)
				{
					this._State[j + 0] = this._State[j + 12];
					this._State[j + 4] = this._State[j + 8];
					this._State[j + 8] = this._State[j + 4];
					this._State[j + 12] = this._State[j + 0];
				}
			}
		}

		#endregion Public Method
	}
}
