using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {
	public Settings subtitleSettings;

	[SerializeField]
	private Toggle activateSubtitles;
	[SerializeField]
	private GameObject SubtitleSettingsUI;

	private string subtitleLanguage = "";
	private bool setter;

	private void Start() {
		if (subtitleSettings.displaySubtitles) {
			activateSubtitles.isOn = true;
			SubtitleSettingsUI.SetActive(true);
		} else {
			activateSubtitles.isOn = false;
			SubtitleSettingsUI.SetActive(false);
		}
	}

	public void SetSubtitleActive() {
		if (activateSubtitles.isOn) {
			subtitleSettings.displaySubtitles = true;
			SubtitleSettingsUI.SetActive(true);
		} else {
			subtitleSettings.displaySubtitles = false;
			SubtitleSettingsUI.SetActive(false);
		}
	}

	public void ChangeSubtitleLanguage(Text language) {
		subtitleLanguage = language.text;

		switch (subtitleLanguage) {
			case "Deutsch":
				subtitleLanguage = "Englisch";
				language.text = "Englisch";
				subtitleSettings.isEnglish = true;
				break;

			case "Englisch":
				subtitleLanguage = "Deutsch";
				language.text = "Deutsch";
				subtitleSettings.isEnglish = false;
				break;

			default:
				subtitleLanguage = "Englisch";
				language.text = "Englisch";
				subtitleSettings.isEnglish = true;
				break;
		}
	}
}
