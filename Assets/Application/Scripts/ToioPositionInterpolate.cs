using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class ToioPositionInterpolate
    {
        public Vector3 position
        {
            get
            {
                return currentPosition;
            }
        }
        public Quaternion rotation
        {
            get
            {
                return currentRotation;
            }
        }

        private Vector3 lastCubePosition;
        private double lastCubePositionTime;
        private Quaternion lastCubeRotation;

        private Vector3 currentPosition;
        private Quaternion currentRotation;

        private float estimateSpeed = 0.2f;

        private bool isFirstSet = true;


        public bool Update(toio.Cube cube)
        {
            if(cube == null) { return false; }
            if (cube.isGrounded)
            {
                var pos = ToioPositionConverter.ConvertPosition(cube.pos);
                var cubePos = new Vector3(pos.x, 0.0f, pos.y);
                var cubeRot = ToioPositionConverter.GetRotation(cube.angle);
                if (isFirstSet)
                {
                    this.lastCubePosition = cubePos;
                    this.currentPosition = cubePos;
                    this.lastCubeRotation = cubeRot;
                    this.currentRotation = cubeRot;

                    isFirstSet = false;
                }
                else
                {
                    var distance = (this.lastCubePosition - cubePos).magnitude;

                    if (distance >= 0.005f )
                    {
                        this.estimateSpeed = (float)(distance / (Time.timeAsDouble - lastCubePositionTime));

                        this.estimateSpeed = Mathf.Min(this.estimateSpeed, 0.3f);
                        this.lastCubePosition = cubePos;
                        this.lastCubePositionTime = Time.timeAsDouble;
                    }

                    var angle = Quaternion.Angle(this.lastCubeRotation, cubeRot);
                    if (angle >= 5)
                    {
                        this.lastCubeRotation = cubeRot;
                    }
                }
            }

            this.currentRotation = Quaternion.RotateTowards(this.currentRotation, this.lastCubeRotation, 360.0f * 3.0f * Time.deltaTime);
            this.currentPosition = Vector3.MoveTowards(this.currentPosition, this.lastCubePosition, Time.deltaTime * estimateSpeed);
            //            this.currentPosition = this.lastCubePosition;
            return cube.isGrounded;
        }


    }
}
