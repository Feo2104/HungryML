using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryBananaLogic : MonoBehaviour {

    public bool respawn;
    public HungryBananaArea myArea;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public void OnEaten() {
        if (respawn) 
        {
            OnRespawn();
        }
        else 
        {
            OnHide();
        }
    }

    public void OnRespawn()
    {
        transform.position = myArea.GetRandomPositionInArea() + Vector3.up * 2f;
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }
}
