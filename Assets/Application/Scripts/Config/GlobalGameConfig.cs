using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BMProject
{
    [System.Serializable]
    public class GlobalGameConfig 
    {
        private static GlobalGameConfig s_currentConfig;
        public enum RotateType :int
        {
            None = 0,
            RightUp = 1,
            LeftUp = 2,
        }

        [SerializeField]
        public Vector2Int areaRightFront;
        [SerializeField]
        public Vector2Int areaLeftBack;
        [SerializeField]
        public RotateType rotateType = RotateType.None;

        private static string FilePath
        {
            get
            {
                string path;
#if UNITY_EDITOR
                path = "config.json";
#else
                path = Path.Combine(Application.persistentDataPath, "config.json");
#endif
                return path;
            }
        }

        public static bool HasSaveData()
        {

            string path = FilePath;
            return File.Exists(path);
        }

        public static GlobalGameConfig currentConfig
        {
            get
            {
                if (s_currentConfig == null)
                {
                    s_currentConfig = Load();
                }
                if(s_currentConfig == null)
                {
                    s_currentConfig = GetDefaultConfig();
                }
                return s_currentConfig;
            }
        }

        public void Save()
        {
            var jsonStr = JsonUtility.ToJson(this);
            string path = FilePath;
            File.WriteAllText(path , jsonStr);
        }

        private static GlobalGameConfig Load()
        {
            string path = FilePath;
            if (File.Exists(path))
            {
                string str = File.ReadAllText(path);
                return JsonUtility.FromJson<GlobalGameConfig>(str);
            }
            return null;
        }

        private static GlobalGameConfig GetDefaultConfig()
        {
            var config = new GlobalGameConfig();
            /* TX Shadow Bomb Field */
            config.areaRightFront = new Vector2Int(600, 830);
            config.areaLeftBack = new Vector2Int(390, 600);
            config.rotateType = RotateType.None;
            return config;
        }


    }
}