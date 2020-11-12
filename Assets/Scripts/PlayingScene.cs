using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.Playables;

namespace BMProject
{
    public class PlayingScene : MonoBehaviour
    {
        private CubeManager cubeManager;
        private Cube cube;

        public LeftTimer leftTimer;
        public ToioController controller;
        public ToioEventController eventCtrl;
        public PlayableDirector startTimeline;


        async void Start()
        {
            leftTimer.SetTimer(30.0f);
            this.cubeManager = ToioConnectionMgr.Instance.cubeManager;
            this.cube = await ToioConnectionMgr.Instance.ConnectCube();


            this.StartCoroutine(PlayStart());
        }

        // プレイ開始
        private IEnumerator PlayStart()
        {
            startTimeline.Play();
            while(startTimeline.state == PlayState.Playing)
            {
                yield return null;
            }

            this.controller.Init(cubeManager, cube);
            this.eventCtrl.InitCube(cubeManager, cube);
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