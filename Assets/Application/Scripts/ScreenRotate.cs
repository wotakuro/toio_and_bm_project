using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

// ‰¼‘Î‰ž’†
namespace BMProject
{
    public class ScreenRotate : MonoBehaviour
    {
        [SerializeField]
        private UniversalAdditionalCameraData mainCamera;
        [SerializeField]
        private UniversalAdditionalCameraData uiCamera;

        [SerializeField]
        private RawImage mainCameraTarget;
        [SerializeField]
        private RawImage uiCameraTarget;

        [SerializeField]
        private Transform playingUI;

        private RenderTexture mainCameraRt;
        private RenderTexture uiCameraRt;

        // Start is called before the first frame update
        void Start()
        {
            mainCamera.cameraStack.Clear();
            mainCameraRt = new RenderTexture( (int)(Screen.height * 0.7f), (int)(Screen.width * 0.7f), 24);
            mainCamera.GetComponent<Camera>().targetTexture = mainCameraRt;

            uiCamera.renderType = CameraRenderType.Base;
            var uiCameraObject = uiCamera.GetComponent<Camera>();


            uiCameraRt = new RenderTexture(Screen.height, Screen.width, 24);
            uiCameraObject.targetTexture = uiCameraRt;
            uiCameraObject.orthographicSize = 27.0f;

            mainCameraTarget.gameObject.SetActive(true);
            uiCameraTarget.gameObject.SetActive(true);

            mainCameraTarget.texture = mainCameraRt;
            uiCameraTarget.texture = uiCameraRt;
            mainCameraTarget.rectTransform.sizeDelta = new Vector2(Screen.height, Screen.width);
            uiCameraTarget.rectTransform.sizeDelta = new Vector2(Screen.height, Screen.width);

            playingUI.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        }
        private void OnDestroy()
        {
            mainCameraRt.Release();
            uiCameraRt.Release();
        }
    }
}