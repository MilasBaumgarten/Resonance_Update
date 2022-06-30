using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPrompt : MonoBehaviour
{
    
    [SerializeField] private Image promptImage;
    [SerializeField] private Image viewModifier, grabModifier;

    private void Start() {
        promptImage.enabled = viewModifier.enabled = grabModifier.enabled = false;
    }

    public void TogglePrompt(bool onOff, bool isGrabable){
        promptImage.enabled = onOff;
        viewModifier.enabled = onOff && !isGrabable;
        grabModifier.enabled = onOff && isGrabable;
    }

}
