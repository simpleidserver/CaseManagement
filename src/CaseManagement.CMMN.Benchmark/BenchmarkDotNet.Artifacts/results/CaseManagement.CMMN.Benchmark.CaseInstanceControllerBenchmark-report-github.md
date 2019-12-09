``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host] : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), X64 RyuJIT

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
|                      Method |         Mean |     Error |    StdDev |      Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |-------------:|----------:|----------:|-----------:|------:|------:|----------:|
|          CreateCaseInstance |     1.069 ms | 0.0621 ms | 0.1782 ms |          - |     - |     - |  11.52 KB |
| LaunchEventListener2Seconds | 2,006.229 ms | 1.3547 ms | 1.2672 ms | 64000.0000 |     - |     - |  11.52 KB |
