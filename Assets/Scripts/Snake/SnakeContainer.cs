using LeandroExhumed.SnakeGame.AI;
using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeContainer : MonoInstaller
    {
        [SerializeField]
        private SnakeData data;

        public override void InstallBindings ()
        {
            ResolveMVC();
            Container.BindInstance(data).AsSingle();
            Container.BindFactory<IAIInputModel, IAIInputModel.Factory>()
                .FromComponentInNewPrefabResource("SimulatedInput");
        }

        private void ResolveMVC ()
        {
            Container.Bind<ISnakeModel>().To<SnakeModel>().AsSingle();
            Container.Bind<IController>().To<SnakeController>().AsSingle();
            Container.BindInstance(GetComponent<SnakeView>()).AsSingle();
        }
    }
}