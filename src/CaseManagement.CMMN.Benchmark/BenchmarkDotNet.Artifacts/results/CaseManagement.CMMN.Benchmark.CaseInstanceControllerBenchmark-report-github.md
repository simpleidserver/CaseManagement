``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362, VM=VMware
Intel Core i5-6440HQ CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host] : .NET Core 2.2.8 (CoreCLR 4.6.28207.03, CoreFX 4.6.28208.02), X64 RyuJIT DEBUG  [AttachedDebugger]

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|                Method |     Mean |    Error |   StdDev |   Median |     Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------- |---------:|---------:|---------:|---------:|----------:|------:|------:|----------:|
| CreateCaseWithOneTask | 123.9 ms |  8.83 ms | 25.05 ms | 116.1 ms |         - |     - |     - |   8.61 KB |
| LaunchCaseWithOneTask | 392.8 ms | 28.68 ms | 81.35 ms | 385.4 ms | 1000.0000 |     - |     - |   8.61 KB |
