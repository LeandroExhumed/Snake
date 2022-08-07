using LeandroExhumed.SnakeGame.Input;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeContainer : MonoInstaller
    {
        [SerializeField]
        private bool isAI = false;

        [SerializeField]
        private SnakeData data;

        public override void InstallBindings ()
        {
            ResolveMVC();
            Container.BindInstance(data).AsSingle();
            if (!isAI)
            {
                Container.Bind<IMovementRequester>().To<InputFacade>().AsSingle();
            }
            else
            {
                Container.BindInstance(GetComponent<MonoBehaviour>()).AsSingle();
                Container.Bind<IMovementRequester>().FromComponentInNewPrefabResource("SimulatedInput").AsSingle();
            }
        }

        private void ResolveMVC ()
        {
            Container.Bind<ISnakeModel>().To<SnakeModel>().AsSingle();
            Container.Bind<SnakeController>().AsSingle();
            Container.BindInstance(GetComponent<SnakeView>()).AsSingle();
        }
    }
}