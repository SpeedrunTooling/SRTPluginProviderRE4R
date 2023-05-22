using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SRTPluginBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using SRTPluginProducerRE4R.Pages.Shared;

namespace SRTPluginProducerRE4R
{
    public class SRTPluginProducerRE4R : PluginBase<SRTPluginProducerRE4R>, IPluginProducer
    {
        private readonly ILogger<SRTPluginProducerRE4R> logger;
        private readonly IPluginHost pluginHost;
        private readonly PluginConfiguration config;

        // Properties
        public override IPluginInfo Info => new PluginInfo();
        public object? Data { get; private set; }
        public DateTime? LastUpdated { get; private set; }

        // Fields
        private GameMemoryRE4RScanner gameMemoryScanner;
        private IGameMemoryRE4R gameMemoryRE4R;

        public SRTPluginProducerRE4R(ILogger<SRTPluginProducerRE4R> logger, IPluginHost pluginHost) : base()
        {
            this.logger = logger;
            this.pluginHost = pluginHost;

            Process? gameProc = Process.GetProcessesByName("re4")?.FirstOrDefault();
            gameMemoryScanner = new GameMemoryRE4RScanner(gameProc);
            config = DbLoadConfiguration().ConfigDictionaryToModel<PluginConfiguration>();
		}

        public void Refresh()
        {
            gameMemoryRE4R = gameMemoryScanner.Refresh();
            Data = gameMemoryRE4R;
            LastUpdated = DateTime.UtcNow;
        }

        public async Task<IActionResult> HttpHandlerAsync(Controller controller)
        {
            string command = controller.RouteData.Values["Command"] as string;
            switch (command)
            {
                // GET: /api/v1/Plugin/SRTPluginProducerRE4R/Config
                case "Config":
                    {
                        try
                        {
							return controller.View(command, config);
                        }
                        catch (Exception ex)
                        {
                            return controller.BadRequest(ex?.ToString());
                        }
                    }

                // Example of handling unknown http requests.
                // GET: /api/v1/Plugin/SRTPluginProducerRE4R/rksjbvgjbaethkae
                default:
                    {
                        return controller.NotFound($"Unknown command: {((IDictionary<string, object>)controller.RouteData.Values)["Command"]}{Environment.NewLine}Parameters: {controller.Request.Query.Select(a => $"\"{a.Key}\"=\"{a.Value}\"").Aggregate((o, n) => $"{o}, {n}")}");
                    }
            }
        }

        public override void Dispose()
        {
            gameMemoryScanner?.Dispose();
            gameMemoryScanner = null;
            gameMemoryRE4R = null;
			DbSaveConfiguration(config.ModelToConfigDictionary());
		}

        public override async ValueTask DisposeAsync()
        {
            Dispose();
            await Task.CompletedTask;
        }

        public bool Equals(IPlugin? other) => Equals(this, other);
        public bool Equals(IPluginProducer? other) => Equals(this, other);


        // OLD CODE

        //private Process process;
        //private GameMemoryRE4RScanner gameMemoryScanner;
        //private Stopwatch stopwatch;
        //public IPluginInfo Info => new PluginInfo();
        //public bool GameRunning
        //{
        //    get
        //    {
        //        if (gameMemoryScanner != null && !gameMemoryScanner.ProcessRunning)
        //        {
        //            process = GetProcess();
        //            if (process != null)
        //                gameMemoryScanner.Initialize(process); // Re-initialize and attempt to continue.
        //        }

        //        return gameMemoryScanner != null && gameMemoryScanner.ProcessRunning;
        //    }
        //}

        //public int Startup(IPluginHostDelegates hostDelegates)
        //{
        //    this.hostDelegates = hostDelegates;
        //    process = GetProcess();
        //    gameMemoryScanner = new GameMemoryRE4RScanner(process);
        //    stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    return 0;
        //}

        //public int Shutdown()
        //{
        //    gameMemoryScanner?.Dispose();
        //    gameMemoryScanner = null;
        //    stopwatch?.Stop();
        //    stopwatch = null;
        //    return 0;
        //}

        //public object PullData()
        //{
        //    try
        //    {
        //        if (!GameRunning) // Not running? Bail out!
        //            return null;

        //        if (stopwatch.ElapsedMilliseconds >= 2000L)
        //        {
        //            gameMemoryScanner.UpdatePointers();
        //            stopwatch.Restart();
        //        }
        //        return gameMemoryScanner.Refresh();
        //    }
        //    catch (Win32Exception ex)
        //    {
        //        if ((ProcessMemory.Win32Error)ex.NativeErrorCode != ProcessMemory.Win32Error.ERROR_PARTIAL_COPY)
        //            hostDelegates.ExceptionMessage(ex);// Only show the error if its not ERROR_PARTIAL_COPY. ERROR_PARTIAL_COPY is typically an issue with reading as the program exits or reading right as the pointers are changing (i.e. switching back to main menu).

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        hostDelegates.ExceptionMessage(ex);
        //        return null;
        //    }
        //}

        //private Process GetProcess() => Process.GetProcessesByName("re4")?.FirstOrDefault() ?? Process.GetProcessesByName("re4demo")?.FirstOrDefault();
    }
}
