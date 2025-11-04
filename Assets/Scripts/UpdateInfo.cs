using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UpdateInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject skill;

    void Start()
    {
        UpdateSkillInfo();
    }

    public void UpdateSkillInfo()
    {
        text.text = skill.GetComponent<AttackAction>().skillName;
    }
}
