
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CollisionInfo), typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    private Rigidbody cached_RB;
    private Rigidbody RB => cached_RB ??= GetComponent<Rigidbody>();

    private CollisionInfo cached_CI;
    private CollisionInfo CI => cached_CI ??= GetComponent<CollisionInfo>();


    private GameplayControlsInput cached_GameplayControlsInput;
    private GameplayControlsInput GameplayControlsInput => cached_GameplayControlsInput ??= GetComponentInParent<GameplayControlsInput>();

    private Vector2 RawMovementInput = new(0, 0);

    [SerializeField] private Transform cameraTransform;
    private Vector3 WorldMovementDirection => (Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * RawMovementInput._x0y()).normalized;

    [Header("Base Speed"), Tooltip("the base speed for the controller")]
    [SerializeField] private float speed = 15f;

    [Header("Grounded")]
    [SerializeField, Tooltip("Acceleration, relative to base speed")] private float Acceleration = 2f;
    [SerializeField, Tooltip("Decceleration, relative to base speed")] private float Decceleration = 3.3333f;

    [Header("GroundedAirborn")]
    [SerializeField, Tooltip("Acceleration while Airborne, relative to base speed")] private float AirAcceleration = 1;
    [SerializeField, Tooltip("Decceleration while Airborne, relative to base speed")] private float AirDecceleration = 1.3333f;

    [Header("Sliding")]
    [SerializeField, Tooltip("Acceleration while sliding, relative to base speed")] private float SlopeAcceleration = 1;
    [SerializeField, Tooltip("Decceleration while sliding, relative to base speed")] private float SlopeDecceleration = 1.3333f;
    
    [SerializeField, Tooltip("Downhill acceleration, relative to base speed")] private float SlopeDownwardsAcceleration = 1f;
    [SerializeField, Tooltip("Max downhill speed, relative to base speed")] private float slopeDownwardsSpeed = 1.6666f;

    void FixedUpdate()
    {
        RB.useGravity = !CI.Grounded;
        DoMovemement(speed);
    }

    void OnEnable() => GameplayControlsInput.Controls.Motion.Move.AddCallback(SetInput);
    void OnDisable() => GameplayControlsInput.Controls.Motion.Move.RemoveCallback(SetInput);
    private void SetInput(InputAction.CallbackContext callbackContext) => RawMovementInput = callbackContext.ReadValue<Vector2>();

    void DoMovemement(float speed)
    {
        var acceleration = CI.FlatGround ? Acceleration : CI.Grounded ? SlopeAcceleration : AirAcceleration;
        var decceleration = CI.FlatGround ? Decceleration : CI.Grounded ? SlopeDecceleration : AirDecceleration;
        var movementDirection = WorldMovementDirection;

        var slopeDownVector = Vector3.zero;
        var DownhillComponent = Vector3.zero;
        if (CI.Grounded)
        {
            var rot = Quaternion.FromToRotation(Vector3.up, CI.ContactNormal);
            movementDirection = rot * movementDirection;
            if (!CI.FlatGround)
            {
                var rotAxis = Vector3.Cross(Vector3.up, CI.ContactNormal);
                slopeDownVector = Quaternion.AngleAxis(90f, rotAxis) * CI.ContactNormal;
                if (slopeDownVector.y > 0) slopeDownVector = -slopeDownVector;
                DownhillComponent = Mathf.Max(0, Vector3.Dot(RB.velocity, slopeDownVector)) * slopeDownVector;
            }
        }

        var ForwardComponent = Mathf.Max(0, Vector3.Dot(RB.velocity - DownhillComponent, movementDirection)) * movementDirection;
        var NonForwardComponent = RB.velocity - DownhillComponent - ForwardComponent;

        //Accelerate Downhill until SlopeDownwardsSpeed is reached, then deccelerate to maintain downhillspeed
        DownhillComponent = Vector3.MoveTowards(DownhillComponent, slopeDownVector * speed * slopeDownwardsSpeed, speed * slopeDownwardsSpeed * SlopeDownwardsAcceleration * Time.fixedDeltaTime);

        //Accelerate movement towards MovementDirection or stay at higher movement
        if (ForwardComponent.sqrMagnitude < speed * speed)
            ForwardComponent = Vector3.MoveTowards(ForwardComponent, movementDirection * speed, speed * acceleration * Time.fixedDeltaTime);

        //Decellerate movement that doesnt go towards current MovementDirection
        NonForwardComponent = Vector3.MoveTowards(NonForwardComponent, Vector3.zero, speed * decceleration * Time.fixedDeltaTime);
        if (!CI.Grounded) RB.velocity = ForwardComponent + DownhillComponent + NonForwardComponent._x0z() + RB.velocity._0y0();
        else RB.velocity = ForwardComponent + NonForwardComponent + DownhillComponent;
    }
}
