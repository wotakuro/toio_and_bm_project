using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class ToioPositionConverter
    {
        private const int LeftX = 98;
        private const int RightX = 402;
        private const int UpY = 142;
        private const int DownY = 358;

        private const float A3_Width = 0.420f;
        private const float A3_Height = 0.297f;

        public static Vector2Int GetInitializePosition(Vector2Int areaRightFront,Vector2Int areaLeftBack)
        {
            return new Vector2Int((areaRightFront.x + areaLeftBack.x) / 2,
                    (areaLeftBack.y + areaRightFront.y * 4) / 5);
        }
        public static int GetInitializeRotation(Vector2Int areaRightFront, Vector2Int areaLeftBack)
        {
            // ‰¼‘Î‰ž
            if(areaRightFront.x < areaLeftBack.x)
            {
                if(areaRightFront.y < areaLeftBack.y)
                {
                    return 270;
                }
                else
                {
                    return 90;
                }
            }
            else
            {
                if (areaRightFront.y < areaLeftBack.y)
                {
                    return 180;
                }
                else
                {
                    return 0;

                }
            }
        }

        public static Vector2 ConvertPosition(Vector2 pos)
        {
            float toioWidth = (RightX - LeftX);
            float toioHeight = (DownY - UpY);
            float widthScale = A3_Width / -toioWidth;
            float heightScale = A3_Height / toioHeight;

            pos.x *= widthScale;
            pos.y *= heightScale;

            return pos;
        }

        public static Quaternion GetRotation(int angle)
        {
            return Quaternion.Euler(0, angle - 90, 0);
        }
    }
}