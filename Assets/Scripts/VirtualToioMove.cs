using System.Collections;
using System.Collections.Generic;
using toio;
using toio.Simulator;
using UnityEngine;

namespace BMProject
{
    [RequireComponent(typeof(Rigidbody))]
    public class VirtualToioMove : MonoBehaviour
    {
        readonly float motorTau = 0.04f;

        private float speedL;
        private float speedR;
        private Rigidbody rb;
        //private Cube cube;

        private void Start()
        {
            rb = this.GetComponent<Rigidbody>();
        }

        public void Move(int left,int right)
        {
            
        }


        private void FixedUpdate()
        {
            
        }

        void Simulate(float motorLeft,float motorRight)
        {
            var dt = Time.deltaTime;

            float deadzone = 8f;
            // 目標速度を計算
            // target speed
            float targetSpeedL = motorLeft * CubeSimulator.VDotOverU / Mat.DotPerM;
            float targetSpeedR = motorRight * CubeSimulator.VDotOverU / Mat.DotPerM;
            if (Mathf.Abs(motorLeft) < deadzone) targetSpeedL = 0;
            if (Mathf.Abs(motorRight) < deadzone) targetSpeedR = 0;

            speedL += (targetSpeedL - speedL) / Mathf.Max(motorTau, dt) * dt;
            speedR += (targetSpeedR - speedR) / Mathf.Max(motorTau, dt) * dt;

        }

        // toioSDKよりシミュレーターの処理を
        private void _SetSpeed(float speedL, float speedR)
        {
            this.rb.angularVelocity = transform.up * (float)((speedL - speedR) / CubeSimulator.TireWidthM);
            var vel = transform.forward * (speedL + speedR) / 2;
            var dv = vel - this.rb.velocity;
            this.rb.AddForce(dv, ForceMode.VelocityChange);
        }

    }
}