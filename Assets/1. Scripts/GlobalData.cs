using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New GlobalData", menuName = "Data/GlobalData"), System.Serializable]
public class GlobalData : ScriptableObject
{
    public AudioList audioList;
    public string startMusicName;
    public GameObject lastSpurtLightPrefab;

    [Space(16)]
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public Transform playerTransform;

    public void Init(Transform playerT, GameObject enemyParent)
    {
        audioList.globalData = this;

        playerTransform = playerT;

        if(enemyParent != null)
        {
            for(int childIndex = 0; childIndex < enemyParent.transform.childCount; childIndex += 1)
            {
                GameObject enemyObject =  enemyParent.transform.GetChild(childIndex).gameObject;

                if(enemyObject.TryGetComponent(out Enemy enemy))
                {
                    enemy.globalData = this;
                }
            }
        }
    }
}
