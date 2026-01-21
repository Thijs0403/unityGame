using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("References")]
    public Transform player;           
    public DayNightCycle dayNightCycle; 
    private NavMeshAgent agent;
    private AudioSource audioSource;

    [Header("Movement Settings")]
    public float daySpeed = 3f;      // Snelheid overdag
    public float nightSpeed = 7f;    // Snelheid 's nachts
    public float detectionDistance = 10f;

    [Header("Attack Settings")]
    public float attackDistance = 1.5f; // Afstand om te kunnen slaan
    public float damage = 10f;          // Schade per klap
    public float attackSpeed = 1.5f;    // Vertraging tussen aanvallen
    private float nextAttackTime = 0f;

    [Header("Audio")]
    public AudioClip groanClip;
    public float groanCooldown = 5f;
    private float nextGroanTime = 0f;

    private Vector3 lastPosition; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        // Automatisch Player vinden op basis van de "Player" Tag
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Automatisch DayNightCycle vinden in de scene
        if (dayNightCycle == null)
            dayNightCycle = Object.FindFirstObjectByType<DayNightCycle>();

        lastPosition = transform.position;
    }

    void Update()
    {
        if (agent == null || player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // Check of de speler dichtbij genoeg is om te achtervolgen
        if (distance <= detectionDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // Bepaal snelheid op basis van dag of nacht
            if (dayNightCycle != null && dayNightCycle.IsDay())
                agent.speed = daySpeed;
            else
                agent.speed = nightSpeed;

            // --- ATTACK LOGICA ---
            if (distance <= attackDistance)
            {
                AttackPlayer();
            }

            // Geluid afspelen
            if (audioSource != null && groanClip != null && Time.time >= nextGroanTime)
            {
                audioSource.PlayOneShot(groanClip);
                nextGroanTime = Time.time + groanCooldown;
            }
        }
        else
        {
            // Blijf staan als de speler te ver weg is
            agent.isStopped = true;
        }
    }

    void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
            // Zoek het Health script op de speler capsule
            Health playerHealth = player.GetComponent<Health>();
            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Doe schade aan de speler
                Debug.Log("Zombie valt aan! Speler krijgt " + damage + " schade.");
            }

            nextAttackTime = Time.time + attackSpeed;
        }
    }
}