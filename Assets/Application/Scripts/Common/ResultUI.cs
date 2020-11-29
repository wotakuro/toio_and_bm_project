using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro scoreText;
    // todo 仮
    public void SetScore(int score)
    {
        scoreText.text = score + "点";
    }

    public void StartResult()
    {
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
