using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartManager : MonoBehaviour
{

    public VisualPlayer visualPlayer;
    public InfoPanel infoPanel;
    public CharacterController characterController;
    public InputManager inputManager;


    public List<Part> collectedHeads = new List<Part>();
    public List<Part> collectedTorsos = new List<Part>();
    public List<Part> collectedArms = new List<Part>();
    public List<Part> collectedLegs = new List<Part>();


    private int currentHeadIndex = 0;
    private int currentTorsoIndex = 0;
    private int currentArmsIndex = 0;
    private int currentLegsIndex = 0;

    public Part activeHead;
    public Part activeTorso;
    public Part activeArms;
    public Part activeLegs;

    public float totalSpeedModifier = 0.0f;
    public float totalWeightModifier = 0.0f;
    public float totalJumpHeightModifier = 0.0f;
    public float totalLongJumpModifier = 0.0f;


    public void ChangeHead() {
        currentHeadIndex++;
        
        if(currentHeadIndex >= collectedHeads.Count) {
            currentHeadIndex = 0;
        }

        ActivatePart(collectedHeads[currentHeadIndex]);
    }

    public void ChangeTorso() {
        currentTorsoIndex++;

        if (currentTorsoIndex >= collectedTorsos.Count) {
            currentTorsoIndex = 0;
        }

        ActivatePart(collectedTorsos[currentTorsoIndex]);
    }

    public void ChangeArms() {
        currentArmsIndex++;

        if (currentArmsIndex >= collectedArms.Count) {
            currentArmsIndex = 0;
        }

        ActivatePart(collectedArms[currentArmsIndex]);
    }

    public void ChangeLegs() {
        currentLegsIndex++;

        if (currentLegsIndex >= collectedLegs.Count) {
            currentLegsIndex = 0;
        }


        ActivatePart(collectedLegs[currentLegsIndex]);
    }


    public void ActivatePart(Part _part) {
        if (_part.partType == Part.PartType.Head) {
            visualPlayer.equipedParts[0] = _part.partName.ToLower();
            activeHead = _part;
        } else if (_part.partType == Part.PartType.Torso) {
            visualPlayer.equipedParts[1] = _part.partName.ToLower();
            activeTorso = _part;
        } else if (_part.partType == Part.PartType.Arms) {
            visualPlayer.equipedParts[2] = _part.partName.ToLower();
            visualPlayer.equipedParts[3] = _part.partName.ToLower();
            activeArms = _part;
        } else if (_part.partType == Part.PartType.Legs) {
            visualPlayer.equipedParts[5] = _part.partName.ToLower();
            visualPlayer.equipedParts[4] = _part.partName.ToLower();
            activeLegs = _part;
        }

        visualPlayer.UpdateParts();
        infoPanel.ShowTitle(_part.partName, _part.partInfo, _part.partType.ToString());
        ApplyModifiers();
    }


    public void ApplyModifiers() {
        totalSpeedModifier = activeHead.speedModifier + activeTorso.speedModifier + activeArms.speedModifier + activeLegs.speedModifier;
        totalWeightModifier = activeHead.weightModifier + activeTorso.weightModifier + activeArms.weightModifier + activeLegs.weightModifier;
        totalJumpHeightModifier = activeHead.heightJumpModifier + activeTorso.heightJumpModifier + activeArms.heightJumpModifier + activeLegs.heightJumpModifier;
        totalLongJumpModifier = activeHead.longJumpModifier + activeTorso.longJumpModifier + activeArms.longJumpModifier + activeLegs.longJumpModifier;

        characterController.speedModifier = totalSpeedModifier;
        characterController.weightModifier = totalWeightModifier;
        characterController.jumpHeightModifier = totalJumpHeightModifier;
        characterController.longJumpModifier = totalLongJumpModifier;

        if(activeHead.hasDoubleJump || activeTorso.hasDoubleJump || activeArms.hasDoubleJump ||activeLegs.hasDoubleJump) {
            inputManager.hasDoubleJump = true;
        } else {
            inputManager.hasDoubleJump = false;
        }

        float maxGlideTime = 0.0f;
        if(activeHead.hasGlide) {
            maxGlideTime += 1.0f;
        }
        if (activeTorso.hasGlide) {
            maxGlideTime += 1.0f;
        }
        if (activeArms.hasGlide) {
            maxGlideTime += 1.0f;
        }
        if (activeLegs.hasGlide) {
            maxGlideTime += 1.0f;
        }
        characterController.maxGlideTime = maxGlideTime;

        if (activeHead.hasGlide || activeTorso.hasGlide || activeArms.hasGlide || activeLegs.hasGlide) {
            inputManager.hasGlide = true;
        } else {
            inputManager.hasGlide = false;
        }

    }

}
