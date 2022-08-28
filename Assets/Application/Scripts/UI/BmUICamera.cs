using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject.UI
{
    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class BmUICamera : MonoBehaviour
    {
        public enum RotateType{
            None,
            RightUp,
            LeftUp,
        }
        public static BmUICamera Instance{get;private set;}

        public Camera CameraComponent{get;private set;}

        [SerializeField]
        private RotateType m_rotateType;

        public RotateType rotateType{
            get{
                return m_rotateType;
            }
            set
            {
                m_rotateType = value;
            }
        }
        
        public Vector3 position{
            get{
                return transform.position;
            }
        }

        public Ray GetRay(Vector3 screenPosition) {
            var ray = this.CameraComponent.ScreenPointToRay(screenPosition);
            return ray;
        }

        public Vector2 virtualScreenSize{get;private set;}

        void Awake(){
            Instance = this;
            this.CameraComponent = this.GetComponent<Camera>();
            this.transform.rotation = Quaternion.identity;
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                return;
            }
#endif

            switch (GlobalGameConfig.currentConfig.rotateType)
            {
                case GlobalGameConfig.RotateType.None:
                    rotateType = RotateType.None;
                    break;
                case GlobalGameConfig.RotateType.RightUp:
                    rotateType = RotateType.RightUp;
                    break;
                case GlobalGameConfig.RotateType.LeftUp:
                    rotateType = RotateType.LeftUp;
                    break;
            }
        }


        void Update(){
            #if UNITY_EDITOR
            if(!Instance){                
                Instance = this;
            }
            if(!CameraComponent){             
                this.CameraComponent = this.GetComponent<Camera>();
            }
            #endif
            if(!CameraComponent){                
                return;
            }
            virtualScreenSize = new Vector2( CameraComponent.orthographicSize / (float)Screen.height * (float)Screen.width,
                            CameraComponent.orthographicSize);
        }
#if UNITY_EDITOR
        public void ForceUpdateImmediate()
        {
            var sprs = this.GetComponentsInChildren<BmUISprite>(true);
            foreach(var spr in sprs)
            {
                spr.ForceUpdateImmidiate();
            }
        }
#endif

        void OnDestroy(){
            if(Instance == this){
                Instance = null;
            }
        }
    }
}