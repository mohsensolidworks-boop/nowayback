using System.Collections.Generic;
using Main.Context.Core.Logger;
using UnityEngine;

namespace Main.Infrastructure.General
{
    public class GameObjectPoolStack
    {
        private ManualBehaviour _gameObject;
        private readonly Transform _pool;
        private readonly Stack<IPoolable> _stack;
        private readonly HashSet<int> _set;

        public GameObjectPoolStack(ManualBehaviour go, int size, Transform pool)
        {
            _gameObject = go;
            _pool = pool;
            _stack = new Stack<IPoolable>(size);
            _set = new HashSet<int>();
        }

        public void Push(IPoolable poolable)
        {
            var hc = poolable.GetHashCode();

            if (_set.Contains(hc))
            {
                Log.Error(this, LogTag.Gameplay, $"Same item to push: {poolable.GetType().Name}");
                return;
            }

            _set.Add(hc);
            _stack.Push(poolable);
        }

        public IPoolable Pop()
        {
            if (_stack.Count == 0)
            {
                Log.Debug(this, LogTag.Gameplay, $"Requested item is not in the pool: {_gameObject.GetType().Name}");
                return (IPoolable)Object.Instantiate(_gameObject, _pool);
            }

            var item = _stack.Pop();
            _set.Remove(item.GetHashCode());
            return item;
        }

        public int Count()
        {
            return _stack.Count;
        }

        public void Clear()
        {
            _gameObject = null;
            _set.Clear();
            
            using (IEnumerator<IPoolable> enumerator = _stack.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    DestroyItem(enumerator.Current);
                }
            }
            
            _stack.Clear();
        }
        
        public void DecreaseCountTo(int amount)
        {
            var diff = _stack.Count - amount;
         
            if (diff <= 0)
            {
                Log.Debug(this, LogTag.Gameplay, $"Requested decrease amount is higher than the stack count of {_gameObject.GetType().Name}");
                return;
            }

            for (int i = 0; i < diff; i++)
            {
                var item = _stack.Pop();
                _set.Remove(item.GetHashCode());
                DestroyItem(item);
            }
        }

        private static void DestroyItem(IPoolable item)
        {
            var monoScript = (ManualBehaviour)item;
            if (monoScript != null && monoScript.gameObject != null)
            {
                Object.Destroy(monoScript.gameObject);
            }
        }
    }
}
