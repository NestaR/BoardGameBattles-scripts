using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed = 5f;             // The speed at which the object moves
    public float deleteX = -20f;
    bool reset;     // The speed at which the background scrolls
    public float resetX = -20f;          // The x coordinate at which the background resets

    private float startX;                // The starting x coordinate of the background

    private void Start()
    {
        startX = transform.position.x;   // Store the starting x coordinate of the background
    }

    private void Update()
    {
        // Move the background from right to left
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Check if the background has reached the reset x coordinate
        if (transform.position.x <= resetX)
        {
            ResetBackground();            // Reset the background to its starting position
        }
    }

    private void ResetBackground()
    {
        // Calculate the offset needed to reset the background
        float offset = Mathf.Abs(resetX - startX);

        // Move the background to the reset position
        transform.position += Vector3.right * offset;
    }
}
