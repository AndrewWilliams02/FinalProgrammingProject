using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    public TextMeshProUGUI[] equipmentText = new TextMeshProUGUI[3];
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI moneyText;
    public float money;

    [SerializeField] DataList dataList;

    [SerializeField] GameObject stateManager;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject targetIndicator;

    [SerializeField] GameObject[] equipmentInfo = new GameObject[3];
    [SerializeField] TextMeshProUGUI[] equipmentInfoText = new TextMeshProUGUI[3];
    bool[] infoVisible = new bool[3];

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
        equipmentText[0].text = "Weapon\nSlot";
        equipmentText[1].text = "Armor\nSlot";
        equipmentText[2].text = "Ring\nSlot";
        for (int i = 0; i < infoVisible.Length; i++)
        {
            infoVisible[i] = false;
        }
    }

    void Start()
    {
        //Sets the values to the healthbar using the players health stats
        health = maxHealth;
        healthBar.minValue = 0;
        UpdateHealthBar();
        healthBar.value = health;
        healthText.text = $" HP: {health}";
        moneyText.text = $"${money}";
        UpdateStatText();
    }

    void Update()
    {
        

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
                //Debug.Log(totalDamage);

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

        //Keeps players health updated
        healthBar.value = health;
        healthText.text = $" HP: {health}";

        damageAppliedMult = 1; //Resets damage applied multiplier

        if (health <= 0)
        {
            for (int i = targets.Count - 1; i >= 0; i--) 
            {
                targets[i].SendMessage("ApplyDamage", 1000000f);
            }
            playerUI.SetActive(false);
            transform.position = new Vector3(-4, 100, 0);
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
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    public void ResetPlayer()
    {
        weaponSlot = null;
        armorSlot = null;
        ringSlot = null;
        UpdateBonuses();
        UpdateStats();

        currentSkills[0] = dataList.allSkills[0];
        currentSkills[1] = null;
        currentSkills[2] = null;
        currentSkills[3] = null;
        UpdateSkills();

        maxHealth = 100;
        health = maxHealth;
        damage = 0;
        damageReduction = 0;
        regeneration = 0;
        critChance = 0;
        critMultiplier = 0;
        money = 0;

        transform.position = new Vector3(-4, 0, 0);
        playerUI.SetActive(true);
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    public void EquipItem(Item item)
    {
        if (item.itemType == ItemType.Weapon)
        {
            weaponSlot = item;
            UpdateBonuses();
            UpdateStats();
            equipmentText[0].text = item.name;
            equipmentText[0].color = Color.green;
        }
        else if (item.itemType == ItemType.Armor)
        {
            armorSlot = item;
            UpdateBonuses();
            UpdateStats();
            equipmentText[1].text = item.name;
            equipmentText[0].color = Color.green;
        }
        else if (item.itemType == ItemType.Ring)
        {
            ringSlot = item;
            UpdateBonuses();
            UpdateStats();
            equipmentText[2].text = item.name;
            equipmentText[0].color = Color.green;
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
        critChance = 0 + critChanceBonus;
        regeneration = 0 + regeneration;
        critMultiplier = 0 + critMultiplierBonus;
        damageAppliedMult = 1f;
        ResetHealth();
    }

    public void UpdateStatText()
    {
        statsText.text = $"Max HP: {maxHealth}\nRegeneration: {regeneration}/turn\nDamage Bonus: +{damage}\nDamage Reduction: {damageReduction}%\nCrit Chance Bonus: +{critChance}%\nCrit Multiplier Bonus: +{critMultiplier}";
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

    public bool CanBuy(float amount)
    {
        if (money-amount >= 0)
        {
            money -= amount;
            moneyText.text = $"${money}";
            return true;
        }
        else
        {
            Debug.Log("Cant Afford");
            return false;
        }
    }

    public void AddMoney(float amount)
    {
        money += amount;
        moneyText.text = $"${money}";
    }

    public void DisplayEquipmentInfo(int slot)
    {
        switch (slot)
        {
            case 0:
                if (!infoVisible[slot] && weaponSlot != null)
                {
                    string text = "";
                    equipmentInfo[slot].SetActive(true);
                    if (weaponSlot.name != null) { text = weaponSlot.name; } else { text = "None"; break; }
                    if (weaponSlot.flatHP > 0) { text += $"\nHP: {weaponSlot.flatHP}"; }
                    if (weaponSlot.percentHP > 0) { text += $"\nHP%: {weaponSlot.percentHP}%"; }
                    if (weaponSlot.flatDamage > 0) { text += $"\nDmg: {weaponSlot.flatDamage}"; }
                    if (weaponSlot.percentDamage > 0) { text += $"\nDmg%: {weaponSlot.percentDamage}%"; }
                    if (weaponSlot.flatDamageRed > 0) { text += $"\nDmgRed: {weaponSlot.flatDamageRed}"; }
                    if (weaponSlot.percentDamageRed > 0) { text += $"\nDmgRed%: {weaponSlot.percentDamageRed}%"; }
                    if (weaponSlot.critChance > 0) { text += $"\nCritChance: {weaponSlot.critChance}%"; }
                    if (weaponSlot.critMultiplier > 0) { text += $"\nCritMult: {weaponSlot.critMultiplier}x"; }
                    if (weaponSlot.regen > 0) { text += $"\nRegen: {weaponSlot.regen}/Turn"; }
                    equipmentInfoText[slot].text = text;
                    infoVisible[slot] = true;
                } else
                {
                    infoVisible[slot] = false;
                    equipmentInfo[slot].SetActive(false);
                }
                    return;
            case 1:
                if (!infoVisible[slot] && armorSlot != null)
                {
                    string text = "";
                    equipmentInfo[slot].SetActive(true);
                    if (armorSlot.name != null) { text = armorSlot.name; } else { text = "None"; break; }
                    if (armorSlot.flatHP > 0) { text += $"\nHP: {armorSlot.flatHP}"; }
                    if (armorSlot.percentHP > 0) { text += $"\nHP%: {armorSlot.percentHP}%"; }
                    if (armorSlot.flatDamage > 0) { text += $"\nDmg: {armorSlot.flatDamage}"; }
                    if (armorSlot.percentDamage > 0) { text += $"\nDmg%: {armorSlot.percentDamage}%"; }
                    if (armorSlot.flatDamageRed > 0) { text += $"\nDmgRed: {armorSlot.flatDamageRed}"; }
                    if (armorSlot.percentDamageRed > 0) { text += $"\nDmgRed%: {armorSlot.percentDamageRed}%"; }
                    if (armorSlot.critChance > 0) { text += $"\nCritChance: {armorSlot.critChance}%"; }
                    if (armorSlot.critMultiplier > 0) { text += $"\nCritMult: {armorSlot.critMultiplier}x"; }
                    if (armorSlot.regen > 0) { text += $"\nRegen: {armorSlot.regen}/Turn"; }
                    equipmentInfoText[slot].text = text;
                    infoVisible[slot] = true;
                }
                else
                {
                    infoVisible[slot] = false;
                    equipmentInfo[slot].SetActive(false);
                }
                return;
            case 2:
                if (!infoVisible[slot] && ringSlot != null)
                {
                    string text = "";
                    equipmentInfo[slot].SetActive(true);
                    if (ringSlot.name != null) { text = ringSlot.name; } else { text = "None"; break; }
                    if (ringSlot.flatHP > 0) { text += $"\nHP: {ringSlot.flatHP}"; }
                    if (ringSlot.percentHP > 0) { text += $"\nHP%: {ringSlot.percentHP}%"; }
                    if (ringSlot.flatDamage > 0) { text += $"\nDmg: {ringSlot.flatDamage}"; }
                    if (ringSlot.percentDamage > 0) { text += $"\nDmg%: {ringSlot.percentDamage}%"; }
                    if (ringSlot.flatDamageRed > 0) { text += $"\nDmgRed: {ringSlot.flatDamageRed}"; }
                    if (ringSlot.percentDamageRed > 0) { text += $"\nDmgRed%: {ringSlot.percentDamageRed}%"; }
                    if (ringSlot.critChance > 0) { text += $"\nCritChance: {ringSlot.critChance}%"; }
                    if (ringSlot.critMultiplier > 0) { text += $"\nCritMult: {ringSlot.critMultiplier}x"; }
                    if (ringSlot.regen > 0) { text += $"\nRegen: {ringSlot.regen}/Turn"; }
                    equipmentInfoText[slot].text = text;
                    infoVisible[slot] = true;
                }
                else
                {
                    infoVisible[slot] = false;
                    equipmentInfo[slot].SetActive(false);
                }
                return;
        }
    }
}