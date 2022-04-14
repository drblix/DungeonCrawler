using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject arrow;

    [SerializeField] [Min(2f)]
    private float moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal * Time.deltaTime * moveSpeed, vertical * Time.deltaTime * moveSpeed);

        rb.MovePosition(rb.position + movement);
    }
}
