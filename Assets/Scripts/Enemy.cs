using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    float health, critChance, attackAccuracy;
    Vector2 attackDamage;
    string enemyName;

    GameObject player, enemyController;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    void Start()
    {
        //Finds player target
        player = GameObject.Find("Player");

        enemyController = GameObject.Find("EnemyController");
    }

    void Update()
    {
        //Keeps enemy's health updated
        healthBar.value = health;
        healthText.text = $" HP: {health}";

        Kill();
    }

    void AttackPlayer()
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
        health = Mathf.Round(health * 10) / 10;
    }

    void RandomizeEnemy(EnemyTemplate enemy)
    {
        //Initialize enemy stats from scriptable object
        health = enemy.MaxHP;
        critChance = enemy.critChance;
        attackAccuracy = enemy.attackAccuracy;
        attackDamage = enemy.attackDamage;
        enemyName = enemy.name;

        //Sets the values to the healthbar using the enemy's health stats
        healthBar.minValue = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    void Kill()
    {
        if (health <= 0)
        {
            enemyController.SendMessage("RemoveEnemy", gameObject);
            player.SendMessage("RemoveEnemy", gameObject);
            enemyController.SendMessage("CheckEnemies");
            Destroy(gameObject);
        }
    }
}
