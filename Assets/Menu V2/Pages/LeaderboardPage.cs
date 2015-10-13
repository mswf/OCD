using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardPage : IPage 
{
	private HighscoreManager _manager = new HighscoreManager();
	private List<string> _tracks = new List<string>();
	private List<Highscore> _list = new List<Highscore>();
	

	private void Start()
	{
		Initialize();
		_tracks.Add("MainTrack");
		_list = _manager.getHighscoreList( _tracks[0] );
		_list.Add(GetHighscore(1,"Weikie", 5250));
		_list.Add(GetHighscore(2,"Miso", 2250));
		_list.Add(GetHighscore(3,"Gerben", 3250));
		_list.Add(GetHighscore(4,"Robin", 4250));
		_list.Add(GetHighscore(5,"Arni", 5250));

		string trackStat = "";
		for (int i = 0; i < 5; i++) 
		{
			trackStat = _list[i].name + ": ";
			int time = _list[i].totalTime;
			int minute = time / 60;
			int seconds = time - (minute * 60);
			trackStat += minute.ToString() + ":" + seconds.ToString();
			menuLables[i].SetText(trackStat);
		}

		if (menuButtons.Count > 0)
		{
			menuButtons[0].SetButtonMethod(BackButton);
		}
	}

	public override void Update ()
	{
		base.Update();
	}

	private void BackButton()
	{
		menuButtons[0].PlayButtonPressSound(menuButtons[0].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Main);
		animator.SlideOut();
	}

	public Highscore GetHighscore(int id, string name, int total)
	{
		Highscore score = new Highscore();
		score.id = id;
		score.name = name;
		score.totalTime = total;
		return score;
	}
}
