using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Snake : Agent
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private GameObject target;
    [SerializeField] private float firstDistanceFromTarget = 1f;
    [SerializeField] private float secondDistanceFromTarget = 2f;
    [SerializeField] private float thirdDistanceFromTarget = 3f;
    private Rigidbody2D _rb;
    private Vector3 vectorForce;
    private float distance;

    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0, 0, 0);
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
        vectorForce = new Vector3();
        vectorForce.x = actions.ContinuousActions[0];
        vectorForce.y = actions.ContinuousActions[1];
        _rb.AddForce(vectorForce * speed);
        
        distance = Vector3.Distance(transform.localPosition, target.transform.localPosition);
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