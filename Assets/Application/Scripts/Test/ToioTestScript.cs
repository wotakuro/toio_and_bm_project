using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using toio;
using BMProject;
using System.Text;

public class ToioTestScript : MonoBehaviour
{
    private Cube cube;
    private CubeManager cubeManager;
    [SerializeField]
    public Text info;

    private StringBuilder sb = new StringBuilder();

    // Start is called before the first frame update
    async void Start()
    {
        this.cubeManager = ToioConnectionMgr.Instance.cubeManager;
        this.cube = await ToioConnectionMgr.Instance.ConnectCube();


    }

    // Update is called once per frame
    void Update()
    {
        if(cube != null)
        {
            sb.Clear();
            sb.Append("toio (").Append(cube.pos.x).Append(",").
                Append(cube.pos.y).Append(") angle:").Append(cube.angle).
                Append("\ncollision:").Append(cube.isCollisionDetected);
           
            this.info.text = sb.ToString();
        }
    }
}
