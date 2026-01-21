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
    public float nightMoonChance = 1f; 

    private const float sunriseHour = 6f;
    private const float sunsetHour = 18f;
    private const float moonSynodicDay = 24.84f; 
    private const float moonPhaseDays = 29.5f;   

    private float TimePercent => (time % dayDuration) / dayDuration;
    private float CurrentHour => TimePercent * 24f;

    private bool showMoonThisNight = false; 
    private bool nightStarted = false;      

    void Update()
    {
        UpdateTime();
        UpdateSun();
        UpdateMoon();
        UpdateLightActivation();
        UpdateSkybox();
        UpdateTimeUI();
    }

    private void UpdateTime()
    {
        time += Time.deltaTime;
    }

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

    private void UpdateMoon()
    {
        if (moonPivot == null || moonLight == null) return;

        // Maan rotatie om de aarde
        float moonDegreesPerSecond = 360f / (moonSynodicDay * 3600f);
        moonPivot.Rotate(Vector3.right, moonDegreesPerSecond * Time.deltaTime);

        // Rotatie van de bollen zelf voor 3D effect
        if (FullMoonSphere != null && FullMoonSphere.activeSelf)
            FullMoonSphere.transform.Rotate(Vector3.up, 10f * Time.deltaTime);
        
        if (HalfMoonSphere != null && HalfMoonSphere.activeSelf)
            HalfMoonSphere.transform.Rotate(Vector3.up, 10f * Time.deltaTime);
    }

    private void UpdateLightActivation()
    {
        bool isNight = CurrentHour < sunriseHour || CurrentHour >= sunsetHour;

        // 1. Bepaal of het nacht is en of de maan mag schijnen
        if (isNight)
        {
            if (!nightStarted)
            {
                showMoonThisNight = Random.value < nightMoonChance;
                nightStarted = true; 
            }
        }
        else
        {
            nightStarted = false;
            showMoonThisNight = false;
        }

        if (sunLight != null) sunLight.enabled = !isNight;
        if (moonLight != null) moonLight.enabled = showMoonThisNight;

        // 2. Bepaal de MAANFASE (Full of Half)
        float lunarRotation = (time / (moonPhaseDays * dayDuration)) * 360f;
        float phase = Mathf.Cos(lunarRotation * Mathf.Deg2Rad); // -1 = nieuw, 1 = vol
        bool shouldBeFull = phase > 0.33f;

        // 3. Zet de juiste bol aan/uit (en zorg dat ze overdag uit zijn)
        if (FullMoonSphere != null) 
        {
            FullMoonSphere.SetActive(showMoonThisNight && shouldBeFull);
        }

        if (HalfMoonSphere != null) 
        {
            HalfMoonSphere.SetActive(showMoonThisNight && !shouldBeFull);
        }
    }

    private void UpdateSkybox()
    {
        if (daySkybox == null || nightSkybox == null || sunLight == null) return;

        float t = sunLight.intensity;
        RenderSettings.skybox.Lerp(nightSkybox, daySkybox, t);
        DynamicGI.UpdateEnvironment();
    }

    private void UpdateTimeUI()
    {
        if (timeDisplay == null) return;

        int hour = Mathf.FloorToInt(CurrentHour);
        int minute = Mathf.FloorToInt((CurrentHour - hour) * 60f);
        timeDisplay.text = $"{hour:00}:{minute:00}";
    }

    public bool IsDay()
    {
        return CurrentHour >= sunriseHour && CurrentHour < sunsetHour;
    }
}