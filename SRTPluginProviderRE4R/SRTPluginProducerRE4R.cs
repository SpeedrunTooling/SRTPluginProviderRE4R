using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SRTPluginBase;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SRTPluginProducerRE4R
{
    public class SRTPluginProducerRE4R : PluginBase<SRTPluginProducerRE4R>, IPluginProducer
    {
        private readonly ILogger<SRTPluginProducerRE4R> logger;
        private readonly IPluginHost pluginHost;

		// Properties
		public override IPluginInfo Info => new PluginInfo();

        // Fields
        private GameMemoryRE4RScanner gameMemoryScanner;

		public SRTPluginProducerRE4R(ILogger<SRTPluginProducerRE4R> logger, IPluginHost pluginHost) : base()
        {
            this.logger = logger;
            this.pluginHost = pluginHost;

			Process? gameProc = Process.GetProcessesByName("re4")?.FirstOrDefault();
            gameMemoryScanner = new GameMemoryRE4RScanner(gameProc);
            Configuration = (IPluginConfiguration)DbLoadConfiguration().ConfigDictionaryToModel<PluginConfiguration>();

			// Register pages.
			registeredPages.Add("MainHUD", async (Controller controller) => controller.Content(Properties.Resources.RE4R, "text/html", Encoding.UTF8)); // GET: /api/v1/Plugin/SRTPluginProducerRE4R/MainHUD
			registeredPages.Add("Config", async (Controller controller) =>
            {
				if (controller.Request.Query.ContainsKey("Config"))
                    Configuration = JsonSerializer.Deserialize<PluginConfiguration>(controller.Request.Query["Config"]);
				return controller.View("Config", Configuration as PluginConfiguration);
            });
		}

        public object? Refresh() => gameMemoryScanner.Refresh();

		public override void Dispose()
        {
            gameMemoryScanner?.Dispose();
            gameMemoryScanner = null;
			DbSaveConfiguration(((PluginConfiguration)Configuration).ModelToConfigDictionary());
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
