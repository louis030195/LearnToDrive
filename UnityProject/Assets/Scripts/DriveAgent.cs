using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveAgent : Agent {
    
    RayPerception rayPer;

    [HideInInspector]
    public WallDetect wallDetect;

    float previousDistanceTarget = float.MaxValue;
    float previousDistanceSpawn = float.MinValue;
    float previousDistancePosition = float.MinValue;
    Vector3 previousPosition = Vector3.zero;

    public Transform Target;
    public float speed = 10;

    private void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        wallDetect = GetComponent<WallDetect>();
        wallDetect.agent = this;
        rayPer = GetComponent<RayPerception>();

    }

    public override void CollectObservations()
    {
        // Calculate relative position
        // Vector3 relativePosition = Target.position - this.transform.position;

        // Relative position
        // AddVectorObs(relativePosition.x);
        // AddVectorObs(relativePosition.z);

        //Detect wall
        float rayDistance = 12f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
        string[] detectableObjects;
        detectableObjects = new string[] { "wall" };
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));

        AddVectorObs(transform.rotation);
        AddVectorObs(transform.position);

    }



    public override void AgentAction(float[] vectorAction, string textAction)
	{




        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.position);
        float distanceFromSpawn = Vector3.Distance(Vector3.zero, transform.position);
        float distanceFromPrevious = Vector3.Distance(previousPosition, transform.position);

        // Reached target
        if (distanceToTarget < 2f)
        {
            Done();
            AddReward(1.0f);
        }

        // Getting closer
        /*
        if (distanceToTarget < previousDistance)
        {
            AddReward(0.005f);
        }*/




        // Positive reward if getting away from previous position
        if (distanceFromPrevious > previousDistancePosition)
        {
            AddReward(0.01f);
        }
        /*
        // Positive reward if getting away from 0,0,0
        if (distanceFromSpawn > previousDistanceSpawn)
        {
            AddReward(0.01f);
        }*/





        // Time penalty
        AddReward(-0.005f);

        previousDistanceTarget = distanceToTarget;
        previousDistanceSpawn = distanceFromSpawn;
        previousPosition = transform.position;

        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);


        transform.Rotate(new Vector3(0, Mathf.Clamp(vectorAction[1], -1, 1), 0));
        
        Vector3 velocity = Vector3.zero;
        velocity.z = Mathf.Clamp(vectorAction[0], -1, 1);
        // move the object
        transform.Translate(velocity * speed);
        
    }

    public override void AgentReset()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = new Vector3(0, 90, 0);
    }

    public override void AgentOnDone()
    {

    }


    public void HasHitWall()
    {
        AddReward(-1f);
        Done();
    }
}
