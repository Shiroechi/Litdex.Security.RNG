# Litdex.Security.RNG

This is an individual module containing the RNG set from
[Litdex](https://github.com/Shiroechi/Litdex).

# Download

[![Nuget](https://img.shields.io/nuget/v/litdex?label=Litdex)](https://www.nuget.org/packages/Litdex/)
[![Nuget](https://img.shields.io/nuget/v/Litdex.Security.RNG?label=Litdex.Security.RNG)](https://www.nuget.org/packages/Litdex.Security.RNG)

# This package contains:

- [JSF](http://burtleburtle.net/bob/rand/smallprng.html)
- [Middle Square Weyl Sequence](https://en.wikipedia.org/wiki/Middle-square_method)
- [PCG](https://en.wikipedia.org/wiki/Permuted_congruential_generator)
- SplitMix64
- [Squares](<https://en.wikipedia.org/wiki/Counter-based_random_number_generator_(CBRNG)#Squares_RNG>)
- [WyRNG](https://github.com/wangyi-fudan/wyhash)
- [Xoroshiro](http://prng.di.unimi.it/)

# How to use

For detailed use read [How to use](https://github.com/Shiroechi/Litdex.Security.RNG/wiki/How-to-use)
or [Documentation](https://github.com/Shiroechi/Litdex.Security.RNG/wiki/Documentation)

The simple way to use

```C#
var rng = new Xoroshiro128plus();
var randomInt = rng.NextInt();
```

Want to create your own RNG?? Then read [Custom RNG](https://github.com/Shiroechi/Litdex.Security.RNG/wiki/Custom-RNG)

# Donation

Like this library? Please consider donation

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/X8X81SP2L)
