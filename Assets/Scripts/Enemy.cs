using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    float health = 100;
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    void Start()
    {
        //Sets the values to the healthbar using the enemy's health stats
        healthBar.minValue = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    void Update()
    {
        //Keeps enemy's health updated
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    //Function that applies damage to this enemy when called
    void ApplyDamage(float totalDamage)
    {
        health -= totalDamage;
    }
}
