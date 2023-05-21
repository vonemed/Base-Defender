using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObjectInstaller<GameConfig>
{

    public GenericSettings gameSettings;
    
    [Serializable]
    public class GenericSettings
    {
        public float spawnTimer = 5f;
        public int maxEnemySpawn = 100;
    }
    
    public override void InstallBindings()
    {
        Container.BindInstance(gameSettings.spawnTimer);
        Container.BindInstance(gameSettings.maxEnemySpawn);
    }
}
