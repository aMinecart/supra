using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody rb; // player rigidbody
    public LayerMask terrain_layers; // environment box collider

    [HideInInspector] public Vector2 rot;
    private float sens = 0.1f;

    private Vector2 move_force;
    private bool grounded_status;
    private bool jump_status;
    private bool can_jump;

    // based off of TF2's 100% movement speed values for now
    private float max_speed = 5.715f; // 1 foot = 16 Hammer Units, 1 foot = 0.3048 meters
    private float max_airspeed = 0.5715f;

    private float acceleration = 5.6f;
    private float air_acceleration;

    private float jump_velocity = 5.111651396549759f; // using 268.3281572999747f in HU for now
    private float friction = 4.8f;
    private float gravity = 15.24f; // 800 HU^2 per second

    private Vector3 applyFriction (Vector3 curr_velocity, float friction)
    {
        float speed = curr_velocity.magnitude;
        if (speed != 0)
        {
            float reduction = speed * friction * Time.fixedDeltaTime;
            curr_velocity *= Mathf.Max(speed - reduction, 0) / speed;
        }

        return curr_velocity;
    }

    private Vector3 applyGravity(Vector3 curr_velocity, float gravity_acceleration)
    {
        curr_velocity.y -= gravity_acceleration * Time.fixedDeltaTime;
        return curr_velocity;
    }

    private Vector3 applyGravity(Vector3 curr_velocity, float gravity_acceleration, float gravity_multiplier)
    {
        curr_velocity.y -= gravity_acceleration * gravity_multiplier * Time.fixedDeltaTime;
        return curr_velocity;
    }

    private Vector3 accelerate (Vector3 curr_velocity, float max_velocity, Vector3 accel_dir, float accel_rate)
    {
        float proj_velocity = Vector3.Dot(curr_velocity, accel_dir);
        // float accel_velocity = accel_rate * Time.fixedDeltaTime;
        float accel_velocity = accel_rate * max_velocity * Time.fixedDeltaTime;

        if (proj_velocity + accel_velocity > max_velocity)
        {
            accel_velocity = Mathf.Max(max_velocity - proj_velocity, 0);
        }

        Vector3 new_velocity = curr_velocity + accel_velocity * accel_dir;
        return new_velocity;
    }

    // Start is called before the first frame update
    void Start()
    {
        air_acceleration = acceleration * 10f;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnLook(InputValue look_value)
    {
        rot += look_value.Get<Vector2>() * sens;
    }

    private void OnMove(InputValue movement_value)
    {
        move_force = movement_value.Get<Vector2>();
    }

    private void OnJump()
    {
        jump_status = true;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        grounded_status = true;
        can_jump = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0.0f, rot.x, 0.0f);


        Vector3 move_direction = new Vector3(move_force.x, 0.0f, move_force.y);
        move_direction = transform.rotation * move_direction;

        if (grounded_status)
        {
            rb.velocity = applyFriction(rb.velocity, friction);
            rb.velocity = accelerate(rb.velocity, max_speed, move_direction, acceleration);
        }
        else
        {
            rb.velocity = accelerate(rb.velocity, max_airspeed, move_direction, air_acceleration);
        }

        if (jump_status && can_jump)
        {
            rb.velocity = new Vector3(rb.velocity.x, jump_velocity, rb.velocity.z);
            grounded_status = false;
            can_jump = false;
        }

        rb.velocity = applyGravity(rb.velocity, gravity);

        jump_status = false;
    }
}