using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private List<EnemyStatus> activeList = new();
    private Queue<EnemyStatus> poolList = new();

    private void Start()
    {
        CreatePool();
        SpawnAll();

        GameEvents.OnReset += ResetEnemies;
    }

    private void OnDestroy()
    {
        GameEvents.OnReset -= ResetEnemies;
    }

    void CreatePool()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);

            EnemyStatus enemy = obj.GetComponent<EnemyStatus>();
            enemy.SetSpawner(this);

            poolList.Enqueue(enemy);
        }
    }

    void SpawnAll()
    {
        for(int i = 0;i < spawnPoints.Length;i++)
        {
            Spawn(i);
        }
    }

    void Spawn(int index)
    {
        if (poolList.Count == 0) return;

        EnemyStatus enemy = poolList.Dequeue();

        enemy.transform.position = spawnPoints[index].position;
        enemy.ResetEnemy();

        enemy.gameObject.SetActive(true);

        activeList.Add(enemy);
    }

    public void ReturnToPool(EnemyStatus enemy)
    {
        activeList.Remove(enemy);

        enemy.gameObject.SetActive(false);
        poolList.Enqueue(enemy);
    }

    public void ResetEnemies()
    {
        foreach (var enemy in activeList)
        {
            enemy.gameObject.SetActive(false);
            poolList.Enqueue(enemy);
        }

        activeList.Clear();
        SpawnAll();
    }
}
