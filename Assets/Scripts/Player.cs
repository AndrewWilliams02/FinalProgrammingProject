using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    float health;
    float maxHealth = 100;
    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] GameObject currentTarget;

    void Start()
    {
        health = maxHealth;
        healthBar.minValue = 0;
        UpdateHealthBar();
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    void Update()
    {
        healthBar.value = health;
        healthText.text = $" HP: {health}";
    }

    void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
    }

    public void UseSkill(GameObject action)
    {
        AttackAction skill = action.GetComponent<AttackAction>();

        float damage = Mathf.Round(Random.Range(skill.damage.x, skill.damage.y));
        int hitCount = skill.hitCount, buffDuration = skill.buffDuration;
        float accuracy = skill.accuracy, critChance = skill.critChance, recoilDamage = skill.recoilDamage, damageBuff = skill.damageBuff, damageReductionBuff = skill.damageReductionBuff;
        float multiplier = 1f;
        if (Random.Range(0, 1) <= critChance)
        {
            multiplier = 1.25f;
        }
        float totalDamage = Mathf.Round((damage * hitCount * multiplier) * 10f) / 10f;

        if (Random.Range(0, 1) <= accuracy)
        {
            if (recoilDamage > 0) { health -= recoilDamage; }
            currentTarget.SendMessage("ApplyDamage", totalDamage);
            Debug.Log($"Attack dealt {totalDamage} damage!");
        } else
        {
            Debug.Log("Attack Missed!");
        }
    }
}
