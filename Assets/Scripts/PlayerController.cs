using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float velocity = 5f;
    public float jumpForce = 7f;
    public float raycastLength = 0.1f;
    public LayerMask layerMask;

    [Header("Combat")]
    public int life = 3;
    public float bounceForce = 5f;
    public GameObject swordRange;

    [Header("References")]
    public Animator animator;

    private Rigidbody2D rb;
    private bool isOnGround;
    private bool isReceivingDamage;
    private bool isAttacking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
        UpdateAnimations();
        CheckFallOffMap();
    }

    private void HandleInput()
    {
        if (!isAttacking)
        {
            Move();
            Jump();
        }

        if (isOnGround && !isAttacking && Input.GetKeyDown(KeyCode.F))
        {
            Attacking();
        }
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float velocityX = inputX * Time.deltaTime * velocity;

        animator.SetFloat("movement", Mathf.Abs(velocityX * velocity));

        if (inputX != 0)
            transform.localScale = new Vector3(Mathf.Sign(inputX), 1, 1);

        if (!isReceivingDamage)
        {
            transform.position += new Vector3(velocityX, 0, 0);
        }
    }

    private void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, layerMask);
        isOnGround = hit.collider != null;

        if (isOnGround && Input.GetKeyDown(KeyCode.Space) && !isReceivingDamage)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void CheckFallOffMap()
    {
        if (transform.position.y < -10f && !isReceivingDamage)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayGameOverSound();
                AudioManager.Instance.StopBackgroundMusic();
            }

            GameManager.Instance.ShowGameOver();
            gameObject.SetActive(false);
        }
    }

    private void UpdateAnimations()
    {
        animator.SetBool("isOnGround", isOnGround);
        animator.SetBool("isReceivingDamage", isReceivingDamage);
        animator.SetBool("isAttacking", isAttacking);
    }

    public void Attacking()
    {
        isAttacking = true;
        swordRange.SetActive(true);
    }

    public void DeactivateAttack()
    {
        isAttacking = false;
        swordRange.SetActive(false);
    }

    public void DeactivateDamage()
    {
        isReceivingDamage = false;
        rb.velocity = Vector2.zero;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void ReceivingDamage(Vector2 direction, int damageAmount)
    {
        if (isReceivingDamage) return;

        life -= damageAmount;

        if (life <= 0)
        {
            Die();
            return;
        }

        isReceivingDamage = true;
        Vector2 bounce = new Vector2(transform.position.x - direction.x, 0.2f).normalized;
        rb.AddForce(bounce * bounceForce, ForceMode2D.Impulse);

        StartCoroutine(InvulnerabilityDelay(1f));
    }

    private IEnumerator InvulnerabilityDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        isReceivingDamage = false;
        rb.velocity = Vector2.zero;
    }

    private void Die()
    {
        Debug.Log("Jugador derrotado");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverSound();
            AudioManager.Instance.StopBackgroundMusic();
        }

        GameManager.Instance.ShowGameOver();
        gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLength);
    }
}