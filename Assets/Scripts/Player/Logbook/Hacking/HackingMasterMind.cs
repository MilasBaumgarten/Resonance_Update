// Author: Noah Stolz
// Used to store some refernces for the Hacking Master Mind minigame

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackingMasterMind : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The parent of all the solution colums")]
    private Transform solution;

    [SerializeField]
    [Tooltip("The parent of all the solution colums")]
    private Transform iconButtons;

    [SerializeField]
    private GameObject columPrefab;

    [SerializeField]
    private Sprite[] resultSprites;

    [SerializeField]
    private int MaxNummberOfTries;

    [SerializeField]
    private Hacking hacking;

    int nummberOfColors;

    int currentPosition = 0;

    int[] correctCode;
    List<int> correctCodeDistribution;
    int[] currentCode;

    int triesLeft;

    List<Image> slots = new List<Image>();
    List<Image> results = new List<Image>();
    List<Image> icons = new List<Image>();
    List<GameObject> buttons = new List<GameObject>();
    List<GameObject> colums = new List<GameObject>();

    public void InitMasterMind()
    {

        int nummberOfTries = MaxNummberOfTries;

        for (int i = 0; i < nummberOfTries; i++)
        {
            
            GameObject col = Instantiate(columPrefab, solution);

            colums.Add(col);

            for (int k = 0; k < 4; k++)
            {

                slots.Add(col.transform.GetChild(k).GetChild(0).GetComponent<Image>());
                
            }

            for(int k = 0; k < 4; k++)
            {

                results.Add(col.transform.GetChild(4).GetChild(k).GetChild(0).GetComponent<Image>());

            }
        }

         foreach (Transform child in iconButtons)
         {

            buttons.Add(child.gameObject);
            icons.Add(child.GetChild(0).GetComponent<Image>());

         }

    }

    public void StartMasterMind(int nummberOfTries, int nummberOfColors)
    {
        // Set the right nummber of buttons active
        for(int i = 0; i < buttons.Count; i++)
        {

            buttons[i].SetActive(false);

            if (i < nummberOfColors)
            {
                buttons[i].SetActive(true);

            }

        }

        // Set the right nummber of colums active
        for (int i = 0; i < colums.Count; i++)
        {

            colums[i].SetActive(false);

            if (i < nummberOfTries)
                colums[i].SetActive(true);

        }

        // Clear the slots
        for (int i = 0; i < slots.Count; i++)
        {

            slots[i].enabled = false;

        }

        // Clear results
        for (int i = 0; i < results.Count; i++)
        {

            results[i].enabled = false;

        }

        this.nummberOfColors = nummberOfColors;
        triesLeft = nummberOfTries;
        currentPosition = 0;
        currentCode = new int[4];
        GenerateSolution(nummberOfColors);
    }

    public void AddChoice(int icon)
    {
        slots[currentPosition].color = Color.white;
        slots[currentPosition].sprite = icons[icon].sprite;
        slots[currentPosition].enabled = true;

        currentCode[currentPosition % 4] = icon;

        if((currentPosition + 1) % 4 == 0)
        {

            int right = 0;
            int wrongPosition = 0;

            if (CheckForSuccess(out right, out wrongPosition)) {

                hacking.OnSuccess();

            } else
            {
                ShowResult(right, wrongPosition);
                currentCode = new int[4];

                triesLeft--;

                if(triesLeft <= 0)
                {

                    hacking.StopHacking();

                }

            }

        }

        currentPosition++;

    }

    /// <summary>Used to add a choice without a button</summary>
    /// <param name="icon">The value of the added button</param>
    /// <param name="random">If true icon will be overwritten with a random value</param>
    public void AddChoice(int icon, bool random)
    {
        if (currentCode == null)
        {

            return;

        }

        if (random)
        {

            icon = Random.Range(icon, nummberOfColors);

        }

        slots[currentPosition].color = Color.red;
        slots[currentPosition].sprite = icons[icon].sprite;
        slots[currentPosition].enabled = true;
        
        currentCode[currentPosition % 4] = icon;

        if ((currentPosition + 1) % 4 == 0)
        {

            int right = 0;
            int wrongPosition = 0;

            if (CheckForSuccess(out right, out wrongPosition))
            {

                hacking.OnSuccess();

            }
            else
            {
                ShowResult(right, wrongPosition);
                currentCode = new int[4];

                triesLeft--;

                if (triesLeft <= 0)
                {

                    hacking.StopHacking();

                }

            }

        }

        currentPosition++;

    }

    bool CheckForSuccess(out int right, out int wrongPosition)
    {

        bool success = true;
        
        List<int> correctCodeDistCopy = new List<int>();

        for(int i = 0; i < correctCodeDistribution.Count; i++)
        {

            correctCodeDistCopy.Add(correctCodeDistribution[i]);

        }

        right = 0;
        wrongPosition = 0;

        for (int i = 0; i < correctCode.Length; i++)
        {

            if (correctCode[i] == currentCode[i])
            {
                if (hacking.debugMode)
                    print("right: " + correctCodeDistCopy[currentCode[i]].ToString());
                correctCodeDistCopy[currentCode[i]]--;
                right++;

            }
            else
            {

                success = false;

            }

        }

        for (int i = 0; i < correctCode.Length; i++)
        {
            if (correctCodeDistCopy[currentCode[i]] > 0 && !(correctCode[i] == currentCode[i]))
            {

                if (hacking.debugMode)
                    print("wrong: " + correctCodeDistCopy[currentCode[i]].ToString());
                correctCodeDistCopy[currentCode[i]]--;
                wrongPosition++;

                success = false;

            } 

        }

        return success;
    }

    void GenerateSolution(int nummberOfColors)
    {

        correctCode = new int[4];
        correctCodeDistribution = new List<int>();

        for(int i = 0; i < nummberOfColors; i++)
        {

            correctCodeDistribution.Add(0);

        }

        string solutinText = "|";

        for (int i = 0; i < 4; i++)
        {

            correctCode[i] = Random.Range(0, nummberOfColors);

            correctCodeDistribution[correctCode[i]]++;

            solutinText += correctCode[i].ToString() + "|";

        }

        if(hacking.debugMode)
            print(solutinText);

    }

    void ShowResult(int right, int wrongPosition)
    {

        int indexCorrection = -3;

        for(int i = 0; i < 4; i++)
        {

            if(right > 0)
            {

                results[currentPosition + indexCorrection].sprite = resultSprites[1];
                results[currentPosition + indexCorrection].enabled = true;
                right--;
                indexCorrection++;

            }else if (wrongPosition > 0)
            {

                results[currentPosition + indexCorrection].sprite = resultSprites[0];
                results[currentPosition + indexCorrection].enabled = true;
                wrongPosition--;
                indexCorrection++;

            }

        }
                
    }
}
