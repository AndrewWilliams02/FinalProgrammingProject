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

    public Item randomItem;
    public Skills randomSkill;
    public int randomMerch;
    float cost;

    private void Awake()
    {
        playerScript = player.GetComponent<Player>();
    }

    private void OnEnable()
    {
        RandomizeSlot();
        purchaseButton.SetActive(true);
    }

    void RandomizeSlot()
    {
        randomMerch = Random.Range(0, 2);

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
                    skillSlotUI.SetActive(true);
                    return;
            }
        }
    }

    public void ReplaceSkill(int slot)
    {
        playerScript.currentSkills[slot] = randomSkill;

        playerScript.UpdateSkills();
        skillSlotUI.SetActive(false);
    }
}
