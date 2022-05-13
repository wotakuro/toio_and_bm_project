using System.Collections;
using toio;
using UnityEngine;

namespace BMProject
{
    public class WaitForToioGroundCheck : CustomYieldInstruction
    {
        private double time;
        private Cube cube;
        public WaitForToioGroundCheck(Cube c)
        {
            this.cube = c;
            time = Time.timeAsDouble;
        }

        public override bool keepWaiting
        {
            get
            {
                if (cube.isGrounded) { return false; }
                if (Time.timeAsDouble - time > 2.0) { return false; }
                return true;
            }
        }
    }
}
