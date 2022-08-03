using Zenject;

namespace LeandroExhumed.SnakeGame.Grid
{
    public class GridContainer : MonoInstaller
    {
        public override void InstallBindings ()
        {
            ResolveMVC();
        }

        private void ResolveMVC ()
        {
            Container.Bind<IGridModel>().FromInstance(new GridModel(30, 30)).AsSingle();
            Container.Bind<GridController>().AsSingle();
            Container.BindInstance(GetComponent<GridView>()).AsSingle();
        }
    }
}