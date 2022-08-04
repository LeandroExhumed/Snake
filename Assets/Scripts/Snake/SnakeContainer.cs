using LeandroExhumed.SnakeGame.AI;
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

        [SerializeField]
        private BodyPartFacade bodyPartPrefab;

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
                Container.Bind<IMovementRequester>().To<SimulatedInput>().AsSingle();
                Container.Bind<PathFinding>().AsSingle();
            }

            Container.BindFactory<IBodyPartModel, IBodyPartModel.Factory>().FromComponentInNewPrefab(bodyPartPrefab)
                .AsSingle();
        }

        private void ResolveMVC ()
        {
            Container.Bind<ISnakeModel>().To<SnakeModel>().AsSingle();
            Container.Bind<SnakeController>().AsSingle();
            Container.BindInstance(GetComponent<SnakeView>()).AsSingle();
        }
    }
}