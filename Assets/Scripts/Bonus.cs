using UnityEngine;

public class Bonus : MonoBehaviour
{
    private float minSpeed = 3f;
    private float maxSpeed = 5f;

    private float speed;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);//bepaal de variërende snelheid van het object
    }

    private void Update()
    {
        Move();
    }

    private void Move() //deze methode beweegt het object (in de in inspector gegeven richting van een andere script)
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
