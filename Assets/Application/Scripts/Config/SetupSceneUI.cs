using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BMProject.UI;
using Cysharp.Threading.Tasks;
using toio;
using UnityEngine.SceneManagement;

namespace BMProject
{
    public class SetupSceneUI : MonoBehaviour
    {
        private enum EStep
        {
            RotateTypeSelect,
            AreaRightFrontSelect,
            AreaLeftBackSelect,
            Confirm
        }
        private static bool isFirst = true;

        [SerializeField]
        private GameObject selectRotate;
        [SerializeField]
        private GameObject toioConnecting;


        [SerializeField]
        private GameObject rightFrontSetup;
        [SerializeField]
        private SpriteProgress rightFrontProgress;


        [SerializeField]
        private GameObject leftBackSetup;
        [SerializeField]
        private SpriteProgress leftBackProgress;


        [SerializeField]
        private GameObject confirm;

        private BmUICamera uiCamera;
        private GlobalGameConfig gameConfig;
        private EStep step = EStep.RotateTypeSelect;

        private Cube cube;
        ToioPlayAreaPositionDetector detector;

        private Coroutine areaMoveCoroutine;
        private void Start()
        {
            uiCamera = BmUICamera.Instance;
            if (isFirst)
            {
                if (GlobalGameConfig.HasSaveData())
                {
                    isFirst = false;
                    NextScene();
                    return;
                }
            }
            isFirst = false;
            gameConfig = GlobalGameConfig.currentConfig;
            selectRotate.SetActive(true);
            toioConnecting.SetActive(false);
            rightFrontSetup.SetActive(false);
            leftBackSetup.SetActive(false);
            confirm.SetActive(false);

            this.ConnectCube();
        }

        private void OnDestroy()
        {
            if (this.cube != null)
            {
                ToioConnectionMgr.Instance.ReleaseCube(this.cube);
            }
        }
        private async void ConnectCube()
        {
            this.cube = await ToioConnectionMgr.Instance.ConnectCube();
            this.detector = new ToioPlayAreaPositionDetector(cube);
        }


        #region ROTATE_MODE
        public void OnSelectNone()
        {
            uiCamera.rotateType = BmUICamera.RotateType.None;
        }
        public void OnSelectLeftUp()
        {
            uiCamera.rotateType = BmUICamera.RotateType.LeftUp;
        }
        public void OnSelectRightUp()
        {
            uiCamera.rotateType = BmUICamera.RotateType.RightUp;
        }

        public void OnSubmitSelectNone()
        {
            uiCamera.rotateType = BmUICamera.RotateType.None;
            gameConfig.rotateType = GlobalGameConfig.RotateType.None;
            RotateSelectComplete();
        }
        public void OnSubmitLeftUp()
        {
            uiCamera.rotateType = BmUICamera.RotateType.LeftUp;
            gameConfig.rotateType = GlobalGameConfig.RotateType.LeftUp;
            RotateSelectComplete();
        }
        public void OnSubmitRightUp()
        {
            uiCamera.rotateType = BmUICamera.RotateType.RightUp;
            gameConfig.rotateType = GlobalGameConfig.RotateType.RightUp;
            RotateSelectComplete();
        }

        private void RotateSelectComplete()
        {
            this.step = EStep.AreaRightFrontSelect;
            this.selectRotate.SetActive(false);
        }
        #endregion ROTATE_MODE

        private void Update()
        {
            if (this.step == EStep.RotateTypeSelect)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                OnSubmitSelectNone();
#endif
                return;
            }
            toioConnecting.SetActive(cube == null);

            if (this.detector == null)
            {
                return;
            }
            switch (this.step)
            {
                case EStep.AreaRightFrontSelect:
                    rightFrontSetup.SetActive(true);
                    this.detector.Update();
                    rightFrontProgress.progress = this.detector.second / 5.0f;
                    if (detector.second >= 5.0f)
                    {
                        gameConfig.areaRightFront = new Vector2Int((int)detector.Position.x, (int)detector.Position.y);
                        rightFrontSetup.SetActive(false);
                        this.cube.PlayPresetSound(6);
                        this.detector.AddExcludePoint(detector.Position);
                        this.step = EStep.AreaLeftBackSelect;
                    }
                    break;
                case EStep.AreaLeftBackSelect:
                    leftBackSetup.SetActive(true);
                    this.detector.Update();
                    leftBackProgress.progress = this.detector.second / 5.0f;
                    if (detector.second >= 5.0f)
                    {
                        leftBackSetup.SetActive(false);
                        gameConfig.areaLeftBack = new Vector2Int((int)detector.Position.x, (int)detector.Position.y);
                        this.cube.PlayPresetSound(6);
                        confirm.SetActive(true);
                        this.step = EStep.Confirm;
                        areaMoveCoroutine = StartCoroutine(StartAreaMove());
                    }
                    break;
            }
        }

        private IEnumerator StartAreaMove()
        {
            yield return new WaitForSeconds(0.5f);

            var initPos = ToioPositionConverter.GetInitializePosition(this.gameConfig.areaRightFront, this.gameConfig.areaLeftBack);
            var initRot = ToioPositionConverter.GetInitializeRotation(this.gameConfig.areaRightFront, this.gameConfig.areaLeftBack);
            this.cube.TargetMove(initPos.x, initPos.y, initRot,
                0,0,Cube.TargetMoveType.RoundBeforeMove,40);
            yield return new WaitForSeconds(3.0f);
            Vector2Int[] goals = new Vector2Int[]
            {
                new Vector2Int(this.gameConfig.areaRightFront.x, this.gameConfig.areaRightFront.y),
                new Vector2Int(this.gameConfig.areaRightFront.x, this.gameConfig.areaLeftBack.y),
                new Vector2Int(this.gameConfig.areaLeftBack.x, this.gameConfig.areaLeftBack.y),
                new Vector2Int(this.gameConfig.areaLeftBack.x, this.gameConfig.areaRightFront.y),
            };

            while (true)
            {

                int idx = 0;
                for (int i = 0; i < 5; ++i)
                {
                    yield return new WaitForToioMovePosition(cube, goals[idx%4]);

                    this.cube.TargetMove(goals[idx%4].x, goals[idx%4].y, 0,
                        0, 0, Cube.TargetMoveType.RoundBeforeMove, 45,
                        Cube.TargetSpeedType.UniformSpeed, Cube.TargetRotationType.NotRotate);
                    yield return new WaitForToioMovePosition(cube, goals[idx%4]);
                    ++idx;
                }
                yield return new WaitForSeconds(1.0f);
                this.cube.TargetMove(initPos.x, initPos.y, initRot,
                    0, 0, Cube.TargetMoveType.RoundBeforeMove, 40);
                yield return new WaitForSeconds(8.0f);
            }
        }

        public void SkipPlayArea()
        {
            this.cube.PlayPresetSound(5);
            leftBackSetup.SetActive(false);
            rightFrontSetup.SetActive(false);

            confirm.SetActive(true);
            this.step = EStep.Confirm;
        }

        public void CompleteSetting()
        {
            this.confirm.SetActive(false);
            this.gameConfig.Save();
            NextScene();
        }
        private void NextScene()
        {
            SceneManager.LoadScene("SelectMode");
            ;
            if (cube != null)
            {
                cube.Move(0, 0, 0, Cube.ORDER_TYPE.Strong);
            }
        }

        public void BackToSetup()
        {
            this.selectRotate.SetActive(true);
            this.confirm.SetActive(false);
            this.step = EStep.RotateTypeSelect;
            this.detector.Clear();
            if(areaMoveCoroutine != null)
            {
                StopCoroutine(areaMoveCoroutine);
                areaMoveCoroutine = null;
                if(cube != null) { 
                    cube.Move(0, 0, 0,Cube.ORDER_TYPE.Strong); 
                }
            }
        }
        
    }
}
