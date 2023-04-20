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
        speed = Random.Range(minSpeed, maxSpeed); //bepaalt randomly hoe snel het gaat
        jumpingAgent = GameObject.FindObjectOfType(typeof(JumpingAgent)) as JumpingAgent; //object referentie van jumpingAgent om reward te geven


    }
    private void Update()
    {
        Move();
        
    }
    private void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime); //beweging obstacle
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction; //naar welke richting hij moet gaan aangezien hij zowel kan instantiëren aan de linkerzijde als de rechterzijde
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) //bij botsing met de muur betekent dat dat de Agent de obstacle ontweken heeft en wordt in JumpingAgent daarvoor beloond
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
