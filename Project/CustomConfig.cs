namespace AdventOfCode2024;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Filters;

public class CustomConfig : ManualConfig
{
    public CustomConfig(string benchmarkName)
    {
        AddFilter(new NameFilter(name => name.Contains(benchmarkName)));
        Add(DefaultConfig.Instance.GetExporters().ToArray());
        Add(DefaultConfig.Instance.GetLoggers().ToArray());
        Add(DefaultConfig.Instance.GetColumnProviders().ToArray());

    }
}
