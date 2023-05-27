using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;
using Zenject.SpaceFighter;

namespace Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        public static PlayerController Player;

        public PlayerStates playerState;
        public float reload;

        private FixedJoystick _joystick;
        public PlayerConfig _playerConfig;
        [SerializeField] private Animator animator;
        
        //Visuals
        public UnityEngine.UI.Image healthBar;

        public int health;
        //TODO: Add separate respawn class to handle  this later
        public Vector3 respawnPos;
        
        public enum PlayerStates
        {
            Idle,
            Moving,
            Shooting,
            Dying
        }

        [Inject]
        public void Constructor(FixedJoystick fixedJoystick, PlayerConfig playerConfig)
        {
            Player = this;
            
            _joystick = fixedJoystick;
            _playerConfig = playerConfig;

            reload = _playerConfig.rateOfFire;
            health = _playerConfig.health;
            healthBar.fillAmount = health / 100f;

            playerState = PlayerStates.Idle;
        }

        private void Update()
        {
            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            {
               
                animator.Play("Running");
                Vector3 moveVector = new Vector3(-(_joystick.Horizontal * (_playerConfig.speed * Time.deltaTime)), 0, -(_joystick.Vertical * (_playerConfig.speed * Time.deltaTime)));
                // transform.Translate(moveVector);
                transform.position += moveVector;
                Vector3 pos = transform.position;
                Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, 5 * Time.deltaTime, 0.0f);
                transform.localRotation = Quaternion.LookRotation(direction);
                
                pos.z = Mathf.Clamp(pos.z, 0f, 45f);;
                pos.x = Mathf.Clamp(pos.x, -17f, 17f);
                pos.y = -1f;

                transform.position = pos;
            }
            else
            {
                animator.Play("Dynamic Idle");
            }

            
            
            // transform.LookAt(new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y), Vector3.up);
                
            healthBar.fillAmount = health / 100f;
            
            //Temp
            //TODO: Move to player combat?
            foreach (var enemy in EnemyPooler.Instance.enemies)
            {
                if (enemy.gameObject.activeSelf && Vector3.Distance(transform.position, enemy.transform.position) < 5f)
                {
                    //BAD, redo later
                    if (reload <= 0)
                    {
                        playerState = PlayerStates.Shooting;
                        var bullet = BulletPooler.Instance.GetBullet();
                        bullet.transform.position = transform.position;
                        bullet.LaunchBullet(enemy, _playerConfig.damage);

                        reload = _playerConfig.rateOfFire;
                        break;
                    }
                }
            }

            if (reload > 0) reload -= Time.deltaTime;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                playerState = PlayerStates.Dying;
                Respawn();
            }
        }
        
        //TODO: Add separate respawn class to handle  this later
        public void Respawn()
        {
            health = _playerConfig.health;
            transform.position = respawnPos;
        }

        private void StateExecution()
        {
            switch (playerState)
            {
                case PlayerStates.Idle:

                    break;
                
                case PlayerStates.Moving:

                    break;
                
                case PlayerStates.Shooting:

                    break;
                
                case PlayerStates.Dying:

                    break;
            }
        }
    }
}