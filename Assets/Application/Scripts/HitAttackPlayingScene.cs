using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.Playables;
using System.Text;

namespace BMProject
{
    public class HitAttackPlayingScene : MonoBehaviour
    {
        private CubeManager cubeManager;
        private Cube cube;

        [SerializeField]
        private WaitingToioConnect waitingToioConnect;
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

        public bool isDisconnect = false;
        private bool isPlaying = false;

        private bool isCallOnDisconnectCube = false;

        private void Awake()
        {
            startTimeline.gameObject.SetActive(false);
            timeOutTimeline.gameObject.SetActive(false);
            playingUI.SetActive(false);
            resultUI.gameObject.SetActive(false);
        }

        async void Start()
        {
            waitingToioConnect.StartWaiting();
            this.cubeManager = ToioConnectionMgr.Instance.cubeManager;
            this.cube = await ToioConnectionMgr.Instance.ConnectCube();
            
            playingUI.SetActive(true);
            leftTimer.SetTimer(30.0f);
            waitingToioConnect.EndWaiting();
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
            this.leftTimer.CountStart(OnTimeOver);
            this.isPlaying = true;
        }

        bool isGameOver = false;
        void OnTimeOver()
        {
            this.isPlaying = false;
            isGameOver = true;
            controller.DisableInput();
            eventCtrl.EndEvent(this.cubeManager, this.cube);

            timeOutTimeline.gameObject.SetActive(true);
            timeOutTimeline.Play();
            this.playingUI.SetActive(false);

            this.SetResult();
            

            timeOutTimeline.stopped +=(a)=> {
                resultUI.StartResult(); 
            };
        }

        private void SetResult()
        {
            this.resultUI.SetRule("ヒットアタック").
                SetTimer("プレイ時間", leftTimer.GetLimitTime() + "秒").
                SetScore(PlayingScoreBoard.Instance.GetScore() + "点");
            this.resultUI.SetDetail(GenerateDetail());
        }

        private List<string> GenerateDetail()
        {
            var timings = PlayingScoreBoard.Instance.GetTimings();
            List<string> resultList;
            if( timings.Count == 0)
            {
                resultList = new List<string>(1);
                resultList.Add("ヒットなし");
                return resultList;
            }
            resultList = new List<string>(timings.Count);
            var sb = new StringBuilder(64);
            foreach ( var timing in timings)
            {
                sb.Clear();
                sb.AppendFormat("{0:f2}", timing);
                sb.Append("秒にヒット");
                resultList.Add(sb.ToString());

            }

            return resultList;
        }

        private void OnDisconnectCube()
        {
          //  this.leftTimer.Pause();
        }

        private void Update()
        {
            if (isPlaying)
            {
                if (!cube.isConnected || isDisconnect)
                {
                    if (!isCallOnDisconnectCube)
                    {
                        OnDisconnectCube();
                        isCallOnDisconnectCube = true;
                    }
                }
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