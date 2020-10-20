using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace toio.Simulator
{
    internal class CubeSimImpl
    {
        public virtual int maxMotor{get;}
        public virtual int deadzone{get;}
        public List<Cube.SoundOperation[]> presetSounds = new List<Cube.SoundOperation[]>();


        protected CubeSimulator cube;
        public CubeSimImpl(CubeSimulator cube)
        {
            this.cube = cube;
        }


        // ============ Simulate ============
        public virtual void Simulate(){
            SimulateMotor();
        }


        // ============ toio ID ============
        public virtual int x { get; internal set; }
        public virtual int y { get; internal set; }
        public virtual int deg { get; internal set; }
        public virtual int xSensor { get; internal set; }
        public virtual int ySensor { get; internal set; }
        public virtual uint standardID { get; internal set; }
        public virtual bool onMat { get; internal set; } = false;
        public virtual bool onStandardID { get; internal set; } = false;
        public virtual void StartNotification_StandardID(System.Action<uint, int> action)
        { NotSupportedWarning(); }
        public virtual void StartNotification_StandardIDMissed(System.Action action)
        { NotSupportedWarning(); }
        public virtual void StartNotification_PositionID(System.Action<int, int, int, int, int> action)
        { NotSupportedWarning(); }
        public virtual void StartNotification_PositionIDMissed(System.Action action)
        { NotSupportedWarning(); }


        // ============ Button ============
        public virtual bool button {
            get{ NotSupportedWarning(); return default; }
            internal set{ NotSupportedWarning(); }}
        public virtual void StartNotification_Button(System.Action<bool> action)
        { NotSupportedWarning(); }


        // ============ Motion Sensor ============
        // ---------- 2.0.0 ----------
        // Sloped
        public virtual bool sloped {
            get{ NotSupportedWarning(); return default; }
            internal set{ NotSupportedWarning(); }}
        public virtual void StartNotification_Sloped(System.Action<bool> action)
        { NotSupportedWarning(); }

        // Collision Detected
        public virtual bool collisionDetected {
            get{ NotSupportedWarning(); return default; }
            internal set{ NotSupportedWarning(); }}
        public virtual void StartNotification_CollisionDetected(System.Action<bool> action)
        { NotSupportedWarning(); }
        // ---------- 2.1.0 ----------
        // Pose
        public virtual Cube.PoseType pose {
            get{ NotSupportedWarning(); return default; }
            internal set{ NotSupportedWarning(); }}
        public virtual void StartNotification_Pose(System.Action<Cube.PoseType> action)
        { NotSupportedWarning(); }
        // Double Tap
        public virtual bool doubleTap {
            get{ NotSupportedWarning(); return default; }
            internal set{ NotSupportedWarning(); }}
        public virtual void StartNotification_DoubleTap(System.Action<bool> action)
        { NotSupportedWarning(); }



        // ============ Motor ============
        protected float speedL = 0;  // (m/s)
        protected float speedR = 0;
        protected float motorLeft{get; set;} = 0;   // モーター指令値
        protected float motorRight{get; set;} = 0;
        public virtual void SimulateMotor()
        {
            var dt = Time.deltaTime;

            // 目標速度を計算
            // target speed
            float targetSpeedL = motorLeft * CubeSimulator.VDotOverU / Mat.DotPerM;
            float targetSpeedR = motorRight * CubeSimulator.VDotOverU / Mat.DotPerM;
            if (Mathf.Abs(motorLeft) < deadzone) targetSpeedL = 0;
            if (Mathf.Abs(motorRight) < deadzone) targetSpeedR = 0;

            // 速度更新
            // update speed
            if (cube.forceStop || this.button)   // 強制的に停止
            {
                speedL = 0; speedR = 0;
            }
            else
            {
                if (cube.offGroundL) targetSpeedL = 0;
                if (cube.offGroundR) targetSpeedR = 0;
                speedL += (targetSpeedL - speedL) / Mathf.Max(cube.motorTau,dt) * dt;
                speedR += (targetSpeedR - speedR) / Mathf.Max(cube.motorTau,dt) * dt;
            }

            cube._SetSpeed(speedL, speedR);
        }



        // ============ Commands ============
        public virtual void Move(int left, int right, int durationMS)
        { NotSupportedWarning(); }
        public virtual void StopLight()
        { NotSupportedWarning(); }
        public virtual void SetLight(int r, int g, int b, int durationMS)
        { NotSupportedWarning(); }
        public virtual void SetLights(int repeatCount, Cube.LightOperation[] operations)
        { NotSupportedWarning(); }
        public virtual void PlaySound(int repeatCount, Cube.SoundOperation[] operations)
        { NotSupportedWarning(); }
        public virtual void PlayPresetSound(int soundId, int volume)
        { NotSupportedWarning(); }
        public virtual void StopSound()
        { NotSupportedWarning(); }
        public virtual void SetSlopeThreshold(int angle)
        { NotSupportedWarning(); }

        protected virtual void NotSupportedWarning()
        { Debug.LogWarning("Not Supported in this firmware version."); }
    }
}