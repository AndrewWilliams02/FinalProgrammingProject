using UnityEngine;

public class AttackAction : MonoBehaviour
{
    //SKill info for the current skill
    public Rarity rarity;
    public string skillName;

    //Skill stats for the current skill
    public Vector2 damage;
    public int hitCount;
    public float accuracy, critChance, recoilDamage;
    public bool hasAoe;

    //Function that updates the current skill slots info with the current skill whenever called
    public void UpdateSkillInfo(Skills skill)
    {
        if (skill != null)
        {
            rarity = skill.rarity;
            skillName = skill.skillName;
            hitCount = skill.hitCount;
            damage = skill.damage;
            accuracy = skill.accuracy;
            critChance = skill.critChance;
            recoilDamage = skill.recoilDamage;
            hasAoe = skill.hasAoe;
        }
        else
        {
            rarity = Rarity.Common;
            skillName = "";
            hitCount = 0;
            damage = Vector2.zero;
            accuracy = 0;
            critChance = 0;
            recoilDamage = 0;
            hasAoe = false;
        }
    }
}
