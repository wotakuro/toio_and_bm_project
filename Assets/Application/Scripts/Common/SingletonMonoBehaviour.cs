using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class UnitySingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        protected void Awake()
        {
            instance = this as T;
        }

        protected void OnDestroy()
        {
            instance = null;
        }
    }
}