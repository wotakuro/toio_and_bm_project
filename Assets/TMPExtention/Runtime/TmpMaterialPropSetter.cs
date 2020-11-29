using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPExtention
{
    [ExecuteAlways]
    public class TmpMaterialPropSetter : MonoBehaviour
    {
        [SerializeField]
        private Color faceColor;
        [SerializeField]
        private float softness;

        [SerializeField]
        private Color outlineColor;
        [SerializeField]
        private float outlineThickness;

        private Renderer renderComponent;
        private MaterialPropertyBlock propertyBlock;
        private CanvasRenderer canvasRenderer;

        // props
        private static int outlineColorProp = Shader.PropertyToID("_OutlineColor");
        private static int outlineThicknessProp = Shader.PropertyToID("_OutlineWidth");
        private static int faceColorProp = Shader.PropertyToID("_FaceColor");
        private static int softnessProp = Shader.PropertyToID("_OutlineSoftness");

        void Awake()
        {
            renderComponent = this.GetComponent<Renderer>();
            if (renderComponent)
            {
                propertyBlock = new MaterialPropertyBlock();
            }
            else
            {
                canvasRenderer = this.GetComponent<CanvasRenderer>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (propertyBlock != null)
            {
                propertyBlock.SetColor(outlineColorProp, outlineColor);
                propertyBlock.SetFloat(outlineThicknessProp, outlineThickness);
                propertyBlock.SetColor(faceColorProp, faceColor);
                propertyBlock.SetFloat(softnessProp, softness);
                this.renderComponent.SetPropertyBlock(propertyBlock);
                return;
            }
        }
    }
}
