using LeandroExhumed.SnakeGame.Snake;

namespace LeandroExhumed.SnakeGame.Collectables
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