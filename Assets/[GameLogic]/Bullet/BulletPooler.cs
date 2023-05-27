using System.Collections.Generic;
using Bullet;
using UnityEngine;
using Zenject;

public sealed class BulletPooler : MonoBehaviour
{
    public static BulletPooler Instance;

    public BulletConfig _bulletConfig;
    
    public GameObject bulletPrefab;
    public List<BulletController> bullets;
    public int amountToPool;
    private GameObject parentObj;
    
    [Inject]
    public void Constructor(BulletConfig bulletConfig)
    {
        _bulletConfig = bulletConfig;
    }
    
    private void Awake()
    {
        Instance = this;

        bullets = new List<BulletController>();
        
        parentObj = new GameObject();
        parentObj.name = "BulletPool";

        for (int i = 0; i < amountToPool; i++)
        {
            var obj = Instantiate(bulletPrefab, parentObj.transform);
            obj.name = obj.name + "[" + i + "]";
            obj.SetActive(false);
            obj.GetComponent<BulletController>().Constructor(_bulletConfig);
            bullets.Add(obj.GetComponent<BulletController>());
        }
        
        GameStates.ChangeState(GameStates.State.GameStart);
    }

    public BulletController GetBullet()
    {
        foreach (var bullet in bullets)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }

        var obj = Instantiate(bulletPrefab, parentObj.transform);
        obj.name = "Extra";
        obj.SetActive(true);
        bullets.Add(obj.GetComponent<BulletController>());
        return obj.GetComponent<BulletController>();
    }
    //
    // public void SpawnEnemies(int amount)
    // {
    //     for (int i = 0; i < amount; i++)
    //     {
    //         var enemy = GetBullet();
    //         var randX = Random.Range(-20, 20);
    //         var randZ = Random.Range(-20, 20);
    //
    //         enemy.transform.position = new Vector3(randX, 0, randZ);
    //         enemy.gameObject.SetActive(true);
    //     }
    // }
}
