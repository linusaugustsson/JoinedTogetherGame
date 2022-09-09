using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AudioListEntryClip
{
    public AudioClip clip;

    [Space(8)]
    public bool looping = false;

    [Space(8), Range(-128f, 128f)]
    public int priority = 0;

    [Space(8)]
    [Range(-1f, 0f)]
    public float minVolume;
    [Range(-1f, 0f)]
    public float maxVolume;

    [Space(8)]
    [Range(-4f, 2f)]
    public float minPitch;
    [Range(-4f, 2f)]
    public float maxPitch;

    [Space(8), Range(-1f, 1f), Tooltip("Pan Stereo: Negative values are left, positive are right")]
    public float panStereo = 0f;
    [Range(-1f, 0f), Tooltip("Spatial Blend: -1 feels 2D while 0 feels 3D")]
    public float spatialBlend = 0f;
}

[System.Serializable]
public struct AudioListEntry
{
    public string name;
    public string[] transitionsToSoundEffect;
    public string[] transitionsToMusic;
    public AudioListEntryClip[] entries;
}

[CreateAssetMenu(fileName = "New AudioList", menuName = "Data/AudioList"), System.Serializable]
public class AudioList : ScriptableObject
{
    public GameObject audioSourcePrefab;
    public GameObject transitionPrefab;
    public AudioListEntry[] soundEffects;
    public AudioListEntry[] music;

    [HideInInspector]
    public GlobalData globalData;

    public void SetSourceFromEntryClip(ref AudioSource source, AudioListEntryClip entry)
    {
        if(source != null)
        {
            if(source.isPlaying)
            {
                source.Stop();
            }

            source.clip = entry.clip;
            source.volume = Random.Range(entry.minVolume, entry.maxVolume) + 1f;
            source.pitch = Random.Range(entry.minPitch, entry.maxPitch) + 1f;
            source.spread = 180;
            source.loop = entry.looping;
            source.priority = entry.priority + 128;
            source.panStereo = entry.panStereo;
            source.spatialBlend = entry.spatialBlend + 1f;
        }
    }

    public void PlayEntryClip(ref AudioSource source, AudioListEntryClip entry, float delay = 0f)
    {
        if(source != null)
        {
            SetSourceFromEntryClip(ref source, entry);

            if(delay > 0f)
            {
                source.PlayDelayed(delay);
            }
            else
            {
                source.Play();
            }
        }
    }

    public void PlayRandomFromList(ref AudioSource source, AudioListEntry list, float delay = 0f)
    {
        if(source != null && list.entries.Length > 0)
        {
            int index = Random.Range(0, list.entries.Length - 1);

            PlayEntryClip(ref source, list.entries[index], delay);
        }
    }

    public void PlaySoundEffect(ref AudioSource source, int index, float delay = 0f)
    {
        if(source != null && index >= 0 && index < soundEffects.Length)
        {
            PlayRandomFromList(ref source, soundEffects[index], delay);
        }
    }

    public void PlaySoundEffect(ref AudioSource source, string name, float delay = 0f)
    {
        if(source != null)
        {
            PlaySoundEffect(ref source, FindSoundEffect(name), delay);
        }
    }

    public GameObject PlaySoundEffect(Transform parent, int index, float delay = 0f)
    {
        GameObject result = Instantiate(audioSourcePrefab, parent);

        if(result.TryGetComponent(out AudioSource source))
        {
            PlaySoundEffect(ref source, index, delay);

            if(!source.loop)
            {
                Destroy(result, source.clip.length);

                if(soundEffects[index].transitionsToSoundEffect.Length > 0 || soundEffects[index].transitionsToMusic.Length > 0)
                {
                    GameObject transitionObject = Instantiate(transitionPrefab, parent);

                    if(transitionObject.TryGetComponent(out AudioTransition transition))
                    {
                        transition.endTime = Time.time + source.clip.length;
                        transition.entry = soundEffects[index];
                    }
                }
            }

            return result;
        }
        else
        {
            Destroy(result);
        }

        return null;
    }

    public GameObject PlaySoundEffect(Transform parent, string name, float delay = 0f)
    {
        return PlaySoundEffect(parent, FindSoundEffect(name), delay);
    }

    public void PlayMusic(ref AudioSource source, int index, float delay = 0f)
    {
        if(source != null && index >= 0 && index < music.Length)
        {
            PlayRandomFromList(ref source, music[index], delay);
        }
    }

    public void PlayMusic(ref AudioSource source, string name, float delay = 0f)
    {
        if(source != null)
        {
            PlayMusic(ref source, FindMusic(name), delay);
        }
    }

    public GameObject PlayMusic(Transform parent, int index, float delay = 0f)
    {
        GameObject result = Instantiate(audioSourcePrefab, parent);

        if(result.TryGetComponent(out AudioSource source))
        {
            PlayMusic(ref source, index, delay);

            if(!source.loop)
            {
                Destroy(result, source.clip.length);

                if(music[index].transitionsToSoundEffect.Length > 0 || music[index].transitionsToMusic.Length > 0)
                {
                    GameObject transitionObject = Instantiate(transitionPrefab, parent);

                    if(transitionObject.TryGetComponent(out AudioTransition transition))
                    {
                        transition.endTime = Time.time + source.clip.length;
                        transition.entry = music[index];
                    }
                }
            }

            return result;
        }
        else
        {
            Destroy(result);
        }

        return null;
    }

    public GameObject PlayMusic(Transform parent, string name, float delay = 0f)
    {
        return PlayMusic(parent, FindMusic(name), delay);
    }

    public int FindSoundEffect(string name)
    {
        for(int itIndex = 0; itIndex < soundEffects.Length; itIndex += 1)
        {
            if(soundEffects[itIndex].name == name)
            {
                return itIndex;
            }
        }

        return -1;
    }

    public int FindMusic(string name)
    {
        for(int itIndex = 0; itIndex < music.Length; itIndex += 1)
        {
            if(music[itIndex].name == name)
            {
                return itIndex;
            }
        }

        return -1;
    }
}
