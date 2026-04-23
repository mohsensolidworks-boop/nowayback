using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Main.Infrastructure.General
{
    public class GameObjectPool
    {
        private readonly GameObjectPoolStack[] _poolArray;
        private Transform _pool;

        public GameObjectPool(Type poolIdType)
        {
            _poolArray = new GameObjectPoolStack[Enum.GetNames(poolIdType).Length];
            _pool = new GameObject
            {
                name = poolIdType.Name
            }.transform;

            Object.DontDestroyOnLoad(_pool);
        }

        public void CreatePool<T>(T go, int amount) where T : ManualBehaviour, IPoolable
        {
            var lst = _poolArray[go.GetPoolId()];
            if (lst == null)
            {
                lst = new GameObjectPoolStack(go, amount, _pool);
                _poolArray[go.GetPoolId()] = lst;
            }

            var currentAmount = lst.Count();
            for (var i = currentAmount; i < amount; i++)
            {
                var obj = Object.Instantiate(go, _pool);
                obj.gameObject.SetActive(false);
                lst.Push(obj);
            }
        }

        public T Spawn<T>(int poolId, bool activate) where T : ManualBehaviour, IPoolable
        {
            var lst = _poolArray[poolId];
            if (lst == null)
            {
                return null;
            }
            
            var obj = lst.Pop();
            var castedObject = (T) obj;
            if (activate)
            {
                castedObject.gameObject.SetActive(true);
            }

            castedObject.OnSpawn();
            return castedObject;
        }

        public void Recycle<T>(T go) where T : ManualBehaviour, IPoolable
        {
            var lst = _poolArray[go.GetPoolId()];
            if (lst == null)
            {
                return;
            }

            go.gameObject.SetActive(false);
            go.transform.SetParent(_pool);
            go.OnRecycle();
            lst.Push(go);
        }

        public void DecreasePool(int poolId, int count)
        {
            _poolArray[poolId]?.DecreaseCountTo(count);
        }

        public void ClearPool(int poolId)
        {
            _poolArray[poolId]?.Clear();
            _poolArray[poolId] = null;
        }
   
        public void ClearAllPools()
        {
            for (var i = 0; i < _poolArray.Length; i++)
            {
                ClearPool(i);
            }
        }

        public bool IsPoolAlive()
        {
            return _pool != null;
        }

        public bool HasPool(int poolId)
        {
            return _poolArray[poolId] != null;
        }
        
        public void Destroy()
        {
            Object.Destroy(_pool.gameObject);
            _pool = null;
        }

        public int GetPoolLength(int poolId)
        {
            return _poolArray[poolId] != null ? _poolArray[poolId].Count() : 0;
        }
    }
}
