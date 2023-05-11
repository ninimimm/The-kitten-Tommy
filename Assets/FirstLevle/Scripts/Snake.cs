using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class Snake : Agent
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private GameObject target;
    [SerializeField] private float firstDistanceFromTarget = 1f;
    [SerializeField] private float secondDistanceFromTarget = 2f;
    [SerializeField] private float thirdDistanceFromTarget = 3f;
    private Rigidbody2D _rb;
    private Vector3 startPosition = new (1,0,0);
    
    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        startPosition = transform.localPosition;
        
    }

    public override void OnEpisodeBegin()
    {
        //var random = Random.Range(3, 9);
        //var index = Random.Range(0, 2);
        //transform.position = new Vector3(random,-0.5f,0);
        //if (index == 0)
        //    target.transform.position = new Vector3(Random.Range(1,random),-0.5f,0);
        //else
        //    target.transform.position = new Vector3(Random.Range(random,9),-0.5f,0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.transform.localPosition);
        
        sensor.AddObservation(_rb.velocity.x);
        sensor.AddObservation(_rb.velocity.y);
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        transform.LookAt(target.transform);
        var vectorForce = new Vector3();
        vectorForce.x = actions.ContinuousActions[0];
        vectorForce.y = actions.ContinuousActions[1];
        _rb.AddForce(vectorForce * speed);
        
        var distance = Vector3.Distance(transform.localPosition, target.transform.localPosition);
        if (distance < firstDistanceFromTarget)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if (distance < secondDistanceFromTarget)
            SetReward(0.3f);
        else if (distance < thirdDistanceFromTarget)
            SetReward(0.1f);
        else
            SetReward(-0.2f);

        if (transform.localPosition.x < -2 || transform.localPosition.x > 10)
        {
            SetReward(-1f);
            EndEpisode();
        }
        // Reset rotations around x- and y-axes
        transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxisRaw("Horizontal");
        continuousActionsOut[1] = Input.GetAxisRaw("Vertical");
    }

}