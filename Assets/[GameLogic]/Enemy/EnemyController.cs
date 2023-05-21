using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    //Visuals
    public UnityEngine.UI.Image healthBar;
    
    public EnemyConfig _enemyConfig;

    [SerializeField] private EnemyStates enemyState;
    
    public int health;
    public float reload;
    
    //Timers
    [SerializeField] private float wanderingTimer = 0f;
    [SerializeField] private float idleTimer = 0f;

    [SerializeField] private Vector3 randWanderingPos;

    [Inject]
    public void Constructor(EnemyConfig enemyConfig)
    {
        _enemyConfig = enemyConfig;

        health = _enemyConfig.health;
        healthBar.fillAmount = health;

        enemyState = EnemyStates.Wandering;
        reload = 0;
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

    private void Update()
    {
        CheckState();
        StateExecute();
    }

    #region PseudoAi

    public enum EnemyStates
    {
        Idle,
        Wandering,
        Attack
    }

    public void CheckState()
    {
        if (Vector3.Distance(transform.position, PlayerController.Player.transform.position) < _enemyConfig.detectionRange)
        {
            enemyState = EnemyStates.Attack;
        }
        else if(idleTimer <= 0 && enemyState == EnemyStates.Idle)
        {
            //Temp
            //TODO: Remove magical numbers
            var randX = Random.Range(-10, 10);
            var randZ = Random.Range(-10, 10);

            randWanderingPos = new Vector3(randX, 0, randZ);
            wanderingTimer = UnityEngine.Random.Range(_enemyConfig.wanderingMinTime, _enemyConfig.wanderingMaxTime);
            enemyState = EnemyStates.Wandering;
            
        } else if (wanderingTimer <= 0 && enemyState == EnemyStates.Wandering)
        {
            idleTimer = UnityEngine.Random.Range(_enemyConfig.idleMinTime, _enemyConfig.idleMaxTime);
            enemyState = EnemyStates.Idle;
        }
    }

    public void StateExecute()
    {
        switch (enemyState)
        {
            case EnemyStates.Idle:
                idleTimer -= Time.deltaTime;
                break;
            
            case EnemyStates.Wandering:
                wanderingTimer -= Time.deltaTime;
                var randDir = randWanderingPos - transform.position;
                transform.Translate(randDir * (_enemyConfig.speed * Time.deltaTime));
                break;
            
            case EnemyStates.Attack:
                var direction = PlayerController.Player.transform.position - transform.position;
                transform.Translate(direction * (_enemyConfig.speed * Time.deltaTime));
                
                if (Vector3.Distance(transform.position, PlayerController.Player.transform.position) < _enemyConfig.attackRange)
                {
                    reload -= Time.deltaTime;
                    if (reload <= 0)
                    {
                        PlayerController.Player.TakeDamage(_enemyConfig.damage);
                        reload = _enemyConfig.attackSpeed;
                    }
                }

                break;
        }
    }
    
    #endregion
}
