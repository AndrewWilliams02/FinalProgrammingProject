using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DataList : ScriptableObject
{
    //Lists for the scriptable objects of each data type to be refrenced by scripts
    public List<Item> allItems = new List<Item>();
    public List<EnemyTemplate> allEnemies = new List<EnemyTemplate>();
    public List<Skills> allSkills = new List<Skills>();
}
