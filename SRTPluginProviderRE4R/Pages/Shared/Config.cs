using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SRTPluginProducerRE4R.Pages.Shared
{
	public class PluginConfigurationModel : PageModel
    {
        [BindProperty]
        public PluginConfiguration Configuration { get; set; }

        public void OnGet()
        {
            // Initialize the PluginConfiguration object
            Configuration = new PluginConfiguration();
        }

        public IActionResult OnPostSaveConfiguration()
        {
            if (!ModelState.IsValid)
            {
                // Handle invalid form data
                return Page();
            }

            // Access the updated PluginConfiguration object using the Configuration property
            var updatedConfiguration = Configuration;

            // Perform any necessary processing or validation on the updatedConfiguration object

            // Update the PluginConfiguration object with the new values
            // For simplicity, let's assume you have a reference to the original configuration object
            var originalConfiguration = GetOriginalConfiguration();
            //originalConfiguration.Debug = updatedConfiguration.Debug;
            //originalConfiguration.CenterPlayerHP = updatedConfiguration.CenterPlayerHP;
            //originalConfiguration.CenterBossHP = updatedConfiguration.CenterBossHP;
            //originalConfiguration.ShowHPBars = updatedConfiguration.ShowHPBars;
            //originalConfiguration.ShowDuffle = updatedConfiguration.ShowDuffle;
            //originalConfiguration.EnemyLimit = updatedConfiguration.EnemyLimit;
            //originalConfiguration.ShowDamagedEnemiesOnly = updatedConfiguration.ShowDamagedEnemiesOnly;
            //originalConfiguration.ShowBossOnly = updatedConfiguration.ShowBossOnly;
            //originalConfiguration.ShowDifficultyAdjustment = updatedConfiguration.ShowDifficultyAdjustment;
            //originalConfiguration.ShowPTAS = updatedConfiguration.ShowPTAS;
            //originalConfiguration.ShowPosition = updatedConfiguration.ShowPosition;
            //originalConfiguration.ShowRotation = updatedConfiguration.ShowRotation;
            //originalConfiguration.FontSize = updatedConfiguration.FontSize;
            //originalConfiguration.ScalingFactor = updatedConfiguration.ScalingFactor;
            //originalConfiguration.PositionX = updatedConfiguration.PositionX;
            //originalConfiguration.PositionY = updatedConfiguration.PositionY;
            //originalConfiguration.EnemyHPPositionX = updatedConfiguration.EnemyHPPositionX;
            //originalConfiguration.EnemyHPPositionY = updatedConfiguration.EnemyHPPositionY;
            //originalConfiguration.StringFontName = updatedConfiguration.StringFontName;

            // Redirect to a success page or display a success message
            return RedirectToPage("SuccessPage");
        }

        private PluginConfiguration GetOriginalConfiguration()
        {
            // Replace this with your logic to retrieve the original configuration object
            // For example, you might fetch it from a database or another data source
            return new PluginConfiguration();
        }
    }
}
