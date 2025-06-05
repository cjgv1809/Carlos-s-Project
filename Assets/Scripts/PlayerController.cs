using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocity = 5f;
    public float jumpForce = 10f;
    public float bounceForce = 8f;
    public float raycastLength = 0.1f;
    public LayerMask layerMask;
    private bool isOnGround;
    private bool isReceivingDamage;
    private bool isAttacking;
    private Rigidbody2D rb;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            Moving();
            Jumping();
        }

        if (isOnGround && !isAttacking && Input.GetKeyDown(KeyCode.F))
        {
            Attacking();
        }

        animator.SetBool("isOnGround", isOnGround);
        animator.SetBool("isReceivingDamage", isReceivingDamage);
        animator.SetBool("isAttacking", isAttacking);
    }

    public void Moving()
    {
        float velocityX = Input.GetAxis("Horizontal") * Time.deltaTime * velocity;

        animator.SetFloat("movement", velocityX * velocity);

        if (velocityX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (velocityX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        Vector3 position = transform.position;

        if (!isReceivingDamage) transform.position = new Vector3(velocityX + position.x, position.y, position.z);
    }

    public void Jumping()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, layerMask);
        isOnGround = hit.collider != null;

        if (isOnGround && Input.GetKeyDown(KeyCode.Space) && !isReceivingDamage)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void ReceivingDamage(Vector2 direction, int damageAmount)
    {
        if(!isReceivingDamage)
        {
            isReceivingDamage = true;
            Vector2 bounce = new Vector2(transform.position.x - direction.x, 0.2f).normalized;
            rb.AddForce(bounce * bounceForce, ForceMode2D.Impulse);
        }
    }

    public void DeactivateDamage()
    {
        isReceivingDamage = false;
        rb.velocity = Vector2.zero;
    }

    public void Attacking()
    {
        isAttacking = true;
    }

    public void DeactivateAttack()
    {
        isAttacking = false;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLength);
    }
}
