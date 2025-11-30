using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] DataList dataList;

    [SerializeField] GameObject stateManager;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject targetIndicator;

    [SerializeField] GameObject[] skillSlots = new GameObject[4];
    public Skills[] currentSkills = new Skills[4];
    [SerializeField] Item weaponSlot, armorSlot, ringSlot;

    float health; //Players current health
    float maxHealth = 100;
    float damage = 0;
    float damageReduction = 0;
    float regeneration = 0;
    float critChance = 0;
    float critMultiplier = 0;

    int targetIndex = 0;

    float flatHP, percentHp;
    float flatDamage, percentDamage;
    float flatDamageRed, percentDamageRed;
    float critChanceBonus, critMultiplierBonus;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] float damageAppliedMult = 1f;

    [SerializeField] GameObject currentTarget; //The current enemy targetted by player

    public List<GameObject> targets = new List<GameObject>(); //Handles all enemies

    void Awake()
    {
        currentSkills[0] = dataList.allSkills[0];
        UpdateSkills();
    }

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CycleTarget();
        }
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
                float critCheck = Random.Range(0f, 1f);
                float accuracyCheck = Random.Range(0f, 1f);
                if (critCheck <= critChanceTemp + critChance) //Checks if the player's attack crits and adjusts the modifier if so
                {
                    multiplier = 1.25f + critMultiplier;
                }
                float totalDamage = Mathf.Round(damageTemp * multiplier * 10f) / 10f; //Calculates the players total damage rounded to the nearest tenth
                Debug.Log(totalDamage);

                if (accuracyCheck <= accuracyTemp) //Checks if the player hits the enemy according to the current skills accuracy
                {
                    if (recoilDamage != 0) { health -= recoilDamage; } //Deals self damage to the player if the skill has recoil
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
        Heal(new Vector2(0, 10));
    }

    //Applies damage to the player
    void ApplyDamage(float totalDamage)
    {
        health -= totalDamage * damageAppliedMult * (1 - damageReduction);
        health = Mathf.Round(health * 10) / 10;
        //Debug.Log($"Player recieved {totalDamage * damageAppliedMult} damage!");

        damageAppliedMult = 1; //Resets damage applied multiplier

        if (health <= 0)
        {
            for (int i = targets.Count - 1; i >= 0; i--) 
            {
                targets[i].SendMessage("ApplyDamage", 1000000f);
            }
            ResetPlayer();
            playerUI.SetActive(false);
            transform.position = new Vector3(4, 100, 0);
            stateManager.SendMessage("GameOver");
        }
    }

    //Updates the players target list
    public void UpdateEnemies(List<GameObject> aliveEnemies)
    {
        targets = aliveEnemies;
        if (targets.Count != 0)
        {
            currentTarget = targets[0];
            targetIndex = 0;
            UpdateTargetIndicator();
        }
    }

    public void RegenHealth()
    {
        Heal(new Vector2 (regeneration, regeneration));
    }

    public void Rest(float modifier)
    {
        float heal = maxHealth * modifier;
        Heal(new Vector2(heal, heal));
    }

    void ResetHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Heal(Vector2 amount)
    {
        float healAmount = Random.Range(amount.x, amount.y);
        health += healAmount;
        health = Mathf.Round(health * 10) / 10;
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

    public void EquipItem(Item item)
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
        maxHealth = Mathf.Round((100 + flatHP) * (1 + percentHp) * 10) / 10;
        damage = (0 + flatDamage) * (1 + percentDamage);
        damageReduction = (0 + flatDamageRed) * (1 + percentDamageRed);
        regeneration = 0 + regeneration;
        critChance = 0 + critChanceBonus;
        critMultiplier = 0 + critMultiplierBonus;
        damageAppliedMult = 1f;
        ResetHealth();
    }

    void CycleTarget()
    {
        if (targets.Count > 1)
        {
            targetIndex++;
            currentTarget = targets[targetIndex%targets.Count];
            UpdateTargetIndicator();
        }
    }

    void UpdateTargetIndicator()
    {
        Vector3 currentPos = targetIndicator.transform.position;
        currentPos = new Vector3(currentTarget.transform.position.x + 1, currentTarget.transform.position.y, 0);
        targetIndicator.transform.position = currentPos;
    }

    public void UpdateSkills()
    {
        for(int i = 0; i < currentSkills.Length; i++)
        {
            if (currentSkills[i] != null)
            {
                AttackAction skill = skillSlots[i].GetComponent<AttackAction>();
                skill.UpdateSkillInfo(currentSkills[i]);
            }
        }
    }
}
