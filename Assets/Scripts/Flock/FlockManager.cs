using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public Boid BoidPrefab;
    Boid[] boids;
    public int number = 10;
    GameObject center;

    public Vector2 range = new Vector2(10, 10);
    // Start is called before the first frame update
    void Start()
    {
        boids = new Boid[number];


        for (int i = 0; i < number; i++)
        {
            SpawnBoid(BoidPrefab, i);
        }
        center = GameObject.FindGameObjectWithTag("center");

    }
    // Update is called once per frame
    void Update()
    {
        foreach (var boid in boids)
        {
            boid.Move(boids, center.transform.position, Time.deltaTime);
        }
    }
    private void SpawnBoid(Boid prefab, int index)
    {
        Boid boidInstance = Instantiate(
            prefab,
            Vector2.zero,
            Quaternion.Euler(0, 0, Random.Range(0f, 360f)),
            transform
            );
        boidInstance.transform.localPosition = (Vector2)boidInstance.transform.localPosition + new Vector2(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y));
        boids[index] = boidInstance;
    }
}
