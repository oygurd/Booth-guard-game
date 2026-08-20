using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class DayTimeManager : MonoBehaviour
{
    public Light sun;
    public LightingSettings lightingSettings;
    
    public float dayStartTime;
    public float timer;

    public float sunPosition;

    private float timePassed;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = dayStartTime;
        sun =  GetComponent<Light>();
        sun.colorTemperature = 2300;

        RenderSettings.ambientIntensity = 1;

    }

    // Update is called once per frame
    void Update()
    {
        DayTimeCounter();
    }
    
    public void DayTimeCounter()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            sunPosition += Time.deltaTime * 180 / dayStartTime;
        }
        
        transform.rotation = Quaternion.Euler(sunPosition, 70, 0);

        sun.colorTemperature += Time.deltaTime * 20000 / dayStartTime;
        sun.intensity -= Time.deltaTime * 2 / dayStartTime;
        
        RenderSettings.ambientIntensity -= Time.deltaTime * 1 / dayStartTime;
        RenderSettings.reflectionIntensity -= Time.deltaTime * 1 / dayStartTime;
        
       timePassed += Time.deltaTime;
       if (timePassed > 20)
       {
           StartCoroutine(updateLighting(10));
           timePassed = 0;
       }
        
    }

    

    IEnumerator updateLighting(float interval)
    {
        
        yield return new WaitForSeconds(interval);
        DynamicGI.UpdateEnvironment();
    }
}
