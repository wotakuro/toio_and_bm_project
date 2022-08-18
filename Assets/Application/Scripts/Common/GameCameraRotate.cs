using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BMProject
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [DefaultExecutionOrder(999)]
    public class GameCameraRotate : MonoBehaviour
    {
        public enum RotateType
        {
            None,
            RightUp,
            LeftUp,
        }

        [SerializeField]
        private RotateType m_rotateType;

        private Camera m_Camera;

        // Start is called before the first frame update
        void Awake()
        {
            m_Camera = this.GetComponent<Camera>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
#if UNITY_EDITOR
            if (!m_Camera)
            {
                m_Camera = this.GetComponent<Camera>();
            }
#endif
            Quaternion q;
            switch (m_rotateType)
            {
                case RotateType.LeftUp:
                    q = Quaternion.AngleAxis(-90,m_Camera.transform.forward);
                    this.transform.rotation = q * this.transform.rotation;
                    break;
                case RotateType.RightUp:
                    q = Quaternion.AngleAxis(90, m_Camera.transform.forward);
                    this.transform.rotation = q * this.transform.rotation;
                    break;
            }
        }
    }

}