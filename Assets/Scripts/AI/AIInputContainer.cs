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
            Container.Bind<PathFinding>().AsSingle();
        }

        private void ResolveMVC ()
        {
            Container.Bind<IAIInputModel>().To<AIInputModel>().AsSingle();
            Container.Bind<AIInputController>().AsSingle();
            Container.BindInstance(GetComponent<AIInputView>()).AsSingle();
        }
    }
}