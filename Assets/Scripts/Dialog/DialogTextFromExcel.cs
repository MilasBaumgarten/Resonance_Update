using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

/**
 * should be attached to: DialogManager
 * Author: Leon Ullrich
 * Description: 
 *  -this script reads and parses a csv file and puts the read values in public variables
 *  -put this on the DialogManager-GameObject in the scene
 *  -script takes filename from DialogTrigger.cs
 *  -CSV-Files are stored in Assets/resources/csv
 *  
 *  Author: Marisa Schmelzer
 *  Description:
 *   -added audio to this script
 *   
 *  Changes: Noah Stolz
 *  Added oneliner support
 *  
 */
public class DialogTextFromExcel {

	// use these variables as reference from other scripts  
	public List<float> timeToDisplay = new List<float>();
	public List<string> germanSubtitles = new List<string>();
	public List<string> englishSubtitles = new List<string>();
	public List<string> audio = new List<string>();

	// variables for one liners 
	public List<string> oneLinerID = new List<string>();
    public List<float>  oneLinerPlayTimer = new List<float>();
	public List<string> oneLinerGermanSubtitles = new List<string>();
	public List<string> oneLinerEnglishSubtitles = new List<string>();
	public List<string> oneLinerAudio = new List<string>();

	/// <summary>
	/// Get the id, subtitles and audiofiles from the oneliner.csv
	/// </summary>
	public void GetOneLinerValues() {
		// open streamreader 
		using (var reader = new StreamReader(Application.streamingAssetsPath + "/resources/csv/oneliner.csv")) {

			// while streamreader has not reached end of file
			while (!reader.EndOfStream) {

				// read every single line
				var line = reader.ReadLine();
				// split the line into seperate values
				var values = SplitCsvOneLiner(line);

				// assign values to different lists
				oneLinerID.Add(values[0]);
				oneLinerPlayTimer.Add(float.Parse(values[1], CultureInfo.InvariantCulture));
				oneLinerGermanSubtitles.Add(values[2]);
				oneLinerEnglishSubtitles.Add(values[3]);

                string germanAudio = values[4].Replace("\"", "");
                germanAudio = germanAudio.Trim(' ');
                germanAudio = germanAudio.Split('.')[0];
				oneLinerAudio.Add(germanAudio);
			}

		}

	}

	public void GetValues(string filename) {
		// open streamreader
		using (var reader = new StreamReader(Application.streamingAssetsPath + "/resources/csv/" + filename + ".csv")) {
			// while streamreader has not reached end of file
			while (!reader.EndOfStream) {
				// read every single line
				var line = reader.ReadLine();
                // split the line into seperate values
                var values = SplitCsvLine(line);
				// assign values to different lists
				timeToDisplay.Add(float.Parse(values[0], CultureInfo.InvariantCulture));
				germanSubtitles.Add(values[1]);
				englishSubtitles.Add(values[2]);
				
                string audioPath = values[3].Replace("\"", "");
				audioPath = audioPath.Trim(' ');
				audioPath = audioPath.Split('.')[0];
				audio.Add(audioPath);
			}
		}
	}

    static public string[] SplitCsvOneLiner(string line) {
		string[] content = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

		// remove quotes
		content[2] = content[2].Replace("\"", "");
		content[3] = content[3].Replace("\"", "");
		return content;
	}

	static public string[] SplitCsvLine(string line) {
		string[] content = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

		// remove quotes
		content[1] = content[1].Replace("\"", "");
		content[2] = content[2].Replace("\"", "");

		return content;
	}

	public void ClearData() {
        timeToDisplay.Clear();
        germanSubtitles.Clear();
        englishSubtitles.Clear();
        audio.Clear();
    }

    public float GetTimeToDisplay() {
        float timeSum = 0;
        foreach(float time in timeToDisplay) {
            timeSum += time;
        }
        return timeSum;
    }
}

