#if UNITY_ANDROID && !UNITY_EDITOR 
#define UNITY_ANDROID_RUNTIME
#endif


#if UNITY_ANDROID_RUNTIME

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wotakuro
{
    public class AndroidAutoBoot
    {

        private static bool cachedFlag = false;
        private static bool isRead = false;

        public static void SetEnable(bool flag)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.wotakuro.autoboot.BootEventReciever"))
            {
                jc.CallStatic("setEnabled", GetContext(), flag);
                cachedFlag = flag;
            }
        }

        public static bool GetEnable()
        {
            if (isRead) { return cachedFlag; }

            using (AndroidJavaClass jc = new AndroidJavaClass("com.wotakuro.autoboot.BootEventReciever"))
            {
                isRead = true;
                cachedFlag = jc.CallStatic<bool>("getEnabled", GetContext());

                return cachedFlag;
            }
        }



        private static AndroidJavaObject GetContext()
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
                    return context;
                }
            }
        }
    }
}

#endif