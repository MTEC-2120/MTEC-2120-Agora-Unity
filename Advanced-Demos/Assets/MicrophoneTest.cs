using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneTest : MonoBehaviour
{

	AudioSource audioSource;

	string selectedDevice; 
	string[] devices; 

	bool debug = true;

	public int maxFreq = 44100;//48000;

	//The maximum amount of sample data that gets loaded in, best is to leave it on 256, unless you know what you are doing. 
	//A higher number gives more accuracy but lowers performance alot, it is best to leave it at 256.
	public int amountSamples = 256;

	public void Start()
    {

		audioSource = GetComponent<AudioSource>(); 
		devices = Microphone.devices; 

		selectedDevice = devices[1];

	}
    /*
	 * For the above control the mic start or stop
	 */
    public void StartMicrophone()
	{
		//Starts recording
		audioSource.clip = Microphone.Start(selectedDevice, true, 10, maxFreq);

		if (debug)
		{
			Debug.Log("Selected device: " + selectedDevice);
		}

		// Wait until the recording has started
		while (!(Microphone.GetPosition(selectedDevice) > 0))
		{
			if (debug)
			{
				Debug.Log("Waiting on recording to start...");
			}
		}

		if (debug)
		{
			Debug.Log("Playing the recorded audio...");
		}
		// Play the audio recording
		audioSource.Play();

		Debug.Log("Audio data is : " + GetDataStream());
	}

	/*
 * The main data stream from the microphone
 */
	float GetDataStream()
	{
		if (Microphone.IsRecording(selectedDevice))
		{
			float[] samples = new float[amountSamples]; //Converts to a float
														//float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];

			audioSource.clip.GetData(samples, 0);
			return Sum(samples) / amountSamples;
		}
		else
		{
			Debug.Log("The active microphone is not recording.");
			return 0.0f;
		}
	}

	private float Sum(params float[] samples)
	{
		float result = 0.0f;
		for (int i = 0; i < samples.Length; i++)
		{
			result += Mathf.Abs(samples[i]);
		}
		return result;
	}


	public void StopMicrophone()
	{
		if (debug)
		{
			Debug.Log("Stopping the microphone...");
		}

		//Stops the audio
		audioSource.Stop();

		//Stops the recording of the device
		Microphone.End(selectedDevice);

	}
}
