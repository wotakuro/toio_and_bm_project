using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;

namespace BMProject
{
    public class PlayingScene : MonoBehaviour
    {
        private CubeManager cubeManager;
        private Cube cube;

        public ToioController controller;

        async void Start()
        {
            cubeManager = new CubeManager();
            cube = await cubeManager.SingleConnect();

            controller.Init(cubeManager, cube);
        }
    }
}