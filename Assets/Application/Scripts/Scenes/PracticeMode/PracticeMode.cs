using System.Collections;
using System.Collections.Generic;
using toio;
using UnityEngine;


namespace BMProject
{
    public class PracticeMode : MonoBehaviour
    {

        private CubeManager cubeManager;
        private Cube cube;

        private float cubeTime = 0.0f;
        private bool cubeIsWake = false;
        // Start is called before the first frame update
        async void Start()
        {

            // sleep OFF
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            this.cubeManager = ToioConnectionMgr.Instance.cubeManager;
            this.cube = await ToioConnectionMgr.Instance.ConnectCube();
            if (this.cube != null)
            {
            }
            cube.collisionCallback.AddListener("Collision", OnCollisionToio);
            cube.Move(-80, -80, 100, Cube.ORDER_TYPE.Strong);
            cubeIsWake = true;
        }
        void OnCollisionToio(Cube cube)
        {
            if (cubeIsWake && cubeTime > 0.5f)
            {
                cube.Move(80, 80, 100, Cube.ORDER_TYPE.Strong);
                cubeTime = 0.0f;
                cubeIsWake = false;
            }
        }

        private void Update()
        {
            if(cube == null) { return; }
            cubeTime += Time.deltaTime;

            if(!cubeIsWake && cubeTime > 2.5f)
            {
                cube.Move(-80, -80, 100, Cube.ORDER_TYPE.Strong);
                cubeTime = 0.0f;
                cubeIsWake = true;
            }
        }

        private void OnDestroy()
        {
            Disconnect();
            // Sleep off
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        private void Disconnect()
        {
            if (cubeManager != null && cube != null)
            {
                ToioConnectionMgr.Instance.ReleaseCube(cube);
                cube = null;
            }
        }

    }
}