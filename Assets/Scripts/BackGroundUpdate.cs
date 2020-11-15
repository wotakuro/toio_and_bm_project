using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundUpdate : MonoBehaviour
{
    private Renderer rendererObj;

    private int lineWidthProp;
    private int linePadProp;
    private int lineColorProp;
    private int scrollProp;

    private Vector4 scrollParam;
    private bool moveFlag;


    private MaterialPropertyBlock propertyBlock;
    // Start is called before the first frame update
    void Start()
    {
        this.scrollParam = new Vector4();
        this.propertyBlock = new MaterialPropertyBlock();

        this.rendererObj = this.gameObject.GetComponent<Renderer>();
        this.lineWidthProp = Shader.PropertyToID("_LineSize");
        this.linePadProp = Shader.PropertyToID("_LinePad");
        this.lineColorProp = Shader.PropertyToID("_LineColor");
        this.scrollProp = Shader.PropertyToID("_ScrollParam");

        propertyBlock.SetFloat(lineWidthProp, 0.002f);
        propertyBlock.SetFloat(linePadProp, 0.04f);
        rendererObj.SetPropertyBlock(propertyBlock);
    }

    // Update is called once per frame
    void Update()
    {
        moveFlag = true;
        if (moveFlag)
        {
            this.scrollParam.y += Time.deltaTime * 0.1f;
        }
        propertyBlock.SetFloat(lineWidthProp, 0.002f +
            0.0006f * Mathf.Cos(this.scrollParam.y * 50));

        propertyBlock.SetVector(scrollProp, scrollParam);
        rendererObj.SetPropertyBlock(propertyBlock);
    }
}
