namespace LeandroExhumed.SnakeGame.Snake
{
    public class DefaultBlockContainer : BlockContainer
    {
        protected override void ResolveMVC ()
        {
            Container.Bind<IBlockModel>().To<BlockModel>().AsSingle();
            base.ResolveMVC();
        }
    }
}