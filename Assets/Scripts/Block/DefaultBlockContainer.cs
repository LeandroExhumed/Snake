namespace LeandroExhumed.SnakeGame.Block
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