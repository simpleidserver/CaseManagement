``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host] : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), X64 RyuJIT DEBUG  [AttachedDebugger]

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|                Method |       Mean |     Error |   StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------- |-----------:|----------:|---------:|------:|------:|------:|----------:|
| CreateCaseWithOneTask |   9.013 ms | 0.4803 ms | 1.307 ms |     - |     - |     - |   8.71 KB |
| LaunchCaseWithOneTask | 235.238 ms | 4.6902 ms | 4.606 ms |     - |     - |     - |   8.71 KB |
