using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField] private float speedMultiplier;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float horizontalSpeed = Input.GetAxis("Horizontal") * speedMultiplier;
        float verticalSpeed = Input.GetAxis("Vertical") * speedMultiplier;
        rb2d.velocity = new Vector2(Mathf.Lerp(0, horizontalSpeed, 0.8f),
                                    Mathf.Lerp(0, verticalSpeed, 0.8f));
    }

    private void Update()
    {
    }
}