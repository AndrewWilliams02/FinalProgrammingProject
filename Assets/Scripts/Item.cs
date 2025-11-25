using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    //Inputs for item info
    [Header("Item Info")]
    public string itemName;
    public Rarity rarity;
    public ItemType itemType;

    //Inputs for item stats
    [Header("Item Stats")]
    [Tooltip("Flat Health Increase")]
    public float flatHP;
    [Tooltip("Percent Health Increase")]
    public float percentHP;
    [Tooltip("Flat Damage Increase")]
    public float flatDamage;
    [Tooltip("Percent Damage Increase")]
    public float percentDamage;
    [Tooltip("Flat Damage Reduction Increase")]
    public float flatDamageRed;
    [Tooltip("Percent Damage Reduction Increase")]
    public float percentDamageRed;
    [Tooltip("Crit Chance Increase")]
    public float critChance;
    [Tooltip("Crit Multiplier Increase")]
    public float critMultiplier;
    [Tooltip("Regeneration Rate")]
    public float regen;
}
