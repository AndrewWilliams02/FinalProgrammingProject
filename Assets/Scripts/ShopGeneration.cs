using TMPro;
using UnityEngine;

public class ShopGeneration : MonoBehaviour
{
    [SerializeField] DataList dataList;
    [SerializeField] GameObject player;
    Player playerScript;
    [SerializeField] TextMeshProUGUI merchText, costText;
    [SerializeField] GameObject purchaseButton;
    [SerializeField] GameObject skillSlotUI;

    //Varables to track the random skill or item being generated in a shop slot
    public Item randomItem;
    public Skills randomSkill;

    public int randomMerch; //Variable that randomizes which data type shows up in the shop (item or skill)
    float cost;

    private void Awake()
    {
        playerScript = player.GetComponent<Player>();
    }

    //When the gameobject becomes enabled, randomize the shop slot and re-enables buy button
    private void OnEnable()
    {
        RandomizeSlot();
        purchaseButton.SetActive(true);
    }

    //Function that randomizes the data in the current shop slot
    void RandomizeSlot()
    {
        randomMerch = Random.Range(0, 2); //Picks a random data type integer (item or skill)

        //Switch statement that sets the data type and displays said types name and cost
        switch (randomMerch)
        {
            case 0:
                randomItem = dataList.allItems[Random.Range(0, dataList.allItems.Count)];
                merchText.text = randomItem.name;
                cost = randomItem.cost;
                costText.text = $"${cost}";
                return;
            case 1:
                randomSkill = dataList.allSkills[Random.Range(0, dataList.allSkills.Count)];
                merchText.text = randomSkill.name;
                cost = randomSkill.cost;
                costText.text = $"${cost}";
                return;
        }
    }

    //Function that equips the item or skill to the player if the cost requirement is met
    public void Buy()
    {
        if (playerScript.CanBuy(cost))
        {
            switch (randomMerch)
            {
                case 0:
                    purchaseButton.SetActive(false);
                    playerScript.EquipItem(randomItem);
                    return;
                case 1:
                    purchaseButton.SetActive(false);
                    skillSlotUI.SetActive(true); //Displays UI to decide which slot skill will go into
                    return;
            }
        }
    }

    //Function that replaces the current skill in a slot with the newly bought skill
    public void ReplaceSkill(int slot)
    {
        playerScript.currentSkills[slot] = randomSkill;

        playerScript.UpdateSkills();
        skillSlotUI.SetActive(false);
    }
}
