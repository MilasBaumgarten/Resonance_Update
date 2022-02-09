using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuffleList : MonoBehaviour {

    private List<Text> texts = new List<Text>();

    void Awake()
    {

        foreach (Transform child in this.transform)
        {

            texts.Add(child.GetComponentInChildren<Text>());

        }

    }

    void OnEnable()
    {

        int n = texts.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, texts.Count);
            string text = texts[k].text;
            texts[k].text = texts[n].text;
            texts[n].text = text;
        }

    }
}
