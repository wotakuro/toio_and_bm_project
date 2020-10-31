using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using UnityEngine.UI;

namespace BMProject
{
    public class ToioController : MonoBehaviour
    {
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
    }
}
