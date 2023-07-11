using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector2 Velocity { get; set; } = Vector2.zero;
    [SerializeField]
    [Range(0f, 10f)] float Speed = 3f;
    [SerializeField]
    [Range(1f, 100f)] protected float ProtectedRange = 3f;
    [SerializeField]
    [Range(1f, 100f)] protected float VisualRange = 5f;
    [SerializeField]

    [Range(1f, 200f)] protected int GoalOffset = 50;

    [SerializeField]
    [Range(0f, 1f)] private float cohesionFactor = 0.15f;

    [SerializeField]
    [Range(0f, 100f)] public float smoothness = 50f;
    [SerializeField]
    [Range(0f, 1f)] float alignmentFactor = 0.5f;
    private Vector2 screenBounds;

    private void Awake()
    {
        screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

    }

    public void Move(Boid[] flock, Vector2 goal, float time)
    {
        var separationDirection = Vector2.zero;
        var alignmentDirection = Vector2.zero;
        var cohesionDirection = Vector2.zero;
        var count = 0;
        var separationCount = 0;

        Vector2 v1 = Vector2.zero, v2 = Vector2.zero, v3 = Vector2.zero, v4 = Vector2.zero, v5 = Vector2.zero, v6 = Vector2.zero, v7 = Vector2.zero;

        foreach (Boid boid in flock)
        {
            if (boid != this)
            {
                Vector2 diff = boid.transform.position - transform.position;
                float distance = diff.magnitude;
                if (distance < VisualRange)
                {
                    // calculate separation force
                    if (distance < ProtectedRange)
                    {
                        separationDirection -= diff.normalized / distance;
                        separationCount++;
                    }

                    // calculate cohesion and alignment force
                    if (distance < VisualRange)
                    {
                        cohesionDirection += (Vector2)boid.transform.position;
                        alignmentDirection += (Vector2)boid.Velocity;
                        count++;
                    }
                }
            }
        }

        if (count > 0)
        {
            cohesionDirection /= count;
            alignmentDirection /= count;
        }

        // Normalize separationDirection to avoid biasing the separation behavior
        if (separationCount > 0)
        {
            separationDirection /= separationCount;
        }
        separationDirection = separationDirection.normalized;
        v1 = (cohesionDirection - (Vector2)transform.position) * cohesionFactor;
        v2 = separationDirection * smoothness * time;
        v3 = (alignmentDirection - Velocity) * alignmentFactor;
        v4 = GetGoal(this, goal);
        v5 = BoundPosition(this);

        // // Only for deers
        // v6 = AvoidRedZones(this, time);
        // v7 = AvoidWolves(this, time);

        // apply all rules
        Velocity += v1 + v2 + v3 + v4 + v5;
        Velocity = LimitVelocity(this);
        transform.up = Velocity;

        transform.position = Vector2.Lerp(transform.position, (Vector2)transform.position + Velocity, Speed * time);
    }


    Vector2 BoundPosition(Boid item)
    {
        Vector2 v = Vector2.zero;
        // float Xmin = -20f, Xmax = 20f, Ymin = -10f, Ymax = 10f;

        if (item.transform.position.x < -screenBounds.x)
            v.x = 15;

        else if ((item.transform.position.x > screenBounds.x))
            v.x = -15;

        if (item.transform.position.y < -screenBounds.y)
            v.y = 20;

        else if ((item.transform.position.y > screenBounds.y))
            v.y = -20;
        return v;
    }

    //limit boid speed
    Vector2 LimitVelocity(Boid item)
    {
        Vector2 min = item.Velocity;
        if (min.magnitude > Speed)
        {
            min = min.normalized * Speed;
        }
        return min;
    }

    private Vector2 GetGoal(Boid item, Vector2 goal)
    {

        return (goal - (Vector2)item.transform.position).normalized / GoalOffset;
    }

    // protected virtual Vector2 AvoidRedZones(Boid item, float time)
    // {
    //     return Vector2.zero;
    // }


    // protected virtual Vector2 AvoidWolves(Boid item, float time)
    // {
    //     return Vector2.zero;
    // }
}
