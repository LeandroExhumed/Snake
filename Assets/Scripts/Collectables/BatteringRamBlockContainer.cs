namespace LeandroExhumed.SnakeGame.Block
{
    public class BatteringRamBlockContainer : BlockContainer
    {
        protected override void ResolveMVC ()
        {
            Container.Bind<IBlockModel>().To<BatteringRamBlockModel>().AsSingle();
            base.ResolveMVC();
        }
    }
}