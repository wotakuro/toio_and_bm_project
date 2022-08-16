using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using toio;
namespace BMProject
{
    public class ToioGroundAdjuster{
        
        private VirtualToioMove vitualToio;
        private Cube targetCube;
        private double startTime;
        private double lastTime;

        private double lastCommandTime = -1;

        public bool isGroundFound {get;private set;}= false;


        public void Start(Cube c,VirtualToioMove vToio=null){
            this.targetCube = c;
            vitualToio = vToio;
            this.startTime = Time.timeAsDouble;
            this.isGroundFound = false;
            Application.targetFrameRate = 30;
        }
        
        // Update is called once per frame
        public bool Update()
        {

            double currentTime = Time.timeAsDouble;
            double timeFromStart = currentTime - startTime;
            if(this.targetCube.isGrounded){
                this.targetCube.Move(  0,0,0) ;
                return true;
            }
            bool endFlag =  (timeFromStart > 2.0);

            if(!endFlag && currentTime - lastCommandTime > 0.05){
                int movePow = (int) (20 * Mathf.Sin( (float)timeFromStart * 180.0f) );
                this.targetCube.Move(  movePow,-movePow,0) ;
                lastCommandTime = currentTime;
            }

            this.lastTime = currentTime;
            if(endFlag){
                this.targetCube.Move(  0,0,0) ;
            }
            return endFlag;
        }
    }
}
