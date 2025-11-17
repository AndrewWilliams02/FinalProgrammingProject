using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CSVtoScriptableObjects
{
    private static string enemyCSVPath = "/Editor/CSVs/Enemies.csv", skillCSVPath = "/Editor/CSVs/Skills.csv", itemCSVPath;
    int currentLine = 0;

    [MenuItem("Generation/Generate Skills")] //Ceates a new selection under the "Generation" tab in the unity editor to generate skills
    public static void GenerateSkills()
    {
        //Gets each line from the skills CSV file and seperates them into an array
        string[] allLines = File.ReadAllLines(Application.dataPath + skillCSVPath);

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

            //Creates the skill scriptable object as a new assets inside of its respective folder
            AssetDatabase.CreateAsset(skill, $"Assets/Scriptable Objects/Skills/{skill.skillName}.asset");
        }
        //Saves the assets
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Generation/Generate Enemies")] //Ceates a new selection undert the "Generation" tab in the unity editor to generate skills
    public static void GenerateEnemies()
    {
        //Simular code as previous function, but altered to create enemies using the enemies CSV
        string[] allLines = File.ReadAllLines(Application.dataPath + enemyCSVPath);

        for (int i = 1; i < allLines.Length; i++)
        {
            string[] splitData = allLines[i].Split(',');

            EnemyTemplate enemy = ScriptableObject.CreateInstance<EnemyTemplate>();
            enemy.enemyName = splitData[0];
            enemy.enemyType = (EnemyTypes)Enum.Parse(typeof(EnemyTypes), splitData[1]);
            enemy.maxHP = float.Parse(splitData[2]);
            enemy.attackDamage = new Vector2(float.Parse(splitData[2]), float.Parse(splitData[3]));
            enemy.attackAccuracy = float.Parse(splitData[4]);
            enemy.critChance = float.Parse(splitData[5]);

            AssetDatabase.CreateAsset(enemy, $"Assets/Scriptable Objects/Enemies/{enemy.enemyName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Generation/Generate Items")]
    public static void GenerateItems()
    {

    }
}
