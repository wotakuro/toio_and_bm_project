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

        [SerializeField]
        private LeftTimer leftTimer;
        [SerializeField]
        private ToioController controller;
        [SerializeField]
        private ToioEventController eventCtrl;
        [SerializeField]
        private PlayableDirector startTimeline;
        [SerializeField]
        private PlayableDirector timeOutTimeline;
        [SerializeField]
        private GameObject playingUI;

        [SerializeField]
        private ResultUI resultUI;

        private void Awake()
        {
            startTimeline.gameObject.SetActive(false);
            timeOutTimeline.gameObject.SetActive(false);
            playingUI.SetActive(false);
            resultUI.gameObject.SetActive(false);
        }

        async void Start()
        {
            this.cubeManager = ToioConnectionMgr.Instance.cubeManager;
            this.cube = await ToioConnectionMgr.Instance.ConnectCube();

            playingUI.SetActive(true);
            leftTimer.SetTimer(30.0f);
            this.StartCoroutine(PlayStart());
        }

        // プレイ開始
        private IEnumerator PlayStart()
        {
            startTimeline.gameObject.SetActive(true);
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

            timeOutTimeline.gameObject.SetActive(true);
            timeOutTimeline.Play();
            //this.playingUI.SetActive(false);
            //timeOutTimeline.stopped +=(a)=> { resultUI.StartResult(); };
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