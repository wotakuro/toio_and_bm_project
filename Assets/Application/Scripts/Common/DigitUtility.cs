using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class DigitUtility
    {
        public readonly static char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static void SetText(char[] chBuf,int chIdx,int param, int digits, bool fillZero = true)
        {
            int div = Pow10(digits-1);
            for (int i = 0; i < digits; ++i)
            {
                int p = param / div;
                if (fillZero || p > 0 || i == (digits-1))
                {
                    chBuf[i + chIdx] = DigitUtility.digits[p % 10];
                }
                else
                {
                    chBuf[i + chIdx] = ' ';
                }
                div = div / 10;
            }
        }
        private static int Pow10(int n)
        {
            int p = 1;
            for(int i = 0; i < n; ++i)
            {
                p *= 10;
            }

            return p;
        }

    }
}