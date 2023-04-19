using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float minSpeed = 2f;
    private float maxSpeed = 5f;
    private float speed;
    private Vector3 direction = Vector3.right;
    private JumpingAgent jumpingAgent;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        jumpingAgent = GameObject.FindObjectOfType(typeof(JumpingAgent)) as JumpingAgent;


    }
    private void Update()
    {
        Move();
        
    }
    private void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddJumpReward();
            Destroy(gameObject);
        }
    }
    public void AddJumpReward()
    {
        jumpingAgent.JumpedReward();
    }
}
