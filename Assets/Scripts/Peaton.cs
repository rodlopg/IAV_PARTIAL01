using UnityEngine;

public class Peaton : SteeringMovement
{
    public float detectionRadius = 15f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        Ladron[] thieves = FindObjectsOfType<Ladron>();
        Ladron closestThief = null;
        float closestDist = detectionRadius;

        foreach (var thief in thieves)
        {
            float dist = Vector3.Distance(transform.position, thief.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestThief = thief;
            }
        }

        if (closestThief != null)
        {
            Flee(closestThief.transform.position);
        }
        else
        {
            Wander();
        }
    }
}
