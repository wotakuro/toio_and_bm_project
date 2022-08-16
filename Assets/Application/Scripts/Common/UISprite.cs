using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    [RequireComponent(typeof(Camera))]
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

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}