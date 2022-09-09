using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{

    public CharacterController characterController;
    public CharacterCamera characterCamera;


    private const string MouseXInput = "Mouse X";
    private const string MouseYInput = "Mouse Y";
    private const string MouseScrollInput = "Mouse ScrollWheel";
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";


    private const string StickXInput = "Stick X";
    private const string StickYInput = "Stick Y";

    public VisualPlayer visualPlayer;
    public float attackDuration;

    public PlayerInput playerInput;

    public GlobalData globalData;
    public GameObject enemyParent;

    public GameManager gameManager;

    public int killGoal = 10;
    public int numKills = 0;

    public GameObject musicObject;

    public TextMeshProUGUI killCounter;


    // Start is called before the first frame update
    void Start()
    {
        globalData.player = this;
        globalData.Init(transform, enemyParent);

        musicObject = globalData.audioList.PlayMusic(characterController.transform, globalData.startMusicName);

        for(int enemyIndex = 0; enemyIndex < enemyParent.transform.childCount; enemyIndex += 1)
        {
            GameObject enemyObject = enemyParent.transform.GetChild(enemyIndex).gameObject;

            if(enemyObject.TryGetComponent(out Enemy enemy))
            {
                enemy.StartEnemy();
            }
        }

        visualPlayer.globalData = globalData;

        Cursor.lockState = CursorLockMode.Locked;

        visualPlayer.weaponHitboxObject.SetActive(false);

        // Tell camera to follow transform
        characterCamera.SetFollowTransform(characterController.cameraFollowPoint);

        // Ignore the character's collider(s) for camera obstruction checks
        characterCamera.IgnoredColliders.Clear();
        characterCamera.IgnoredColliders.AddRange(characterController.GetComponentsInChildren<Collider>());

        killGoal = enemyParent.transform.childCount;
        killCounter.text = "Kills: " + numKills.ToString() + "/" + killGoal.ToString();
    }

    public void CheckIfWin() {
        numKills = 1 + killGoal - enemyParent.transform.childCount;
        killCounter.text = "Kills: " + numKills.ToString() + "/" + killGoal.ToString();

        if(enemyParent.transform.childCount <= 4)
        {
            for(int itIndex = 0; itIndex < enemyParent.transform.childCount; itIndex += 1)
            {
                Instantiate(globalData.lastSpurtLightPrefab, enemyParent.transform.GetChild(itIndex).GetChild(0));
            }
        }

        if(numKills == killGoal) {
            gameManager.WinGame();

            Destroy(musicObject);

            globalData.audioList.PlaySoundEffect(characterController.transform, "win");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(attackDuration > 0f)
        {
            attackDuration -= Time.deltaTime;

            if(attackDuration <= 0f)
            {
                visualPlayer.weaponHitboxObject.SetActive(false);
            }
        }

        for(int enemyIndex = 0; enemyIndex < enemyParent.transform.childCount; enemyIndex += 1)
        {
            GameObject enemyObject = enemyParent.transform.GetChild(enemyIndex).gameObject;

            if(enemyObject.TryGetComponent(out Enemy enemy))
            {
                enemy.UpdateEnemy(characterController.transform.position);
            }
        }

        /*
        if (Input.GetMouseButtonDown(0)) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        */
        HandleCharacterInput();
    }
    public InputManager inputManager;
    private void HandleCharacterInput() {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        /*
        characterInputs.moveAxisForward = Input.GetAxisRaw(VerticalInput);
        characterInputs.moveAxisRight = Input.GetAxisRaw(HorizontalInput);
        characterInputs.cameraRotation = characterCamera.Transform.rotation;
        characterInputs.jumpDown = Input.GetKeyDown(KeyCode.Space);
        */

        characterInputs.moveAxisForward = inputManager.moveVal.y;
        characterInputs.moveAxisRight = inputManager.moveVal.x;
        characterInputs.cameraRotation = characterCamera.Transform.rotation;
        //characterInputs.jumpDown = Input.GetKeyDown(KeyCode.Space);



        //characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
        //characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

        // Apply inputs to character
        characterController.SetInputs(ref characterInputs);
    }

    private void LateUpdate() {
        // Handle rotating the camera along with physics movers
        if (characterCamera.RotateWithPhysicsMover && characterController.motor.AttachedRigidbody != null) {
            characterCamera.PlanarDirection = characterController.motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * characterCamera.PlanarDirection;
            characterCamera.PlanarDirection = Vector3.ProjectOnPlane(characterCamera.PlanarDirection, characterController.motor.CharacterUp).normalized;
        }

        HandleCameraInput();
    }

    public float cameraSensY = 0.25f;
    public float cameraSensX = 0.5f;

    public float cameraSensMouseY = 0.1f;
    public float cameraSensMouseX = 0.25f;

    public float cameraSensGamepadY = 0.25f;
    public float cameraSensGamepadX = 0.5f;

    private void HandleCameraInput() {
        // Create the look input vector for the camera
        //float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
        //float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
        //Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        float mouseLookAxisUp = inputManager.lookVal.y * cameraSensY;
        float mouseLookAxisRight = inputManager.lookVal.x * cameraSensX;
        Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        // Prevent moving the camera while the cursor isn't locked
        //if (Cursor.lockState != CursorLockMode.Locked) {
        //   lookInputVector = Vector3.zero;
        //}


        //float stickLookAxisUp = Input.GetAxisRaw(StickYInput);
        //float stickLookAxisRight = Input.GetAxisRaw(StickXInput);



        //lookInputVector = new Vector3(stickLookAxisRight, stickLookAxisUp, 0f);

        // Apply inputs to the camera
        characterCamera.UpdateWithInput(Time.deltaTime, 0.0f, lookInputVector);
        /*
        // Handle toggling zoom level
        if (Input.GetMouseButtonDown(1)) {
            characterCamera.TargetDistance = (characterCamera.TargetDistance == 0f) ? characterCamera.DefaultDistance : 0f;
        }
        */
    }

}
