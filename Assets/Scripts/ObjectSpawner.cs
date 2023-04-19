using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    private float minSpawnInterval = 7f;
    private float maxSpawnInterval = 9.5f;
    private float minBonusSpawnInterval = 6f;
    private float maxBonusSpawnInterval = 8f;

    private Vector3 bonusSpawnPosition1 = new Vector3(0, 0.5f, 0);
    private Vector3 bonusSpawnPosition2 = new Vector3(0, 5f, 0);

    [SerializeField] private Vector3 direction = Vector3.right;

    private bool lastObstacleFromThisGenerator = false;

    private IEnumerator Start()
    {
        

        while (true)
        {
            float spawnInterval = objectToSpawn.CompareTag("Bonus") ? Random.Range(minBonusSpawnInterval, maxBonusSpawnInterval) : Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
            Spawn();
        }
    }
   

    private void Spawn()
    {
        Vector3 spawnPosition = transform.parent.TransformPoint(transform.localPosition);

        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        spawnedObject.transform.SetParent(transform.parent);

        if (objectToSpawn.CompareTag("Bonus"))
        {
            float yPosition = Random.Range(0, 2) == 0 ? bonusSpawnPosition1.y : bonusSpawnPosition2.y;
            spawnedObject.transform.localPosition = new Vector3(spawnedObject.transform.localPosition.x, yPosition, spawnedObject.transform.localPosition.z);
        }
        else if (objectToSpawn.CompareTag("Obstacle"))
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