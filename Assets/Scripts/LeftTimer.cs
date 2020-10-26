using UnityEngine;
using TMPro;

namespace BMProject
{
    public class LeftTimer : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public float time = 30.0f;
        private char[] charBuf = new char[8];

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

        private void Update()
        {
            time -= Time.deltaTime;
            if (time < 0.0f)
            {
                time = 0.0f;
            }
            Apply(time);
        }


    }
}