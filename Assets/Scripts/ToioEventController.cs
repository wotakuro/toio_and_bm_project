using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using TMPro;

namespace BMProject
{
    public class ToioEventController : MonoBehaviour
    {
        public TextMeshProUGUI hitNumUI;
        private char[] hitNumCharArr;

        private readonly static char[] digits = new char[] { '0', '1', '2', '3', '4','5','6','7','8','9' };

        private int hitNum = 0;

        private float tm = 0.0f;
        private float lastHitTime = -10.0f;

        private void Awake()
        {
            hitNumCharArr = new char[16];
            for (int i = 0; i < 3; ++i)
            {
                hitNumCharArr[i] = ' ';
            }
            hitNumCharArr[3] = 'H';
            hitNumCharArr[4] = 'I';
            hitNumCharArr[5] = 'T';
            SetText(0);
        }

        private void Update()
        {
            this.tm += Time.deltaTime;
        }

        // Start is called before the first frame update
        public void InitCube(CubeManager mgr, Cube c)
        {
            c.collisionCallback.AddListener("ToioEventCtrl",OnCubeHit);
        }

        // キューブがヒットした時の処理
        private void OnCubeHit(Cube c)
        {
            if( this.tm - this.lastHitTime < 0.2f)
            {
                return;
            }
            c.PlayPresetSound(0);
            c.TurnLedOn(255, 200, 200, 120);
            ++hitNum;
            this.lastHitTime = this.tm;
            SetText(hitNum);
        }

        private void SetText(int param)
        {
            int div = 100;
            for (int i = 0; i < 3; ++i)
            {
                int p = param / div;
                if( p > 0 || i == 2)
                {
                    hitNumCharArr[i] = digits[p %10];
                }
                else
                {
                    hitNumCharArr[i] = ' ';
                }
                div = div / 10;
            }

            hitNumUI.SetCharArray(hitNumCharArr);
        }

    }
}