namespace Litdex.Security.RNG {
  /// <summary>
  /// Base class for 64 bit RNG.
  /// </summary>
  public abstract class Random64 : Random {
#region Protected Method

    /// <summary>
    /// Generate next random number.
    /// </summary>
    /// <returns>A 64-bit unsigned integer.</returns>
    protected abstract ulong Next();

#endregion Protected Method

#region Public Method

    /// <inheritdoc/>
    public override string AlgorithmName() { return "Random64"; }

    /// <inheritdoc/>
    public override bool NextBoolean() { return this.Next() >> 63 == 0; }

    /// <inheritdoc/>
    public override byte[] NextBytes(int length) {
      ulong sample = 0;
      var data = new byte[length];

      for (var i = 0; i < length; i++) {
        if (i % 8 == 0) {
          sample = this.Next();
        }
        data[i] = (byte) sample;
        sample >>= 8;
      }
      return data;
    }

    /// <inheritdoc/>
    public override uint NextInt() { return (uint) this.Next(); }

    /// <inheritdoc/>
    public override ulong NextLong() { return this.Next(); }

#endregion Public Method
  }
}
