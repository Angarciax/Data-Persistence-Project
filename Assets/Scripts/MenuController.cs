using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    { // As GameManager only starts once, HoF has to be refreshed every Title scene load
        gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<GameManager>().PaintHoF();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSmall()
    {
        gameManager.GetComponent<GameManager>().StartMainScene(0);
    }
    public void StartMedium()
    {
        gameManager.GetComponent<GameManager>().StartMainScene(1);
    }
    public void StartLarge()
    {
        gameManager.GetComponent<GameManager>().StartMainScene(2);
    }
}
