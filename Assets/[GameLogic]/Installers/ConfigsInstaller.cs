using Zenject;

public class ConfigsInstaller : MonoInstaller
{
    public GameConfig gameConfig;
    public PlayerConfig playerConfig;
    public EnemyConfig enemyConfig;
    public BulletConfig bulletConfig;

    public override void InstallBindings()
    {
        // Container.BindInstances(gameConfig, playerConfig);
        Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle().NonLazy();
        Container.Bind<PlayerConfig>().FromInstance(playerConfig).AsSingle().NonLazy();
        Container.Bind<EnemyConfig>().FromInstance(enemyConfig).AsSingle().NonLazy();
        Container.Bind<BulletConfig>().FromInstance(bulletConfig).AsSingle().NonLazy();
    }
}