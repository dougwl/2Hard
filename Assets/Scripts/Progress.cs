using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour {

	public Text time;
	public Text matches;
	public Text normal, duo, slow, walls, survival, pulse, ghost;
	public Text level;
	public Image levelProgress;

	void Start () {
		time.text = PlayerPrefs.GetFloat("TimePlayed").ToString("F2", CultureInfo.InvariantCulture);
		matches.text = PlayerPrefs.GetInt("MatchesPlayed").ToString();
		normal.text = PlayerPrefs.GetFloat("BestScoreNormal").ToString("F2", CultureInfo.InvariantCulture);
		duo.text = PlayerPrefs.GetFloat("BestScoreDuo").ToString("F2", CultureInfo.InvariantCulture);
		slow.text = PlayerPrefs.GetFloat("BestScoreSlow").ToString("F2", CultureInfo.InvariantCulture);
		walls.text = PlayerPrefs.GetFloat("BestScoreNoWalls").ToString("F2", CultureInfo.InvariantCulture);
		survival.text = PlayerPrefs.GetFloat("BestScoreSurvival").ToString("F2", CultureInfo.InvariantCulture);
		pulse.text = PlayerPrefs.GetFloat("BestScorePulse").ToString("F2", CultureInfo.InvariantCulture);
		ghost.text = PlayerPrefs.GetFloat("BestScoreGhost").ToString("F2", CultureInfo.InvariantCulture);
		level.text = PlayerPrefs.GetInt("Level").ToString();
		levelProgress.fillAmount = PlayerPrefs.GetInt("Points")/20f;
	}
	
}
