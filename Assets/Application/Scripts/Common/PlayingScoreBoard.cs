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
        private int currentScore = 0;

        private void Awake()
        {
            InitHitArray();
        }

        public void AddScore(int p)
        {
            currentScore += p;
            SetScore(currentScore);
        }

        public int GetScore()
        {
            return this.currentScore;
        }

        private void SetScore(int score)
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