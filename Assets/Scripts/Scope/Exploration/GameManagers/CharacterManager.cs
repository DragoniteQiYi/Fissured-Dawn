using FissuredDawn.Data.Exploration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FissuredDawn.Scope.Exploration.GameManagers
{
    public class CharacterManager : MonoBehaviour, IEntityManager<MapCharacter>
    {
        [Inject] protected readonly IObjectResolver Container;

        protected readonly Dictionary<int, MapCharacter> EntityMap = new Dictionary<int, MapCharacter>();

        public IReadOnlyDictionary<int, MapCharacter> SpawnedEntities => EntityMap;

        public int EntityCount => EntityMap.Count;

        [SerializeField] private GameObject[] _characters;

        private void Start()
        {
            SpawnAll(Vector3.zero);
        }

        /// <summary>
        /// 初始化读取游戏状态
        /// </summary>
        public void Initialize()
        {
            Debug.Log("[CharacterManager]：角色管理器已初始化");
        }

        public bool Contains(int instanceId) => EntityMap.ContainsKey(instanceId);

        public MapCharacter GetEntity(int instanceId) => EntityMap.TryGetValue(instanceId, out var entity) ? entity : null;

        public bool TryDestroy(int instanceId)
        {
            if (EntityMap.TryGetValue(instanceId, out var entity))
            {
                Destroy(entity.GetComponent<MapCharacter>());
                return true;
            }
            return false;
        }

        public bool Destroy(MapCharacter entity) => TryDestroy(entity.GetInstanceID());

        public void DestroyMultiple(IEnumerable<MapCharacter> entities)
        {
            foreach (var entity in entities.ToList())
            {
                Destroy(entity);
            }
        }

        public void DestroyMultiple(IEnumerable<int> instanceIds)
        {
            foreach (var id in instanceIds.ToList())
            {
                TryDestroy(id);
            }
        }

        public void DestroyAll()
        {
            foreach (var entity in EntityMap.Values.ToList())
            {
                Destroy(entity.GetComponent<MapCharacter>());
            }
            EntityMap.Clear();
        }

        public MapCharacter Spawn(int prefabIndex, Vector3 position)
        {
            throw new NotImplementedException();
        }

        public MapCharacter Spawn(string prefabName, Vector3 position)
        {
            throw new NotImplementedException();
        }

        public bool TrySpawn(int prefabIndex, Vector3 position, out MapCharacter entity)
        {
            throw new NotImplementedException();
        }

        public List<MapCharacter> SpawnAll(Vector3 position)
        {
            var characterList = new List<MapCharacter>();
            foreach (var character in _characters)
            {
                characterList.Add(Container
                    .Instantiate(character, position, Quaternion.identity, gameObject.transform)
                    .GetComponent<MapCharacter>()
                    );
            }
            return characterList;
        }
    }
}
