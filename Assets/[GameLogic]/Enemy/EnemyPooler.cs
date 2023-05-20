using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    public static EnemyPooler Instance;
    
    public GameObject enemyPrefab;
    public List<GameObject> enemies;
    public int amountToPool;
    
    private void Awake()
    {
        Instance = this;

        enemies = new List<GameObject>();
        
        var parentObj = new GameObject();
        parentObj.name = "EnemyPool";

        for (int i = 0; i < amountToPool; i++)
        {
            var obj = Instantiate(enemyPrefab, parentObj.transform);
            obj.name = obj.name + "[" + i + "]";
            obj.SetActive(false);
            enemies.Add(obj);
        }
        
        GameStates.ChangeState(GameStates.State.GameStart);
    }

    public GameObject GetEnemy()
    {
        foreach (var enemy in enemies)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        return null;
    }
    
    public void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var enemy = GetEnemy();
            var randX = Random.Range(-40, 40);
            var randZ = Random.Range(-40, 40);

            enemy.transform.position = new Vector3(randX, 0, randZ);
            enemy.SetActive(true);
        }
    }
}
