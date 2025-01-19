using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5f;                // Base walking speed
    public float sprintMultiplier = 1.5f;  // Multiplier for sprinting speed
    public float acceleration = 5f;        // Rate of acceleration
    [SerializeField] private Rigidbody rb; // Reference to the player's Rigidbody
    [SerializeField] private bool isSprinting = false; // Whether the player is sprinting
    [SerializeField] private float stamina = 200f; // Player's stamina
    [SerializeField] private Animator animator; // Reference to the player's Animator
    [SerializeField] private Slider staminaSlider; // Reference to the stamina slider
    
    //AudioSources for running and walking
    [SerializeField] private AudioSource running;
    [SerializeField] private AudioSource walking;
    

    private float _targetSpeed = 0f;      // Desired speed based on input
    private float _currentSpeed = 0f;    // Current speed (accelerates toward _targetSpeed)
    private bool _disableSprint = false; // Whether sprinting is disabled
    private float _initialStamina;
    private float _xAxis = 0f;
    private float _zAxis = 0f;
    public bool frozen = false;


    void Start()
    {
        _initialStamina = stamina;
        staminaSlider.maxValue = _initialStamina;
        _targetSpeed = 0f;
    }

    void FixedUpdate()
    {
        if (frozen) return;
        
        // Play the appropriate audio clip
        if (_currentSpeed > 0)
        {
            if (isSprinting)
            {
                if (!running.isPlaying)
                {
                    running.Play();
                    walking.Stop();
                }
            }
            else
            {
                if (!walking.isPlaying)
                {
                    walking.Play();
                    running.Stop();
                }
            }
        }
        else
        {
            walking.Stop();
            running.Stop();
        }
        
        // Calculate movement input magnitude
        bool isMoving = _xAxis != 0 || _zAxis != 0;

        // Determine the target speed based on input and sprinting state
        _targetSpeed = isMoving ? (isSprinting ? speed * sprintMultiplier : speed) : 0f;
        
        // Decrease stamina if sprinting
        if (isSprinting)
        {
            stamina -= 35f * Time.fixedDeltaTime;
            if (stamina <= 0)
            {
                isSprinting = false;
                _disableSprint = true;
            }
        }
        else
        {
            stamina += 25f * Time.fixedDeltaTime;
            if (stamina > _initialStamina)
            {
                stamina = _initialStamina;
                _disableSprint = false;
            }
        }
        //update stamina slider
        staminaSlider.value = stamina;

        // Gradually adjust the current speed toward the target speed
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, acceleration * Time.fixedDeltaTime);

        // Calculate the movement vector
        Vector3 movement = new Vector3(_xAxis, 0, _zAxis).normalized * _currentSpeed;
        
        // Update the Animator with the current speed
        animator.SetFloat("velocity", _currentSpeed);

        // Move the Rigidbody
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        //Change rotation of player to 270 degrees if it goes left on x axis
        if (_xAxis < 0)
        {
            transform.rotation = Quaternion.Euler(-90, 90, 90);
        }else if (_xAxis > 0)
        {
            transform.rotation = Quaternion.Euler(90, 90, 90);
        }
    }

    #region Movement

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        _xAxis = input.x;
        _zAxis = input.y;
    }

    public void OnSprint(InputValue value)
    {
        if (_disableSprint) return;
        isSprinting = value.isPressed;
    }
    
    public void FreezeMovement()
    {
        frozen = true;
        //Stop all sounds
        walking.Stop();
        running.Stop();
    }
    
    public void UnfreezeMovement()
    {
        frozen = false;
    }

    #endregion
}
