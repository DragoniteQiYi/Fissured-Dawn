using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace FissuredDawn.Scene.Spawner
{
    public interface IEntityManager<TEntity> where TEntity : MonoBehaviour
    {
        //[Inject] protected readonly IObjectResolver Container;

        //protected readonly Dictionary<int, TEntity> EntityMap = new Dictionary<int, TEntity>();
        IReadOnlyDictionary<int, TEntity> SpawnedEntities { get; }
        int EntityCount { get; }

        event Action<TEntity> OnEntitySpawned;
        event Action<TEntity> OnEntityDestroyed;
        event Action OnAllEntitiesDestroyed;

        void Initialize();

        // IEntitySpawner 接口实现
        TEntity Spawn(int prefabIndex, Vector3 position);
        TEntity Spawn(string prefabName, Vector3 position);
        bool TrySpawn(int prefabIndex, Vector3 position, out TEntity entity);
        List<MapCharacter> SpawnAll(Vector3 position);

        // IEntityManager 接口实现
        bool Contains(int instanceId);

        TEntity GetEntity(int instanceId);

        bool TryDestroy(int instanceId);

        bool Destroy(TEntity entity);

        void DestroyMultiple(IEnumerable<TEntity> entities);

        void DestroyMultiple(IEnumerable<int> instanceIds);

        void DestroyAll();
    }
}
