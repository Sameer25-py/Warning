using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Pooler : MonoBehaviour
    {
        [SerializeField] private List<PoolObject> prefabs;
        private                  List<PoolObject> pool             = new();
        public                   int              InitialPoolCount = 100;
        public                   Vector2          IniitalPosition;
        public                   Vector2          MinMaxXPosition;


        public static Action<PoolObject> AddBackToPool;

        private void OnEnable()
        {
            AddBackToPool += OnAddBackToPoolCalled;
        }

        private void OnDisable()
        {
            AddBackToPool += OnAddBackToPoolCalled;
        }

        private void OnAddBackToPoolCalled(PoolObject obj)
        {
            obj.gameObject.SetActive(false);
            obj.gameObject.transform.position = obj.transform.position = new Vector3(
                Random.Range(MinMaxXPosition.x, MinMaxXPosition.y),
                IniitalPosition.y, 0);
            pool.Add(obj);
        }

        public void GeneratePool()
        {
            for (int i = 0; i < InitialPoolCount; i++)
            {
                var obj = Instantiate(prefabs[Random.Range(0, prefabs.Count)]);
                obj.transform.position = new Vector3(Random.Range(MinMaxXPosition.x, MinMaxXPosition.y),
                    IniitalPosition.y, 0);
                pool.Add(obj);
            }
        }

        public void DestroyPool()
        {
            foreach (PoolObject o in pool.Where(o => o))
            {
                Destroy(o.gameObject);
            }

            pool.Clear();
        }

        public PoolObject GetFromPool()
        {
            if (pool.Count > 0)
            {
                PoolObject obj = pool[0];
                pool.Remove(obj);
                return obj;
            }

            return null;
        }
    }
}