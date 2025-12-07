using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEditor.Animations;

public class Enemy : MonoBehaviour
{
    //Variables to handle enemy stats
    float money, health, critChance, attackAccuracy;
    int exp;
    Vector2 attackDamage;
    public string enemyName;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI nameplate;

    GameObject player, enemyController;
    public Animator animator;
    SpriteRenderer sprite;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        enemyController = GameObject.Find("EnemyController");
    }

    void Update()
    {
        //Keeps enemy's health updated
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    //Function that when called, attacks the player
    IEnumerator AttackPlayer()
    {
        Animator pAnimator = player.GetComponent<Animator>();
        Vector3 originalPos = transform.position;
        Vector3 pos = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.x);
        transform.position = pos;
        animator.Play(enemyName+"_Attack");
        yield return new WaitForSeconds(0.35f);

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
            pAnimator.Play("Player_Hurt");

            player.SendMessage("ApplyDamage", totalDamage); //Calls the "Apply Damage" function on the player and deals the total attack damage
        }
        else
        {
            Debug.Log($"{enemyName}s Attack Missed!");
        }

        yield return new WaitForSeconds(0.2f);
        transform.position = originalPos;
        animator.Play(enemyName + "_Idle");
        yield return new WaitForSeconds(0.3f);
        pAnimator.Play("Player_Idle");

    }

    //Function that applies damage to this enemy when called
    void ApplyDamage(float totalDamage)
    {
        health -= totalDamage;
        health = Mathf.Round(health * 10) / 10;
        //Debug.Log($"Player dealt {totalDamage} damage to {enemyName}!");

        Kill();
    }

    //Function that randomizes the enemy type & stats
    public void RandomizeEnemy(EnemyTemplate enemy, float modifier, Sprite spr, RuntimeAnimatorController anim)
    {
        //Initialize enemy stats from scriptable object
        health = Mathf.Round(enemy.maxHP * modifier * 10) / 10;
        critChance = enemy.critChance;
        attackAccuracy = enemy.attackAccuracy;
        attackDamage = new Vector2(Mathf.Round(enemy.attackDamage.x * modifier * 10) / 10, Mathf.Round(enemy.attackDamage.y * modifier * 10) / 10);
        enemyName = enemy.name;
        money = enemy.money;
        exp = enemy.exp;

        //Sets the values to the healthbar using the enemy's health stats
        healthBar.minValue = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
        healthText.text = $" HP: {health}";
        nameplate.text = enemy.name;

        animator.runtimeAnimatorController = anim;
        sprite.sprite = spr;

        animator.Play(enemyName + "_Idle");
    }

    //Function to delete the enemy on death
    void Kill()
    {
        if (health <= 0)
        {
            //Deletes current enemy from list and destroys it
            EnemyController controller = enemyController.GetComponent<EnemyController>();
            controller.RemoveEnemy(gameObject);
            Player playerScript = player.GetComponent<Player>();
            playerScript.AddMoney(money);
            playerScript.AddExperience(exp);
            Destroy(gameObject);
        }
    }
}
