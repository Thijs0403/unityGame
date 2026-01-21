using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Health Stats")]
    public float maxHealth = 50f;
    public float currentHealth = 50f;

    [Header("Movement Settings")]
    public Transform player;           
    public DayNightCycle dayNightCycle; 
    private NavMeshAgent agent;

    public float daySpeed = 3f;      
    public float nightSpeed = 7f;    
    public float detectionDistance = 10f;
    public float attackDistance = 1.5f; 

    [Header("Attack Settings")]
    public float damage = 10f;          
    public float attackSpeed = 1.5f;    
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (dayNightCycle == null)
            dayNightCycle = Object.FindFirstObjectByType<DayNightCycle>();
    }

    void Update()
    {
        if (agent == null || player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // Snelheid aanpassen aan tijd
            if (dayNightCycle != null && dayNightCycle.IsDay())
                agent.speed = daySpeed;
            else
                agent.speed = nightSpeed;

            // Aanvallen als hij dichtbij is
            if (distance <= attackDistance)
            {
                AttackPlayer();
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Stuur 'this' mee zodat de speler weet welke zombie aanvalt
                playerHealth.TakeDamage(damage, this);
            }
            nextAttackTime = Time.time + attackSpeed;
        }
    }
}