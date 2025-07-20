using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Player Detection")]
    public Transform player;
    public float detectionRadius = 5f;

    [Header("Movement")]
    public float speed = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Fall Settings")]
    public float fallThresholdY = -10f;

    [Header("Health")]
    public int health = 2;

    [Header("Knockback")]
    public float knockbackForce = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool isMoving;
    private bool isDead = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Auto-asignar player si no está en el inspector
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        HandleMovement();
        CheckFallOffMap();
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (distanceToPlayer < detectionRadius && isGroundAhead)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.localScale = new Vector3(direction.x < 0 ? 1 : -1, 1, 1);
            movement = new Vector2(direction.x, 0);
            isMoving = true;
        }
        else
        {
            movement = Vector2.zero;
            isMoving = false;
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        animator.SetBool("isMoving", isMoving);
    }

    private void CheckFallOffMap()
    {
        if (transform.position.y < fallThresholdY)
        {
            Die();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                if (playerController.IsAttacking())
                {
                    TakeDamage(1, playerController.transform.position);
                }
                else
                {
                    Vector2 damageDirection = new Vector2(transform.position.x, 0);
                    playerController.ReceivingDamage(damageDirection, 1);
                }
            }
        }
    }

    public void TakeDamage(int damageAmount, Vector2 damageSourcePosition)
    {
        if (isDead) return;

        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashDamageEffect());
            ApplyKnockback(damageSourcePosition);
        }
    }

    private void ApplyKnockback(Vector2 sourcePosition)
    {
        Vector2 knockbackDir = (transform.position - (Vector3)sourcePosition).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
    }

    private IEnumerator FlashDamageEffect()
    {
        // Parpadeo: baja el alpha (transparencia)
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        movement = Vector2.zero;
        isMoving = false;

        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 0.6f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}
