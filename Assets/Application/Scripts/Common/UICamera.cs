using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    [ExecuteAlways]
    public class UICamera : MonoBehaviour
    {
        public static UICamera Instance{get;private set;}

        void Awake(){
            Instance = this;
        }
        void OnDestroy(){
            if(Instance == this){
                Instance = null;
            }
        }
    }
}