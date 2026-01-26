using FissuredDawn.Global.Interfaces.GameManagers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Scope.Exploration.Generated
{
    public class CharacterInteractController : MonoBehaviour
    {
        [Inject] private readonly IInputManager _inputManager;
    }
}
