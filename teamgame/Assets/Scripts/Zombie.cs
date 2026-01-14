using UnityEngine;
using UnityEngine.AI;

public class SmartZombieAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // De transform van de speler
    public DayNightCycle dayNightCycle; // Referentie naar je dag/nacht script
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    [Header("Movement Settings")]
    public float daySpeed = 3f;      // Snelheid overdag (rustiger)
    public float nightSpeed = 7f;    // Snelheid 's nachts (agressief)
    public float detectionDistance = 10f; // Hoe dichtbij de speler moet zijn

    [Header("Audio Settings")]
    public AudioClip groanClip;      // Het geluid dat de zombie maakt
    public float groanCooldown = 5f; // Tijd tussen geluiden
    private float nextGroanTime = 0f;

    private Vector3 lastPosition;    // De plek waar de zombie naar terugkeert

    void Start()
    {
        // Componenten ophalen
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Automatisch de speler zoeken op basis van de Tag "Player"
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Automatisch de DayNightCycle in de scene zoeken
        if (dayNightCycle == null)
            dayNightCycle = GameObject.FindObjectOfType<DayNightCycle>();

        // Sla de huidige positie op als startpunt
        lastPosition = transform.position;
    }

    void Update()
    {
        // Stop als de agent of speler ontbreekt
        if (agent == null || player == null) return;

        // Bepaal de huidige snelheid op basis van de tijd van de dag
        bool isDay = (dayNightCycle != null) ? dayNightCycle.IsDay() : true;
        float currentRunSpeed = isDay ? daySpeed : nightSpeed;

        // Bereken de afstand tot de speler (geoptimaliseerd met sqrMagnitude)
        float sqrDistance = (player.position - transform.position).sqrMagnitude;

        if (sqrDistance <= detectionDistance * detectionDistance)
        {
            // --- VOLG SPELER ---
            agent.SetDestination(player.position);
            agent.speed = currentRunSpeed;

            // Speel geluid af als de cooldown voorbij is
            if (audioSource != null && groanClip != null && Time.time >= nextGroanTime)
            {
                audioSource.PlayOneShot(groanClip);
                nextGroanTime = Time.time + groanCooldown;
            }
        }
        else
        {
            // --- TE VER WEG ---
            // Ga terug naar de laatste positie of sta stil
            agent.SetDestination(lastPosition);
            
            // 's Nachts dwalen ze een beetje rond, overdag staan ze stil
            agent.speed = isDay ? 0f : 2f; 
        }

        // Update de animator voor loop/ren animaties
        if (animator != null)
            animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}