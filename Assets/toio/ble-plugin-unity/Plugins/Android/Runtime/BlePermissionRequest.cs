
#if UNITY_ANDROID && !UNITY_EDITOR
#define UNITY_ANDROID_RUNTIME
#endif

#define UNITY_ANDROID_RUNTIME

#if UNITY_ANDROID_RUNTIME
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Android;

namespace toio.Android
{
    public class BlePermissionRequest
    {
        Action initializedAction;
        Action<string> errorAction;

        public BlePermissionRequest(Action success,Action <string> error)
        {
            this.initializedAction = success;
            this.errorAction = error;
        }


        public IEnumerator Request()
        {
            var permissions = GetPermissionStr();
            foreach (var permission in permissions)
            {
                bool flag = true;
                while (flag)
                {
                    if (!Permission.HasUserAuthorizedPermission(permission))
                    {
                        Permission.RequestUserPermission(permission);
                    }
                    yield return null;
                    if (!Permission.HasUserAuthorizedPermission(permission))
                    {
                        this.errorAction("No Permission:" + permission);
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            this.initializedAction();
        }

        private string[] GetPermissionStr()
        {
            int apiLevel = GetAPIVersion();
            //            permission = Permission.CoarseLocation;
            return new string[] { Permission.FineLocation };
        }
        private int GetAPIVersion()
        {
            var cls = new AndroidJavaClass("android.os.Build$VERSION");
            var apiLevel = cls.GetStatic<int>("SDK_INT");
            return apiLevel;
        }


    }
}
#endif