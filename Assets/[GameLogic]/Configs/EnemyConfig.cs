using UnityEngine;

[CreateAssetMenu(menuName = "Configs/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public int health = 100;
    public float speed = 4f;
    public int damage = 1;
}
