using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BMProject
{
    public class PlayingScoreBoard : UnitySingletonBehaviour<PlayingScoreBoard>
    {
        public TextMeshPro hitNumUI;
        private char[] hitNumCharArr;
        private int currentScore = 0;
        private List<float> addScoreTimings;

        private new void Awake()
        {
            base.Awake();
            this.addScoreTimings = new List<float>(32);
            InitHitArray();
        }

        public void AddScore(int p,float time)
        {
            currentScore += p;
            this.addScoreTimings.Add(time);
            SetScore(currentScore);
        }

        public int GetScore()
        {
            return this.currentScore;
        }
        public List<float> GetTimings()
        {
            return this.addScoreTimings;
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