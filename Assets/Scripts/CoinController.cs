using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private bool collected = false;
    private float destroyDelay = 0.3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected) return;

        if (collision.CompareTag("Player"))
        {
            collected = true;
            GameManager.Instance.AddCoin();
            AudioManager.Instance.PlayCoinSound();

            // Opcional: Desactiva el collider para evitar múltiples recogidas
            GetComponent<Collider2D>().enabled = false;

            // Inicia animación de escala
            StartCoroutine(ScaleAndDestroy());
        }
    }

    private IEnumerator ScaleAndDestroy()
    {
        float t = 0f;
        Vector3 originalScale = transform.localScale;

        while (t < destroyDelay)
        {
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t / destroyDelay);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}