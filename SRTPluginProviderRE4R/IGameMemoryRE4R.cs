using SRTPluginProviderRE4R.Structs;
using System;

namespace SRTPluginProviderRE4R
{
    public interface IGameMemoryRE4R
    {
        string GameName { get; }

        string VersionInfo { get; }

        GameTimer Timer { get; }

        long IGTCalculated { get; }

        long IGTCalculatedTicks { get; }

        TimeSpan IGTTimeSpan { get; }

        string IGTFormattedString { get; }
    }
}
