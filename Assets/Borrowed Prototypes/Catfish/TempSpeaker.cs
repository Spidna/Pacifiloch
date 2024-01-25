using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpeaker : MonoBehaviour
{
    public TutorialEvents events;
    public float range;
    private Quaternion og;

    private void Start()
    {
        events.onUpdate += rockAbout;
        og = transform.rotation;
    }
    private void OnDestroy()
    {
        events.onUpdate -= rockAbout;
    }

    public void rockAbout()
    {
        if(events.voice.isPlaying)
        {
            // Set the size of the float array based on the desired frequency resolution
            int sampleSize = 1024;
            float[] samples = new float[sampleSize];

            // Get output data from the audio source
            events.voice.GetOutputData(samples, 0);

            // Calculate the RMS (Root Mean Square) of the audio samples
            float rmsValue = 0.0f;
            foreach (float sample in samples)
            {
                rmsValue += sample * sample;
            }
            rmsValue /= sampleSize;
            rmsValue = Mathf.Sqrt(rmsValue);



            transform.rotation = Quaternion.Euler(Random.Range(-range, range) * rmsValue, 0f, 0f) * og;
        }
    }
}
