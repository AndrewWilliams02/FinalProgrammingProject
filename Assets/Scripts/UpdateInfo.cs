using TMPro;
using UnityEngine;

public class UpdateInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject skill;

    void OnEnable()
    {
        UpdateSkillInfo();
    }

    public void UpdateSkillInfo()
    {
        if (skill != null)
        {
            text.text = skill.GetComponent<AttackAction>().skillName;
        }
    }
}
