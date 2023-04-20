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
            AddReward(-reward); //zomaar springen wordt bestraft
        }

        timeSinceLastObstacle += Time.deltaTime;
        if (timeSinceLastObstacle > maxTimeWithoutObstacle) //checkt elke drie seconden of het een collisie deed met de obstacle, zo niet dan wordt hij beloond en zal timesincelastobstacle weer 0f worden
        {
            AddReward(0.1f);
            timeSinceLastObstacle = 0f;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) //heuristic voor als ik het zelf wil laten springen, handig voor testing interne functie componenten, maar niet voor tijdens training
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
            AddReward(0.5f); //bonus geeft reward
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-1.05f); //botsing met obstacle geeft punishment (negatieve reward)
            EndEpisode();
        }
    }

    public void JumpedReward() //beloning voor ontwijken obstacle
    {
        AddReward(0.4f);
        timeSinceLastObstacle = 0f;
    }
}