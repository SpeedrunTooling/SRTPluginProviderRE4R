using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SRTPluginBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace SRTPluginProducerRE4R
{
    public class SRTPluginProducerRE4R : PluginBase<SRTPluginProducerRE4R>, IPluginProducer
    {
        private readonly ILogger<SRTPluginProducerRE4R> logger;
        private readonly IPluginHost pluginHost;
        public static PluginConfiguration Config { get; set; }

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
			Config = DbLoadConfiguration().ConfigDictionaryToModel<PluginConfiguration>();
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
							return controller.View(command, Config);
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
			DbSaveConfiguration(Config.ModelToConfigDictionary());
		}
        public override async ValueTask DisposeAsync()
        {
            Dispose();
            await Task.CompletedTask;
        }
        public bool Equals(IPlugin? other) => Equals(this, other);
        public bool Equals(IPluginProducer? other) => Equals(this, other);
    }
}
