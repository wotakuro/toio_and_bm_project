using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;


namespace BMProject
{
    public class ToioPlayAreaPositionDetector 
    {
        private const float checkLength = 5.0f;
        private Cube cube;
        private double groundDetectTime = -1.0;
        private Vector2 cubePos;
        public float second { get; set; } = 0.0f;

        private List<Vector2> excludePoints = new List<Vector2>();

        public ToioPlayAreaPositionDetector(Cube c)
        {
            this.cube = c;
        }

        public void AddExcludePoint(Vector2 point)
        {
            excludePoints.Add(point);
        }
        public void Clear()
        {
            excludePoints.Clear();
            second = 0.0f;
            groundDetectTime = -1.0;
        }


        public Vector2 Position
        {
            get { return cubePos; }
        }

        // Update is called once per frame
        public void Update()
        {
            if (!cube.isGrounded)
            {
                second = 0.0f;
                groundDetectTime = -1.0;
                return;
            }
            if (groundDetectTime < 0.0)
            {
                groundDetectTime = Time.timeAsDouble;
                cubePos = this.cube.pos;
            }
            foreach(var excludePoint in excludePoints)
            {
                if((cubePos-excludePoint).sqrMagnitude < checkLength * checkLength)
                {
                    groundDetectTime = -1.0;
                    second = 0.0f;
                    return;
                }
            }

            if ( (cube.pos-cubePos).sqrMagnitude >= checkLength * checkLength)
            {
                groundDetectTime = -1.0;
                second = 0.0f;
                return;
            }



            second = (float)(Time.timeAsDouble - groundDetectTime);

        }
    }
}