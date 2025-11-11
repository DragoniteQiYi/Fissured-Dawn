using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FissuredDawn.Shared.Constants
{
    public static class ConfigPath
    {
        private static readonly string BasePath = Path.Combine(Application.dataPath, "Data", "Configs");

        public static readonly string SceneConfigPath = 
            Path.Combine(BasePath, "Scenes.json");
    }
}
