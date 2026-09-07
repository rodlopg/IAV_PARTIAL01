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
        if (agent != null) agent.SetDestination(this.transform.position - fleeVector);
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
    public void Wander()
    {
        Vector3 wanderTarget = Vector3.zero;
        float wanderRadius = 10;
        float wanderDistance = 20;
        float wanderJitter = 5;

        wanderTarget += new Vector3 (Random.Range(-1.0f,1.0f)*wanderJitter,0,Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();

        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0,0,wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);
        Seek(targetWorld);
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