using LeandroExhumed.SnakeGame.Match;
using Zenject;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridContainer : MonoInstaller
    {
        [Inject]
        private readonly GameData matchData;

        public override void InstallBindings ()
        {
            ResolveMVC();
        }

        private void ResolveMVC ()
        {
            Container.Bind<IGridModel<INodeModel>>().FromInstance(new GridModel<INodeModel>(
                matchData.BoardSize.x, matchData.BoardSize.y)).AsSingle();
            Container.Bind<GridController>().AsSingle();
            Container.BindInstance(GetComponent<GridView>()).AsSingle();
        }
    }
}