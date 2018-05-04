using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveAgent : Agent {

    Rigidbody rBody;
    RayPerception rayPer;

    [HideInInspector]
    public WallDetect wallDetect;

    float previousDistance = float.MaxValue;
    Vector3 previousPosition = Vector3.zero;

    public Transform Target;
    public float speed = 10;

    private void Awake()
    {

    }

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
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
        Vector3 relativePosition = Target.position - this.transform.position;

        // Relative position
        AddVectorObs(relativePosition.x);
        AddVectorObs(relativePosition.z);

        //Detect wall
        float rayDistance = 12f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
        string[] detectableObjects;
        detectableObjects = new string[] { "wall" };
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));


        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
    }



    public override void AgentAction(float[] vectorAction, string textAction)
	{
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.position);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            Done();
            AddReward(1.0f);
        }
        
        // Getting closer
        if (distanceToTarget < previousDistance)
        {
            AddReward(0.01f);
        }

        if(Vector3.Distance(transform.position,previousPosition) > 0.1f)
        {
            AddReward(0.1f * Vector3.Distance(transform.position, previousPosition));
        }

        //Debug.Log($"Distance : {Vector3.Distance(transform.position, previousPosition)}");  

        // Time penalty
        AddReward(-0.005f);

        previousDistance = distanceToTarget;
        previousPosition = transform.position;

        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
        rBody.AddForce(controlSignal * speed);
    }

    public override void AgentReset()
    {
        this.transform.position = Vector3.zero;
        rBody.angularVelocity = Vector3.zero;
        rBody.velocity = Vector3.zero;
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
