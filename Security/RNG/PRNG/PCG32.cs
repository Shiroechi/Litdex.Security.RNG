using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG {
  /// <summary>
  /// Implemetation of Permuted Congruential Generator
  /// "Minimal C implementation" from
  /// http://www.pcg-random.org/download.html
  /// </summary>
  public class PCG32 : Random64 {
#region Member

    private ulong _Seed; // state in original code
    private ulong _Increment;

#endregion Member

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="seed">Seed.</param>
    /// <param name="increment">Increment.</param>
    public PCG32(ulong seed = 0, ulong increment = 0) {
      if (seed <= 0 || increment <= 0) {
        this.Reseed();
      } else {
        this._Seed = seed;
        this._Increment = increment;
      }
    }

    /// <summary>
    /// Destructor.
    /// </summary>
    ~PCG32() {
      this._Seed = 0;
      this._Increment = 0;
    }

#region Protected Method

    /// <inheritdoc/>
    protected override ulong Next() {
      var oldseed = this._Seed;
      this._Seed = oldseed * 6364136223846793005 + (this._Increment | 1);
      var xorshifted = (uint)((oldseed >> 18) ^ oldseed) >> 27;
      var rot = (uint)(oldseed >> 59);
      return (xorshifted >> (int) rot) | (xorshifted << (int)((-rot) & 31));
    }

#endregion Protected Method

#region Public Method

    /// <inheritdoc/>
    public override string AlgorithmName() { return "PCG32"; }

    /// <inheritdoc/>
    public override void Reseed() {
      var bytes = new byte[8];
      using(var rng = new RNGCryptoServiceProvider()) {
        rng.GetNonZeroBytes(bytes);
        this._Seed = BitConverter.ToUInt64(bytes, 0);
        rng.GetNonZeroBytes(bytes);
        this._Increment = BitConverter.ToUInt64(bytes, 0);
      }
    }

#endregion Public Method
  }
}
