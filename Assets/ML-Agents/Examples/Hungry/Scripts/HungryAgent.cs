using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class HungryAgent : Agent
{
    public int MAX_DETECTABLE_OBJECTS = 20;
    private HungryAcademy myAcademy;
    public GameObject area;
    HungryBananaArea myArea;
    Rigidbody agentRb;
    private int bananas;
    
    // Speed of agent movement.
    public float moveSpeed = 2;
    public Material normalMaterial;
    public Material badMaterial;
    public Material goodMaterial;
    public Material frozenMaterial;
    public bool contribute;
    private RayPerception rayPer;
    public bool useVectorObs;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        agentRb = GetComponent<Rigidbody>();
        Monitor.verticalOffset = 1f;
        myArea = area.GetComponent<HungryBananaArea>();
        rayPer = GetComponent<RayPerception>();
        myAcademy = FindObjectOfType<HungryAcademy>();
    }

    public override void CollectObservations()
    {
        //if (useVectorObs)
        //{
        //    float rayDistance = 50f;
        //    float[] rayAngles = { 0f, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f, 360f };
        //    string[] detectableObjects = { "banana", "agent", "wall" };
        //    AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        //    Vector3 localVelocity = transform.InverseTransformDirection(agentRb.velocity);
        //    AddVectorObs(localVelocity.x);
        //    AddVectorObs(localVelocity.z);
        //}

        if (useVectorObs)
        {

            int objNumber = 0;
            List<float> listObs = new List<float>();
            while (objNumber < MAX_DETECTABLE_OBJECTS)
            {
                for (int i = 0; i < myArea.listBananas.Count; i++)
                {
                    if (!myArea.listBananas[i].gameObject.activeSelf) continue;
                    Vector3 dirToTarget = myArea.listBananas[i].transform.position - this.transform.position;

                    float[] subList = new float[(int)DETECTABLE_OBJECTS.None + 3];
                    subList = UpdateSubList2((int)DETECTABLE_OBJECTS.Banana, dirToTarget, subList);

                    listObs.AddRange(subList);
                    objNumber++;
                    if (objNumber >= MAX_DETECTABLE_OBJECTS) break;
                }

                for (int i = 0; i < MAX_DETECTABLE_OBJECTS; i++)
                {
                    float[] subList = new float[(int)DETECTABLE_OBJECTS.None + 3];
                    subList = UpdateSubList2((int)DETECTABLE_OBJECTS.None, Vector3.zero, subList);
                    
                    listObs.AddRange(subList);
                    objNumber++;
                    if (objNumber >= MAX_DETECTABLE_OBJECTS) break;
                }
            }

            AddVectorObs(listObs);

            Vector3 localVelocity = transform.InverseTransformDirection(agentRb.velocity);
            AddVectorObs(localVelocity.x);
            AddVectorObs(localVelocity.z);
        }
    }

    //public override void CollectObservations()
    //{
    //    //if (useVectorObs)
    //    //{
    //    //    float rayDistance = 50f;
    //    //    float[] rayAngles = { 0f, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f, 360f };
    //    //    string[] detectableObjects = { "banana", "agent", "wall" };
    //    //    AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
    //    //    Vector3 localVelocity = transform.InverseTransformDirection(agentRb.velocity);
    //    //    AddVectorObs(localVelocity.x);
    //    //    AddVectorObs(localVelocity.z);
    //    //}

    //    if (useVectorObs)
    //    {
    //        //float rayDistance = 50f;
    //        //float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
    //        //string[] detectableObjects = { "wall" };
    //        //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

    //        int objNumber = 0;
    //        List<float> listObs = new List<float>();
    //        while (objNumber < MAX_DETECTABLE_OBJECTS)
    //        {
    //            //for (int i = 0; i < myArea.listWalls.Count; i++)
    //            //{
    //            //    if (!myArea.listWalls[i].gameObject.activeSelf) continue;
    //            //    Vector3 dirToTarget = myArea.listWalls[i].transform.position - this.transform.position;
    //            //    dirToTarget.y = 0;

    //            //    float angle = Vector2.SignedAngle(new Vector2(dirToTarget.x, dirToTarget.z), new Vector2(this.transform.forward.x, this.transform.forward.z));
    //            //    float distance = dirToTarget.magnitude;
    //            //    AddVectorObs(new List<float> { (float)DETECTABLE_OBJECTS.Wall, angle, distance });
    //            //    objNumber++;
    //            //    if (objNumber >= MAX_DETECTABLE_OBJECTS) break;
    //            //}

    //            for (int i = 0; i < myArea.listBananas.Count; i++)
    //            {
    //                if (!myArea.listBananas[i].gameObject.activeSelf) continue;
    //                Vector3 dirToTarget = myArea.listBananas[i].transform.position - this.transform.position;
    //                dirToTarget.y = 0;

    //                float angle = Vector2.SignedAngle(new Vector2(dirToTarget.x, dirToTarget.z), new Vector2(this.transform.forward.x, this.transform.forward.z));
    //                float distance = dirToTarget.magnitude;
    //                float[] subList = new float[(int)DETECTABLE_OBJECTS.None + 2];
    //                subList = UpdateSubList((int)DETECTABLE_OBJECTS.Banana, angle, distance, subList);

    //                //AddVectorObs(new List<float> { (float)DETECTABLE_OBJECTS.Banana, distance });
    //                listObs.AddRange(subList);
    //                objNumber++;
    //                if (objNumber >= MAX_DETECTABLE_OBJECTS) break;
    //            }

    //            for (int i = 0; i < MAX_DETECTABLE_OBJECTS; i++)
    //            {
    //                float[] subList = new float[(int)DETECTABLE_OBJECTS.None + 2];
    //                subList = UpdateSubList((int)DETECTABLE_OBJECTS.None, 0, 0, subList);

    //                //AddVectorObs(new List<float> { (float)DETECTABLE_OBJECTS.None, 0 });
    //                listObs.AddRange(subList);
    //                objNumber++;
    //                if (objNumber >= MAX_DETECTABLE_OBJECTS) break;
    //            }
    //        }

    //        AddVectorObs(listObs);

    //        Vector3 localVelocity = transform.InverseTransformDirection(agentRb.velocity);
    //        AddVectorObs(localVelocity.x);
    //        AddVectorObs(localVelocity.z);
    //    }
    //}

    public float[] UpdateSubList(int objectType, float angle, float distance, float[] subList)
    {
        for(int i=0; i < subList.Length; i++)
        {
            if(i == objectType)
            {
                subList[i] = 1;
            }
            else if(i == subList.Length - 2)
            {
                subList[i] = angle/360;
            }
            else if (i == subList.Length - 1)
            {
                subList[i] = distance/ (myArea.range * 2f);
            }
            else
            {
                subList[i] = 0;
            }
        }
        return subList;
    }

    public float[] UpdateSubList2(int objectType, Vector3 objectPosition, float[] subList)
    {
        for (int i = 0; i < subList.Length; i++)
        {
            if (i == objectType)
            {
                subList[i] = 1;
            }
            else if (i == subList.Length - 3)
            {
                subList[i] = Mathf.Min(15f, objectPosition.x/15f);
            }
            else if (i == subList.Length - 2)
            {
                subList[i] = Mathf.Min(15f, objectPosition.z / 15f);
            }
            else if (i == subList.Length - 1)
            {
                subList[i] = Mathf.Min(2f, objectPosition.y / 2f);
            }
            else
            {
                subList[i] = 0;
            }
        }
        return subList;
    }

    public Color32 ToColor(int hexVal)
    {
        byte r = (byte)((hexVal >> 16) & 0xFF);
        byte g = (byte)((hexVal >> 8) & 0xFF);
        byte b = (byte)(hexVal & 0xFF);
        return new Color32(r, g, b, 255);
    }

    public void MoveAgent(float[] act)
    {

        Vector3 dirToGo = Vector3.zero;
        Vector2 rotateDir = Vector3.zero;
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            dirToGo = new Vector3(Mathf.Clamp(act[1], -1f, 1f), 0, Mathf.Clamp(act[0], -1f, 1f));
            //dirToGo.Normalize();
        }
        else
        {
            var forwardAxis = (int)act[0];
            var rotateAxis = (int)act[1];

            switch (forwardAxis)
            {
                case 1:
                    dirToGo = transform.forward;
                    break;
                //case 2:
                //    dirToGo = -transform.forward;
                //    break;
            }

            switch (rotateAxis)
            {
                case 1:
                    rotateDir = -transform.up;
                    break;
                case 2:
                    rotateDir = transform.up;
                    break;
            }
        }
        agentRb.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
        transform.LookAt(transform.position + dirToGo);
        //transform.Rotate(rotateDir, Time.fixedDeltaTime * 300f);

        if (agentRb.velocity.sqrMagnitude > 25f) // slow it down
        {
            agentRb.velocity *= 0.95f;
        }
    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        AddReward(-0.001f);
        MoveAgent(vectorAction);
    }

    public override void AgentReset()
    {
        agentRb.velocity = Vector3.zero;
        bananas = 0;
        transform.position = myArea.GetRandomPositionInArea() + Vector3.up * 2f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("banana"))
        {
            collision.gameObject.GetComponent<HungryBananaLogic>().OnEaten();
            AddReward(1f);
            bananas += 1;
            if (contribute)
            {
                myAcademy.totalScore += 1;
            }
        }

        if (collision.gameObject.CompareTag("wall"))
        {
            //AddReward(-1f);
            //Done();
        }
    }

    public override void AgentOnDone()
    {

    }
}
