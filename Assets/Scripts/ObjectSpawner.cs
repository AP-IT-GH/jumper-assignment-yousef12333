using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    private float minSpawnInterval = 7f;
    private float maxSpawnInterval = 9.5f;
    private float minBonusSpawnInterval = 6f;
    private float maxBonusSpawnInterval = 8f;

    private Vector3 bonusSpawnPosition1 = new Vector3(0, 0.5f, 0); //twee posities voor de bonus, hij kan zowel boven zweven als op de grond bewegen.
    private Vector3 bonusSpawnPosition2 = new Vector3(0, 5f, 0);

    [SerializeField] private Vector3 direction = Vector3.right;

    private bool lastObstacleFromThisGenerator = false;

    private IEnumerator Start()
    {
        

        while (true) //randomly bepalen wanneer de bonus wordt gespawned
        {
            float spawnInterval = objectToSpawn.CompareTag("Bonus") ? Random.Range(minBonusSpawnInterval, maxBonusSpawnInterval) : Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
            Spawn();
        }
    }
   

    private void Spawn()
    {
        Vector3 spawnPosition = transform.parent.TransformPoint(transform.localPosition); //begint bij waar zij geïnstantieerd worden

        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        spawnedObject.transform.SetParent(transform.parent);

        if (objectToSpawn.CompareTag("Bonus")) //als het object een bonus is, moet de positie random gespawned worden, maar alleen aan één zijde (links van de agent i.p.v ook rechts)
        {
            float yPosition = Random.Range(0, 2) == 0 ? bonusSpawnPosition1.y : bonusSpawnPosition2.y;
            spawnedObject.transform.localPosition = new Vector3(spawnedObject.transform.localPosition.x, yPosition, spawnedObject.transform.localPosition.z);
        }
        else if (objectToSpawn.CompareTag("Obstacle")) //als het om een obstacle gaat, moet het afwisselend geïnstantieerd worden van de linkerzijde naar de rechterzijde en vice versa.
        {
            if (lastObstacleFromThisGenerator)
            {
                spawnedObject.transform.SetParent(transform.parent.GetChild(0));
                lastObstacleFromThisGenerator = false;
            }
            else
            {
                lastObstacleFromThisGenerator = true;
            }
            Obstacle obstacle = spawnedObject.GetComponent<Obstacle>();

            if (direction != Vector3.zero)
            {
                obstacle.SetDirection(direction);
            }
            spawnedObject.transform.localPosition = transform.localPosition;
        }
    }
}