



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using System.Text;
using System;


namespace BMProject
{
    [DefaultExecutionOrder(-200)]
    public class InputWrapper :MonoBehaviour{

        public enum CurrentInputMethod
        {
            Mouse,
            Controller,
        }

        public enum Key
        {
            Select = 0,
            Up = 1,
            Down = 2,
            Left = 3,
            Right = 4,
        }
        private static readonly int KeyNum = 5;

        private int[] keyFrames = new int[KeyNum];
        private CurrentInputMethod m_currentInput = CurrentInputMethod.Controller;
        private Vector3 m_currentMousePosition;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            var gmo = new GameObject("InputWrapper",typeof(InputWrapper));
            GameObject.DontDestroyOnLoad(gmo);
        }

        public static InputWrapper Instance { get; private set; }


        public bool updatePositionFlag
        {
            get
            {
                return (m_currentInput == CurrentInputMethod.Mouse);
            }
        }

        public Vector3 pointPosition
        {
            get
            {
                return m_currentMousePosition;//;
            }
        }

        public bool isOnClicked
        {
            get
            {
                return Input.GetMouseButtonUp(0);
            }
        }

        public void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }



        public bool IsKey(Key key)
        {
            return (keyFrames[(int)key] > 0);
        }
        public bool IsKeyDown(Key key)
        {
            return (keyFrames[(int)key] == 1);
        }
        public bool IsKeyUp(Key key)
        {
            return (keyFrames[(int)key] == -1);
        }

        private void Start()
        {
            m_currentMousePosition = Input.mousePosition;
        }

        private void Update()
        {
            bool anyKeyIsOn = false;
            for(int i = 0; i < keyFrames.Length; i++)
            {
                if(IsKeyOn(i))
                {
                    anyKeyIsOn = true;
                    if (keyFrames[i] <= 0)
                    {
                        keyFrames[i] = 1;
                    }
                    else
                    {
                        ++keyFrames[i];
                    }
                }
                else
                {
                    if (keyFrames[i] > 0)
                    {
                        keyFrames[i] = -1;
                    }
                    else
                    {
                        keyFrames[i] = 0;
                    }
                }

            }
            if(anyKeyIsOn)
            {
                m_currentInput = CurrentInputMethod.Controller;
            }
            var mousePos = Input.mousePosition;
            if( (mousePos - m_currentMousePosition).sqrMagnitude > float.Epsilon)
            {
                m_currentInput = CurrentInputMethod.Mouse;
            }
            m_currentMousePosition = mousePos;
        }

        private bool IsKeyOn(int idx)
        {
            Key keycode = (Key)idx;
            switch (keycode)
            {
                case Key.Select:
                    return Input.GetKey(KeyCode.KeypadEnter) ||
                        Input.GetKey(KeyCode.Return)||  Input.GetKey((KeyCode)10) || Input.GetButton("Fire1");
                case Key.Up:
                    return Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0.2f;
                case Key.Down:
                    return Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Vertical") <- 0.2f;
                case Key.Left:
                    return Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") <- 0.2f;
                case Key.Right:
                    return Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0.2f;
            }
            return false;
        }


    }

}