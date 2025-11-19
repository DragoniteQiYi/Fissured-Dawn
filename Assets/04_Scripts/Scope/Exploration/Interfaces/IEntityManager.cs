using FissuredDawn.Data.Exploration;
using System.Collections.Generic;
using UnityEngine;

namespace FissuredDawn.Scope.Exploration.Interfaces
{
    public interface IEntityManager<TEntity> where TEntity : MonoBehaviour
    {
        IReadOnlyDictionary<int, TEntity> SpawnedEntities { get; }
        int EntityCount { get; }

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
