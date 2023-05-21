using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyPooler : MonoBehaviour
{
    public static EnemyPooler Instance;

    public EnemyConfig _enemyConfig;
    
    public GameObject enemyPrefab;
    public List<EnemyController> enemies;
    public List<GameObject> aliveEnemies;
    public int amountToPool;
    private GameObject parentObj; 
    
    [Inject]
    public void Constructor(EnemyConfig enemyConfig)
    {
        _enemyConfig = enemyConfig;
    }
    
    private void Awake()
    {
        Instance = this;

        enemies = new List<EnemyController>();
        
        parentObj = new GameObject();
        parentObj.name = "EnemyPool";

        for (int i = 0; i < amountToPool; i++)
        {
            var obj = Instantiate(enemyPrefab, parentObj.transform);
            obj.name = obj.name + "[" + i + "]";
            obj.SetActive(false);
            obj.GetComponent<EnemyController>().Constructor(_enemyConfig);
            enemies.Add(obj.GetComponent<EnemyController>());
        }
        
        GameStates.ChangeState(GameStates.State.GameStart);
    }

    public EnemyController GetEnemy()
    {
        foreach (var enemy in enemies)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                return enemy;
            }
        }

        var obj = Instantiate(enemyPrefab, parentObj.transform);
        obj.name = "Extra";
        obj.SetActive(true);
        enemies.Add(obj.GetComponent<EnemyController>());
        return obj.GetComponent<EnemyController>();
    }
    
    public void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var enemy = GetEnemy();
            var randX = Random.Range(-20, 20);
            var randZ = Random.Range(-20, 20);

            enemy.transform.position = new Vector3(randX, 0, randZ);
            enemy.gameObject.SetActive(true);
            
            aliveEnemies.Add(enemy.gameObject);
        }
    }
}
