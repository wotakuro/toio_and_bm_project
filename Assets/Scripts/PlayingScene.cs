using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using Cysharp.Threading.Tasks.Triggers;

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

        bool isGameOver = false;
        void OnTimeOver()
        {
            isGameOver = true;
            controller.DisableInput();
            eventCtrl.EndEvent(this.cubeManager, this.cube);
        }

        private void Update()
        {
            if (isGameOver && Input.GetMouseButtonDown(0))
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