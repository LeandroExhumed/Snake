using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.AI
{
    public class SimulatedInputContainer : MonoInstaller
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
            Container.Bind<ISimulatedInput>().To<SimulatedInput>().AsSingle();
            Container.Bind<SimulatedInputController>().AsSingle();
            Container.BindInstance(GetComponent<SimulatedInputView>()).AsSingle();
        }
    }
}