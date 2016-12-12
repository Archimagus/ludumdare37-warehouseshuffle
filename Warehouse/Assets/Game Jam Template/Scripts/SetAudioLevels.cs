using UnityEngine;
using UnityEngine.Audio;

public class SetAudioLevels : MonoBehaviour
{
	public AudioMixer mainMixer;                    //Used to hold a reference to the AudioMixer mainMixer

	public void SetMasterVolume(float level)
	{
		var vol = level.todB();
		mainMixer.SetFloat("masterVol", vol);
	}

	//Call this function and pass in the float parameter musicLvl to set the volume of the AudioMixerGroup Music in mainMixer
	public void SetMusicLevel(float level)
	{
		var vol = level.todB();
		mainMixer.SetFloat("musicVol", vol);
	}

	//Call this function and pass in the float parameter sfxLevel to set the volume of the AudioMixerGroup SoundFx in mainMixer
	public void SetSfxLevel(float level)
	{
		var vol = level.todB();
		mainMixer.SetFloat("sfxVol", vol);
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