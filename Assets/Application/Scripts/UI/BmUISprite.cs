using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject.UI
{
    [ExecuteAlways]
    public class BmUISprite : MonoBehaviour
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
        private float m_FixedWidth = -1.0f;


        [SerializeField]
        private Vector2 m_position;
        [SerializeField]
        private HorizontalPoint horizontalPoint = HorizontalPoint.Center;
        [SerializeField]
        private VerticalPoint verticalPoint = VerticalPoint.Center;

        [SerializeField]
        private int depth;

        [SerializeField]
        private float rotateAngle;

        [SerializeField]
        private Vector2 selfBoxCenter;
        [SerializeField]
        private Vector2 selfBoxSize;

        private void OnEnable()
        {

            var uiCamera = BmUICamera.Instance;
            if (!uiCamera)
            {
                Debug.LogWarning("uiCamera is not alive");
                return;
            }

            Vector3 position;
            position.z = 1.0f + depth * 0.1f;
            position.x = GetActualPositionX();
            position.y = GetActualPositionY();

            // X position
            transform.localPosition = position;
            transform.localRotation = GetActualRotation();
            transform.localScale = GetActualScale();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            var uiCamera = BmUICamera.Instance;
            if(!uiCamera){
                Debug.LogWarning("uiCamera is not alive");
                return;
            }

            Vector3 position;
            position.z = 1.0f + depth * 0.1f;
            position.x = GetActualPositionX();
            position.y = GetActualPositionY();

            // X position
            transform.localPosition = position;
            transform.localRotation = GetActualRotation();
            transform.localScale = GetActualScale();
        }

#if UNITY_EDITOR
        public void ForceUpdateImmidiate()
        {
            this.LateUpdate();
        }

#endif

        private Vector3 GetActualScale(bool forceNonRotate = false)
        {
            if (m_FixedWidth < float.Epsilon || this.selfBoxSize.x == 0.0f)
            {
                return Vector3.one;
            }

            var uiCamera = BmUICamera.Instance;
            float size = 1.0f;
            if (uiCamera.rotateType == BmUICamera.RotateType.None)
            {
                size = (uiCamera.virtualScreenSize.x * 2.0f / this.selfBoxSize.x) * m_FixedWidth;
            }
            else
            {
                size = (uiCamera.virtualScreenSize.y * 2.0f / this.selfBoxSize.x) * m_FixedWidth;
            }

            return new Vector3(size, size, 1.0f);
        }

        private Quaternion GetActualRotation(bool forceNonRotate = false)
        {
            var uiCamera = BmUICamera.Instance;
            float offset = 0.0f;
            if (!forceNonRotate)
            {
                switch (uiCamera.rotateType)
                {
                    case BmUICamera.RotateType.RightUp:
                        offset = -90;
                        break;
                    case BmUICamera.RotateType.LeftUp:
                        offset = 90;
                        break;
                }
            }
            return Quaternion.Euler(0,0 ,rotateAngle + offset);
        }

        private float GetActualPositionX(bool forceNonRotate = false){
            var uiCamera = BmUICamera.Instance;
            Vector2 virtualScreenSize = uiCamera.virtualScreenSize;
            if(uiCamera.rotateType ==  BmUICamera.RotateType.None || forceNonRotate)
            {
                switch(horizontalPoint){
                    case HorizontalPoint.Left:
                        return m_position.x * virtualScreenSize.x  - virtualScreenSize.x
                            + selfBoxSize.x * 0.5f - selfBoxCenter.x;         
                    case HorizontalPoint.Center:
                        return m_position.x * virtualScreenSize.x - selfBoxCenter.x;
                    case HorizontalPoint.Right:
                        return m_position.x  * virtualScreenSize.x + virtualScreenSize.x
                            - selfBoxSize.x * 0.5f - selfBoxCenter.x;
                }
            }
            else{
                float offscale = 1;
                if(uiCamera.rotateType ==  BmUICamera.RotateType.LeftUp){
                    offscale = -1;
                }
                switch(verticalPoint){
                    case VerticalPoint.Up:
                        return (m_position.y * virtualScreenSize.x  + virtualScreenSize.x
                            - selfBoxSize.y * 0.5f - selfBoxCenter.y) * offscale ;         
                    case VerticalPoint.Center:
                        return (m_position.y * virtualScreenSize.x - selfBoxCenter.y) * offscale ;
                    case VerticalPoint.Down:
                        return (m_position.y  * virtualScreenSize.x - virtualScreenSize.x
                            + selfBoxSize.y * 0.5f - selfBoxCenter.y) * offscale ;
                }
            }
            
            return 0.0f;
        }

        private float GetActualPositionY(bool forceNonRotate = false){
            var uiCamera = BmUICamera.Instance;
            Vector2 virtualScreenSize = uiCamera.virtualScreenSize;
            if(uiCamera.rotateType ==  BmUICamera.RotateType.None || forceNonRotate){
                switch(verticalPoint){
                    case VerticalPoint.Up:
                        return m_position.y * virtualScreenSize.y  + virtualScreenSize.y
                            - selfBoxSize.y * 0.5f - selfBoxCenter.y;
                    case VerticalPoint.Center:
                        return m_position.y * virtualScreenSize.y - selfBoxCenter.y;
                    case VerticalPoint.Down:
                        return m_position.y  * virtualScreenSize.y - virtualScreenSize.y
                            + selfBoxSize.y * 0.5f - selfBoxCenter.y;
                }
            }
            else{
                float offscale = 1;
                if(uiCamera.rotateType ==  BmUICamera.RotateType.LeftUp){
                    offscale = -1;
                }
                switch(horizontalPoint){
                    case HorizontalPoint.Left:
                        return (-m_position.x * virtualScreenSize.y  + virtualScreenSize.y
                            - selfBoxSize.x * 0.5f) * offscale - selfBoxCenter.x;         
                    case HorizontalPoint.Center:
                        return (-m_position.x * virtualScreenSize.y ) * offscale - selfBoxCenter.x;
                    case HorizontalPoint.Right:
                        return (-m_position.x  * virtualScreenSize.y - virtualScreenSize.y
                            + selfBoxSize.x * 0.5f) * offscale - selfBoxCenter.x;
                }
            }

            return 0.0f;
        }

        public void CalculateSelfBox()
        {
            // set non rotate
            Vector3 position;
            position.z = 1.0f + depth * 0.1f;
            position.x = GetActualPositionX(true);
            position.y = GetActualPositionY(true);
            transform.localPosition = position;
            transform.localRotation = GetActualRotation(true);
            transform.localScale = Vector3.one;

            //
            var bounds = GetSelfBoundingBox();
            this.selfBoxCenter.x = (bounds.center.x - transform.position.x );
            this.selfBoxCenter.y = (bounds.center.y- transform.position.y );

            this.selfBoxSize.x = bounds.size.x;
            this.selfBoxSize.y = bounds.size.y;

            // with rotate
            position.x = GetActualPositionX();
            position.y = GetActualPositionY();
            transform.localPosition = position;
            transform.localRotation = GetActualRotation();

        }

        private Bounds GetSelfBoundingBox()
        {
            var renderers = this.GetComponentsInChildren<Renderer>(true);

            Bounds bounds = new Bounds();
            bool isFirst = true;
            foreach (var renderer in renderers)
            {
                if (isFirst)
                {
                    bounds = renderer.bounds;
                    isFirst = false;
                }
                else
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
            return bounds;
        }

#if UNITY_EDITOR 
        public bool debugDrawGizmo { get; set; }
        private void OnDrawGizmosSelected()
        {
            if (debugDrawGizmo)
            {
                var backupColor = Gizmos.color;
                Gizmos.color = Color.red;
                Bounds bounds = GetSelfBoundingBox();
                Gizmos.DrawCube(bounds.center, bounds.size);
                Gizmos.color = backupColor;
            }

        }

#endif

    }
}