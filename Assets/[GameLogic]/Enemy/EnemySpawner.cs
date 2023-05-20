using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float wait = 5f;
    // Start is called before the first frame update
    void Update()
    {
        if (GameStates.state == GameStates.State.GameStart && wait <= 0)
        {
            EnemyPooler.Instance.SpawnEnemies(1);
            wait = 15f;
        }
        else
        {
            wait -= Time.deltaTime;
        }
    }
}
