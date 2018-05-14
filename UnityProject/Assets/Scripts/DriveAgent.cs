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
        // AddVectorObs(rBody.velocity.x);
        // AddVectorObs(rBody.velocity.z);
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

        // Positive reward if getting away from previousPosition
        if (Vector3.Distance(transform.position,previousPosition) > 0.1f)
        {
            AddReward(0.1f * Mathf.Clamp(Vector3.Distance(transform.position, previousPosition),1,10));
            // Debug.Log($"Distance from previousPosition reward : { 0.1f * Vector3.Distance(transform.position, previousPosition) }");
            //AddReward(0.7f); 
        }
        /*
        // Positive reward if getting away from 0,0,0
        if (Vector3.Distance(Vector3.zero, transform.position) > 0.1f)
        {
            AddReward(0.01f);
        }*/

        /*
        // Positive reward if getting closer to the target
        if (Vector3.Distance(transform.position, Target.position) > 0.1f)
        {
            //AddReward(0.01f * Vector3.Distance(transform.position, Target.position));
            // Debug.Log($"Distance from target reward : { 0.01f * Vector3.Distance(transform.position, Target.position) }");
            AddReward(0.1f); // 0.1f = v0.3 |`0.15f = v0.4 
        }
        */



        // Time penalty
        AddReward(-0.005f);

        previousDistance = distanceToTarget;
        previousPosition = transform.position;

        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
        //controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
        //rBody.AddForce(controlSignal * speed);

        transform.Rotate(new Vector3(0, Mathf.Clamp(vectorAction[1], -1, 1), 0));
        
        Vector3 velocity = Vector3.zero;
        velocity.z = Mathf.Clamp(vectorAction[0], -1, 1);
        // move the object
        transform.Translate(velocity * speed);
        
    }

    public override void AgentReset()
    {
        transform.position = Vector3.zero;
        //transform.rotation = new Quaternion(0, 10, 0, 0);
        transform.eulerAngles = new Vector3(0, 90, 0);
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
