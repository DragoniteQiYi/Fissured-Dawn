using UnityEngine;

namespace FissuredDawn
{
    public class StartupComponent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
