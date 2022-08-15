using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Match;
using LeandroExhumed.SnakeGame.Services;
using LeandroExhumed.SnakeGame.Snake;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame
{
    public class ApplicationContainer : MonoInstaller
    {
        [SerializeField]
        private AudioSource musicAudioSource;
        [SerializeField]
        private AudioSource sfxAudioSource;

        [SerializeField]
        private GameData matchData;

        [SerializeField]
        private SnakeList snakeList;

        [SerializeField]
        private LobbyFacade lobby;
        [SerializeField]
        private MatchFacade match;
        [SerializeField]
        private GridFacade levelGrid;

        public override void InstallBindings ()
        {
            ResolveServices();

            Container.BindInstance(matchData).AsSingle();

            Container.BindInstance(snakeList.Snakes).AsSingle();
            ResolveEntities();
        }

        private void ResolveServices ()
        {
            Container.Bind<Pool>().AsSingle();
            ResolveAudio();
        }

        private void ResolveAudio ()
        {
            Container.BindInstance(musicAudioSource).AsCached();
            Container.BindInstance(sfxAudioSource).WithId("SFX").AsCached();
            Container.BindInstance(transform).WhenInjectedInto<AudioProvider>();
            Container.Bind<AudioProvider>().AsSingle();
        }

        private void ResolveEntities ()
        {
            Container.Bind<ILobbyModel>().FromInstance(lobby).AsSingle();
            Container.Bind<IMatchModel>().FromInstance(match).AsSingle();
            Container.Bind<IGridModel<INodeModel>>().FromInstance(levelGrid).AsSingle();
        }
    } 
}