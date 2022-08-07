using LeandroExhumed.SnakeGame.Snake;

namespace LeandroExhumed.SnakeGame.Collectables
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