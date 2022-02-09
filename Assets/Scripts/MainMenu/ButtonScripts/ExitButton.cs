/* Script that controls exit button function
 * 
 * must be a component of button object
 * need OnClick() reference
 * 
 * Author: Maximilian Rietzsch
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

    public void ExitApplication(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
