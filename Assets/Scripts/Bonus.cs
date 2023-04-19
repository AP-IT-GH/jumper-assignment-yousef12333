using UnityEngine;

public class Bonus : MonoBehaviour
{
    private float minSpeed = 3f;
    private float maxSpeed = 5f;

    private float speed;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
