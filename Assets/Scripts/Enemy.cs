using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyTemplate enemyType;
    float health, critChance, attackAccuracy;
    Vector2 attackDamage;
    string enemyName;

    GameObject player;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    void Start()
    {
        //Initialize enemy stats from scriptable object
        health = enemyType.MaxHP;
        critChance = enemyType.critChance;
        attackAccuracy = enemyType.attackAccuracy;
        attackDamage = enemyType.attackDamage;
        enemyName = enemyType.name;

        //Sets the values to the healthbar using the enemy's health stats
        healthBar.minValue = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
        healthText.text = $" HP: {health}";

        //Finds player target
        player = GameObject.Find("Player");
    }

    void Update()
    {
        //Keeps enemy's health updated
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    public void AttackPlayer()
    {
        float damage = Random.Range(attackDamage.x, attackDamage.y);
        float multiplier = 1f;
        if (Random.Range(0, 1) <= critChance)
        {
            multiplier = 1.25f;
        }
        float totalDamage = Mathf.Round((damage * multiplier) * 10f) / 10f;

        if (Random.Range(0, 1) <= attackAccuracy)
        {
            player.SendMessage("ApplyDamage", totalDamage);
            Debug.Log($"{enemyName}s dealt {totalDamage} damage to the Player!");
        }
        else
        {
            Debug.Log($"{enemyName}s Attack Missed!");
        }
    }

    //Function that applies damage to this enemy when called
    void ApplyDamage(float totalDamage)
    {
        health -= totalDamage;
    }
}
