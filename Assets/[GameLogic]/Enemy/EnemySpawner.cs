using UnityEngine;
using Zenject;

public sealed class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float wait = 0;

    private GameConfig _gameConfig;

    [Inject]
    public void Constructor(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (EnemyPooler.Instance.aliveEnemies.Count < _gameConfig.gameSettings.maxEnemySpawn)
        {
            if (GameStates.state == GameStates.State.GameStart && wait <= 0)
            {
                EnemyPooler.Instance.SpawnEnemies(1, spawnPosition.position);
                wait = _gameConfig.gameSettings.spawnTimer;
            }
            else
            {
                wait -= Time.deltaTime;
            }
        }
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPosition.position;
    }
}
