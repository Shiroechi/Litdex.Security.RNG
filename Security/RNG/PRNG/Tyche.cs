using System;
using System.Security.Cryptography;

// http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.714.1893&rep=rep1&type=pdf

namespace Litdex.Security.RNG.PRNG {
  /// <summary>
  /// <see cref="Tyche"/> is based on ChaCha's quarter-round.
  /// </summary>
  public class Tyche : Random32 {
#region Member

    private uint _A, _B, _C, _D;

#endregion Member

#region Constructor &Destructor

    public Tyche(ulong seed = 0, uint idx = 0) {
      this.Init(seed, idx);
      for (var i = 0; i < 20; i++) {
        this.Mix();
      }
    }

    ~Tyche() { this._A = this._B = this._C = this._D = 0; }

#endregion Constructor &Destructor

#region Protected Method

    /// <inheritdoc/>
    protected override uint Next() {
      this.Mix();
      return this._B;
    }

    /// <summary>
    /// Initialzied internal state.
    /// </summary>
    /// <param name="seed"></param>
    /// <param name="idx"></param>
    protected void Init(ulong seed, uint idx) {
      this._A = (uint)(seed / uint.MaxValue);
      this._B = (uint)(seed % uint.MaxValue);
      this._C = 2654435769;
      this._D = idx ^ 1367130551;
    }

    /// <summary>
    /// Update internal state based on quater round function of ChaCha stream
    /// chiper.
    /// </summary>
    protected void Mix() {
      this._A += this._B;
      this._D ^= this._A;
      this._D = this._D << 16 | this._D >> 16;

      this._C += this._D;
      this._B ^= this._C;
      this._B = this._B << 12 | this._B >> 20;

      this._A += this._B;
      this._D ^= this._A;
      this._D = this._D << 8 | this._D >> 24;

      this._C += this._D;
      this._B ^= this._C;
      this._B = this._B << 7 | this._B >> 25;
    }

#endregion Protected Method

#region Public Method

    /// <inheritdoc/>
    public override string AlgorithmName() { return "Tyche"; }

    /// <inheritdoc/>
    public override void Reseed() {
      using(var rng = new RNGCryptoServiceProvider()) {
        var bytes = new byte[8];
        rng.GetNonZeroBytes(bytes);
        this.Init(BitConverter.ToUInt32(bytes, 0), 0);
      }

      for (var i = 0; i < 20; i++) {
        this.Mix();
      }
    }

#endregion Public Method
  }
}
