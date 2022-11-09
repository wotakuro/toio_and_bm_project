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
            if (areaRightFront.x < areaLeftBack.x)
            {
                if (areaRightFront.y < areaLeftBack.y)
                {
                    return new Vector2Int((areaRightFront.x + areaLeftBack.x) / 2,
                            (areaLeftBack.y *4 + areaRightFront.y) / 5);
                }
                else
                {
                    return new Vector2Int((areaRightFront.x  + areaLeftBack.x * 4) / 5,
                            (areaLeftBack.y + areaRightFront.y) / 2);
                }
            }
            else
            {
                if (areaRightFront.y < areaLeftBack.y)
                {
                    return new Vector2Int((areaRightFront.x + areaLeftBack.x *4) / 5,
                            (areaLeftBack.y + areaRightFront.y ) / 2);
                }
                else
                {
                    return new Vector2Int((areaRightFront.x + areaLeftBack.x) / 2,
                            (areaLeftBack.y * 4 + areaRightFront.y) / 5);

                }
            }

        }
        public static int GetInitializeRotation(Vector2Int areaRightFront, Vector2Int areaLeftBack)
        {
            if(areaRightFront.x < areaLeftBack.x)
            {
                if(areaRightFront.y < areaLeftBack.y)
                {
                    return 270; // ok 
                }
                else
                {
                    return 180;
                }
            }
            else
            {
                if (areaRightFront.y < areaLeftBack.y)
                {
                    return 0;
                }
                else
                {
                    return 90;

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


        public static bool IsInPlayArea( Vector2 pos, Vector2Int areaRightFront, Vector2Int areaLeftBack,float margin = 0.0f)
        {
            if ( (areaRightFront.x - margin <= pos.x && pos.x <= areaLeftBack.x + margin) ||
                (areaRightFront.x + margin >= pos.x && pos.x >= areaLeftBack.x - margin) ) {

                if ((areaRightFront.y -margin <= pos.y && pos.y <= areaLeftBack.y + margin) ||
                    (areaRightFront.y +margin >= pos.y && pos.y >= areaLeftBack.y - margin))
                {
                    return true;
                }
            }
            return false;
        }
        public static Quaternion GetRotation(int angle)
        {
            return Quaternion.Euler(0, angle - 90, 0);
        }
    }
}