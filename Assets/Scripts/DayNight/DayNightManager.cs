using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro; // using text mesh for the clock display

using UnityEngine.Rendering; // used to access the volume component
using UnityEngine.Rendering.Universal;

public class DayNightManager : MonoBehaviour
{
    //public TextMeshProUGUI timeDisplay; // Display Time
    //public TextMeshProUGUI dayDisplay; // Display Day
    public Light2D globalLight2D;
    public Light2D insideGlobalLight2D;

    public float tick; // Increasing the tick, increases second rate
    public float seconds;
    public int mins;
    public int hours;
    public int days = 1;

    public bool activateLights; // checks if lights are on
    private Light2D[] lights; // all the lights we want on when its dark


    public SpriteRenderer[] stars; // star sprites 

    private float lightIntensity;
    float t = 0;

    public static DayNightManager i { get; private set; }
    public Light2D[] Lights { get => lights; set => lights = value; }

    private void Awake()
    {
        i = this;
    }

    // Update is called once per frame
    public void HandleUpdate() // we used fixed update, since update is frame dependant. 
    {
        CalcTime();
        DisplayTime();
        ControlLights();
    }

    public void CalcTime() // Used to calculate sec, min and hours
    {
        seconds += Time.fixedDeltaTime * tick; // multiply time between fixed update by tick

        if (seconds >= 60) // 60 sec = 1 min
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
        }

        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
            days += 1;
        }
    }

   
        public void ControlLights() // used to adjust the post processing slider.
    {
        DayNightManager.i.Lights = GameObject.FindObjectsOfType<Light2D>();
         var day = new Color(1, 1, 1);

        var night = new Color(0.6f, 0.55f, 1);
       

        if (hours > 21 && hours < 24 || hours > 0 && hours < 6) {

            if (lightIntensity >= 0.5f)
                lightIntensity -= 0.005f;


            if (globalLight2D.color == night)
                t = 0;
            else {
                t += Time.fixedDeltaTime / 2.0f;
                globalLight2D.color = Color.Lerp(day, night, t);
            }


            globalLight2D.intensity = lightIntensity;


            if (lightIntensity <= 0.6f) {
                for (int i = 0; i < Lights.Length; i++)
                {
                    if (Lights[i] != globalLight2D && Lights[i] != insideGlobalLight2D)
                    Lights[i].enabled = true; // turn them all on
                }
                globalLight2D.enabled = true;
                insideGlobalLight2D.enabled = true;
            }


        }

        if (hours > 6 && hours < 21)
        {
            if (lightIntensity <= 0.8f)
                lightIntensity += 0.005f;

            if (globalLight2D.color == day)
                t = 0;
            else {
                t += Time.fixedDeltaTime / 2.0f;
                globalLight2D.color = Color.Lerp(night, day, t);
            }


            globalLight2D.intensity = lightIntensity;


            if (lightIntensity >= 0.65f)
            {
                for (int i = 0; i < Lights.Length; i++)
                {
                    if (Lights[i] != globalLight2D && Lights[i] != insideGlobalLight2D)
                        Lights[i].enabled = false; // turn them all on
                }
            }
            globalLight2D.enabled = true;
            insideGlobalLight2D.enabled = true;
        }
    }

    public void DisplayTime() // Shows time and day in ui
    {

        //timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins); // The formatting ensures that there will always be 0's in empty spaces
        //dayDisplay.text = "Day: " + days; // display day counter
    }
}