using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Transform player;          // Player transform
    public DayNightCycle dayNightCycle; // Optioneel, kan automatisch gevonden worden
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    public float runSpeed = 4f;
    public float detectionDistance = 10f;
    public AudioClip groanClip;
    public float groanCooldown = 5f;
    private float nextGroanTime = 0f;

    private Vector3 lastPosition; // plek waar zombie stopt

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Automatisch Player vinden als niet ingevuld
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Automatisch DayNightCycle vinden als niet ingevuld
        if (dayNightCycle == null)
            dayNightCycle = Object.FindFirstObjectByType<DayNightCycle>();

        // Startpositie opslaan
        lastPosition = transform.position;
    }

    void Update()
    {
        if (agent == null || player == null) return;

        float sqrDistance = (player.position - transform.position).sqrMagnitude;

        if (sqrDistance <= detectionDistance * detectionDistance)
        {
            // Volg speler
            agent.SetDestination(player.position);
            agent.speed = runSpeed;

            // Groan geluid
            if (audioSource != null && groanClip != null && Time.time >= nextGroanTime)
            {
                audioSource.PlayOneShot(groanClip);
                nextGroanTime = Time.time + groanCooldown;
            }
        }
        else
        {
            // Te ver weg: blijf op plek
            agent.SetDestination(lastPosition);
            agent.speed = 0;
        }

        if (animator != null)
            animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
