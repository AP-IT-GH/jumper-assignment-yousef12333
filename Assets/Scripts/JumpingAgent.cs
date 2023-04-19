using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class JumpingAgent : Agent
{
    private Rigidbody rBody;
    private bool isOnGround = true;
    private float timeSinceLastObstacle = 0f;
    private float maxTimeWithoutObstacle = 3f;

    float reward = 0.25f;

    public override void OnEpisodeBegin()
    {
        isOnGround = true;
        transform.localPosition = Vector3.zero;
        rBody.velocity = Vector3.zero;
        timeSinceLastObstacle = 0f;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (isOnGround && actionBuffers.DiscreteActions[0] == 1)
        {
            isOnGround = false;
            rBody.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
            AddReward(-reward);
        }

        timeSinceLastObstacle += Time.deltaTime;
        if (timeSinceLastObstacle > maxTimeWithoutObstacle)
        {
            AddReward(0.1f);
            timeSinceLastObstacle = 0f;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isOnGround = true;
        }
        else if (other.gameObject.CompareTag("Bonus"))
        {
            AddReward(0.5f);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-0.3f); //probeer eens naar -0.6f te zetten later. Ik krijg een toename grafiek, maar het blijft een beetje te lang op de grond liggen. In het begin wal het wel juist.
            EndEpisode();
        }
    }

    public void JumpedReward()
    {
        AddReward(0.2f);
        timeSinceLastObstacle = 0f;
    }
}