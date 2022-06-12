// Author: Noah Stolz
// Script made out of laziness

using UnityEngine;

public class ResearchConsole : MonoBehaviour {

	[SerializeField]
	private LogbookManager logbookManager;

	public void Open() {
		logbookManager.OpenLogbook();
		logbookManager.EnableOnePanel("research");
	}
}
