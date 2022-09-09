using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BodyPartEntry
{
    public string name;
    public GameObject headPrefab;
    public GameObject torsoPrefab;
    public GameObject rightArmPrefab;
    public GameObject leftArmPrefab;
    public GameObject rightLegPrefab;
    public GameObject leftLegPrefab;

    [HideInInspector]
    public GameObject[] prefabs;
};

[CreateAssetMenu(fileName = "New BodyPartList", menuName = "Data/Body Part List"), System.Serializable]
public class BodyPartList : ScriptableObject
{
    public BodyPartEntry[] entries;
}
