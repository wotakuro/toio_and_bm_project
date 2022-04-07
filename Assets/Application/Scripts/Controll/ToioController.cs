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
        private int targetToioID = 0;

        private VirtualToioMove virtualToio;

        private Cube targetCube;
        private CubeManager cubeManager;
        private bool updateFlag = false;

        private static Dictionary<int, ToioController> s_instances = new Dictionary<int, ToioController>();


        public static ToioController GetToioController(int toioID) {
            ToioController controller;
            if(s_instances.TryGetValue(toioID,out controller)){
                return controller;
            }
            return null;
        }

        private void OnEnable() { 
        
            if(s_instances.ContainsKey(this.targetToioID))
            {
                Debug.LogError("Already have toio");
            }
            s_instances.Add(this.targetToioID, this);
        }
        private void OnDisable()
        {
            s_instances.Remove(this.targetToioID);
        }

        public void InitializeController(CubeManager mgr , Cube c)
        {
            this.targetCube = c;
            this.cubeManager = mgr;
            this.OnEnableInput(cubeManager, targetCube);
            this.updateFlag = true;

            this.virtualToio = VirtualToioMove.GetVirtualToio(this.targetToioID);
            if (this.virtualToio != null)
            {
                this.virtualToio.SetCube(targetCube);
            }
        }
        public void DisableInput()
        {
            this.updateFlag = false;
            this.OnDisableInput();
            this.targetCube.Move(0, 0, 0, Cube.ORDER_TYPE.Strong);
        }

        private void Update()
        {
            if( this.targetCube != null)
            {
                if (this.updateFlag)
                {
                    this.UpdateCube(this.cubeManager, this.targetCube);
                }
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
                Cube.ORDER_TYPE orderType = Cube.ORDER_TYPE.Weak;
                if( i == 2) { orderType = Cube.ORDER_TYPE.Strong; }
                this.targetCube.Move(left, right, actualDuration, orderType);
                yield return null;
            }
        }

    }
}
