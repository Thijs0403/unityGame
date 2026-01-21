using UnityEngine;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [Header("Lights")]
    public Light sunLight;
    public Light moonLight;

    [Header("Moon")]
    public Transform moonPivot;
    public GameObject FullMoonSphere;
    public GameObject HalfMoonSphere;

    [Header("UI")]
    public TextMeshProUGUI timeDisplay;

    [Header("Time Settings")]
    public float dayDuration = 1200f; // 20 minuten = 1200 sec
    private float time;

    [Header("Skybox")]
    public Material daySkybox;
    public Material nightSkybox;

    [Header("Moon Settings")]
    [Range(0f, 1f)]
    public float nightMoonChance = 1f; // kans dat de maan zichtbaar is per nacht

    private const float sunriseHour = 6f;
    private const float sunsetHour = 18f;
    private const float moonSynodicDay = 24.84f; // uren voor synodische dag
    private const float moonPhaseDays = 29.5f;   // dagen voor volle cyclus

    private float TimePercent => (time % dayDuration) / dayDuration;
    private float CurrentHour => TimePercent * 24f;

    // -----------------------------
    // Variabelen voor maan-flikkering voorkomen
    // -----------------------------
    private bool showMoonThisNight = false; // of de maan deze nacht zichtbaar is
    private bool nightStarted = false;      // om te detecteren wanneer de nacht start

    void Update()
    {
        UpdateTime();
        UpdateSun();
        UpdateMoon();
        UpdateLightActivation();
        UpdateSkybox();
        UpdateTimeUI();
    }

    // -----------------------------
    // 1️⃣ Tijd bijwerken
    // -----------------------------
    private void UpdateTime()
    {
        time += Time.deltaTime;
    }

    // -----------------------------
    // 2️⃣ Zon rotatie en intensiteit
    // -----------------------------
    private void UpdateSun()
    {
        if (sunLight == null) return;

        float sunRotation = TimePercent * 360f - 90f;
        sunLight.transform.rotation = Quaternion.Euler(sunRotation, 170f, 0f);

        float sunIntensity;
        if (TimePercent < 0.25f) sunIntensity = Mathf.Lerp(0f, 1f, TimePercent * 4f);
        else if (TimePercent > 0.75f) sunIntensity = Mathf.Lerp(1f, 0f, (TimePercent - 0.75f) * 4f);
        else sunIntensity = 1f;

        sunLight.intensity = sunIntensity;
    }

    // -----------------------------
    // 3️⃣ Maan rotatie en fase
    // -----------------------------
    private void UpdateMoon()
    {
        if (moonPivot == null || moonLight == null) return;

        // Langzame dagelijkse rotatie van de maan
        float moonDegreesPerSecond = 360f / (moonSynodicDay * 3600f);
        moonPivot.Rotate(Vector3.right, moonDegreesPerSecond * Time.deltaTime);

        // Bereken fase van de maan (0 = nieuwe maan, 1 = volle maan)
        float lunarRotation = (time / (moonPhaseDays * dayDuration)) * 360f;
        float phase = Mathf.Cos(lunarRotation * Mathf.Deg2Rad); // -1 = nieuw, 1 = vol

        // Schakel juiste maanbolletjes in op basis van fase
        if (phase > 0.33f)
        {
            if (FullMoonSphere != null) FullMoonSphere.SetActive(true);
            if (HalfMoonSphere != null) HalfMoonSphere.SetActive(false);
        }
        else
        {
            if (FullMoonSphere != null) FullMoonSphere.SetActive(false);
            if (HalfMoonSphere != null) HalfMoonSphere.SetActive(true);
        }

        // Rotatie rond eigen as voor 3D-effect
        if (FullMoonSphere != null && FullMoonSphere.activeSelf)
            FullMoonSphere.transform.Rotate(Vector3.up, 10f * Time.deltaTime);
        //if (HalfMoonSphere != null && HalfMoonSphere.activeSelf)
           // HalfMoonSphere.transform.Rotate(Vector3.up, 10f * Time.deltaTime);
    }

    // -----------------------------
    // 4️⃣ Licht activeren en maan zichtbaar maken
    // -----------------------------
    private void UpdateLightActivation()
    {
        bool isNight = CurrentHour < sunriseHour || CurrentHour >= sunsetHour;

        // Bepaal één keer per nacht of de maan zichtbaar is
        if (isNight)
        {
            if (!nightStarted)
            {
                showMoonThisNight = Random.value < nightMoonChance;
                nightStarted = true; // nacht gestart, dus geen nieuwe random meer
            }
        }
        else
        {
            // Reset bij dag
            nightStarted = false;
            showMoonThisNight = false;
        }

        // Zon aan/uit
        if (sunLight != null) sunLight.enabled = !isNight;

        // Maan aan/uit volgens random nachtvisibiliteit
        if (moonLight != null) moonLight.enabled = showMoonThisNight;
        if (FullMoonSphere != null) FullMoonSphere.SetActive(showMoonThisNight && FullMoonSphere.activeSelf);
        if (HalfMoonSphere != null) HalfMoonSphere.SetActive(showMoonThisNight && HalfMoonSphere.activeSelf);
    }

    // -----------------------------
    // 5️⃣ Skybox overgang dag/nacht
    // -----------------------------
    private void UpdateSkybox()
    {
        if (daySkybox == null || nightSkybox == null || sunLight == null) return;

        float t = sunLight.intensity;
        RenderSettings.skybox.Lerp(nightSkybox, daySkybox, t);
        DynamicGI.UpdateEnvironment();
    }

    // -----------------------------
    // 6️⃣ Tijd in UI
    // -----------------------------
    private void UpdateTimeUI()
    {
        if (timeDisplay == null) return;

        int hour = Mathf.FloorToInt(CurrentHour);
        int minute = Mathf.FloorToInt((CurrentHour - hour) * 60f);
        timeDisplay.text = $"{hour:00}:{minute:00}";
    }

    // -----------------------------
    // Check of het dag is
    // -----------------------------
    public bool IsDay()
    {
        return CurrentHour >= sunriseHour && CurrentHour < sunsetHour;
    }
}
