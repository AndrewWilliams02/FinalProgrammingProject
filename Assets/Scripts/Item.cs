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
    [Tooltip("Flat Damage Reduction")]
    public float flatDamageRed;
    [Tooltip("Percent Damage Reduction")]
    public float percentDamageRed;
    [Tooltip("Regeneration Rate")]
    public float regen;
}
