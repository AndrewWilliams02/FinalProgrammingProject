using UnityEngine;

public class AttackAction : MonoBehaviour
{
    public Skills deafualtSkill;
    //Skill Info
    public Rarity rarity;
    public string skillName, skillDescription;
    //Skill Stats
    public Vector2 damage;
    public int hitCount, buffDuration;
    public float accuracy, critChance, recoilDamage, damageBuff, damageReductionBuff;

    private void Awake()
    {
        UpdateSkillInfo(deafualtSkill);
    }

    public void UpdateSkillInfo(Skills skill)
    {
        if (skill != null)
        {
            rarity = skill.rarity;
            skillName = skill.skillName;
            skillDescription = skill.skillDescription;
            hitCount = skill.hitCount;
            buffDuration = skill.buffDuration;
            damage = skill.damage;
            accuracy = skill.accuracy;
            critChance = skill.critChance;
            recoilDamage = skill.recoilDamage;
            damageBuff = skill.damageBuff;
            damageReductionBuff = skill.damageReductionBuff;
        }
    }
}
