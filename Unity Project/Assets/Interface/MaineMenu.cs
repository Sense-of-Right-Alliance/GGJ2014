using UnityEngine;
using System.Collections;

public class MaineMenu : MonoBehaviour {

  enum MenuState { Intro, Splash, TrendSetter, Crook, Detective, Oddball }

  public Texture2D title;
  public Texture2D backdrop;
  public Texture2D[] screens;

  private MenuState state;
  private Rect titleRect;
  private float titleSpeed = 250.0f;

	// Use this for initialization
	void Start () {
    int width = Screen.width / 3;
    int height = (int)((float)width * ((float)title.height / (float)title.width));
    titleRect = new Rect(Screen.width / 2 - width / 2, -height, width, height);
    state = MenuState.Intro;
	}
	
	// Update is called once per frame
	void Update () {
	  if (Input.GetButtonDown("P1_Start"))
    {
     
      if(state == MenuState.Oddball){ 
        // CHANGE SCENES
        Application.LoadLevel("GameScene");
      } else {
        state++;
      }
    }

    if (state == MenuState.Intro)
    {
      titleRect.y += titleSpeed * Time.deltaTime;
      if(titleRect.y > Screen.height / 2 - titleRect.height / 2) {
        titleRect.y = Screen.height / 2 - titleRect.height / 2;
      }
    }
	}

  void OnGUI() {
    switch(state){
      case(MenuState.Intro):
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.width * backdrop.height / backdrop.width), backdrop);
        GUI.DrawTexture(titleRect, title);
        break;
      case(MenuState.Splash):
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screens[0]);
        break;
      case(MenuState.TrendSetter):
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screens[1]);
        break;
      case(MenuState.Crook):
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screens[2]);
        break;
      case(MenuState.Detective):
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screens[3]);
        break;
      case(MenuState.Oddball):
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screens[4]);
        break;
    }
  }
}
