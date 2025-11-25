using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject stateManager;
    [SerializeField] GameObject playerUI;

    float health; //Players current health
    public float maxHealth = 100;
    public float damageBonus = 0;
    public float damageReduction = 0;
    public float regeneration = 0;
    public float critChanceBonus = 0;
    public float critMultiplierBonus = 0;

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
                    float damage = Mathf.Round(Random.Range(skill.damage.x + damageBonus / 2, skill.damage.y + damageBonus));
                    float accuracy = skill.accuracy, critChance = skill.critChance, recoilDamage = skill.recoilDamage;
                    float multiplier = 1f;
                    if (Random.Range(0, 1) <= critChance + critChanceBonus) //Checks if the player's attack crits and adjusts the modifier if so
                    {
                        multiplier = 1.25f + critMultiplierBonus;
                    }
                    float totalDamage = Mathf.Round((damage * multiplier) * 10f) / 10f; //Calculates the players total damage rounded to the nearest tenth

                    if (Random.Range(0, 1) <= accuracy) //Checks if the player hits the enemy according to the current skills accuracy
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
        maxHealth = 100;
        health = maxHealth;
        damageBonus = 0;
        damageReduction = 0;
        regeneration = 0;
        critChanceBonus = 0;
        critMultiplierBonus = 0;
        damageAppliedMult = 1f;
    }
}
