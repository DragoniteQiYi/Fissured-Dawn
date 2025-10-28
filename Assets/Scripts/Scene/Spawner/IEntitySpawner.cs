//using System;
//using System.Collections.Generic;
//using UnityEngine;

///*
// * 2025/10/24 我在思考：真的需要这么多方法么
// */
//namespace FissuredDawn.Scene.Spawner
//{
//    // 核心生成接口
//    public interface IEntitySpawner<TEntity> where TEntity : MonoBehaviour
//    {
//        event Action<TEntity> OnEntitySpawned;
//        TEntity Spawn(int prefabIndex, Vector3 position);
//        TEntity Spawn(string prefabName, Vector3 position);
//        bool TrySpawn(int prefabIndex, Vector3 position, out TEntity entity);
//    }

//    // 实体管理接口，扩展了生成功能
//    public interface IEntityManager<TEntity> : IEntitySpawner<TEntity> where TEntity : MonoBehaviour
//    {
//        IReadOnlyDictionary<int, TEntity> SpawnedEntities { get; }
//        event Action<TEntity> OnEntityDestroyed;
//        event Action OnAllEntitiesDestroyed;

//        int EntityCount { get; }

//        bool Contains(int instanceId);
//        TEntity GetEntity(int instanceId);

//        bool TryDestroy(int instanceId);
//        bool Destroy(TEntity entity);
//        void DestroyMultiple(IEnumerable<TEntity> entities);
//        void DestroyMultiple(IEnumerable<int> instanceIds);
//        void DestroyAll();
//    }

//    // 预制件管理接口
//    public interface IPrefabProvider<TEntity> where TEntity : MonoBehaviour
//    {
//        IReadOnlyList<TEntity> AvailablePrefabs { get; }
//        bool AddPrefab(TEntity prefab);
//        bool RemovePrefab(TEntity prefab);
//    }

//    // 一个集大成的接口，如果需要的话
//    public interface ICompleteEntitySpawner<TEntity> : IEntityManager<TEntity>, IPrefabProvider<TEntity> where TEntity : MonoBehaviour { }
//}
