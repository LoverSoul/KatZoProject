using System;
using System.Collections.Generic;
using UniRx.Operators;
using UnityEngine;

public class PoolController : MonoBehaviour, IController
{
    public List<InitialPool> InitialPools;

    private Dictionary<Type, Pool> _pools;
    private readonly Vector2 _initialPoint = new Vector2(-500f, -500f);

    public void CreatePool(Type type, GameObject prefab, int initialPoolSize)
    {
        if (!_pools.ContainsKey(type))
        {
            GameObject newPoolContainer = new GameObject($"Container [{Utilites.SplitCamelCase(type.ToString())}]");
            newPoolContainer.transform.position = Vector2.zero;
            newPoolContainer.transform.SetParent(transform);

            _pools.Add(type, new Pool(prefab, newPoolContainer.transform));

            for (int i = 0; i < initialPoolSize; i++)
            {
                AddPoolObject(type);
            }
        }
        else
        {
            Debug.LogWarning($"Pool for type {type} already exists.");
        }
    }

    public void CreatePool<T>(GameObject prefab, int initialPoolSize) where T : class, IPoolable
    {
        CreatePool(typeof(T), prefab, initialPoolSize);
    }

    public T GetPoolObject<T>() where T : class, IPoolable
    {
        var type = typeof(T);
        var unusedPoolObject = _pools[type].ObjectsPool.Find(x => !x.IsActive);

        if (unusedPoolObject != null)
        {
            return unusedPoolObject as T;
        }
        else
        {
            return AddPoolObject(type) as T;
        }
    }

    public T GetActivePoolObject<T>() where T : class, IPoolable
    {
        var type = typeof(T);
        var poolObject = _pools[type].ObjectsPool.Find(x => x.IsActive);

        if (poolObject != null)
        {
            return poolObject as T;
        }
        else
        {
            return AddPoolObject(type) as T;
        }
    }

    public List<IPoolable> GetPool<T>()
    {
        var type = typeof(T);
        return _pools[type].ObjectsPool;
    }

    public List<IPoolable> GetPool(Type type)
    {
        return _pools[type].ObjectsPool;
    }

    public void ApplyAction<T>(Action<T> action) where T : class, IPoolable
    {
        //var type = typeof(T);

        //foreach (var obj in _pools[type].ObjectsPool)
        //{
        //    action.Invoke(obj);
        //}
    }

    private IPoolable AddPoolObject(Type type)
    {
        if (_pools.ContainsKey(type))
        {
            var newPoolObject = Instantiate(_pools[type].Prefab, _initialPoint, Quaternion.identity,
                _pools[type].PoolContainer);

            _pools[type].ObjectsPool.Add(newPoolObject.GetComponent<IPoolable>());
            return newPoolObject.GetComponent<IPoolable>();
        }
        else
        {
            Debug.LogError($"Pool Controller doesn't have pool of type {type}");
            return null;
        }
    }

    public Type ControllerType => typeof(PoolController);

    public void Initialize()
    {
        _pools = new Dictionary<Type, Pool>();

        foreach (var initialPool in InitialPools)
        {
            CreatePool(initialPool.Prefab.GetComponent<IPoolable>().GetType(), initialPool.Prefab,
                initialPool.InitialPoolSize);
        }
    }

    public void OnLevelLoad()
    {

    }

    public void EnableController()
    {

    }

    public void DisableController()
    {

    }

    public class Pool
    {
        public GameObject Prefab;
        public Transform PoolContainer;
        public List<IPoolable> ObjectsPool;

        public Pool(GameObject prefab, Transform poolContainer)
        {
            Prefab = prefab;
            PoolContainer = poolContainer;
            ObjectsPool = new List<IPoolable>();
        }
    }

    [Serializable]
    public class InitialPool
    {
        public GameObject Prefab;
        public int InitialPoolSize;
    }
}
