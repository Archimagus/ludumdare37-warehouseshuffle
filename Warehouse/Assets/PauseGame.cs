using UnityEngine;

public class PauseGame : MonoBehaviour
{
	void OnEnable()
	{
		Time.timeScale = float.Epsilon;
	}

	void OnDisable()
	{
		Time.timeScale = 1f;
	}
}
