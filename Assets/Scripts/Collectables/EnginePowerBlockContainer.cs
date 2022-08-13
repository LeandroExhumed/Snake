namespace LeandroExhumed.SnakeGame.Block
{
    public class EnginePowerBlockContainer : BlockContainer
    {
        protected override void ResolveMVC ()
        {
            Container.Bind<IBlockModel>().To<EnginePowerBlockModel>().AsSingle();
            base.ResolveMVC();
        }
    }
}