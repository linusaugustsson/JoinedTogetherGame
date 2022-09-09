using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{

    public enum PartType {
        Head,
        Torso,
        Arms,
        Legs
    }

    public PartType partType = PartType.Head;

    public string partName = "";
    public string partInfo = "";

    public float speedModifier = 0.0f;
    public float weightModifier = 0.0f;
    public float longJumpModifier = 0.0f;
    public float heightJumpModifier = 0.0f;

    public bool hasGlide = false;
    public bool hasDoubleJump = false;
    



}
