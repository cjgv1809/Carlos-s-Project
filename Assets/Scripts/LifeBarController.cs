using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarController : MonoBehaviour
{
    public Image filledLifeBar;
    private PlayerController playerController;
    private float maxLife;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        maxLife = playerController.life;
    }

    // Update is called once per frame
    void Update()
    {
        filledLifeBar.fillAmount = (float)playerController.life / maxLife;
    }
}
