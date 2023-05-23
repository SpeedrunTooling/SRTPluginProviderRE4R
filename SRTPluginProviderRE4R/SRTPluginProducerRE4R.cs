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

            // Register pages.
            registeredPages.Add("MainHUD", async (Controller controller) => controller.Content(Properties.Resources.RE4R, "text/html", System.Text.Encoding.UTF8)); // GET: /api/v1/Plugin/SRTPluginProducerRE4R/MainHUD
            registeredPages.Add("Config", async (Controller controller) => controller.View("Config", Config));
        }

        public void Refresh()
        {
            gameMemoryRE4R = gameMemoryScanner.Refresh();
            Data = gameMemoryRE4R;
			LastUpdated = DateTime.UtcNow;
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
