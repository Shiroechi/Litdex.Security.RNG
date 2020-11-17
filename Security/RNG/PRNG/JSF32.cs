using System;
using System.Security.Cryptography;

namespace Litdex.Security.RNG.PRNG
{
/// <summary>
/// Implementation of a Bob Jenkins Small Fast (Noncryptographic) PRNGs.
///
/// http://burtleburtle.net/bob/rand/smallprng.html
/// </summary>
public class JSF32 : Random32
{
    private uint[] _Seed = new uint[4];

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="seed">Your seed.</param>
    public JSF32(uint seed = 0)
    {
        if (seed == 0)
        {
            this.Reseed();
        }
        else
        {
            this._Seed[0] = Convert.ToUInt32(0xF1EA5EED);
            this._Seed[1] = this._Seed[2] = this._Seed[3] = seed;

            for (var i = 0; i < 20; i++)
            {
                this.Next();
            }
        }
    }

    /// <summary>
    /// Destructor.
    /// </summary>
    ~JSF32()
    {
        this._Seed = null;
    }

    #region Protected Method

    /// <inheritdoc/>
    protected override uint Next()
    {
        var e = this._Seed[0] - this.Rotate(this._Seed[1], 27);
        this._Seed[0] = this._Seed[1] ^ this.Rotate(this._Seed[2], 17);
        this._Seed[1] = this._Seed[2] + this._Seed[3];
        //this.Seed[1] = this.Seed[2] + this.Rotate(this.Seed[3], 11);
        this._Seed[2] = this._Seed[3] + e;
        this._Seed[3] = e + this._Seed[0];
        return this._Seed[3];
    }

    /// <summary>
    /// Rotate the bit.
    /// </summary>
    /// <param name="value">Number to rotate.</param>
    /// <param name="shift">Bit to rotate.</param>
    /// <returns></returns>
    protected uint Rotate(uint value, int shift)
    {
        return ((value) << (shift)) | ((value) >> (32 - (shift)));
    }

    #endregion Protected Method

    #region Public Method

    /// <inheritdoc/>
    public override string AlgorithmName()
    {
        return "JSF 32 bit";
    }

    /// <inheritdoc/>
    public override void Reseed()
    {
        uint seed;
        using (var rng = new RNGCryptoServiceProvider())
        {
            var bytes = new byte[4];
            rng.GetNonZeroBytes(bytes);
            seed = BitConverter.ToUInt32(bytes, 0);
        }

        this._Seed[0] = 0xF1EA5EED;
        this._Seed[1] = this._Seed[2] = this._Seed[3] = seed;

        for (var i = 0; i < 20; i++)
        {
            this.Next();
        }
    }

    #endregion Public Method
}
}