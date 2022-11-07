using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BMProject.UI
{
    public class SpriteProgress : MonoBehaviour
    {
        private Transform child;

        public float progress { set; get; } = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            child = transform.GetChild(0);
        }

        // Update is called once per frame
        void Update()
        {
            float val = progress;
            if(val > 1.0f) { val = 1.0f; }
            child.localScale = new Vector3(val, child.localScale.y, child.localScale.z);
            child.localPosition = new Vector3( (1.0f- val) * -0.5f, 0, -0.1f);
        }
    }
}