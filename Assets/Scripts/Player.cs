using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject stateManager;
    [SerializeField] GameObject playerUI;

    public Item weaponSlot, armorSlot, ringSlot;

    float health; //Players current health
    float maxHealth = 100;
    float damage = 0;
    float damageReduction = 0;
    float regeneration = 0;
    float critChance = 0;
    float critMultiplier = 0;

    float flatHP, percentHp;
    float flatDamage, percentDamage;
    float flatDamageRed, percentDamageRed;
    float critChanceBonus, critMultiplierBonus;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] float damageAppliedMult = 1f;

    [SerializeField] GameObject currentTarget; //The current enemy targetted by player

    public List<GameObject> targets = new List<GameObject>(); //Handles all enemies

    void Start()
    {
        //Sets the values to the healthbar using the players health stats
        health = maxHealth;
        healthBar.minValue = 0;
        UpdateHealthBar();
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    void Update()
    {
        //Keeps players health updated
            healthBar.value = health;
            healthText.text = $" HP: {health}";
    }

    void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
    }

    //Function that uses the current skill in one of the players 4 skill slots
    public void UseSkill(GameObject action)
    {
        //Refrence the current skill in the skill slot
            AttackAction skill = action.GetComponent<AttackAction>();

            if (skill != null)
            {
                int hitCount = skill.hitCount;

                for (int i = 0; i < hitCount; i++)
                {
                    //Sets the variables needed for damage and buffs from the current skill
                    float damageTemp = Mathf.Round(Random.Range(skill.damage.x + damage / 2, skill.damage.y + damage));
                    float accuracyTemp = skill.accuracy, critChanceTemp = skill.critChance, recoilDamage = skill.recoilDamage;
                    float multiplier = 1f;
                    if (Random.Range(0, 1) <= critChanceTemp + critChance) //Checks if the player's attack crits and adjusts the modifier if so
                    {
                        multiplier = 1.25f + critMultiplier;
                    }
                    float totalDamage = Mathf.Round((damageTemp * multiplier) * 10f) / 10f; //Calculates the players total damage rounded to the nearest tenth

                    if (Random.Range(0, 1) <= accuracyTemp) //Checks if the player hits the enemy according to the current skills accuracy
                    {
                        if (recoilDamage > 0) { health -= recoilDamage; } //Deals self damage to the player if the skill has recoil
                        currentTarget.SendMessage("ApplyDamage", totalDamage); //Calls the "Apply Damage" function on the current enemy target and deals the total damage of the skill
                    }
                    else
                    {
                        Debug.Log("Attack Missed!");
                    }
                }
            }
            else
            {
                Debug.Log("No Skill Equipped");
            }
    }

    //Reduces damage of next attack by 50%
    public void Defend()
    {
        damageAppliedMult = 0.5f;
    }

    //Applies damage to the player
    void ApplyDamage(float totalDamage)
    {
        health -= totalDamage * damageAppliedMult * (1 - damageReduction);
        health = Mathf.Round(health * 10) / 10;
        Debug.Log($"Player recieved {totalDamage * damageAppliedMult} damage!");

        damageAppliedMult = 1; //Resets damage applied multiplier

        if (health < 0)
        {
            for (int i = 0; i < targets.Count; i++) 
            {
                targets[i].SendMessage("ApplyDammage", 1000000f);
            }
            ResetPlayer();
            playerUI.SetActive(false);
            transform.position = new Vector3(4, 100, 0);
            stateManager.SendMessage("GameOver");
        }
    }

    //Updates the players target list
    void UpdateEnemies(List<GameObject> aliveEnemies)
    {
        targets = aliveEnemies;
        currentTarget = targets[0];
    }

    public void RegenHealth()
    {
        Heal(regeneration);
    }

    void ResetHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        ResetHealth();
    }

    public void ResetPlayer()
    {
        weaponSlot = null;
        armorSlot = null;
        ringSlot = null;
        UpdateBonuses();
        UpdateStats();
    }

    public void EquipArmor(Item item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            weaponSlot = item;
            UpdateBonuses();
            UpdateStats();
        }
        else if (item.itemType == ItemType.Armor)
        {
            armorSlot = item;
            UpdateBonuses();
            UpdateStats();
        }
        else if (item.itemType == ItemType.Ring)
        {
            ringSlot = item;
            UpdateBonuses();
            UpdateStats();
        }
    }

    void UpdateBonuses()
    {
        if (weaponSlot != null)
        {
            flatHP = weaponSlot.flatHP;
            percentHp = weaponSlot.percentHP;
            flatDamage = weaponSlot.flatDamage;
            percentDamage = weaponSlot.percentDamage;
            flatDamageRed = weaponSlot.flatDamageRed;
            percentDamageRed = weaponSlot.percentDamageRed;
            critChanceBonus = weaponSlot.critChance;
            critMultiplierBonus = weaponSlot.critMultiplier;
            regeneration = weaponSlot.regen;
        }
        else
        {
            flatHP = 0;
            percentHp = 0;
            flatDamage = 0;
            percentDamage = 0;
            flatDamageRed = 0;
            percentDamageRed = 0;
            critChanceBonus = 0;
            critMultiplierBonus = 0;
            regeneration = 0;
        }

        if (armorSlot != null)
        {
            flatHP += armorSlot.flatHP;
            percentHp += armorSlot.percentHP;
            flatDamage += armorSlot.flatDamage;
            percentDamage += armorSlot.percentDamage;
            flatDamageRed += armorSlot.flatDamageRed;
            percentDamageRed += armorSlot.percentDamageRed;
            critChanceBonus += armorSlot.critChance;
            critMultiplierBonus += armorSlot.critMultiplier;
            regeneration += armorSlot.regen;
        }
        else
        {
            flatHP += 0;
            percentHp += 0;
            flatDamage += 0;
            percentDamage += 0;
            flatDamageRed += 0;
            percentDamageRed += 0;
            critChanceBonus += 0;
            critMultiplierBonus += 0;
            regeneration += 0;
        }

        if (ringSlot != null)
        {
            flatHP += ringSlot.flatHP;
            percentHp += ringSlot.percentHP;
            flatDamage += ringSlot.flatDamage;
            percentDamage += ringSlot.percentDamage;
            flatDamageRed += ringSlot.flatDamageRed;
            percentDamageRed += ringSlot.percentDamageRed;
            critChanceBonus += ringSlot.critChance;
            critMultiplierBonus += ringSlot.critMultiplier;
            regeneration = ringSlot.regen;
        }
        else
        {
            flatHP += 0;
            percentHp += 0;
            flatDamage += 0;
            percentDamage += 0;
            flatDamageRed += 0;
            percentDamageRed += 0;
            critChanceBonus += 0;
            critMultiplierBonus += 0;
            regeneration += 0;
        }
    }

    void UpdateStats()
    {
        maxHealth = (100 + flatHP) * (1 + percentHp);
        damage = (0 + flatDamage) * (1 + percentDamage);
        damageReduction = (0 + flatDamageRed) * (1 + percentDamageRed);
        regeneration = 0 + regeneration;
        critChance = 0 + critChanceBonus;
        critMultiplier = 0 + critMultiplierBonus;
        damageAppliedMult = 1f;
        ResetHealth();
    }
}
