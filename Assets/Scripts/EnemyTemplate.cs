using UnityEngine;

[CreateAssetMenu]
public class EnemyTemplate : ScriptableObject
{
    //Inputs for enemy info
    [Header("Enemy Info")]
    public string enemyName;
    public EnemyTypes enemyType;

    //Inputs for enemies stats
    [Header("Enemy Stats")]
    [Tooltip("Max HP of the enemy")]
    public float maxHP;
    [Tooltip("Minimum(X) and Maximum(Y) base damage for the enemy")]
    public Vector2 attackDamage;
    [Tooltip("Percent chance to hit target")]
    public float attackAccuracy;
    [Tooltip("Percent chance to crit on hit")]
    public float critChance;
    [Tooltip("Currecny gain")]
    public float money;
    [Tooltip("Experience gain")]
    public int exp;
}
