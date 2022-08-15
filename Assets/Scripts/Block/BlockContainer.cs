using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Block
{
    public class BlockContainer : MonoInstaller
    {
        [SerializeField]
        private BlockData data;

        public override void InstallBindings ()
        {
            ResolveMVC();
            Container.BindInstance(data).AsSingle();
        }

        protected virtual void ResolveMVC ()
        {
            Container.Bind<IController>().To<BlockController>().AsSingle();
            Container.BindInstance(GetComponent<BlockView>()).AsSingle();
        }
    }
}