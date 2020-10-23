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
        }

        private void Update()
        {
            if( this.targetCube != null)
            {
                this.UpdateCube(this.cubeManager,this.targetCube);
            }
            
        }

        protected virtual void UpdateCube(CubeManager mgr, Cube c)
        {

        }
    }
}
