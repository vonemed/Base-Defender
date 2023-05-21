using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        public static PlayerController Player;
        //TODO: move it to config
        //temp
        public float reload;
        
        private FixedJoystick _joystick;
        public PlayerConfig _playerConfig;
        private PlayerMovement _movement;
        
        //Visuals
        public UnityEngine.UI.Image healthBar;

        public int health;
        //TODO: Add separate respawn class to handle  this later
        public Vector3 respawnPos;
        //Player stats
        //Player combat
        
        [Inject]
        public void Constructor(FixedJoystick fixedJoystick, PlayerMovement movement, PlayerConfig playerConfig)
        {
            Player = this;
            
            _joystick = fixedJoystick;
            _playerConfig = playerConfig;
            _movement = movement;

            reload = _playerConfig.rateOfFire;
            health = _playerConfig.health;
            healthBar.fillAmount = health / 100f;
        }

        private void Update()
        {
            transform.Translate(new Vector3(-(_joystick.Direction.x * (_playerConfig.speed * Time.deltaTime)), 0, -(_joystick.Direction.y * (_playerConfig.speed * Time.deltaTime))));
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
            
            if(health <= 0) Respawn();
        }
        
        //TODO: Add separate respawn class to handle  this later
        public void Respawn()
        {
            health = _playerConfig.health;
            transform.position = respawnPos;
        }
    }
}