using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class SlotGeneration : MonoBehaviour
{
    [SerializeField] DataList dataList;
    [SerializeField] GameObject player, stateManager;
    StateManager state;
    Player playerScript;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject skillSlotUI;

    public Item randomItem;
    public Skills randomSkill;
    public float healModifier;
    public int randomReward;

    private void Awake()
    {
        playerScript = player.GetComponent<Player>();
        state = stateManager.GetComponent<StateManager>();
    }

    private void OnEnable()
    {
        RandomizeSlot();
    }

    void RandomizeSlot()
    {
        randomReward = Random.Range(0, 3);

        switch (randomReward)
        {
            case 0:
                randomItem = dataList.allItems[Random.Range(0, dataList.allItems.Count)];
                buttonText.text = randomItem.name;
                return;
            case 1:
                randomSkill = dataList.allSkills[Random.Range(0, dataList.allSkills.Count)];
                buttonText.text = randomSkill.name;
                return;
            case 2:
                healModifier = Mathf.Round(Random.Range(0.1f, 0.5f) * 100) / 100;
                Debug.Log(healModifier);
                buttonText.text = $"Heal {healModifier * 100}%";
                return;
        }
    }

    public void SelectSlot()
    {
        switch (randomReward)
        {
            case 0:
                playerScript.EquipItem(randomItem);
                state.RewardSelected();
                return;
            case 1:
                skillSlotUI.SetActive(true);
                return;
            case 2:
                playerScript.Rest(healModifier);
                state.RewardSelected();
                return;
        }
    }

    public void ReplaceSkill(int slot)
    {
        playerScript.currentSkills[slot] = randomSkill;

        playerScript.UpdateSkills();
        skillSlotUI.SetActive(false);
        state.RewardSelected();
    }
}
