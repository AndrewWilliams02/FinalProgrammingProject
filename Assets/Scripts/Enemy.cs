using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //Variables to handle enemy stats
    float health, critChance, attackAccuracy;
    Vector2 attackDamage;
    string enemyName;

    GameObject player, enemyController;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    void Start()
    {
        player = GameObject.Find("Player");
        enemyController = GameObject.Find("EnemyController");
    }

    void Update()
    {
        //Keeps enemy's health updated
        healthBar.value = health;
        healthText.text = $" HP: {health}";

        //Destroys records of enemy
        Kill();
    }

    //Function that 
    void AttackPlayer()
    {
        //Sets the variables needed for damage
        float damage = Random.Range(attackDamage.x, attackDamage.y);
        float multiplier = 1f;
        if (Random.Range(0, 1) <= critChance)
        {
            multiplier = 1.25f;
        }
        float totalDamage = Mathf.Round((damage * multiplier) * 10f) / 10f;

        //Checks if the player hits the enemy according to the current skills accuracy
        if (Random.Range(0, 1) <= attackAccuracy)
        {
            player.SendMessage("ApplyDamage", totalDamage); //Calls the "Apply Damage" function on the player and deals the total attack damage
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
        Debug.Log($"Player dealt {totalDamage} damage to {enemyName}!");
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

    //Function to delete the enemy on death
    void Kill()
    {
        if (health <= 0)
        {
            //Deletes current enemy from list and destroys it
            enemyController.SendMessage("RemoveEnemy", gameObject);
            enemyController.SendMessage("CheckEnemies");
            Destroy(gameObject);
        }
    }
}
