using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BodyPart
{
    HEAD,
    TORSO,
    RIGHT_ARM,
    LEFT_ARM,
    RIGHT_LEG,
    LEFT_LEG,
};

public enum AnimationType
{
    NONE,
    IDLE,
    WALKING,
    ATTACK,
    JUMP,
    FALLING,
    JUMP_SQUAT,
    SCARED,
};

[System.Serializable]
public struct AnimationEntryEntry
{
    public BodyPart[] parts;

    public Vector3 normal;
    public Vector3 angleMin;
    public Vector3 angleMax;
    public float speed;
    public float multiplier;
};

[System.Serializable]
public struct AnimationEntry
{
    public AnimationType type;
    // public string name;
    public bool isLooping;
    public float lengthInSeconds;
    public AnimationEntryEntry[] entries;
};


[CreateAssetMenu(fileName = "New PlayerAnimation", menuName = "Data/Animation"), System.Serializable]
public class PlayerAnimation : ScriptableObject
{
    public AnimationEntry[] entries;
}
