using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMProject
{
    public class TitleBackGround : MonoBehaviour
    {
        private int scrollProp;
        private Vector4 scrollParam;
        private MaterialPropertyBlock propertyBlock;
        private Renderer renderComponent;


        // Start is called before the first frame update
        void Awake()
        {
            this.renderComponent = this.GetComponent<Renderer>();
            this.scrollProp = Shader.PropertyToID("_ScrollParam");
            propertyBlock = new MaterialPropertyBlock();

        }

        // Update is called once per frame
        void Update()
        {
            scrollParam.x -= Time.deltaTime * 0.06f;
            scrollParam.y += Time.deltaTime * 0.04f;
            propertyBlock.SetVector(scrollProp, scrollParam);
            renderComponent.SetPropertyBlock(propertyBlock);
        }
    }
}