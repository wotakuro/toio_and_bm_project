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

        public LeftTimer leftTimer;
        public ToioController controller;
        public ToioEventController eventCtrl;


        async void Start()
        {
            leftTimer.SetTimer(30.0f);
            cubeManager = ToioConnectionMgr.Instance.cubeManager;
            cube = await ToioConnectionMgr.Instance.ConnectCube();

            controller.Init(cubeManager, cube);
            eventCtrl.InitCube(cubeManager, cube);
            // start Count
            leftTimer.CountStart(OnTimeOver);
        }

        void OnTimeOver()
        {

            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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