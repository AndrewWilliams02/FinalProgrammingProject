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
        healthBar.minValue = 0;
        healthBar.maxValue = health;
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    void Update()
    {
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    void ApplyDamage(float totalDamage)
    {
        health -= totalDamage;
    }
}
