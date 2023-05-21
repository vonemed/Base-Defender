using UnityEngine;

[CreateAssetMenu(menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public int health = 100;
    public float speed = 5f;
    public int damage = 5;
    public float rateOfFire = 1f;
}
