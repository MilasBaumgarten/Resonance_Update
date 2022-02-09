using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveModulesOnStart : MonoBehaviour {

	IEnumerator Start () {
		// Wait until Gamemanager has instances
		yield return new WaitUntil(() => GameManager.instance.robert != null);
		
		GameManager.instance.catriona.GetComponent<ArmTool>().RemoveModule(typeof(ForceModule));
		GameManager.instance.catriona.GetComponent<ArmTool>().RemoveModule(typeof(ExtinguisherModule));
		GameManager.instance.robert.GetComponent<ArmTool>().RemoveModule(typeof(ForceModule));
		GameManager.instance.robert.GetComponent<ArmTool>().RemoveModule(typeof(ExtinguisherModule));
	}
}
