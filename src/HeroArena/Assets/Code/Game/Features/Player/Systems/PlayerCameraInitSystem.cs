using Code.Common.Cameras;
using Cysharp.Threading.Tasks;
using Entitas;
using Unity.Netcode;

namespace Code.Game.Features.Player.Systems
{
    public class PlayerCameraInitSystem : IInitializeSystem
    {
        private readonly GameContext _gameContext;
        private readonly ICameraService _cameraService;

        private IGroup<GameEntity> _players;

        public PlayerCameraInitSystem(GameContext gameContext, ICameraService cameraService)
        {
            _gameContext = gameContext;
            _cameraService = cameraService;
        }

        public void Initialize()
        {
            _cameraService.SetTarget(null);

            InitAsync();
        }

        private async void InitAsync()
        {
            _players = _gameContext.GetGroup(GameMatcher
                .AllOf(
                GameMatcher.Player,
                GameMatcher.ClientId,
                GameMatcher.Transform));

            while (!_cameraService.HasTarget())
            {
                await UniTask.Yield();

                foreach (var player in _players)
                {
                    if(NetworkManager.Singleton.LocalClientId == player.clientId.Value)
                    {
                        _cameraService.SetTarget(player.transform.Value);

                        break;
                    }
                }
            }
        }
    }
}