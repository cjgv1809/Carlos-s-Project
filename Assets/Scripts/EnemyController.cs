using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isMoving;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            if (direction.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            movement = new Vector2(direction.x, 0);
            isMoving = true;
        } else
        {
            movement = Vector2.zero;
            isMoving = false;
        }

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        animator.SetBool("isMoving", isMoving);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 damageDirection = new Vector2(transform.position.x, 0);
            collision.gameObject.GetComponent<PlayerController>().ReceivingDamage(damageDirection, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
