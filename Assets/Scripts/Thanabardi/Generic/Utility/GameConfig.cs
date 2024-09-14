using UnityEngine;

namespace Thanabardi.Generic.Utility
{
    public static class GameConfig
    {
        private static ConfigData _configData;
        public static ConfigData ConfigData
        {
            get => GetConfigData();
            private set => ConfigData = value;
        }
        private const string _CONFIG_DATA_PATH = "ConfigData";

        private static ConfigData GetConfigData()
        {
            // load configuration JSON file
            if (_configData == null)
            {
                var ConfigDataJson = Resources.Load<TextAsset>(_CONFIG_DATA_PATH);
                _configData = JsonUtility.FromJson<ConfigData>(ConfigDataJson.text);
            }
            return _configData;
        }
    }

    [SerializeField]
    public class ConfigData
    {
        public int BoardWidth;
        public int BoardHeight;
        public int InitSpawnNumber;
        public SpawnChance[] SpawnChance;
    }

    public class SpawnChance
    {
        public float Rate;
        public int SpawnNumber;
    }
}
