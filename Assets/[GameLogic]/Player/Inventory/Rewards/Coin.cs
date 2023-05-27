using Zenject;
using System.Collections.Generic;
using Player;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour, IReward
{
    public ResourceConfig _resourceConfig;

    private bool pickedUp = false;
    public void Constructor(ResourceConfig resourceConfig)
    {
        _resourceConfig = resourceConfig;
    }

    private void Update()
    {
        if (gameObject.activeSelf && !pickedUp)
        {
            if (Vector3.Distance(transform.position, PlayerController.Player.transform.position) < 2f)
            {
                pickedUp = true;
                PickUp();
            }
        }
    }

    public void PickUp()
    {
        transform.DOMove(new Vector3(PlayerController.Player.transform.position.x, 0, PlayerController.Player.transform.position.z), 0.3f).OnComplete(() =>
        {
            pickedUp = false;
            PlayerInventory.Instance.AddPlayerMoney(_resourceConfig.coinWorth);
            gameObject.SetActive(false);
        });
    }
}
