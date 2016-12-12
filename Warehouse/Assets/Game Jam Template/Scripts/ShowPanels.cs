using UnityEngine;

public class ShowPanels : MonoBehaviour
{

	public GameObject optionsPanel;                         //Store a reference to the Game Object OptionsPanel 
	public GameObject optionsTint;                          //Store a reference to the Game Object OptionsTint 
	public GameObject menuPanel;                            //Store a reference to the Game Object MenuPanel 
	public GameObject pausePanel;                           //Store a reference to the Game Object PausePanel 
	public GameObject gameOverPanel;                        //Store a reference to the Game Object GameOverPanel
	public GameObject gamePanel;                            //Store a reference to the Game Object GamePanel

	//Call this function to activate and display the Options panel during the main menu
	public void ShowOptionsPanel()
	{
		optionsPanel.SetActive(true);
		optionsTint.SetActive(true);
	}

	//Call this function to deactivate and hide the Options panel during the main menu
	public void HideOptionsPanel()
	{
		optionsPanel.SetActive(false);
		optionsTint.SetActive(false);
	}

	private GameObject _gamePanelInstance;
	//Call this function to deactivate and hide the Game Over panel during gameplay
	public void HideGamePanel()
	{
		Destroy(_gamePanelInstance);
	}
	//Call this function to activate and display the Game Over panel during gameplay
	public void ShowGamePanel()
	{
		_gamePanelInstance = Instantiate(gamePanel);
		_gamePanelInstance.transform.SetAsFirstSibling();
	}


	//Call this function to activate and display the Game Over panel during gameplay
	public void ShowGameOverPanel()
	{
		gameOverPanel.SetActive(true);
	}

	//Call this function to deactivate and hide the Game Over panel during gameplay
	public void HideGameOverPanel()
	{
		gameOverPanel.SetActive(false);
	}

	//Call this function to activate and display the main menu panel during the main menu
	public void ShowMenu()
	{
		menuPanel.SetActive(true);
	}

	//Call this function to deactivate and hide the main menu panel during the main menu
	public void HideMenu()
	{
		menuPanel.SetActive(false);
	}

	//Call this function to activate and display the Pause panel during game play
	public void ShowPausePanel()
	{
		pausePanel.SetActive(true);
		optionsTint.SetActive(true);
	}

	//Call this function to deactivate and hide the Pause panel during game play
	public void HidePausePanel()
	{
		pausePanel.SetActive(false);
		optionsTint.SetActive(false);

	}
}