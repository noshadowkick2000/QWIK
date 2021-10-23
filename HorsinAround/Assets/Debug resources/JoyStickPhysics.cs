using System;
using UnityEngine;
using UnityEngine.UI;

namespace Debug_resources
{
  public class JoyStickPhysics : MonoBehaviour
  {
    [SerializeField] private float maxOffset;

    [SerializeField] private float centerForce;
    [SerializeField] private float mass;
    [SerializeField] private float springConstant;
    [SerializeField] private float dampingConstant;
    [SerializeField] private float timeFactor;

    [SerializeField] private float offsetThreshold;

    [SerializeField] private Transform joyStick;
    private Vector3 velocity = new Vector3();

    private bool dragging = false;

    [SerializeField] private SpriteRenderer joyStickImage;
    [SerializeField] private Sprite depressedSprite;
    [SerializeField] private Sprite pressedSprite;

    private void Awake()
    {
      joyStick.transform.localPosition = Vector3.zero;
    }

    public void SetDragging(bool isDragging)
    {
      dragging = isDragging;

      joyStickImage.sprite = isDragging ? pressedSprite : depressedSprite;
    }

    public void DragJoyStick(float x, float y)
    {
      joyStick.localPosition += new Vector3(x, y, 0);
      if (joyStick.localPosition.magnitude > maxOffset)
        joyStick.localPosition = joyStick.localPosition.normalized * maxOffset;
    }

    private void Update()
    {
      if (dragging || (velocity.magnitude < offsetThreshold &&
                       joyStick.localPosition.magnitude < offsetThreshold)) return;

      Vector3 gravityDirection = (Vector3.zero - joyStick.localPosition).normalized;

      Vector3 springForce = -springConstant * -(gravityDirection);
      Vector3 dampingForce = dampingConstant * velocity;
      Vector3 totalForce = springForce + (mass * centerForce * gravityDirection) - dampingForce;
      Vector3 acceleration = totalForce / mass;
      velocity = velocity + acceleration * timeFactor;

      joyStick.localPosition = joyStick.localPosition + velocity * timeFactor;
    }
  }
}