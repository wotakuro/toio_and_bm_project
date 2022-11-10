using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BMProject.UI;

namespace BMProject.UI
{
    [RequireComponent(typeof(BmUISprite))]
    public class BmUISpriteConfigOverride : MonoBehaviour
    {
        [System.Serializable]
        struct OverrideFloatParam
        {
            [SerializeField]
            public bool flag;
            [SerializeField]
            public float param;
        }
        [System.Serializable]
        struct OverrideVector2Param
        {
            [SerializeField]
            public bool flag;
            [SerializeField]
            public Vector2 param;
        }
        [System.Serializable]
        struct OverrideHorizontalPoint
        {
            [SerializeField]
            public bool flag;
            [SerializeField]
            public BmUISprite.HorizontalPoint param;
        }
        [System.Serializable]
        struct OverrideVerticalPointPoint
        {
            [SerializeField]
            public bool flag;
            [SerializeField]
            public BmUISprite.VerticalPoint param;
        }

        [SerializeField]
        private bool isRotateScreen;
        [SerializeField]
        private OverrideFloatParam overrideFixedWidth;
        [SerializeField]
        private OverrideVector2Param overridePosition;
        [SerializeField]
        private OverrideHorizontalPoint overrideHorizontalPoint;
        [SerializeField]
        private OverrideVerticalPointPoint overrideVerticalPoint;

        private BmUISprite bmUI;
        // Start is called before the first frame update
        void Awake()
        {
            bmUI = this.gameObject.GetComponent<BmUISprite>();
            this.ExecuteOverride();
        }
        void ExecuteOverride()
        {
            bool configIsRotate = false;
            switch (GlobalGameConfig.currentConfig.rotateType)
            {
                case GlobalGameConfig.RotateType.None:
                    configIsRotate = false;
                    break;
                case GlobalGameConfig.RotateType.RightUp:
                    configIsRotate = true;
                    break;
                case GlobalGameConfig.RotateType.LeftUp:
                    configIsRotate = true;
                    break;
            }
            // ê›íËÇ∆à·Ç§Ç»ÇÁèàóùÇµÇ»Ç¢
            if (configIsRotate != this.isRotateScreen)
            {
                return;
            }
            if (overrideFixedWidth.flag)
            {
                bmUI.SetFixedWidth(overrideFixedWidth.param);
            }
            if (overridePosition.flag)
            {
                bmUI.SetPosition(overridePosition.param);
            }
            if (overrideHorizontalPoint.flag)
            {
                bmUI.SetHorizontalPoint(overrideHorizontalPoint.param);
            }
            if (overrideVerticalPoint.flag)
            {
                bmUI.SetVerticalPoint(overrideVerticalPoint.param);
            }
        }

#if UNITY_EDITOR
        // Update is called once per frame
        void Update()
        {
            ExecuteOverride();
        }
#endif
    }
}