using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float scareRadius = 24f;
    public string bodyPartSet;

    public float speed = 12f;
    public float turnSpeed = 4f;

    public CharacterController characterController;
    public VisualPlayer visualPlayer;

    public PlayerCharacterInputs inputs;



    [HideInInspector]
    public GlobalData globalData;

    // Start is called before the first frame update
    public void StartEnemy()
    {
        Destroy(visualPlayer.weaponHitboxObject);
        Destroy(visualPlayer.weaponObject);

        if(bodyPartSet.Length <= 0)
        {
            bodyPartSet = "dennis";
        }

        visualPlayer.ChangeBodyPart(BodyPart.HEAD, bodyPartSet);
        visualPlayer.ChangeBodyPart(BodyPart.TORSO, bodyPartSet);
        visualPlayer.ChangeBodyPart(BodyPart.RIGHT_ARM, bodyPartSet);
        visualPlayer.ChangeBodyPart(BodyPart.LEFT_ARM, bodyPartSet);
        visualPlayer.ChangeBodyPart(BodyPart.RIGHT_LEG, bodyPartSet);
        visualPlayer.ChangeBodyPart(BodyPart.LEFT_LEG, bodyPartSet);

        characterController.orientationMethod = OrientationMethod.towardsMovement;
    }

    // Update is called once per frame
    public void UpdateEnemy(Vector3 playerPosition)
    {
        if(transform.position.y < -8192f)
        {
            Destroy(gameObject);
        }

        float distanceToPlayer = Vector3.Distance(playerPosition, characterController.transform.position);

        if(distanceToPlayer <= scareRadius)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, playerPosition - characterController.transform.position, Vector3.up);

            inputs.cameraRotation = Quaternion.Lerp(inputs.cameraRotation, Quaternion.AngleAxis(angle, Vector3.up), Time.deltaTime * turnSpeed);

            float s = ((speed * Mathf.Clamp(1f - (distanceToPlayer / scareRadius), 0f, 0.5f)));
            
            inputs.moveAxisForward = -s;

            Ray ray = new Ray(characterController.transform.position + new Vector3(0f, 1f, 0f), characterController.transform.forward);
            if(Physics.Raycast(ray))
            {
                inputs.moveAxisRight = s * (Mathf.Sin(Time.time * 3f) * 2f);
            }

            visualPlayer.SetSecondaryAnimation(AnimationType.SCARED);
        }
        else
        {
            inputs.moveAxisForward = 0f;
            inputs.moveAxisRight = 0f;

            visualPlayer.SetSecondaryAnimation(AnimationType.NONE);
        }

        characterController.SetInputs(ref inputs);
    }

    public GameObject loot;

    public GameObject bloodSplatter;

    public bool isTutorial = false;
    public GameObject tutorialBush;

    public void DropLoot() {
        Instantiate(bloodSplatter, transform.GetChild(0).position, transform.GetChild(0).rotation);

        if(isTutorial == true) {
            if(tutorialBush != null) {
                Destroy(tutorialBush);
            }
            
        }



        if (loot != null) {
            
            Instantiate(loot, transform.GetChild(0).position, transform.GetChild(0).rotation);
        }
    }


}
