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

        private int moveLeft = 0;
        private int moveRight = 0;
        private int moveCmdDuration = 0;
        private float moveCmdTime = 0;

        private void Start()
        {
            rb = this.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            this.rb.maxAngularVelocity = 21f;
        }

        public void Move(int left,int right)
        {
            this.moveCmdTime = 0;
            this.moveLeft = left;
            this.moveRight = right;
            this.moveCmdDuration = 100;
        }
        public void Move(int left, int right,int durationMs)
        {
            this.moveCmdTime = 0;
            this.moveCmdDuration = durationMs;
            this.moveLeft = left;
            this.moveRight = right;
        }


        private void FixedUpdate()
        {
            float dt = Time.deltaTime;
            this.ScheduleMotor(dt);
            this.Simulate(dt,moveLeft, moveRight);
        }
        private void ScheduleMotor(float dt)
        {
            moveCmdTime -= dt;
            if (this.moveCmdDuration != 0)
            {
                this.moveCmdTime = this.moveCmdDuration * 0.001f;
                this.moveCmdDuration = 0;
            }

            if (this.moveCmdTime < 0)
            {
                this.moveLeft = 0;
                this.moveRight = 0;
            }

        }

        void Simulate(float dt , float motorLeft,float motorRight)
        {

            float deadzone = 8f;
            // 目標速度を計算
            // target speed
            float targetSpeedL = motorLeft * CubeSimulator.VDotOverU / Mat.DotPerM;
            float targetSpeedR = motorRight * CubeSimulator.VDotOverU / Mat.DotPerM;
            if (Mathf.Abs(motorLeft) < deadzone) targetSpeedL = 0;
            if (Mathf.Abs(motorRight) < deadzone) targetSpeedR = 0;

            speedL += (targetSpeedL - speedL) / Mathf.Max(motorTau, dt) * dt;
            speedR += (targetSpeedR - speedR) / Mathf.Max(motorTau, dt) * dt;
            _SetSpeed(speedL, speedR);


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