using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public DayNightCycle dayNightCycle;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    [Header("Movement Settings")]
    public float daySpeed = 3f;
    public float nightSpeed = 7f;
    public float detectionDistance = 10f;

    [Header("Audio Settings")]
    public AudioClip groanClip;
    public float groanCooldown = 5f;
    private float nextGroanTime = 0f;

    private Vector3 lastPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (dayNightCycle == null)
            dayNightCycle = GameObject.FindObjectOfType<DayNightCycle>();

        lastPosition = transform.position;
    }

    void Update()
    {
        if (agent == null || player == null) return;

        bool isDay = (dayNightCycle != null) ? dayNightCycle.IsDay() : true;
        float currentRunSpeed = isDay ? daySpeed : nightSpeed;

        float sqrDistance = (player.position - transform.position).sqrMagnitude;

        if (sqrDistance <= detectionDistance * detectionDistance)
        {
            agent.SetDestination(player.position);
            agent.speed = currentRunSpeed;

            if (audioSource != null && groanClip != null && Time.time >= nextGroanTime)
            {
                audioSource.PlayOneShot(groanClip);
                nextGroanTime = Time.time + groanCooldown;
            }
        }
        else
        {
            agent.SetDestination(lastPosition);
            agent.speed = isDay ? 0f : 2f; 
        }

        if (animator != null)
            animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}