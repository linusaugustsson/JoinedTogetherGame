using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Part headPart;
    public Part torsoPart;
    public Part armsPart;
    public Part legsPart;


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            BodyPartManager bodyPartManager = GameObject.Find("Player").GetComponent<BodyPartManager>();

            bodyPartManager.collectedHeads.Add(headPart);
            bodyPartManager.collectedTorsos.Add(torsoPart);
            bodyPartManager.collectedArms.Add(armsPart);
            bodyPartManager.collectedLegs.Add(legsPart);

            // Play Sound
            bodyPartManager.visualPlayer.globalData.audioList.PlaySoundEffect(bodyPartManager.visualPlayer.transform, "item");

            gameObject.SetActive(false);
        }
    }

}
