``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host] : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), X64 RyuJIT DEBUG

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|                Method |       Mean |     Error |    StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------- |-----------:|----------:|----------:|------:|------:|------:|----------:|
| CreateCaseWithOneTask |   2.904 ms | 0.1368 ms | 0.3968 ms |     - |     - |     - |    7.2 KB |
| LaunchCaseWithOneTask | 107.478 ms | 2.0267 ms | 1.8958 ms |     - |     - |     - |    7.2 KB |
