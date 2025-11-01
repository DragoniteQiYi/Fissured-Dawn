using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FissuredDawn.Shared.Constants
{
    public static class ConfigPath
    {
        public static readonly string SceneConfigPath = 
            Path.Combine(Application.dataPath, "Configurations", "Scenes.json");
    }
}
