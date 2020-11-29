using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class UnitySingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public T Instance
        {
            get
            {
                return instance;
            }
        }

        void Awake()
        {
            instance = this as T;
        }

        void OnDestroy()
        {
            instance = null;
        }
    }
}