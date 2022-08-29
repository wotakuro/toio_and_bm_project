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
        private ToioEventController eventCtrl;
        [SerializeField]
        private PlayableDirector startTimeline;
        [SerializeField]
        private PlayableDirector timeOutTimeline;
        [SerializeField]
        private GameObject playingUI;
        private ToioController controller;
        [SerializeField]
        private float playTime = 30.0f;

        [SerializeField]
        private ResultUI resultUI;

        [SerializeField]
        private GameObject disconnectToioObj;



        public bool isDisconnect = false;
        private bool isPlaying = false;

        private bool isCallOnDisconnectCube = false;
        
        private ToioGroundAdjuster groundAdjuster;

        private void Awake()
        {
            // sleep OFF
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

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
            if(this.cube != null){
                playingUI.SetActive(true);
                leftTimer.SetTimer(playTime);
                waitingToioConnect.EndWaiting();
                this.StartCoroutine(PlayStart());
            }else{
                resultUI.Disconnected();
            }
            
        }

        // todo 仮
        [SerializeField]
        private GameObject controlWithoutMat;
        [SerializeField]
        private GameObject controlWithMat;


        // プレイ開始
        private IEnumerator PlayStart()
        {

            yield return new WaitForToioGroundCheck(this.cube);
            bool foundGrounded = this.cube.isGrounded ;

            
            if (!foundGrounded)
            {
                if(groundAdjuster == null){
                    groundAdjuster = new ToioGroundAdjuster();
                    groundAdjuster.Start(this.cube);
                }
                while (!groundAdjuster.Update())
                {
                    yield return null;
                }
                foundGrounded = groundAdjuster.isGroundFound;
            }
            // Select MovePattenr
            if (foundGrounded)
            {
                controlWithMat.SetActive(true);
            }
            else
            {
                controlWithoutMat.SetActive(true);
            }
            this.controller = ToioController.GetToioController(0);
            this.controller.InitializeController(cubeManager, cube);

            // 初期位置に移動
            if(foundGrounded){
                var currentConfig = GlobalGameConfig.currentConfig;
                Vector2Int initPos = ToioPositionConverter.GetInitializePosition(
                    currentConfig.areaLeftUpper, currentConfig.areaRightDowner);
                int initRot = ToioPositionConverter.GetInitializeRotation(
                    currentConfig.areaLeftUpper, currentConfig.areaRightDowner);

                for (int i = 0; i < 2; ++i)
                {
                    cube.TargetMove(initPos.x, initPos.y, initRot,0,0,
                        Cube.TargetMoveType.RoundBeforeMove,40);
                    yield return new WaitForToioMovePosition(cube, initPos, initRot, 2.0);
                    yield return null;
                }
            }

            // start ready go
            startTimeline.gameObject.SetActive(true);
            startTimeline.Play();
            
            while(startTimeline.state == PlayState.Playing)
            {               
                yield return null;
            }
            // groundCheck
            



            this.controller.EnableInput();
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
            controller.DisableInput();
            eventCtrl.EndEvent(this.cubeManager, this.cube);

            this.playingUI.SetActive(false);
            disconnectToioObj.SetActive(true);
            resultUI.Disconnected();
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
            // Sleep off
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
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