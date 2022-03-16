using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using UnityEngine.UI;

namespace BMProject
{
    public class ToioController : MonoBehaviour
    {
        [SerializeField]
        private VirtualToioMove virtualToio;

        private Cube targetCube;
        private CubeManager cubeManager;

        public void Init(CubeManager mgr , Cube c)
        {
            this.targetCube = c;
            this.cubeManager = mgr;
            this.OnEnableInput(cubeManager, targetCube);

            this.virtualToio.SetCube(targetCube);
        }
        public void DisableInput()
        {
            this.OnDisableInput();
        }

        private void Update()
        {
            if( this.targetCube != null)
            {
                this.UpdateCube(this.cubeManager,this.targetCube);
            }
        }        

        protected virtual void OnEnableInput(CubeManager mgr, Cube c)
        {
        }
        protected virtual void UpdateCube(CubeManager mgr, Cube c)
        {
        }
        protected virtual void OnDisableInput()
        {
        }

        protected void UpdateCubeMove( int left , int right)
        {            
            if (this.cubeManager != null &&
                this.cubeManager.IsControllable(this.targetCube) &&
                CubeOrderBalancer.Instance.IsIdle(this.targetCube))
            {
                this.targetCube.Move(left, right, 0, Cube.ORDER_TYPE.Weak);


                virtualToio.Move(left, right);
            }
        }
        protected void SendMoveCmdCube(int left, int right,int duration)
        {
            if (this.cubeManager != null &&
                this.targetCube != null)
            {
                StartCoroutine(ResendSendMoveCommand(left, right, duration));
                if (virtualToio)
                {
                    virtualToio.Move(left, right, duration);
                }
            }
        }

        private IEnumerator ResendSendMoveCommand(int left, int right, int duration)
        {
            if(duration <= 0) { Debug.LogError("duration should be over 1."); }
            double currentTime = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < 3; ++i)
            {
                int actualDuration = duration - (int)((Time.realtimeSinceStartupAsDouble - currentTime) * 1000.0);

                if(actualDuration <= 0) { yield break; }
                this.targetCube.Move(left, right, actualDuration, Cube.ORDER_TYPE.Strong);
                yield return null;
            }
        }

    }
}
