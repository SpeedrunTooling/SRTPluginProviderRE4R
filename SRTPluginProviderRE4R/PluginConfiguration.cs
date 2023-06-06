using SRTPluginBase;

namespace SRTPluginProducerRE4R
{
	public class PluginConfiguration : IPluginConfiguration
	{
		public bool Debug { get; set; }
		public bool AlignInfoTop { get; set; }
		// public bool ShowInventory { get; set; }
		public bool HideUICutscene { get; set; }
		public bool HideUIInventory { get; set; }
		public bool HideUIPause { get; set; }
		public bool HideUIInShop { get; set; }
		public bool ShowIGT { get; set; }
		public bool ShowHPBars { get; set; }
		public bool ShowDuffle { get; set; }
		public int EnemyLimit { get; set; }
		public bool ShowDamagedEnemiesOnly { get; set; }
		public bool ShowBossOnly { get; set; }
		public bool ShowDifficultyAdjustment { get; set; }
		public bool ShowPTAS { get; set; }
		public bool ShowPosition { get; set; }
		public bool ShowRotation { get; set; }
		// public bool ShowMapLocations { get; set; }
		public float FontSize { get; set; }
		public float ScalingFactor { get; set; }
		public float PositionX { get; set; }
		public float PositionY { get; set; }
        public int PlayerHPType { get; set; }
        public int PlayerHPPosition { get; set; }
		public int BossHPPosition { get; set; }
		public float PlayerHPPositionX { get; set; }
        public float PlayerHPPositionY { get; set; }
        public float EnemyHPPositionX { get; set; }
		public float EnemyHPPositionY { get; set; }

		// public float InventoryPositionX { get; set; }
		// public float InventoryPositionY { get; set; }

		public string StringFontName { get; set; }

		public PluginConfiguration()
		{
			Debug = false;
			AlignInfoTop = false;
			// ShowInventory = true;
			HideUICutscene = true;
			HideUIInventory = true;
			HideUIPause = true;
			HideUIInShop = true;
			ShowIGT = true;
			ShowDuffle = true;
			ShowHPBars = true;
			EnemyLimit = -1;
			ShowDamagedEnemiesOnly = false;
			ShowBossOnly = false;
			ShowDifficultyAdjustment = true;
			ShowPTAS = true;
			// ShowMapLocations = true;
			ShowPosition = true;
			ShowRotation = true;
			FontSize = 16f;
			ScalingFactor = 1f;
			PositionX = 5f;
			PositionY = 50f;
			PlayerHPType = 0;
			PlayerHPPosition = 0;
			BossHPPosition = 0;
			PlayerHPPositionX = -1f;
            PlayerHPPositionY = -1f;
            EnemyHPPositionX = -1f;
			EnemyHPPositionY = -1f;
			// InventoryPositionX = -1f;
			// InventoryPositionY = -1f;
			StringFontName = "Courier New";
		}
	}
}
