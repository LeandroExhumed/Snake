using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
{
    public class SnakeContainer : MonoInstaller
    {
        [SerializeField]
        private BodyPartFacade bodyPartPrefab;

        public override void InstallBindings ()
        {
            ResolveMVC();

            Container.BindFactory<IBodyPartModel, IBodyPartModel.Factory>().FromComponentInNewPrefab(bodyPartPrefab)
                .AsSingle();
        }

        private void ResolveMVC ()
        {
            Container.Bind<ISnakeModel>().To<SnakeModel>().AsSingle();
            Container.Bind<SnakeController>().AsSingle();
        }
    }
}