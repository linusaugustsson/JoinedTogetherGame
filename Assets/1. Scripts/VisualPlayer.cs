using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct CurrentAnimation
{
    public AnimationType type;
    public AnimationType previousType;
    public float time;
};


public class VisualPlayer : MonoBehaviour
{
    // NOTE(Patrik): Dessa vill du ändra runtime
    public CurrentAnimation[] currentAnimations;
    public float animationSpeed;
    public string[] equipedParts;
    public string defaultBodyPart;
    public GlobalData globalData;

    public bool debugUpdateBodyParts;

    [Space(8)]
    // NOTE(Patrik): Dessa vill du ändra i editorn
    public PlayerAnimation animationData;
    public BodyPartList bodyPartList;
    public GameObject inspectorCapsule;
    public Transform[] bodyPartTransforms;

    public GameObject weaponHitboxObject;
    public GameObject weaponObject;

    [HideInInspector]
    public GameObject[] bodyPartObjects;

    public int GetAnimationIndex(AnimationType type)
    {
        for(int itIndex = 0; itIndex < animationData.entries.Length; itIndex += 1)
        {
            if(animationData.entries[itIndex].type == type)
            {
                return itIndex;
            }
        }

        return -1;
    }

    public float GetDurationOfAnimation(AnimationType type)
    {
        int index = GetAnimationIndex(type);

        if(index >= 0)
        {
            if(!animationData.entries[index].isLooping)
            {
                return animationData.entries[index].lengthInSeconds;
            }
        }

        return -1f;
    }

    public void SetPrimaryAnimation(AnimationType type)
    {
        currentAnimations[0].previousType = currentAnimations[0].type;
        currentAnimations[0].type = type;
        currentAnimations[0].time = 0f;
    }

    public void SetPrimaryFallbackAnimation(AnimationType type)
    {
        currentAnimations[0].previousType = type;
    }

    public bool IsPrimaryAnimationPlaying(AnimationType type)
    {
        if(currentAnimations[0].type == type)
        {
            return true;
        }
        return false;
    }

    public void SetSecondaryAnimation(AnimationType type)
    {
        currentAnimations[1].previousType = currentAnimations[1].type;
        currentAnimations[1].type = type;
        currentAnimations[1].time = 0f;
    }

    public void SetSecondaryFallbackAnimation(AnimationType type)
    {
        currentAnimations[1].previousType = type;
    }

    public bool IsSecondaryAnimationPlaying(AnimationType type)
    {
        if(currentAnimations[1].type == type)
        {
            return true;
        }
        return false;
    }

    public void ChangeBodyPart(BodyPart part, string name)
    {
        if(equipedParts == null)
        {
            Debug.Log("equipedParts is null!");
        }
        else if(equipedParts.Length != System.Enum.GetValues(typeof(BodyPart)).Length)
        {
            Debug.Log("equipedParts != BodyPart.Length");
        }

        equipedParts[(int)part] = name;
        UpdateParts();
    }

    private void Awake() {
        int partCount = System.Enum.GetValues(typeof(BodyPart)).Length;
        equipedParts = new string[partCount];
        bodyPartObjects = new GameObject[partCount];

        for (int itIndex = 0; itIndex < partCount; itIndex += 1) {
            equipedParts[itIndex] = defaultBodyPart;
        }

        for (int itIndex = 0; itIndex < bodyPartList.entries.Length; itIndex += 1) {
            bodyPartList.entries[itIndex].prefabs = new GameObject[partCount];

            bodyPartList.entries[itIndex].prefabs[(int)BodyPart.HEAD] = bodyPartList.entries[itIndex].headPrefab;
            bodyPartList.entries[itIndex].prefabs[(int)BodyPart.TORSO] = bodyPartList.entries[itIndex].torsoPrefab;
            bodyPartList.entries[itIndex].prefabs[(int)BodyPart.RIGHT_ARM] = bodyPartList.entries[itIndex].rightArmPrefab;
            bodyPartList.entries[itIndex].prefabs[(int)BodyPart.LEFT_ARM] = bodyPartList.entries[itIndex].leftArmPrefab;
            bodyPartList.entries[itIndex].prefabs[(int)BodyPart.RIGHT_LEG] = bodyPartList.entries[itIndex].rightLegPrefab;
            bodyPartList.entries[itIndex].prefabs[(int)BodyPart.LEFT_LEG] = bodyPartList.entries[itIndex].leftLegPrefab;
        }
    }

    void Start()
    {
        Destroy(inspectorCapsule);

        int animationCount = 2;

        currentAnimations = new CurrentAnimation[animationCount];

        for(int itIndex = 0; itIndex < animationCount; itIndex += 1)
        {
            currentAnimations[itIndex].previousType = AnimationType.NONE;
            currentAnimations[itIndex].type = AnimationType.NONE;
            currentAnimations[itIndex].time = 0f;
        }

        SetPrimaryAnimation(AnimationType.IDLE);
        SetPrimaryAnimation(AnimationType.IDLE);

        animationSpeed = 1f;



        if(defaultBodyPart.Length <= 0)
        {
            defaultBodyPart = "box";
        }



        UpdateParts();
    }

    public void UpdateParts()
    {
        for(int itIndex = 0; itIndex < equipedParts.Length; itIndex += 1)
        {
            for(int partIndex = 0; partIndex < bodyPartList.entries.Length; partIndex += 1)
            {
                BodyPartEntry part = bodyPartList.entries[partIndex];

                if(part.name == equipedParts[itIndex])
                {
                    Destroy(bodyPartObjects[itIndex]);

                    bodyPartObjects[itIndex] = Instantiate(part.prefabs[itIndex], bodyPartTransforms[itIndex]);

                    break;
                }
            }
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        float time = Time.time;

        if(debugUpdateBodyParts)
        {
            UpdateParts();
        }

        for(int itIndex = 0; itIndex < bodyPartTransforms.Length; itIndex += 1)
        {
            bodyPartTransforms[itIndex].localRotation = Quaternion.identity;
        }

        for(int curIndex = 0; curIndex < currentAnimations.Length; curIndex += 1)
        {
            if(currentAnimations[curIndex].type != AnimationType.NONE)
            {
                for(int animIndex = 0; animIndex < animationData.entries.Length; animIndex += 1)
                {
                    if(animationData.entries[animIndex].type == currentAnimations[curIndex].type)
                    {
                        AnimationEntry anim = animationData.entries[animIndex];
                        
                        for(int itIndex = 0; itIndex < anim.entries.Length; itIndex += 1)
                        {
                            AnimationEntryEntry it = anim.entries[itIndex];

                            float s = time;

                            if(anim.isLooping)
                            {
                                s = ((Mathf.Sin(s * (it.speed * animationSpeed)) * it.multiplier) + 1f) * 0.5f;
                            }
                            else
                            {
                                s = 1f - ((currentAnimations[curIndex].time / anim.lengthInSeconds) * it.multiplier);
                            }

                            Vector3 angle = new Vector3(Mathf.Lerp(it.angleMin.x, it.angleMax.x, s), Mathf.Lerp(it.angleMin.y, it.angleMax.y, s), Mathf.Lerp(it.angleMin.z, it.angleMax.z, s));

                            Quaternion rotation = Quaternion.Euler(it.normal.x * angle.x, it.normal.y * angle.y, it.normal.z * angle.z);

                            for(int partIndex = 0; partIndex < it.parts.Length; partIndex += 1)
                            {
                                bodyPartTransforms[(int)it.parts[partIndex]].localRotation = Quaternion.Slerp(Quaternion.identity, rotation, 1f);
                            }
                        }

                        if(!anim.isLooping)
                        {
                            currentAnimations[curIndex].time += deltaTime * animationSpeed;

                            if(currentAnimations[curIndex].time > anim.lengthInSeconds)
                            {
                                currentAnimations[curIndex].time = 0f;

                                if(currentAnimations[curIndex].type == currentAnimations[curIndex].previousType)
                                {
                                    currentAnimations[curIndex].type = AnimationType.NONE;
                                }
                                else
                                {
                                    currentAnimations[curIndex].type = currentAnimations[curIndex].previousType;
                                }
                            }
                        }

                        break;
                    }
                }
            }
        }
    }
}
