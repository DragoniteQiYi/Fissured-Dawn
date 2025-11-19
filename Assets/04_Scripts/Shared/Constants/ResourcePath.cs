using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FissuredDawn.Shared.Constants
{
    public static class ResourcePath
    {
        private static readonly string GeneralPath =
            Path.Combine(Application.dataPath, "02_Gameplay");
        public static readonly string ScenePath = 
            Path.Combine(GeneralPath, "Scenes");
    }
}
