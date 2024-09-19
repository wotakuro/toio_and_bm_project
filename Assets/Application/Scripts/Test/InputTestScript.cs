#if UNITY_ANDROID && !UNITY_EDITOR
#define UNITY_ANDROID_RUNTIME
#endif


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using BMProject;

public class InputTestScript : MonoBehaviour
{
    private Text txt;
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        sb.Clear();
        foreach (InputWrapper.Key val in Enum.GetValues(typeof(InputWrapper.Key)))
        { 
            sb.Append(val.ToString() ).Append(":").Append( InputWrapper.Instance.IsKeyUp(val) ).Append( " " ).
                Append( InputWrapper.Instance.IsKeyDown(val) ).Append( " ").
                Append( InputWrapper.Instance.IsKey(val) ).Append( "\n" );
        }

        sb.Append(InputWrapper.Instance.pointPosition).Append("::")
            .Append(InputWrapper.Instance.isOnClicked).Append("\n");
#if UNITY_ANDROID_RUNTIME && ENABLE_ANDROID_AUTOBOOT
        sb.Append("\nAutoBoot ").Append( Wotakuro.AndroidAutoBoot.GetEnable() );
        if (InputWrapper.Instance.IsKeyDown(InputWrapper.Key.Select))
        {
            Wotakuro.AndroidAutoBoot.SetEnable( !Wotakuro.AndroidAutoBoot.GetEnable());
        }
#endif

        txt.text = sb.ToString();
    }
}
