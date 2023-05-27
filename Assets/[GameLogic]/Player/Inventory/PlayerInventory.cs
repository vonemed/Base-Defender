using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public delegate void MoneyHandler(int amount);

public sealed class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    [CanBeNull] private MoneyHandler _moneyHandler;

    private int _money = 0;
    
    private void Awake()
    {
        Instance = this;
        
        if (!PlayerPrefs.HasKey("PlayerMoney"))
        {
            PlayerPrefs.SetInt("PlayerMoney", _money);
        }
        else
        {
            _money = PlayerPrefs.GetInt("PlayerMoney");
        }
    }

    public void RegisterUIHandler(MoneyHandler _handler)
    {
        _moneyHandler += _handler;
    }
    
    public int GetPlayerMoney() => _money;

    public void AddPlayerMoney(int amount)
    {
        Debug.Log("Player money added");
        _money += amount;
        
        PlayerPrefs.SetInt("PlayerMoney", _money);

        _moneyHandler.Invoke(_money);
    }
}
