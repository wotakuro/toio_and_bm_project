using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;

namespace BMProject
{
    public class PlayingScene : MonoBehaviour
    {
        private CubeManager cubeManager;
        private Cube cube;

        public ToioController controller;
        public ToioEventController eventCtrl;

        private float time = 30.0f;

        async void Start()
        {
            
            cubeManager = ToioConnectionMgr.Instance.cubeManager;
            cube = await ToioConnectionMgr.Instance.ConnectCube();

            controller.Init(cubeManager, cube);
            eventCtrl.InitCube(cubeManager, cube);
        }

        private void Update()
        {
            time -= Time.deltaTime;
            if(time < 0.0f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
        private void OnDestroy()
        {
            Disconnect();
        }

        private void Disconnect()
        {
            if(cubeManager != null && cube != null)
            {
                ToioConnectionMgr.Instance.ReleaseCube(cube);
                cube = null;
            }
        }
    }


}