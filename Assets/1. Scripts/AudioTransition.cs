using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioTransition : MonoBehaviour
{
    public float endTime;
    public GlobalData globalData;
    public AudioListEntry entry;

    public float debugTimeLeft;

    void Update()
    {
        debugTimeLeft = endTime - Time.time;

        if(Time.time + 1f >= endTime)
        {
            if(entry.transitionsToSoundEffect.Length > 0)
            {
                int index = Random.Range(0, entry.transitionsToSoundEffect.Length - 1);

                globalData.audioList.PlaySoundEffect(transform.parent, entry.transitionsToSoundEffect[index], endTime - Time.time);
            }

            if(entry.transitionsToMusic.Length > 0)
            {
                int index = Random.Range(0, entry.transitionsToMusic.Length - 1);

                globalData.audioList.PlayMusic(transform.parent, entry.transitionsToMusic[index], endTime - Time.time);
            }

            Destroy(gameObject);
        }
    }
}
