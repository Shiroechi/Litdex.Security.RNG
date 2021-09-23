# Litdex.Security.RNG

Library that provide basic random generator function and was inspired from Python [random.py](https://github.com/python/cpython/blob/master/Lib/random.py).

[Litdex.Security.RNG](https://github.com/Shiroechi/Litdex.Security.RNG) have already provide some basic random algorithm, so you can use it immediately rather than implement it yourself. But, still [Litdex.Security.RNG](https://github.com/Shiroechi/Litdex.Security.RNG) created with extensibility in mind, so you can implement your own *random generator* with this [library](https://github.com/Shiroechi/Litdex.Security.RNG).

**Note:** This is sub-modlue of [Litdex](https://github.com/Shiroechi/Litdex).

[![CodeFactor](https://www.codefactor.io/repository/github/shiroechi/litdex.security.rng/badge?style=for-the-badge)](https://www.codefactor.io/repository/github/shiroechi/litdex.security.rng)

# Download

[![Nuget](https://img.shields.io/nuget/v/litdex?label=Litdex&style=for-the-badge)](https://www.nuget.org/packages/Litdex/)

[![Nuget](https://img.shields.io/nuget/v/Litdex.Security.RNG?label=Litdex.Security.RNG&style=for-the-badge)](https://www.nuget.org/packages/Litdex.Security.RNG)

# This package contains:

Currently [Litdex.Security.RNG](https://github.com/Shiroechi/Litdex.Security.RNG) support this algorithm:

- [GJrand](http://gjrand.sourceforge.net/)
- [JSF](http://burtleburtle.net/bob/rand/smallprng.html)
- [Middle Square Weyl Sequence](https://en.wikipedia.org/wiki/Middle-square_method)
- [PCG](https://en.wikipedia.org/wiki/Permuted_congruential_generator)
- [Romu](http://romu-random.org/)
- SFC (Chris Doty-Humphrey's Chaotic PRNG)
- [Seiran](https://github.com/andanteyk/prng-seiran)
- [Shioi](https://github.com/andanteyk/prng-shioi)
- [Shishua](https://github.com/espadrine/shishua)
- SplitMix64
- [Squares](<https://en.wikipedia.org/wiki/Counter-based_random_number_generator_(CBRNG)#Squares_RNG>)
- [Tyche](https://www.researchgate.net/publication/233997772_Fast_and_Small_Nonlinear_Pseudorandom_Number_Generators_for_Computer_Simulation)
- [Wyrand](https://github.com/wangyi-fudan/wyhash)
- [Xoroshiro and Xoshiro](http://prng.di.unimi.it/)

All of the algorithm have passing Practrand or Test01 test. But I've never test it individually, the author who is said that their algorithm past Practrand or Test01.

You can check in this website ["PRNG Battle Royale: 47 PRNGs × 9 consoles"](https://rhet.dev/wheel/rng-battle-royale-47-prngs-9-consoles/), the writer have tested some of the algorithm that have been implemented.

# How to use

For detailed use read [How to use](https://github.com/Shiroechi/Litdex.Security.RNG/wiki/How-to-use)
or [Documentation](https://github.com/Shiroechi/Litdex.Security.RNG/wiki/Documentation)

The simple way to use

```C#
// create rng object
var rng = new Xoroshiro128plus();

// get random integer
var randomInt = rng.NextInt();
```

Want to create your own RNG?? Then read [Custom RNG](https://github.com/Shiroechi/Litdex.Security.RNG/wiki/Custom-RNG)

# Bechmark

## 32-bit RNG

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.18363.1500 (1909/November2019Update/19H2)
AMD FX-8800P Radeon R7, 12 Compute Cores 4C+8G, 1 CPU, 4 logical and 4 physical cores
.NET SDK=5.0.401
  [Host]    : .NET 5.0.10 (5.0.1021.41214), X64 RyuJIT
  MediumRun : .NET 5.0.10 (5.0.1021.41214), X64 RyuJIT

Job=MediumRun  IterationCount=15  LaunchCount=2  
WarmupCount=10  

```
|        Method |                 rand |         Mean |      Error |     StdDev |          Min |          Max | Ratio | RatioSD | Rank |  Gen 0 | Allocated |
|-------------- |--------------------- |-------------:|-----------:|-----------:|-------------:|-------------:|------:|--------:|-----:|-------:|----------:|
| System.Random |                    ? | 5,320.380 ns | 30.5306 ns | 45.6968 ns | 5,256.942 ns | 5,395.361 ns |  1.00 |    0.00 |    1 | 0.5341 |     280 B |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |           JSF 32-bit |     9.559 ns |  0.1051 ns |  0.1507 ns |     9.291 ns |     9.994 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt | JSF 3(...)otate [24] |    10.283 ns |  0.0942 ns |  0.1410 ns |    10.099 ns |    10.522 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt | Middl(...)uence [27] |    12.428 ns |  0.0641 ns |  0.0920 ns |    12.310 ns |    12.694 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |    PCG XSH-RR 32-bit |    12.805 ns |  0.0962 ns |  0.1380 ns |    12.547 ns |    13.106 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |    PCG XSH-RS 32-bit |    10.849 ns |  0.6821 ns |  1.0210 ns |     8.613 ns |    12.409 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |     Romu Mono 32-bit |    11.347 ns |  0.0834 ns |  0.1169 ns |    11.136 ns |    11.532 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |     Romu Quad 32-bit |    10.316 ns |  0.1106 ns |  0.1621 ns |    10.075 ns |    10.737 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |     Romu Trio 32-bit |    13.664 ns |  0.1315 ns |  0.1886 ns |    13.373 ns |    14.166 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |           SFC 32-bit |    10.270 ns |  0.0651 ns |  0.0934 ns |    10.090 ns |    10.427 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |              Squares |    13.174 ns |  0.0892 ns |  0.1307 ns |    12.936 ns |    13.476 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |                Tyche |    49.391 ns |  0.2749 ns |  0.4114 ns |    48.846 ns |    49.951 ns |     ? |       ? |    1 |      - |         - |
|               |                      |              |            |            |              |              |       |         |      |        |           |
|       NextInt |              Tyche-i |    20.308 ns |  0.1033 ns |  0.1481 ns |    20.078 ns |    20.541 ns |     ? |       ? |    1 |      - |         - |

## 64-bit RNG

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.18363.1500 (1909/November2019Update/19H2)
AMD FX-8800P Radeon R7, 12 Compute Cores 4C+8G, 1 CPU, 4 logical and 4 physical cores
.NET SDK=5.0.401
  [Host]    : .NET 5.0.10 (5.0.1021.41214), X64 RyuJIT
  MediumRun : .NET 5.0.10 (5.0.1021.41214), X64 RyuJIT

Job=MediumRun  IterationCount=15  LaunchCount=2  
WarmupCount=10  

```
|   Method |                rand |      Mean |     Error |    StdDev |       Min |       Max | Rank |  Gen 0 | Allocated |
|--------- |-------------------- |----------:|----------:|----------:|----------:|----------:|-----:|-------:|----------:|
| NextLong |     Romu Duo 64-bit |  5.547 ns | 0.0658 ns | 0.0985 ns |  5.356 ns |  5.748 ns |    1 |      - |         - |
| NextLong |  Romu Duo Jr 64-bit |  5.583 ns | 0.0647 ns | 0.0948 ns |  5.415 ns |  5.853 ns |    1 |      - |         - |
| NextLong |               Shioi |  5.779 ns | 0.0587 ns | 0.0861 ns |  5.635 ns |  5.930 ns |    2 |      - |         - |
| NextLong |              Seiran |  5.815 ns | 0.0835 ns | 0.1198 ns |  5.623 ns |  6.113 ns |    2 |      - |         - |
| NextLong |      Xoroshiro 128+ |  5.835 ns | 0.0745 ns | 0.1068 ns |  5.632 ns |  6.007 ns |    2 |      - |         - |
| NextLong |     Xoroshiro 128** |  5.965 ns | 0.0624 ns | 0.0914 ns |  5.841 ns |  6.186 ns |    3 |      - |         - |
| NextLong | PCG RXS-M-XS 64-bit |  6.162 ns | 0.0653 ns | 0.0957 ns |  5.981 ns |  6.345 ns |    4 |      - |         - |
| NextLong |          SplitMix64 |  6.751 ns | 0.0550 ns | 0.0753 ns |  6.642 ns |  6.948 ns |    5 |      - |         - |
| NextLong |     Xoroshiro 128++ |  6.812 ns | 0.5558 ns | 0.8318 ns |  5.865 ns |  7.779 ns |    5 |      - |         - |
| NextLong |    Romu Trio 64-bit |  7.095 ns | 0.0581 ns | 0.0851 ns |  6.964 ns |  7.266 ns |    5 |      - |         - |
| NextLong |    Romu Quad 64-bit |  7.684 ns | 0.0862 ns | 0.1291 ns |  7.498 ns |  7.907 ns |    6 |      - |         - |
| NextLong |          SFC 64-bit |  7.842 ns | 0.0950 ns | 0.1393 ns |  7.582 ns |  8.023 ns |    7 |      - |         - |
| NextLong |          JSF 64-bit |  8.376 ns | 0.1285 ns | 0.1923 ns |  8.171 ns |  8.872 ns |    8 |      - |         - |
| NextLong |       Xoshiro 256** |  9.460 ns | 0.0708 ns | 0.0993 ns |  9.350 ns |  9.676 ns |    9 |      - |         - |
| NextLong |        Xoshiro 256+ |  9.494 ns | 0.0752 ns | 0.1125 ns |  9.316 ns |  9.661 ns |    9 |      - |         - |
| NextLong |       Xoshiro 256++ |  9.692 ns | 0.0936 ns | 0.1401 ns |  9.482 ns |  9.940 ns |   10 |      - |         - |
| NextLong |              Wyrand | 11.949 ns | 0.1327 ns | 0.1986 ns | 11.639 ns | 12.344 ns |   11 |      - |         - |
| NextLong |       Xoshiro 512++ | 17.201 ns | 0.1145 ns | 0.1713 ns | 16.926 ns | 17.487 ns |   12 |      - |         - |
| NextLong |        Xoshiro 512+ | 17.292 ns | 0.2144 ns | 0.3005 ns | 16.951 ns | 17.872 ns |   12 |      - |         - |
| NextLong |       Xoshiro 512** | 17.425 ns | 0.2050 ns | 0.3069 ns | 16.905 ns | 17.904 ns |   12 |      - |         - |
| NextLong |              Gjrand | 25.934 ns | 0.2945 ns | 0.4408 ns | 24.915 ns | 26.314 ns |   13 |      - |         - |
| NextLong |             Shishua | 30.672 ns | 0.4714 ns | 0.6909 ns | 29.746 ns | 31.842 ns |   14 | 0.0306 |      16 B |

# Contribute

Feel free to open new [issue](https://github.com/Shiroechi/Litdex.Security.RNG/issues/new) or [PR](https://github.com/Shiroechi/Litdex.Security.RNG/pulls).

# Donation

Like this library? Please consider donation

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/X8X81SP2L)
