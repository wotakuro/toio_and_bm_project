using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BMProject.UI
{

    public class SettingUIWindow : EditorWindow
    {
        [MenuItem("Tools/RotatedUI")]
        public static void Create()
        {
            SettingUIWindow.GetWindow<SettingUIWindow>();
        }

        private RenderTexture m_RotTexture;
        private Material material;

        private void OnEnable()
        {
            material = new Material(Shader.Find("_Hidden/DebugRotateScreen"));
            m_RotTexture = new RenderTexture(1280, 720, 24);
            material.mainTexture = m_RotTexture;
        }
        private void OnDisable()
        {
            if (m_RotTexture)
            {
                m_RotTexture.Release();
                m_RotTexture = null;
            }

        }

        private void OnGUI()
        {
            if (!Camera.main)
            {
                Debug.LogError("Not found CameraMian");
                return;
            }
            if (!material)
            {
                material = new Material(Shader.Find("_Hidden/DebugRotateScreen"));
            }
            if (!m_RotTexture)
            {
                m_RotTexture = new RenderTexture(1280, 720, 24);
                material.mainTexture = m_RotTexture;
            }

            float w = this.position.width - 20;
            var rect = new Rect(10, 10, w, w * m_RotTexture.width / m_RotTexture.height);

            var backup = Camera.main.targetTexture;
            CameraDraw(Camera.main, m_RotTexture,
                GameCameraRotate.Instance,
                BmUICamera.Instance);
            EditorGUI.DrawPreviewTexture(rect, m_RotTexture, material);
        }

        private void CameraDraw(Camera camera, RenderTexture target,
            GameCameraRotate gameCamera,BmUICamera uiCamera,bool isLeftRot = true)
        {
            var backuRt = camera.targetTexture;
            GameCameraRotate.RotateType backupGameRot = GameCameraRotate.RotateType.None;

            BmUICamera.RotateType backupUiRot = BmUICamera.RotateType.None;

            if (gameCamera)
            {
                backupGameRot = gameCamera.rotateType;
            }
            if (uiCamera)
            {
                backupUiRot = uiCamera.rotateType;
            }



            if (isLeftRot)
            {
                if (gameCamera)
                {
                    gameCamera.rotateType = GameCameraRotate.RotateType.LeftUp;
                }
                if (uiCamera)
                {
                    uiCamera.rotateType = BmUICamera.RotateType.LeftUp;
                }
            }
            if (gameCamera)
            {
                gameCamera.ForceUpdateImmidiate();
            }

            if (uiCamera)
            {
                uiCamera.ForceUpdateImmediate();
            }
            camera.targetTexture = target;
            camera.Render();

            // restore backup 
            if (gameCamera)
            {
                gameCamera.rotateType = backupGameRot;
                gameCamera.ForceUpdateImmidiate();
            }
            if (uiCamera)
            {
                uiCamera.rotateType = backupUiRot;
                uiCamera.ForceUpdateImmediate();
            }
            camera.targetTexture = backuRt;

        }

        // Update is called once per frame
        void Update()
        {
            this.Repaint();
        }
    }
}