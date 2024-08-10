using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControls : MonoBehaviour
{
    public float speed = 5f;
    public float direction = 1;
    public float jumpForce = 2f;
    public GatherInput gatherInput;
    public Rigidbody2D rigidbody2D;
    public Animator animator;
    public float rayLength;
    public LayerMask groundLayer;
    public Transform leftPoint;
    private bool Grounded = false;
    private int jumpCount = 0; // ตัวนับจำนวนครั้งที่กระโดด
    public int maxJumps = 2; // จำนวนครั้งที่สามารถกระโดดได้ต่อการลงพื้น 1 ครั้ง

    // Start is called before the first frame update
    void Start()
    {
        gatherInput = GetComponent<GatherInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    private void SetAnimatorValues() 
    {
        animator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        animator.SetFloat("vSpeed", rigidbody2D.velocity.y);
        animator.SetBool("Grounded", Grounded);
    }

    private void CheckStatus() {
        RaycastHit2D leftCheckHit = Physics2D.Raycast(leftPoint.position, Vector2.down, rayLength, groundLayer);
        Grounded = leftCheckHit;

        if (Grounded)
        {
            jumpCount = 1; // รีเซ็ตตัวนับการกระโดดเมื่ออยู่บนพื้น
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimatorValues();
        rigidbody2D.velocity = new Vector2(speed * gatherInput.valueX, rigidbody2D.velocity.y);
    }

    void FixedUpdate()
    {
        Flip();
        move();
        JumpPlayer();
        CheckStatus();
    }

    private void move(){
        rigidbody2D.velocity = new Vector2(speed * gatherInput.valueX, rigidbody2D.velocity.y);
    }

    private void JumpPlayer() 
    {
        if (gatherInput.jumpInput && jumpCount < maxJumps)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            jumpCount++;
        }
        gatherInput.jumpInput = false;
    }

    private void Flip() 
    { 
        if(gatherInput.valueX * direction < 0) 
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            direction *= -1;
        }
    }
}
