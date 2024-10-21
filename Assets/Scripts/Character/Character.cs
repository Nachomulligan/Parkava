//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static GameManager;

//public class WalkController : IMovementController
//{
//    private float movementSpeed;
//    private Transform transform;
//    private Rigidbody rb;
//    private float jumpForce;
//    private Transform groundPoint;
//    private bool isGrounded = true;
//    private LayerMask groundLayer;
//    private float groundRadius;
//    private float coyoteTime;
//    public float coyoteTimeCounter;
//    [SerializeField] private int maxJumpCount = 2;  // Número máximo de saltos permitidos
//    private int currentJumpCount = 0;

//    public WalkController(float movementSpeed, Transform transform, Rigidbody rb, float jumpForce,
//        Transform groundPoint, float groundRadius, LayerMask groundLayer, float coyoteTime, float coyoteTimeCounter)
//    {
//        this.movementSpeed = movementSpeed;
//        this.transform = transform;
//        this.rb = rb;
//        this.jumpForce = jumpForce;
//        this.groundPoint = groundPoint;
//        this.groundLayer = groundLayer;
//        this.groundRadius = groundRadius;
//        this.coyoteTimeCounter = 0;
//        this.coyoteTime = coyoteTime;
//    }

//    public void Move(Vector3 playerInput)
//    {
//        transform.position += playerInput * Time.deltaTime * movementSpeed;
//        if (isGrounded)
//        {
//            coyoteTimeCounter = coyoteTime;
//            currentJumpCount = 0; // Reiniciar el contador de saltos cuando está en el suelo
//        }
//        else
//        {
//            coyoteTimeCounter -= Time.deltaTime;
//            Debug.Log("IsGrounded: " + isGrounded);
//        }
//    }

//    public void Jump()
//    {
//        if (IsGrounded() || coyoteTimeCounter > 0)
//        {
//            if (currentJumpCount < maxJumpCount)
//            {
//                Debug.Log("intento saltar");
//                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//                currentJumpCount++;
//                coyoteTimeCounter = 0;
//            }
//        }
//    }

//    private bool IsGrounded()
//    {
//        isGrounded = Physics.CheckSphere(groundPoint.position, groundRadius, groundLayer);
//        return isGrounded;
//    }
//}


//public class FlyController : IMovementController
//{
//    private float movementSpeed;
//    private Transform transform;

//    public FlyController(float movementSpeed, Transform transform)
//    {
//        this.movementSpeed = movementSpeed;
//        this.transform = transform;
//    }
//    public void Move(Vector3 playerInput)
//    {
//        var forwards = transform.forward * playerInput.y;
//        transform.position += forwards * Time.deltaTime * movementSpeed;
//    }

//    public void Jump()
//    {

//    }
//}

//public interface IMovementController
//{
//    void Move(Vector3 playerInput);
//    void Jump();
//}

//[System.Serializable]
//public struct MovementControllerConfig
//{
//    public float movementSpeed;
//}

//public interface IFly
//{
//    void SetFlyState();
//}

//public interface IWalk
//{
//    void SetWalkState();
//}

//public class Character : MonoBehaviour, IDamageable, IFly, IWalk
//{
//    [SerializeField] private float movementSpeed;
//    [SerializeField] private float rotationSpeed = 10f;
//    [SerializeField] private Transform interactionPoint;
//    [SerializeField] private float interactionRadius;
//    [SerializeField] private LayerMask interactionLayer;
//    [SerializeField] private float maxHealth;
//    [SerializeField] private MovementControllerConfig flyConfig;
//    [SerializeField] private MovementControllerConfig walkConfig;
//    [SerializeField] private float jumpForce;
//    [SerializeField] private Rigidbody rb;
//    [SerializeField] private LayerMask groundLayer;
//    [SerializeField] private Transform groundPoint;
//    [SerializeField] private float groundRadius;
//    [SerializeField] private float coyoteTime;
//    [SerializeField] private float coyoteTimeCounter;

//    private IMovementController currentMovementController;
//    private WalkController walkController;
//    private FlyController flyController;

//    private float currentHealth;

//    private void Awake()
//    {
//        currentHealth = maxHealth;
//        walkController = new WalkController(walkConfig.movementSpeed, transform, rb, jumpForce, groundPoint,
//            groundRadius, groundLayer, coyoteTime, coyoteTimeCounter);
//        flyController = new FlyController(flyConfig.movementSpeed, transform);

//        currentMovementController = walkController;
//    }

//    private Collider[] interactables = new Collider[5];

//    public void Update()
//    {
//        if (GameManager.Instance.GetCurrentState() != GameState.Gameplay)
//            return;

//        RotateCharacter();
//        MoveCharacter();

//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            TryInteract();
//        }

//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            currentMovementController.Jump();
//        }
//    }

//    private void RotateCharacter()
//    {
//        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
//        transform.Rotate(0, mouseX, 0); // Rotamos el personaje sobre el eje Y
//    }

//    private void MoveCharacter()
//    {
//        var horizontal = Input.GetAxisRaw("Horizontal");
//        var vertical = Input.GetAxisRaw("Vertical");

//        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
//        currentMovementController.Move(direction.normalized);
//    }

//    public void SetWalkState()
//    {
//        currentMovementController = walkController;
//    }

//    public void SetFlyState()
//    {
//        currentMovementController = flyController;
//    }

//    public void TakeDamage(float damage)
//    {
//        currentHealth -= damage;
//        if (currentHealth < 0)
//        {
//            Die();
//        }
//    }

//    private void Die()
//    {
//        Destroy(gameObject);
//    }

//    private void TryInteract()
//    {
//        Debug.Log("Tried interacting");

//        int elements = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionRadius,
//            interactables, interactionLayer);
//        if (elements == 0)
//        {
//            Debug.Log("No interactables found");
//            return;
//        }

//        for (int i = 0; i < elements; i++)
//        {
//            var interactable = interactables[i];

//            var interactableComponent = interactable.GetComponent<IInteractable>();

//            if (interactableComponent != null)
//            {
//                interactableComponent.Interact();
//                return;
//            }
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);

//        Gizmos.color = Color.blue;
//        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
//    }
//}
