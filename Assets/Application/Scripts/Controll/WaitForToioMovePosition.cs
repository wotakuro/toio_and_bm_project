using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class WaitForToioMovePosition : CustomYieldInstruction
    {
        private static readonly int NearEqualPos = 20;
        private static readonly int NearEqualAngle = 15;
        private static readonly float JudgeMovePosition = 10.0f;
        private static readonly int JudgeMoveAngle = 10;

        private toio.Cube targetCube;
        private double lastUpdateTime;
        private Vector2 lastPostion;
        private int lastAngle;
        private Vector2Int targetPostion;
        private int targetAngle;
        private double timeoutParam = 0.5;


        public override bool keepWaiting
        {
            get
            {
                if (MoveTimeoutCheck())
                {
                    return false;
                }
                return !IsMoveEnd(targetPostion.x, targetPostion.y,targetAngle);
            }
        }

        private bool MoveTimeoutCheck()
        {
            if(this.targetCube == null){
                return false;
            }
            var currentTime = Time.timeAsDouble;
            var currentPosition = this.targetCube.pos;
            var currentAngle = this.targetCube.angle;
            bool updateTime = false;

            if(this.targetCube.isGrounded){
                if( (currentPosition - lastPostion).sqrMagnitude > JudgeMovePosition * JudgeMovePosition)
                {
                    this.lastPostion = currentPosition;
                    updateTime = true;
                }
                if ( Mathf.Abs(this.lastAngle - currentAngle) >= JudgeMoveAngle) {
                    this.lastAngle = currentAngle;
                    updateTime = true;
                }
            }

            if (updateTime)
            {
                this.lastUpdateTime = currentTime;
            }
            bool isTimeout = (currentTime - lastUpdateTime >= timeoutParam);
            
            /*
            if (isTimeout)
            {
                Debug.Log("WaitPos timeup " + updateTime + "::" + currentTime + "::" + lastUpdateTime);
            }
            */
            return isTimeout;
        }


        public WaitForToioMovePosition(toio.Cube cube,Vector2Int goal,int angle=-1,double timeout = 0.5)
        {
            this.targetCube = cube;
            this.targetPostion = goal;
            this.targetAngle = angle;
            this.lastUpdateTime = Time.timeAsDouble;
            this.lastPostion = cube.pos;
            this.lastAngle = cube.angle;
            this.timeoutParam = timeout;
        }

        private bool IsMoveEnd(int targetX, int targetY, int angle = -1)
        {
            var pos = this.targetCube.pos;
            bool isPosition = ((targetX - NearEqualPos <= pos.x && pos.x <= targetX + NearEqualPos)
                && (targetY - NearEqualPos <= pos.y && pos.y <= targetY + NearEqualPos));

            if (!isPosition)
            {
               // Debug.Log("MoveIsNot End " + targetX + "," + targetY + "  :: " + pos);

                return false;
            }

            if (angle < 0)
            {
                //Debug.Log("movePositionEnd with non-angle");
                return true;
            }
            bool result= (angle - NearEqualAngle <= this.targetCube.angle) && (this.targetCube.angle <= angle + NearEqualAngle);

            //Debug.Log("movePositionEnd " + result);
            return result;

        }


    }
}