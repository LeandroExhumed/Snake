using LeadroExhumed.SnakeGame.Input;
using Zenject;

namespace LeadroExhumed.SnakeGame.Match
{
    public class MatchContainer : MonoInstaller
    {
        public override void InstallBindings ()
        {
            Container.Bind<Input.PlayerInput>().AsSingle();
            Container.Bind<InputFacade>().AsSingle();
        }
    } 
}