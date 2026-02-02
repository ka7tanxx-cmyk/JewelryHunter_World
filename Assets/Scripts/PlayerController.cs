using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
        float axisH = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Rigidbody2D‚ðŽæ‚Á‚Ä‚­‚é
        rbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        axisH = Input.GetAxisRaw("Horizontal");
    }
    private void FixedUpdate()
    {
        rbody.linearVelocity = new Vector2(axisH * 3.0f, rbody.linearVelocity.y);
    }
}
