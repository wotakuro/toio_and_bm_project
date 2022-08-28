using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class ApplicationExitButton : MonoBehaviour
    {
        public void ExitApplication()
        {
            UnityEngine.Application.Quit();            
        }
    }
}