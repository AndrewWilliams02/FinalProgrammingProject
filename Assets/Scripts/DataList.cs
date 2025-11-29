using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DataList : ScriptableObject
{
    public List<Item> allItems = new List<Item>();
    public List<EnemyTemplate> allEnemies = new List<EnemyTemplate>();
    public List<Skills> allSkills = new List<Skills>();
}
