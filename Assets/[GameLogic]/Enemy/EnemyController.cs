using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour
{
    //Visuals
    public UnityEngine.UI.Image healthBar;
    
    public EnemyConfig _enemyConfig;

    public int health;

    [Inject]
    public void Constructor(EnemyConfig enemyConfig)
    {
        _enemyConfig = enemyConfig;

        health = _enemyConfig.health;
        healthBar.fillAmount = health;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Taken damage");
        health -= amount;
        healthBar.fillAmount = (float)health/100;
        
        if(health <= 0) Die();
    }

    public void Die()
    {
        EnemyPooler.Instance.aliveEnemies.Remove(gameObject);
        health = _enemyConfig.health;
        gameObject.SetActive(false);
    }
}
