using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {

  public Texture2D score;
  public GameObject[] players;
  private Rect[] scoreRects;

  public GUIStyle scoreGUIStyle;

	// Use this for initialization
	void Start () {
    int width = (int)(Screen.width / 10.5);
    scoreRects = new Rect[4];
    for(int i = 0; i < 4; i++) {
      scoreRects[i] = new Rect(Screen.width / 3 + Screen.width / 160 + width * i, (int)(Screen.height / 9.5), Screen.width / 10, Screen.width / 10);
    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnGUI() {
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.width * score.height/score.width), score);
    for (int i = 0; i < 4; i++)
    {
      GUI.Label(scoreRects[i], players[i].GetComponent<PlayerClass>().score.ToString(), scoreGUIStyle);
    }
  }
}
