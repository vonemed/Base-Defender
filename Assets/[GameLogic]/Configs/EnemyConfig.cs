using UnityEngine;

[CreateAssetMenu(menuName = "Configs/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public int health = 100;
    public float speed = 3f;
    public int damage = 1;
    public float attackSpeed = 2f;
    public float attackRange = 1f;
    public float detectionRange = 5f;

    [Header("AI")] 
    public float wanderingMinTime = 2f;
    public float wanderingMaxTime = 5f;

    public float idleMinTime = 2f;
    public float idleMaxTime = 3f;
}
