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

    private float currentTime = 0.0f;

    [SerializeField]
    private Color lineColor;

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

        propertyBlock.SetFloat(lineWidthProp, 0.0003f);
        propertyBlock.SetFloat(linePadProp, 0.008f);
        propertyBlock.SetColor(lineColorProp, lineColor);
        rendererObj.SetPropertyBlock(propertyBlock);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveFlag)
        {
            currentTime += Time.deltaTime;
//            this.scrollParam.y += Time.deltaTime * 0.1f;
        }
        propertyBlock.SetFloat(lineWidthProp, 0.0003f +
            0.0001f * Mathf.Cos(currentTime * 5));

        propertyBlock.SetVector(scrollProp, scrollParam);
        rendererObj.SetPropertyBlock(propertyBlock);
    }
}
