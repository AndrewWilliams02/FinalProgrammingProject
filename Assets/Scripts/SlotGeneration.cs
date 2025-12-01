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

    //Varables to track the random skill or item being generated in a shop slot
    public Item randomItem;
    public Skills randomSkill;

    public float healModifier; //Variable to track the % of hp the player heals
    public int randomReward; //Variable that randomizes which data type shows up in the reward slot (item, skill, or heal)

    private void Awake()
    {
        playerScript = player.GetComponent<Player>();
        state = stateManager.GetComponent<StateManager>();
    }

    //When the gameobject becomes enabled, randomize the reward slot
    private void OnEnable()
    {
        RandomizeSlot();
    }

    //Function that randomizes the data in the current reward slot
    void RandomizeSlot()
    {
        randomReward = Random.Range(0, 5); //Picks a random data type integer (item, skill, or heal)

        //Switch statement that sets the data type and displays said types name or value
        switch (randomReward)
        {
            case 0:
                randomItem = dataList.allItems[Random.Range(0, dataList.allItems.Count)];
                buttonText.text = randomItem.name;
                return;
            case 1:
                randomItem = dataList.allItems[Random.Range(0, dataList.allItems.Count)];
                buttonText.text = randomItem.name;
                return;
            case 2:
                randomSkill = dataList.allSkills[Random.Range(0, dataList.allSkills.Count)];
                buttonText.text = randomSkill.name;
                return;
            case 3:
                randomSkill = dataList.allSkills[Random.Range(0, dataList.allSkills.Count)];
                buttonText.text = randomSkill.name;
                return;
            case 4:
                healModifier = Mathf.Round(Random.Range(0.1f, 0.5f) * 100) / 100;
                //Debug.Log(healModifier);
                buttonText.text = $"Heal {healModifier * 100}%";
                return;
        }
    }

    //Function that equips the item/skill to the player or heals them depending on random integer
    public void SelectSlot()
    {
        switch (randomReward)
        {
            case 0:
                playerScript.EquipItem(randomItem);
                state.RewardSelected();
                return;
            case 1:
                playerScript.EquipItem(randomItem);
                state.RewardSelected();
                return;
            case 2:
                skillSlotUI.SetActive(true); //Displays UI to decide which slot skill will go into
                return;
            case 3:
                skillSlotUI.SetActive(true); 
                return;
            case 4:
                playerScript.Rest(healModifier);
                state.RewardSelected();
                return;
        }
    }

    //Function that replaces the current skill in a slot with the newly selected skill
    public void ReplaceSkill(int slot)
    {
        playerScript.currentSkills[slot] = randomSkill;

        playerScript.UpdateSkills();
        skillSlotUI.SetActive(false);
        state.RewardSelected();
    }
}
