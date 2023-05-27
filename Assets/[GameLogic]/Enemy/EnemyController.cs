using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public enum EnemyStates
    {
        Idle,
        Wandering,
        FollowTarget,
        Attack
    }
    
    //Visuals
    [SerializeField] private UnityEngine.UI.Image healthBar;
    [SerializeField] private Animator animator;

    public EnemyConfig _enemyConfig;

    [SerializeField] private EnemyStates enemyState;

    public int health;
    public float reload;

    private bool moving = false;

    private NavMeshAgent agent;

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
        agent = GetComponent<NavMeshAgent>();
        reload = 0;
    }

    private void Update()
    {
        CheckState();
        StateExecute();
        healthBar.fillAmount = (float)health / 100;;
    }

    #region PseudoAi
    
    public void CheckState()
    {
        if (Vector3.Distance(transform.position, PlayerController.Player.transform.position) < _enemyConfig.attackRange)
        {
            agent.isStopped = true;
            agent.ResetPath();
            enemyState = EnemyStates.Attack;
        }
        else  if (Vector3.Distance(transform.position, PlayerController.Player.transform.position) > _enemyConfig.attackRange &&
                  Vector3.Distance(transform.position, PlayerController.Player.transform.position) < _enemyConfig.detectionRange)
        {
            agent.isStopped = true;
            agent.ResetPath();
            enemyState = EnemyStates.FollowTarget;
        }
        else if(Vector3.Distance(transform.position, PlayerController.Player.transform.position) > _enemyConfig.detectionRange
                && idleTimer > 0)
        {
            enemyState = EnemyStates.Idle;
        }
        else if (idleTimer <= 0 && enemyState == EnemyStates.Idle)
        {
            var randX = Random.Range(
                -EnemySpawner.Instance.GetSpawnPoint().x + _enemyConfig.wanderOffset,
                EnemySpawner.Instance.GetSpawnPoint().x + _enemyConfig.wanderOffset);

            var randZ = Random.Range(
                -EnemySpawner.Instance.GetSpawnPoint().z + _enemyConfig.wanderOffset,
                EnemySpawner.Instance.GetSpawnPoint().z + _enemyConfig.wanderOffset);

            randWanderingPos = new Vector3(randX, 0, randZ);
            wanderingTimer = UnityEngine.Random.Range(_enemyConfig.wanderingMinTime, _enemyConfig.wanderingMaxTime);
            enemyState = EnemyStates.Wandering;
        }
        else if (wanderingTimer <= 0 && enemyState == EnemyStates.Wandering)
        {
            idleTimer = UnityEngine.Random.Range(_enemyConfig.idleMinTime, _enemyConfig.idleMaxTime);
            enemyState = EnemyStates.Idle;
        }

        if (reload > 0)
        {
            reload -= Time.deltaTime;
        }
    }

    public void StateExecute()
    {
        switch (enemyState)
        {
            case EnemyStates.Idle:
                idleTimer -= Time.deltaTime;
                agent.isStopped = true;
                moving = false; //trash
                animator.Play("Dynamic Idle");
                break;

            case EnemyStates.Wandering:
                agent.isStopped = false;
                wanderingTimer -= Time.deltaTime;
                // var randDir = randWanderingPos - transform.position;
                // transform.Translate(randDir * (_enemyConfig.speed * Time.deltaTime));
                Vector3 point;

                if (RandomPoint(transform.position, _enemyConfig.wanderOffset, out point) && agent.remainingDistance <= agent.stoppingDistance)
                {
                    animator.Play("Walking");
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    moving = true; //trash
                    agent.SetDestination(point);
                }
                
                break;

            case EnemyStates.FollowTarget:
                // agent.isStopped = true;
                moving = false; //trash
                // var direction = PlayerController.Player.transform.position - transform.position;
                animator.Play("Running");
                
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.SetDestination(PlayerController.Player.transform.position);
                }

                // } else //Reset
                // {
                //     enemyState = EnemyStates.Idle;
                // }
                
                break;
            
            case EnemyStates.Attack:

                if (reload <= 0)
                {
                    animator.Play("Dynamic Idle");
                    animator.Play("Throw");
                    PlayerController.Player.TakeDamage(_enemyConfig.damage);
                    reload = _enemyConfig.attackSpeed;
                }

                break;
        }
    }
    #endregion
    public void TakeDamage(int amount)
    {
        health -= amount;
        healthBar.fillAmount = (float)health / 100;

        if (health <= 0) Die();
    }

    public void Die()
    {
        agent.isStopped = true;
        agent.ResetPath();
        enemyState = EnemyStates.Idle;
        
        RewardPooler.Instance.SpawnReward(new Vector3(transform.position.x, 0, transform.position.z));

        EnemyPooler.Instance.aliveEnemies.Remove(gameObject);
        health = _enemyConfig.health;
        gameObject.SetActive(false);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    


}