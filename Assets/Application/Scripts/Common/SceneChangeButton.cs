using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BMProject
{
    public class SceneChangeButton : MonoBehaviour
    {
        public void ChangeScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}