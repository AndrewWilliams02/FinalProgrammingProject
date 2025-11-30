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
    }

    //Function that 
    void AttackPlayer()
    {
        //Sets the variables needed for damage
        float damage = Random.Range(attackDamage.x, attackDamage.y);
        float multiplier = 1f;
        float critCheck = Random.Range(0f, 1f);
        float accuracyCheck = Random.Range(0f, 1f);
        if (critCheck <= critChance)
        {
            multiplier = 1.25f;
        }
        float totalDamage = Mathf.Round((damage * multiplier) * 10f) / 10f;

        //Checks if the player hits the enemy according to the current skills accuracy
        if (accuracyCheck <= attackAccuracy)
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
        //Debug.Log($"Player dealt {totalDamage} damage to {enemyName}!");

        Kill();
    }

    public void RandomizeEnemy(EnemyTemplate enemy, float modifier)
    {
        //Initialize enemy stats from scriptable object
        health = Mathf.Round(enemy.maxHP * modifier * 10) / 10;
        critChance = enemy.critChance;
        attackAccuracy = enemy.attackAccuracy;
        attackDamage = new Vector2(Mathf.Round(enemy.attackDamage.x * modifier * 10) / 10, Mathf.Round(enemy.attackDamage.y * modifier * 10) / 10);
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
            EnemyController controller = enemyController.GetComponent<EnemyController>();
            controller.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}
