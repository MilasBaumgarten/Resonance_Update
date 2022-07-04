using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {
	public Settings subtitleSettings;

	[SerializeField]
	private Toggle activateSubtitles;
	[SerializeField]
	private GameObject SubtitleSettingsUI;
	[SerializeField]
	private Text subtitleLanguageText;

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

	public void SetSubtitleState(bool state) {
		if (state) {
			subtitleSettings.displaySubtitles = true;
			SubtitleSettingsUI.SetActive(true);
		} else {
			subtitleSettings.displaySubtitles = false;
			SubtitleSettingsUI.SetActive(false);
		}

		if (subtitleSettings.isEnglish) {
			subtitleLanguage = "Englisch";
			subtitleLanguageText.text = "Englisch";
			subtitleSettings.isEnglish = true;
		} else {
			subtitleLanguage = "Deutsch";
			subtitleLanguageText.text = "Deutsch";
			subtitleSettings.isEnglish = false;
		}
	}

	public void SwitchSuptitleLanguage() {
		if (subtitleSettings.isEnglish) {
			subtitleLanguage = "Deutsch";
			subtitleLanguageText.text = "Deutsch";
			subtitleSettings.isEnglish = false;
		} else {
			subtitleLanguage = "Englisch";
			subtitleLanguageText.text = "Englisch";
			subtitleSettings.isEnglish = true;
		}
	}
}
