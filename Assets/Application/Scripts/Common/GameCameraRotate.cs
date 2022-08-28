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

        public RotateType rotateType
        {
            get { return m_rotateType; }
            set { m_rotateType = value; }
        }

        public static GameCameraRotate Instance { get; private set; }

        private Camera m_Camera;

        // Start is called before the first frame update
        void Awake()
        {
            m_Camera = this.GetComponent<Camera>();
            Instance = this;
        }
        private void OnDestroy()
        {
            if(Instance == this)
            {
                Instance = null;
            }
        }

#if UNITY_EDITOR
        private Quaternion originRotate;
        private void Update()
        {
            this.originRotate = transform.rotation;
        }
#endif

        // Update is called once per frame
        void LateUpdate()
        {
#if UNITY_EDITOR
            if (!Instance)
            {
                Instance = this;
            }
            if (!m_Camera)
            {
                m_Camera = this.GetComponent<Camera>();
            }
            this.transform.rotation = originRotate;
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

#if UNITY_EDITOR
        public void ForceUpdateImmidiate()
        {
            this.LateUpdate();
        }
#endif
    }

}