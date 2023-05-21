using Player;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public FixedJoystick joystick;

    public PlayerMovement playerMovement;
    public override void InstallBindings()
    {
        Container.Bind<FixedJoystick>().FromInstance(joystick).AsSingle();
        Container.Bind<PlayerMovement>().FromInstance(playerMovement).AsSingle();
        // Container.Bind<PlayerController>().AsSingle();
    }
}