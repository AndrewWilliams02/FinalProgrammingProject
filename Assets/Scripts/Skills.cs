using UnityEngine;

[CreateAssetMenu]
public class Skills : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;
    public Rarity rarity;
    public string skillDescription;

    [Header("Attack Stats")]
    [Tooltip("Minimum(X) and Maximum(Y) base damage for the skill")]
    public Vector2 damage;
    [Tooltip("Number of times attack hits")]
    public int hitCount;
    [Tooltip("Percent chance to hit target")]
    public float accuracy;
    [Tooltip("Percent chance to crit on hit")]
    public float critChance;
    [Tooltip("Damage dealt to user on hit")]
    public float recoilDamage;

    [Header("Buff Stats")]
    [Tooltip("Amount of turns the buff lasts")]
    public int buffDuration;
    [Tooltip("Multiplier to increase damage dealt")]
    public float damageBuff;
    [Tooltip("Multiplier to reduce damage taken")]
    public float damageReductionBuff;
}
