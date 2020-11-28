using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class OnlySimulator : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
#if !UNITY_EDITOR
        this.gameObject.SetActive(false);
#endif
        }

    }
}
