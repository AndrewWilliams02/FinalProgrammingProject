using UnityEngine;

[CreateAssetMenu]
public class Skills : ScriptableObject //Scriptable object for housing data of unique player skills
{
    //Inputs for skill info
    [Header("Skill Info")]
    public string skillName;
    public Rarity rarity;

    //Inputs for skill's attack stats
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
    [Tooltip("Buy cost")]
    public float cost;
}
