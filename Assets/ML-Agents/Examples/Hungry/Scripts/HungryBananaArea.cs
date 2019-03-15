using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public enum DETECTABLE_OBJECTS
{
    None = 0,
    Banana,
    Wall,
    Agent
}
public class HungryBananaArea : Area
{
    public GameObject banana;
    public GameObject badBanana;
    public int numBananas;
    public int numBadBananas;
    public bool respawnBananas;
    public float range;

    public GameObject groupWalls;
    public List<Transform> listWalls = new List<Transform>();

    public GameObject groupBananas;
    public List<HungryBananaLogic> listBananas = new List<HungryBananaLogic>();
    
    // Use this for initialization
    void Start()
    {
        foreach(Transform trans in groupWalls.GetComponentsInChildren<Transform>())
        {
            if (trans != groupWalls.transform)
            {
                listWalls.Add(trans);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateBanana(int numBana, GameObject bananaType)
    {
        if(numBana < listBananas.Count)
        {
            for(int i = numBana;i< listBananas.Count; i++)
            {
                listBananas[i].OnHide();
            }
        }
        for (int i = 0; i < numBana; i++)
        {
            if (i >= listBananas.Count)
            {
                GameObject bana = Instantiate(bananaType, GetRandomPositionInArea() + Vector3.up,
                                              Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
                listBananas.Add(bana.GetComponent<HungryBananaLogic>());
                listBananas[i].respawn = respawnBananas;
                listBananas[i].myArea = this;
                listBananas[i].transform.parent = groupBananas.transform;
            }
            else
            {
                listBananas[i].OnRespawn();
            }
        }
    }

    public void ResetBananaArea(GameObject[] agents)
    {
        foreach (GameObject agent in agents)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = GetRandomPositionInArea() + Vector3.up * 2f;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }

        CreateBanana(numBananas, banana);
    }

    public Vector3 GetRandomPositionInArea()
    {
        Vector3 randomPos = Vector3.zero;
        Vector2 randomDir = Random.insideUnitCircle * range;
        randomPos = this.transform.position + new Vector3(randomDir.x, 0, randomDir.y);
        return randomPos;
    }

    public override void ResetArea()
    {
    }
}
