using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_TPSController : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    private Animator animator;

    private EventController eventController;
    

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = false;

    private bool infected = false;
    private int numFuckups = 0;

    void Start()
    {
        eventController = GameObject.Find("Canvas").GetComponent<EventController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }

        //Update animations
        bool moving = moveDirection.magnitude != 0.0f;
        animator.SetBool("Moving", moving);
    }

    public void Activate() {
        Camera camera_b = GameObject.Find("Camera").GetComponent<Camera>();
        camera_b.enabled = true;
        
        //Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canMove = true;
    }

     void OnTriggerEnter(Collider other) {
        Wander wanderer = other.gameObject.GetComponent<Wander>();
        if (wanderer != null){
            numFuckups += 1;
            if (wanderer.infected) {
                infected = true;
            }
        }

        Pickups pickup = other.gameObject.GetComponent<Pickups>();
        if (pickup != null) {
            eventController.collected(pickup.type);
            pickup.gameObject.SetActive(false);
        } 

        GameExit exit = other.gameObject.GetComponent<GameExit>();
        if (exit != null) {
            canMove = false;

            Wander[] wanderers = FindObjectsOfType<Wander>();

            foreach (Wander obj in wanderers) {
                obj.Deactivate();
            }

            eventController.dialogue_stage = 69;
            eventController.numFuckups = numFuckups;
            eventController.infected = infected;
            Camera camera_b = GameObject.Find("Camera").GetComponent<Camera>();
            camera_b.enabled = false;
            eventController.next_dialogue(0);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } 
    }
}