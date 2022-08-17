using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject.UI
{
    [ExecuteAlways]
    public class UISprite : MonoBehaviour
    {
        public enum HorizontalPoint :byte{
            Left = 0 ,
            Center = 1,
            Right = 2,
        }
        public enum  VerticalPoint :byte{
            Up = 0,
            Center = 1,
            Down = 2,
        }

        [SerializeField]
        private Vector2 m_position;
        [SerializeField]
        private HorizontalPoint horizontalPoint;
        [SerializeField]
        private VerticalPoint verticalPoint;

        [SerializeField]
        private int depth;

        [SerializeField]
        private float rotateAngle;


        // Update is called once per frame
        void LateUpdate()
        {
            var uiCamera = UICamera.Instance;
            if(!uiCamera){
                Debug.LogWarning("uiCamera is not alive");
                return;
            }

            Vector3 position;
            position.z = 1.0f + depth * 0.1f;
            position.x = CulcPositionX();
            position.y = CulcPositionY();

            // X position
            transform.localPosition = position;
            transform.localRotation = CulcRotation();
        }

        private Quaternion CulcRotation(){
            var uiCamera = UICamera.Instance;
            float offset = 0.0f;
            switch(uiCamera.Rotate){
                case UICamera.RotateType.RightUp:
                    offset = -90;
                    break;
                case UICamera.RotateType.LeftUp:
                    offset = 90;
                    break;
            }
            return Quaternion.Euler(0,0 ,rotateAngle + offset);
        }

        private float CulcPositionX(){
            var uiCamera = UICamera.Instance;
            Vector2 virtualScreenSize = uiCamera.virtualScreenSize;
            if(uiCamera.Rotate ==  UICamera.RotateType.None){
                switch(horizontalPoint){
                    case HorizontalPoint.Left:
                        return m_position.x * virtualScreenSize.x  - virtualScreenSize.x;         
                    case HorizontalPoint.Center:
                        return m_position.x * virtualScreenSize.x;
                    case HorizontalPoint.Right:
                        return m_position.x  * virtualScreenSize.x + virtualScreenSize.x;
                }
            }
            else{
                float offscale = 1;
                if(uiCamera.Rotate ==  UICamera.RotateType.LeftUp){
                    offscale = -1;
                }
                switch(verticalPoint){
                    case VerticalPoint.Up:
                        return (m_position.y * virtualScreenSize.x  + virtualScreenSize.x ) * offscale;         
                    case VerticalPoint.Center:
                        return (m_position.y * virtualScreenSize.x ) * offscale;
                    case VerticalPoint.Down:
                        return (m_position.y  * virtualScreenSize.x - virtualScreenSize.x) * offscale;
                }
            }
            
            return 0.0f;
        }

        private float CulcPositionY(){
            var uiCamera = UICamera.Instance;
            Vector2 virtualScreenSize = uiCamera.virtualScreenSize;
            if(uiCamera.Rotate ==  UICamera.RotateType.None){
                switch(verticalPoint){
                    case VerticalPoint.Up:
                        return m_position.y * virtualScreenSize.y  + virtualScreenSize.y;         
                    case VerticalPoint.Center:
                        return m_position.y * virtualScreenSize.y;
                    case VerticalPoint.Down:
                        return m_position.y  * virtualScreenSize.y - virtualScreenSize.y;
                }
            }
            else{
                float offscale = 1;
                if(uiCamera.Rotate ==  UICamera.RotateType.LeftUp){
                    offscale = -1;
                }
                switch(horizontalPoint){
                    case HorizontalPoint.Left:
                        return (m_position.x * virtualScreenSize.x  + virtualScreenSize.y ) * offscale;         
                    case HorizontalPoint.Center:
                        return (m_position.x * virtualScreenSize.y ) * offscale;
                    case HorizontalPoint.Right:
                        return (m_position.x  * virtualScreenSize.x - virtualScreenSize.y ) * offscale;
                }
            }

            return 0.0f;
        }
        
    }
}