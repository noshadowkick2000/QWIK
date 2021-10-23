using Server.Board_Scene;
using UnityEngine;

namespace Server.Global
{
  public class PlayerController : MonoBehaviour
  {
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
  
    private Rigidbody rb;
    private bool focused;
    private Transform focusedTransform;
    private PlayerAttack playerAttack;

    private void Awake()
    {
      rb = GetComponent<Rigidbody>();
      playerAttack = GetComponent<PlayerAttack>();
    }

    private void FixedUpdate()
    {
      HandleMovement();
    }

    private void Update()
    {
      HandleAttack();
    }

    private void HandleAttack()
    {
      if (Input.GetButtonDown("Fire1"))
      {
        playerAttack.DebugAttack();
      }
    }

    private void HandleMovement()
    {
      float x = Input.GetAxis("Horizontal");
      float y = Input.GetAxis("Vertical");

      Vector3 addPosition = Time.deltaTime * movementSpeed * new Vector3(x, 0, y).normalized;
      rb.MovePosition(transform.position + addPosition);

      SetRotation(x, y);
    }

    private void SetRotation(float x, float y)
    {
      if (focused)
      {
        Vector3 direction = transform.position - focusedTransform.position;
        direction.y = 0;
        direction.Normalize();
        Quaternion rotategoal = Quaternion.LookRotation(direction.normalized, Vector3.up);
        rb.rotation = (rotategoal);
      }
      else
      {
        //rigidbody.rotation = rigidbody.rotation * Quaternion.Euler(0, -CameraRotation, 0);

        float angle = Mathf.Atan2(x, y);
        angle = angle * Mathf.Rad2Deg;
        Quaternion rotateGoal = Quaternion.Euler(0.0f, angle + 180, 0.0f);
        rotateGoal = Quaternion.Slerp(rb.rotation, rotateGoal, rotationSpeed);
        if (x != 0 || y != 0)
        {
          rb.MoveRotation(rotateGoal);
        }

        //rigidbody.rotation = rigidbody.rotation * Quaternion.Euler(0, CameraRotation, 0);
      }
    }
  }
}