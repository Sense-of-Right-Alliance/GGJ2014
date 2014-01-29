using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Interface : MonoBehaviour {

  enum GameInterfaceState { Game, Score, Credits }

  private GameInterfaceState state;
  public Texture2D[] screens;

  public Texture2D score;
  public GameObject[] players;
  private Rect[] scoreRects;

  private float gameTimer = 240.0f;
  private Rect timerRect;

  private int[] rankings = new int[4];

  public GUIStyle scoreGUIStyle;

  List<string> positionStrings;
  int[] positions;

  public GUIStyle baldModeStyle;
  public bool baldModeOn = false;
  private bool baldModeFlash = true;
  private float baldModeFlashTimer = 0.5f;

	// Use this for initialization
	void Start () {
    int width = (int)(Screen.width / 10.5);
    scoreRects = new Rect[4];
    for(int i = 0; i < 4; i++) {
      scoreRects[i] = new Rect(Screen.width / 3 + Screen.width / 160 + width * i, (int)(Screen.height / 9.5), Screen.width / 10, Screen.width / 10);
    }

    timerRect = new Rect(0,0,100,50);

    positionStrings = new List<string>()
    {
      "First",
      "Second",
      "Third",
      "Fourth",
    };
	}
	
	// Update is called once per frame
	void Update () {

    if (gameTimer > 0.0f)
    {
      gameTimer -= Time.deltaTime;
      if (gameTimer <= 0.0f)
      {
        var playersByScore = players.OrderByDescending(u => u.GetComponent<PlayerClass>().score).ToList();
        positions = new int[4];
        for (int i = 0; i < 4; i++) {
          players[i].SetActive(false);
          positions[i] = playersByScore.IndexOf(players[i]);
        }
 

        state = GameInterfaceState.Score;
      }
    }

    if (Input.GetButtonDown("P1_Start"))
    {
      if(state == GameInterfaceState.Score) {
        state = GameInterfaceState.Credits;
      } else if(state == GameInterfaceState.Credits) {
        Application.LoadLevel("MainMenu");
    
      }
    }

    if (baldModeOn)
    {
      baldModeFlashTimer -= Time.deltaTime;
      if(baldModeFlashTimer <= 0.0f) {
        baldModeFlashTimer = 0.5f;
        baldModeFlash = !baldModeFlash;
      }
    }

    // Exit state
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.Quit();
    }
	}

  void OnGUI() {
    switch (state)
    {
      case(GameInterfaceState.Game):
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.width * score.height / score.width), score);
        for (int i = 0; i < 4; i++)
        {
          GUI.Label(scoreRects[i], ((int)(players[i].GetComponent<PlayerClass>().score)).ToString(), scoreGUIStyle);
        }
        GUI.Label(timerRect,gameTimer.ToString(), scoreGUIStyle);

        if(baldModeOn && baldModeFlash) {
          GUI.Label(new Rect(0,0,Screen.width / 4, Screen.height / 6), "BALD", baldModeStyle);
          GUI.Label(new Rect(3 * Screen.width / 4, 0,Screen.width / 4, Screen.height / 6), "MODE", baldModeStyle);
        }

        break;
      case(GameInterfaceState.Score):
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screens[0]);
        scoreGUIStyle.fontSize = 20;
        GUI.Label(new Rect(scoreRects[3].x, Screen.height / 8 + 0 * Screen.height / 5, 100,50), positionStrings[positions[0]] + "   " +  ((int)(players[0].GetComponent<PlayerClass>().score)).ToString(), scoreGUIStyle);

        GUI.Label(new Rect(scoreRects[3].x, Screen.height / 8 + 1 * Screen.height / 5, 100,50), positionStrings[positions[1]] + "   " +  ((int)(players[1].GetComponent<PlayerClass>().score)).ToString(), scoreGUIStyle);
        GUI.Label(new Rect(scoreRects[3].x, Screen.height / 8 + 2 * Screen.height / 5, 100,50), positionStrings[positions[2]] + "   " +  ((int)(players[2].GetComponent<PlayerClass>().score)).ToString(), scoreGUIStyle);
        GUI.Label(new Rect(scoreRects[3].x, Screen.height / 8 + 3 * Screen.height / 5, 100,50), positionStrings[positions[3]] + "   " +  ((int)(players[3].GetComponent<PlayerClass>().score)).ToString(), scoreGUIStyle);

        break;
      case(GameInterfaceState.Credits):
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screens[1]);
        break;
    }
  }
}
