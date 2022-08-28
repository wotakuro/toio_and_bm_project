using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class WaitingToioConnect : MonoBehaviour
    {
        private float animateTime = 0.0f;
        private bool animateFlag = false;
        [SerializeField]
        private Transform animatedCube;

        public void StartWaiting()
        {
            this.transform.transform.localScale = Vector3.zero;
            this.gameObject.SetActive(true);
            animateFlag = true;
        }

        public void EndWaiting()
        {
            animateFlag = false;
            this.gameObject.SetActive(false);
        }


        // Update is called once per frame
        void Update()
        {
            animatedCube.transform.localRotation = Quaternion.Euler(new Vector3(0, Mathf.Max(0.0f,animateTime-0.2f) * 360.0f, 0));
            if (animateFlag)
            {
                animateTime += Time.deltaTime;

                float tm = Mathf.Clamp01( (animateTime - 0.2f)*3f);
                this.transform.transform.localScale = new Vector3(1, tm, tm);
            }
        }
    }
}
