﻿using UnityEngine;
using TMPro;
using System;
using System.Runtime.CompilerServices;

namespace BMProject
{
    public class LeftTimer : UnitySingletonBehaviour<LeftTimer>
    {
        public TextMeshPro text;
        private float limitTime = 30.0f;
        private float leftTime = 30.0f;
        private bool isCount = false;
        private char[] charBuf = new char[8];
        private Action OnEndAction;

        public float GetLeftTime()
        {
            return this.leftTime;
        }
        public float GetLimitTime()
        {
            return this.limitTime;
        }
        public float GetTimeFromStart()
        {
            return (this.limitTime - this.leftTime);
        }

        public void SetTimer(float t)
        {
            this.limitTime = t;
            this.leftTime = t;
            this.Apply(leftTime);
        }

        public void CountStart(Action onEnd)
        {
            this.isCount = true;
            this.OnEndAction = onEnd;
        }

        // Start is called before the first frame update
        void Apply(float tm)
        {
            if (tm < 0.0f)
            {
                tm = 0.0f;
            }
            int minutes = (int)(tm / 60);
            int sec = ((int)tm) % 60;
            int commaSec = (int)(tm * 100) % 100;
            DigitUtility.SetText(charBuf, 0, minutes, 2, true);
            charBuf[2] = ':';
            DigitUtility.SetText(charBuf, 3, sec, 2, true);
            charBuf[5] = ':';
            DigitUtility.SetText(charBuf, 6, commaSec, 2, true);

            this.text.SetCharArray(charBuf, 0, 8);
        }

        public void Pause()
        {
            this.isCount = false;
        }

        private void Update()
        {
            if (isCount)
            {
                leftTime -= Time.deltaTime;
                Apply(leftTime);
                if( leftTime <= 0.0f && this.OnEndAction != null)
                {
                    this.OnEndAction();
                    this.OnEndAction = null;
                }
            }
        }


    }
}