using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public VisualPlayer visualPlayer;
    public Player player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out CharacterController characterController))
        {
            if(characterController.gameObject.transform.parent.gameObject.TryGetComponent(out Enemy enemy))
            {
                // enemy.visualPlayer.equipedParts[index]

                enemy.DropLoot();

                player.globalData.audioList.PlaySoundEffect(transform.parent, "splat");
                player.CheckIfWin();

                Destroy(characterController.gameObject.transform.parent.gameObject);
            }
        }
    }
}
