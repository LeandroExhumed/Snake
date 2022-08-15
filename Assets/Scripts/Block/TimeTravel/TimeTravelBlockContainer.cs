namespace LeandroExhumed.SnakeGame.Block
{
    public class TimeTravelBlockContainer : BlockContainer
    {
        protected override void ResolveMVC ()
        {
            Container.Bind<IBlockModel>().To<TimeTravelBlockModel>().AsSingle();
            base.ResolveMVC();
        }
    }
}