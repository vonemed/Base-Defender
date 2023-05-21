using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    public float wait = 0;

    private GameConfig _gameConfig;

    [Inject]
    public void Constructor(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }
    
    void Update()
    {
        if (EnemyPooler.Instance.aliveEnemies.Count < _gameConfig.gameSettings.maxEnemySpawn)
        {
            if (GameStates.state == GameStates.State.GameStart && wait <= 0)
            {
                EnemyPooler.Instance.SpawnEnemies(1);
                wait = _gameConfig.gameSettings.spawnTimer;
            }
            else
            {
                wait -= Time.deltaTime;
            }
        }
    }
}
