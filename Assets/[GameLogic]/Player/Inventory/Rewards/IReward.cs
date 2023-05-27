using Zenject;

public interface IReward
{
    [Inject]
    public void Constructor(ResourceConfig resourceConfig);
    public void PickUp();
}