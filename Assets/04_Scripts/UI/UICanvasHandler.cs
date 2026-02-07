using UnityEngine;

namespace FissuredDawn
{
    public class UICanvasHandler : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
