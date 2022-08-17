using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject.UI
{
    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class UICamera : MonoBehaviour
    {
        public enum RotateType{
            None,
            RightUp,
            LeftUp,
        }
        public static UICamera Instance{get;private set;}

        public Camera CameraComponent{get;private set;}

        [SerializeField]
        private RotateType m_rotateType;

        public RotateType Rotate{
            get{
                return m_rotateType;
            }
        }
        
        public Vector3 position{
            get{
                return transform.position;
            }
        }

        public Vector2 virtualScreenSize{get;private set;}

        void Awake(){
            Instance = this;
            this.CameraComponent = this.GetComponent<Camera>();
            this.transform.rotation = Quaternion.identity;
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

        void OnDestroy(){
            if(Instance == this){
                Instance = null;
            }
        }
    }
}