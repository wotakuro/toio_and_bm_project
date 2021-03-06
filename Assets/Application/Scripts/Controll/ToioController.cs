﻿using System.Collections;
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
                this.targetCube.Move(left, right, duration, Cube.ORDER_TYPE.Weak);

                if (virtualToio)
                {
                    virtualToio.Move(left, right, duration);
                }
            }
        }
    }
}
