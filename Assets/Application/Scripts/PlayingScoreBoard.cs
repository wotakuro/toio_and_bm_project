using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BMProject
{
    public class PlayingScoreBoard : MonoBehaviour
    {
        public TextMeshPro hitNumUI;
        private char[] hitNumCharArr;

        private void Awake()
        {
            InitHitArray();
        }

        public void SetScore(int score)
        {
            DigitUtility.SetText(hitNumCharArr, 0, score, 3, false);
            hitNumUI.SetCharArray(hitNumCharArr);

        }
        void InitHitArray()
        {
            hitNumCharArr = new char[16];
            for (int i = 0; i < 3; ++i)
            {
                hitNumCharArr[i] = ' ';
            }
            hitNumCharArr[3] = 'H';
            hitNumCharArr[4] = 'I';
            hitNumCharArr[5] = 'T';
            SetScore(0);

        }

    }
}