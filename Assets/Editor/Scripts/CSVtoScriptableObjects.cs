using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSVtoScriptableObjects
{
    private static string enemyCSVPath = "/Editor/CSVs/Enemies.csv", skillCSVPath = "/Editor/CSVs/Skills.csv", itemCSVPath = "/Editor/CSVs/Items.csv";
    private const string dataListPath = "Assets/Scriptable Objects/DataList.asset";

    private static DataList GetDataList()
    {
        DataList dl = AssetDatabase.LoadAssetAtPath<DataList>(dataListPath);
        if (dl == null)
            Debug.LogError("DataList.asset not found!");
        return dl;
    }

    [MenuItem("Generation/Generate Skills")] //Ceates a new selection under the "Generation" tab in the unity editor to generate skills
    public static void GenerateSkills()
    {
        DataList dataList = GetDataList();

        //Gets each line from the skills CSV file and seperates them into an array
        string[] allLines = File.ReadAllLines(Application.dataPath + skillCSVPath);

        dataList.allSkills.Clear();

        //Loop that skips the first line of the CSV file (since they are headers) and creates a new scriptable object per line in the CSV
        for (int i = 1; i < allLines.Length; i++)
        {
            //Splits the data in the line by the "," divider
            string[] splitData = allLines[i].Split(',');

            //Creates instance of the skills scriptable object and sets the data from the CSV into the it using the split data
            Skills skill = ScriptableObject.CreateInstance<Skills>();
            skill.skillName = splitData[0];
            skill.rarity = (Rarity)Enum.Parse(typeof(Rarity), splitData[1]);
            skill.damage = new Vector2(float.Parse(splitData[2]), float.Parse(splitData[3]));
            skill.hitCount = int.Parse(splitData[4]);
            skill.accuracy = float.Parse(splitData[5]);
            skill.critChance = float.Parse(splitData[6]);
            skill.recoilDamage = float.Parse(splitData[7]);
            skill.cost = float.Parse(splitData[8]);
            skill.hasAoe = bool.Parse(splitData[9]);

            //Creates the skill scriptable object as a new assets inside of its respective folder
            AssetDatabase.CreateAsset(skill, $"Assets/Scriptable Objects/Skills/{skill.skillName}.asset");
            dataList.allSkills.Add(skill);
        }
        //Saves the assets
        EditorUtility.SetDirty(dataList);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Generation/Generate Enemies")] //Ceates a new selection undert the "Generation" tab in the unity editor to generate skills
    public static void GenerateEnemies()
    {
        DataList dataList = GetDataList();

        //Simular code as previous function, but altered to create enemies using the enemies CSV
        string[] allLines = File.ReadAllLines(Application.dataPath + enemyCSVPath);

        dataList.allEnemies.Clear();

        for (int i = 1; i < allLines.Length; i++)
        {
            string[] splitData = allLines[i].Split(',');

            EnemyTemplate enemy = ScriptableObject.CreateInstance<EnemyTemplate>();
            enemy.enemyName = splitData[0];
            enemy.enemyType = (EnemyTypes)Enum.Parse(typeof(EnemyTypes), splitData[1]);
            enemy.maxHP = float.Parse(splitData[2]);
            enemy.attackDamage = new Vector2(float.Parse(splitData[3]), float.Parse(splitData[4]));
            enemy.attackAccuracy = float.Parse(splitData[5]);
            enemy.critChance = float.Parse(splitData[6]);
            enemy.money = float.Parse(splitData[7]);
            enemy.exp = int.Parse(splitData[8]);

            AssetDatabase.CreateAsset(enemy, $"Assets/Scriptable Objects/Enemies/{enemy.enemyName}.asset");
            dataList.allEnemies.Add(enemy);
        }

        EditorUtility.SetDirty(dataList);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Generation/Generate Items")]
    public static void GenerateItems()
    {
        DataList dataList = GetDataList();

        string[] allLines = File.ReadAllLines(Application.dataPath + itemCSVPath);

        dataList.allItems.Clear();

        for (int i = 1; i < allLines.Length; i++)
        {
            string[] splitData = allLines[i].Split(',');

            Item item = ScriptableObject.CreateInstance<Item>();
            item.itemName = splitData[0];
            item.rarity = (Rarity)Enum.Parse(typeof(Rarity), splitData[1]);
            item.itemType = (ItemType)Enum.Parse(typeof(ItemType), splitData[2]);
            item.flatHP = float.Parse(splitData[3]);
            item.percentHP = float.Parse(splitData[4]);
            item.flatDamage = float.Parse(splitData[5]);
            item.percentDamage = float.Parse(splitData[6]);
            item.flatDamageRed = float.Parse(splitData[7]);
            item.percentDamageRed = float.Parse( splitData[8]);
            item.critChance = float.Parse(splitData[9]);
            item.critMultiplier = float.Parse(splitData[10]);
            item.regen = float.Parse(splitData[11]);
            item.cost = float.Parse(splitData[12]);

            AssetDatabase.CreateAsset(item, $"Assets/Scriptable Objects/Items/{item.itemName}.asset");
            dataList.allItems.Add(item);
        }

        EditorUtility.SetDirty(dataList);
        AssetDatabase.SaveAssets();
    }
}
