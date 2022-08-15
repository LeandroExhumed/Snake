using System.Collections.Generic;
using UnityEngine;

namespace LeandroExhumed.SnakeGame.Services
{
    public class Pool
    {
        public Transform Container
        {
            get
            {
                if (container == null)
                {
                    container = new GameObject("Pools").transform;
                }

                return container;
            }
        }

        private Transform container;

        private readonly Dictionary<Object, Queue<Object>> pools = new Dictionary<Object, Queue<Object>>();

        public void AddPool (Object prefab, int size, Transform parent = null)
        {
            if (pools.ContainsKey(prefab))
            {
                return;
            }

            Queue<Object> queue = new Queue<Object>();

            for (int i = 0; i < size; ++i)
            {
                var o = Object.Instantiate(prefab);
                GameObject gameObject = GetGameObject(o);

                gameObject.transform.SetParent(parent == null ? Container : parent);
                gameObject.gameObject.SetActive(false);

                queue.Enqueue(o);
            }

            pools[prefab] = queue;
        }

        public T GetObject<T> (Object prefab) where T : Object
        {
            Queue<Object> queue;
            if (pools.TryGetValue(prefab, out queue))
            {
                Object objectToReuse = queue.Dequeue();
                queue.Enqueue(objectToReuse);

                GameObject gameObject = GetGameObject(objectToReuse);
                gameObject.SetActive(true);

                return objectToReuse as T;
            }

            UnityEngine.Debug.LogError("No pool was init with this prefab");
            return null;
        }

        private GameObject GetGameObject (Object instance)
        {
            GameObject gameObject;

            if (instance is Component component)
            {
                gameObject = component.gameObject;
            }
            else
            {
                gameObject = instance as GameObject;
            }

            return gameObject;
        }
    }
}