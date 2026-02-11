using Cysharp.Threading.Tasks;

namespace FissuredDawn.Global.Interfaces.GameServices
{
    public interface IResourceLoader
    {
        UniTask LoadResources(string configPath);
    }
}