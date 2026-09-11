using UnityEngine;
using UnityEngine.AI;

public class SteeringMovement : MonoBehaviour
{
   protected NavMeshAgent agent;
   public GameObject target;

    protected virtual void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    public void Seek(Vector3 location)
    {
        if (agent != null) agent.SetDestination(location);
    }

    public void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        Vector3 targetFleePoint = this.transform.position - fleeVector;
        
        if (agent != null) 
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetFleePoint, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                // If the exact opposite direction is off-mesh, just try to move away slightly
                agent.SetDestination(this.transform.position + (this.transform.position - location).normalized * 2f);
            }
        }
    }

    public void Pursue(){
        if (target == null || agent == null) return;
        Vector3 targetDir = target.transform.position - this.transform.position;
        Drive targetDrive = target.GetComponent<Drive>();
        float targetSpeed = targetDrive != null ? targetDrive.currentSpeed : 0f;
        float lookAhead = targetDir.magnitude/(agent.speed + targetSpeed);
        float toTarget = Vector3.Angle(this.transform.forward,this.transform.TransformVector(targetDir));

        if( toTarget > 90 || targetSpeed < 0.01f){
            agent.SetDestination(target.transform.position);
            return;
        }

        agent.SetDestination(target.transform.position + target.transform.forward * lookAhead * 10);

    }
    public void Evade(){
        if (target == null || agent == null) return;
        Vector3 targetDir = target.transform.position - this.transform.position;
        Drive targetDrive = target.GetComponent<Drive>();
        float targetSpeed = targetDrive != null ? targetDrive.currentSpeed : 0f;
        float lookAhead = targetDir.magnitude/(agent.speed + targetSpeed);

        Flee(target.transform.position + target.transform.forward * lookAhead * 10);

    }
    private Vector3 wanderTarget = Vector3.zero;

    public void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 20;
        float wanderJitter = 5;

        wanderTarget += new Vector3 (Random.Range(-1.0f,1.0f)*wanderJitter,0,Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();

        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0,0,wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.TransformPoint(targetLocal);
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetWorld, out hit, 10f, NavMesh.AllAreas))
        {
            // If the sampled position is too close, we might be facing a wall.
            if (Vector3.Distance(transform.position, hit.position) < 2f)
            {
                wanderTarget = -wanderTarget; // Reverse direction
            }
            Seek(hit.position);
        }
        else
        {
            wanderTarget = -wanderTarget;
            Seek(this.transform.position); // stop or stay in place briefly
        }
    }

    protected static GameObject[] getHidingSpots(string tag)
    {
        GameObject[] hidingSpots = GameObject.FindGameObjectsWithTag(tag);
        return hidingSpots;
    }

    public void Hide()
    {
        if (target == null || agent == null) return;
        float closestDistance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        GameObject[] hidingSpots = getHidingSpots("hide");

        for(int i = 0; i < hidingSpots.Length; i++)
        {
            Vector3 hideDirection = hidingSpots[i].transform.position - target.transform.position;
            Vector3 hidePosition = hidingSpots[i].transform.position + hideDirection.normalized*5;

            if( Vector3.Distance(this.transform.position,hidePosition)< closestDistance){
                closestDistance = Vector3.Distance(this.transform.position,hidePosition);
                chosenSpot = hidePosition;
            }
        }
        Seek(chosenSpot);
    }
    protected virtual void Update()
    {
        //Flee(target.transform.position);
         //Pursue();
         //Evade();
         //Hide();
      //Seek(target.transform.position);
    }
}