using UnityEngine;

public class Ladron : SteeringMovement
{
    public float detectionRadius = 15f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        Peaton[] pedestrians = FindObjectsOfType<Peaton>();
        Peaton closestPedestrian = null;
        float closestPedDist = detectionRadius;

        foreach (var ped in pedestrians)
        {
            float dist = Vector3.Distance(transform.position, ped.transform.position);
            if (dist < closestPedDist)
            {
                closestPedDist = dist;
                closestPedestrian = ped;
            }
        }

        if (closestPedestrian != null)
        {
            Seek(closestPedestrian.transform.position);
        }
        else
        {
            GameObject closestThreat = GetClosestThreat();
            if (closestThreat != null)
            {
                target = closestThreat;
                Evade();
            }
            else
            {
                Wander();
            }
        }
    }

    private GameObject GetClosestThreat()
    {
        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        Police[] police = FindObjectsOfType<Police>();
        foreach (var p in police)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = p.gameObject;
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = player;
            }
        }

        return closest;
    }
}
