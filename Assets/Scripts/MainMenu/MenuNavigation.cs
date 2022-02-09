using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour {

    [SerializeField]
    private Transform contentUI;

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    [Range(0.1f, 5.0f)]
    private float transitionSpeed = 0.5f;

    private Color alphaTransition;

    private float alphaFloat;
    private bool changeToCredits = false;

    private GameObject MatchingUI;
    private GameObject[] ddolRoots;
    private List<GameObject> contentRegister = new List<GameObject>();

    private void Update()
    {
        if (changeToCredits)
        {
            if(alphaFloat < 1)
            {
                alphaFloat += Time.deltaTime * transitionSpeed;
                alphaTransition.a = alphaFloat;
                fadeImage.color = alphaTransition;
            }

            if(alphaFloat >= 1)
            {
                SceneManager.LoadScene("Credits");
            }
        }
    }


    private void Start()
    {
        alphaTransition = fadeImage.color;
        alphaFloat = fadeImage.color.a;

        for (int i = 0; i<contentUI.childCount; i++)
        {
            contentRegister.Add(contentUI.GetChild(i).gameObject);
        }
    }

    public void SetMatchingUI(GameObject _matchingUI)
    {
		MatchingUI = _matchingUI;
    }

    public void OnClickNavigation(Button button)
    {
        string panelName = "";

        switch (button.name)
        {
            #region BaseUI
            case "AchievementsButton":
                panelName = "AchievementsPanel";
                MatchingUI.SetActive(false);
                break;

            case "OptionsButton":
                panelName = "OptionsPanel";
                MatchingUI.SetActive(false);
                break;

            case "CreditsButton":
                fadeImage.gameObject.SetActive(true);
                changeToCredits = true;
                //panelName = "CreditsPanel";
                //MatchingUI.SetActive(false);
                break;

            case "ExitButton":
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
                //panelName = "ExitPanel";
                //MatchingUI.SetActive(false);
                break;

            #endregion
            case "StartButton":
                MatchingUI.SetActive(true);

                foreach (GameObject register in contentRegister)
                {
                    register.gameObject.SetActive(false);
                }
                break;

            default:
                break;
        }
        
        foreach (GameObject register in contentRegister)
        {
            if (!register.name.Equals(panelName))
            {
                register.gameObject.SetActive(false);
            }

            else
                register.gameObject.SetActive(true);
        }
    }
}

