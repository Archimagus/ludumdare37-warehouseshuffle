using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudioLevels : MonoBehaviour
{
	public AudioMixer mainMixer;                    //Used to hold a reference to the AudioMixer mainMixer
	public Slider masterVolumeSlider;
	public Slider musicVolumeSlider;
	public Slider sfxVolumeSlider;

	private bool visible;
	void OnEnable()
	{
		visible = true;
		LoadVolumes();
	}
	void OnDisable()
	{
		visible = false;
	}
	public void LoadVolumes()
	{
		try
		{
			var vol = PlayerPrefs.GetFloat("masterVol", 1.0f);
			mainMixer.SetFloat("masterVol", vol.todB());
			masterVolumeSlider.value = vol;

			vol = PlayerPrefs.GetFloat("musicVol", 1.0f);
			mainMixer.SetFloat("musicVol", vol.todB());
			musicVolumeSlider.value = vol;

			vol = PlayerPrefs.GetFloat("sfxVol", 1.0f);
			mainMixer.SetFloat("sfxVol", vol.todB());
			sfxVolumeSlider.value = vol;

		}
		catch (Exception)
		{
			Debug.Log("What the fuck", this);
		}

	}

	public void SetMasterVolume(float level)
	{
		if (!visible)
			return;
		var vol = level.todB();
		mainMixer.SetFloat("masterVol", vol);
		PlayerPrefs.SetFloat("masterVol", level);
		PlayerPrefs.Save();
	}

	//Call this function and pass in the float parameter musicLvl to set the volume of the AudioMixerGroup Music in mainMixer
	public void SetMusicLevel(float level)
	{
		if (!visible)
			return;
		var vol = level.todB();
		mainMixer.SetFloat("musicVol", vol);
		PlayerPrefs.SetFloat("musicVol", level);
		PlayerPrefs.Save();
	}

	//Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup SoundFx in mainMixer
	public void SetSfxLevel(float level)
	{
		if (!visible)
			return;
		var vol = level.todB();
		mainMixer.SetFloat("sfxVol", vol);
		PlayerPrefs.SetFloat("sfxVol", level);
		PlayerPrefs.Save();
	}
}
public static class volumeMath
{
	public static float todB(this float lin)
	{
		if (lin <= float.Epsilon)
			return -80;
		return Mathf.Log(lin) * 20;
	}

	public static float toLin(this float db)
	{
		return Mathf.Pow(10, db / 20);
	}

}