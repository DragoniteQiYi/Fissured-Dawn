using UnityEngine;

namespace FissuredDawn.Infrastructure.Startup
{
    public class PersistentEventSystem : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
