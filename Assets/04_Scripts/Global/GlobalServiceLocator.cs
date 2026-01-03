using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Global
{
    public class GlobalServiceLocator
    {
        public static IObjectResolver Container { get; private set; }

        public static void SetContainer(IObjectResolver container)
        {
            Container = container;
        }
    }
}
