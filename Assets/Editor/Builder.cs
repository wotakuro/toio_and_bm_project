using UnityEngine;
using UnityEditor.Build;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace BMProject
{
    public class Builder
    {
        public static void BuildiOS()
        {
            BuildOptions option = BuildOptions.None;
            var scenes = EditorBuildSettings.scenes;
            BuildPipeline.BuildPlayer(scenes, "ios", BuildTarget.iOS, option);
        }
    }
}
