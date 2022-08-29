using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public Vector2Int areaLeftUpper;
        [SerializeField]
        public Vector2Int areaRightDowner;
        [SerializeField]
        public RotateType rotateType = RotateType.None;

        public static GlobalGameConfig currentConfig
        {
            get
            {
                if (s_currentConfig == null)
                {
                    s_currentConfig = GetDefaultConfig();
                }
                return s_currentConfig;
            }
        }

        private static GlobalGameConfig GetDefaultConfig()
        {
            var config = new GlobalGameConfig();
            /* TX Shadow Bomb Field */
            config.areaLeftUpper = new Vector2Int(600, 830);
            config.areaRightDowner = new Vector2Int(390, 600);
            config.rotateType = RotateType.RightUp;
            /* toio playmat default */
            /*
            config.areaLeftUpper = new Vector2Int(46, 46);
            config.areaRightDowner = new Vector2Int(312, 239);
            */

            return config;
        }


    }
}