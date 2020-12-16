# Litdex.Security.RNG

Library that provide basic random generator function and was inspired from Python [random.py](https://github.com/python/cpython/blob/master/Lib/random.py).

[Litdex.Security.RNG](https://github.com/Shiroechi/Litdex.Security.RNG) have already provide some basic random algorithm, so you can use it immediately rather than implement it yourself. But still [Litdex.Security.RNG](https://github.com/Shiroechi/Litdex.Security.RNG) created with extensibility in mind, so you can implement your own *random generator* with this [library](https://github.com/Shiroechi/Litdex.Security.RNG).

**Note** 
This is an individual module containing the RNG set from
[Litdex](https://github.com/Shiroechi/Litdex).

[![CodeFactor](https://www.codefactor.io/repository/github/shiroechi/litdex.security.rng/badge?style=for-the-badge)](https://www.codefactor.io/repository/github/shiroechi/litdex.security.rng)

# Download

[![Nuget](https://img.shields.io/nuget/v/litdex?label=Litdex&style=for-the-badge)](https://www.nuget.org/packages/Litdex/)

[![Nuget](https://img.shields.io/nuget/v/Litdex.Security.RNG?label=Litdex.Security.RNG&style=for-the-badge)](https://www.nuget.org/packages/Litdex.Security.RNG)

# This package contains:

- [GJrand](http://gjrand.sourceforge.net/)
- [JSF](http://burtleburtle.net/bob/rand/smallprng.html)
- [Middle Square Weyl Sequence](https://en.wikipedia.org/wiki/Middle-square_method)
- [PCG](https://en.wikipedia.org/wiki/Permuted_congruential_generator)
- [Romu](http://romu-random.org/)
- SplitMix64
- [Squares](<https://en.wikipedia.org/wiki/Counter-based_random_number_generator_(CBRNG)#Squares_RNG>)
- [Tyche](https://www.researchgate.net/publication/233997772_Fast_and_Small_Nonlinear_Pseudorandom_Number_Generators_for_Computer_Simulation)
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
