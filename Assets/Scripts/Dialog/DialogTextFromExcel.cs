using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
	public List<string> germanAudio = new List<string>();
	public List<string> englishAudio = new List<string>();

	// variables for one liners 
	public List<string> oneLinerID = new List<string>();
    public List<float>  oneLinerPlayTimer = new List<float>();
	public List<string> oneLinerGermanSubtitles = new List<string>();
	public List<string> oneLinerEnglishSubtitles = new List<string>();
	public List<string> oneLinerGermanAudio = new List<string>();
	public List<string> oneLinerEnglishAudio = new List<string>();

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
				var values = SplitCsvLine(line);

				// assign values to different lists
				oneLinerID.Add(values[0]);
                oneLinerPlayTimer.Add(float.Parse(values[1]));
				oneLinerGermanSubtitles.Add(values[2]);
				oneLinerEnglishSubtitles.Add(values[3]);

                string germanAudio = values[4].Replace("\"", "");
                germanAudio = germanAudio.Trim(' ');
                germanAudio = germanAudio.Split('.')[0];
                oneLinerGermanAudio.Add(germanAudio);

                /*
                string englishAudio = values[5].Replace("\"", "");
                englishAudio = englishAudio.Trim(' ');
                englishAudio = englishAudio.Split('.')[0];
                oneLinerEnglishAudio.Add(englishAudio);*/
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
				timeToDisplay.Add(float.Parse(values[0]));
				germanSubtitles.Add(values[1]);
				englishSubtitles.Add(values[2]);

                string germanAudio = values[3].Replace("\"", "");
                germanAudio = germanAudio.Trim(' ');
                germanAudio = germanAudio.Split('.')[0];
                this.germanAudio.Add(germanAudio);

                /*
                string englishAudio = values[4].Replace("\"", "");
                englishAudio = englishAudio.Trim(' ');
                englishAudio = englishAudio.Split('.')[0];
                this.englishAudio.Add(englishAudio);*/
			}
		}
	}

    static public string[] SplitCsvLine(string line) {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }

    public void ClearData() {
        timeToDisplay.Clear();
        germanSubtitles.Clear();
        englishSubtitles.Clear();
        germanAudio.Clear();
        englishAudio.Clear();
    }

    public void ClearOneLinerData() {

    }

    public float GetTimeToDisplay() {
        float timeSum = 0;
        foreach(float time in timeToDisplay) {
            timeSum += time;
        }
        return timeSum;
    }
}

