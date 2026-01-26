using FissuredDawn.Global.Interfaces.GameServices;
using VContainer;

namespace FissuredDawn.Global.GameServices
{
    public class GameStateService : IGameStateService
    {
        [Inject] private IGameVariableService _gameVariableService;

        public void DeleteSave(int index)
        {
            throw new System.NotImplementedException();
        }

        public void LoadGame(int index)
        {
            throw new System.NotImplementedException();
        }

        public void SaveGame(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
