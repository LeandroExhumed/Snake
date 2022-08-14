using LeandroExhumed.SnakeGame.Grid;
using LeandroExhumed.SnakeGame.Match;
using LeandroExhumed.SnakeGame.Snake;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame
{
    public class ApplicationContainer : MonoInstaller
    {
        [SerializeField]
        private MatchData matchData;

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
            Container.BindInstance(matchData).AsSingle();

            Container.BindInstance(snakeList.Snakes).AsSingle();

            Container.Bind<ILobbyModel>().FromInstance(lobby).AsSingle();
            Container.Bind<IMatchModel>().FromInstance(match).AsSingle();
            Container.Bind<IGridModel<INode>>().FromInstance(levelGrid).AsSingle();
        }
    } 
}