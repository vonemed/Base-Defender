using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;


namespace Bullet
{
    public class BulletController : MonoBehaviour
    {
        private BulletConfig _bulletConfig;
        
        private EnemyController _target;
        private int _damage;

        [Inject]
        public void Constructor(BulletConfig bulletConfig)
        {
            _bulletConfig = bulletConfig;
        }

        public void LaunchBullet(EnemyController target, int damage)
        {
            _target = target;
            _damage = damage;
        }

        private void Update()
        {
            if (_target != null)
            {
                if (!_target.gameObject.activeSelf)
                {
                    KillBullet();
                    return;
                }
                
                var direction = _target.gameObject.transform.position - transform.position;
                transform.Translate(direction.normalized * (_bulletConfig.speed * Time.deltaTime), Space.World);
                transform.LookAt(_target.transform, Vector3.up);
                // transform.rotation = Quaternion.LookRotation (direction);

                if (Vector3.Distance(transform.position, _target.gameObject.transform.position) <= 0.6f)
                {
                    Hit();
                }
            }
        }

        public void KillBullet()
        {
            _target = null;
            gameObject.SetActive(false);
        }

        public void Hit()
        {
            Debug.Log("Hit");
            _target.TakeDamage(_damage);
            KillBullet();
        }
    }
}