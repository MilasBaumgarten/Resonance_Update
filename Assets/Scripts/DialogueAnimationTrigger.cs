using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimationTrigger : MonoBehaviour {
    public string[] catrionaAnimationQueue;
    public float[] catrionaTimeQueue;
    public string[] robertAnimationQueue;
    public float[] robertTimeQueue;

    [SerializeField]
    private Animator robertAnim;
    [SerializeField]
    private Animator catrionaAnim;

    private bool isTriggered;

void OnTriggerEnter(Collider other){
    if(isTriggered)
	    PlayAnimations();
}

private IEnumerator Start()
{
        isTriggered = true;
    yield return new WaitUntil(() => GameManager.instance.robert);
    
        robertAnim = GameManager.instance.robert.GetComponentInChildren<Animator>();
        catrionaAnim = GameManager.instance.catriona.GetComponentInChildren<Animator>();
    }

    public void PlayAnimations()
    {
        isTriggered = false;
        StartCoroutine(RobertAnimations());
        StartCoroutine(CatrionaAnimations());
    }

    private IEnumerator RobertAnimations() {
        for (int i = 0; i < robertTimeQueue.Length; i++) {

            yield return new WaitForSeconds(robertTimeQueue[i]);

            robertAnim.SetTrigger(robertAnimationQueue[i].ToLower());
        }
    }
    
    private IEnumerator CatrionaAnimations() {
        for (int i = 0; i < catrionaTimeQueue.Length; i++) {
            
            yield return new WaitForSeconds(catrionaTimeQueue[i]);
            
            catrionaAnim.SetTrigger(catrionaAnimationQueue[i].ToLower());
        }
    }
}