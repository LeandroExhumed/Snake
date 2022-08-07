using LeandroExhumed.SnakeGame.Match;
using Zenject;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridContainer : MonoInstaller
    {
        [Inject]
        private readonly MatchData matchData;

        public override void InstallBindings ()
        {
            ResolveMVC();
        }

        private void ResolveMVC ()
        {
            Container.Bind<IGridModel<INode>>().FromInstance(new GridModel<INode>(
                matchData.BoardSize.x, matchData.BoardSize.y)).AsSingle();
            Container.Bind<GridController>().AsSingle();
            Container.BindInstance(GetComponent<GridView>()).AsSingle();
        }
    }
}