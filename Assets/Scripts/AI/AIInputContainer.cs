using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.AI
{
    public class AIInputContainer : MonoInstaller
    {
        [SerializeField]
        private AIData data;

        public override void InstallBindings ()
        {
            ResolveMVC();
            Container.BindInstance(GetComponent<MonoBehaviour>()).AsSingle();
            Container.BindInstance(data).AsSingle();
            Container.Bind<IPathFindingModel>().To<PathFindingModel>().AsSingle();
        }

        private void ResolveMVC ()
        {
            Container.Bind<IAIInputModel>().To<AIInputModel>().AsSingle();
            Container.Bind<IController>().To<AIInputController>().AsSingle();
            Container.BindInstance(GetComponent<AIInputView>()).AsSingle();
        }
    }
}