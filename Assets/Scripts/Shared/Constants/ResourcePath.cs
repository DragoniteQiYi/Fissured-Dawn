using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FissuredDawn.Shared.Constants
{
    public static class ResourcePath
    {
        public static readonly string ScenePath = 
            Path.Combine(Application.dataPath, "Scenes");
    }
}
