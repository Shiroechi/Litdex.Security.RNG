using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG {
  /// <summary>
  /// Xoshiro512**
  /// http://xoshiro.di.unimi.it/xoshiro512starstar.c
  /// </summary>
  public class Xoshiro512starstar : Random64 {
    private ulong[] _State = null;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public Xoshiro512starstar() {
      this._State = new ulong[8];
      this.Reseed();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public Xoshiro512starstar(ulong[] seed) {
      this._State = new ulong[8];
      if (seed.Length < 8) {
        throw new Exception("The generator need 8 seed, your seed " +
                            seed.Length);
      }

      for (var i = 0; i < 8; i++) {
        this._State[i] = seed[i];
      }
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~Xoshiro512starstar() { this._State = null; }

#region Protected Method

    /// <inheritdoc/>
    protected override ulong Next() {
      var result = this.RotateLeft(this._State[1] * 5, 7) * 9;

      var t = this._State[1] << 11;

      this._State[2] ^= this._State[0];
      this._State[5] ^= this._State[1];
      this._State[1] ^= this._State[2];
      this._State[7] ^= this._State[3];
      this._State[3] ^= this._State[4];
      this._State[4] ^= this._State[5];
      this._State[0] ^= this._State[6];
      this._State[6] ^= this._State[7];

      this._State[6] ^= t;

      this._State[7] = this.RotateLeft(this._State[7], 21);

      return result;
    }

    protected ulong RotateLeft(ulong val, int shift) {
      return (val << shift) | (val >> (64 - shift));
    }

#endregion Protected Method

#region Public Method

    /// <inheritdoc/>
    public override string AlgorithmName() { return "Xoshiro 512**"; }

    /// <inheritdoc/>
    public override void Reseed() {
      var bytes = new byte[8];
      using(var rng = new RNGCryptoServiceProvider()) {
        rng.GetNonZeroBytes(bytes);
        this._State[0] = BitConverter.ToUInt64(bytes, 0);
        rng.GetNonZeroBytes(bytes);
        this._State[1] = BitConverter.ToUInt64(bytes, 0);
        rng.GetNonZeroBytes(bytes);
        this._State[2] = BitConverter.ToUInt64(bytes, 0);
        rng.GetNonZeroBytes(bytes);
        this._State[3] = BitConverter.ToUInt64(bytes, 0);
        rng.GetNonZeroBytes(bytes);
        this._State[4] = BitConverter.ToUInt64(bytes, 0);
        rng.GetNonZeroBytes(bytes);
        this._State[5] = BitConverter.ToUInt64(bytes, 0);
        rng.GetNonZeroBytes(bytes);
        this._State[6] = BitConverter.ToUInt64(bytes, 0);
        rng.GetNonZeroBytes(bytes);
        this._State[7] = BitConverter.ToUInt64(bytes, 0);
      }
    }

    /// <summary>
    /// This is the jump function for the generator. It is equivalent
    /// to 2^256 calls to next(); it can be used to generate 2^256
    /// non-overlapping subsequences for parallel computations.
    /// </summary>
    public void NextJump() {
      ulong[] JUMP = {0x33ed89b6e7a353f9, 0x760083d7955323be,
                      0x2837f2fbb5f22fae, 0x4b8c5674d309511c,
                      0xb11ac47a7ba28c25, 0xf1be7667092bcc1c,
                      0x53851efdb6df0aaf, 0x1ebbc8b23eaf25db};

      var s = new ulong[8];

      for (var i = 0; i < 8; i++) {
        for (var b = 0; b < 64; b++) {
          if ((JUMP[i] & ((1UL) << b)) != 0) {
            for (var w = 0; w < 8; w++) {
              s[w] ^= this._State[w];
            }
          }
          this.NextLong();
        }
      }

      for (var i = 0; i < 8; i++) {
        this._State[i] = s[i];
      }
    }

#endregion Public
  }
}
