using LeandroExhumed.SnakeGame.Snake;

namespace LeandroExhumed.SnakeGame.Collectables
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