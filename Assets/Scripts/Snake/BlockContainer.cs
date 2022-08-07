using UnityEngine;
using Zenject;

namespace LeandroExhumed.SnakeGame.Snake
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
            Container.Bind<BlockController>().AsSingle();
            Container.BindInstance(GetComponent<BlockView>()).AsSingle();
        }
    }
}