using UnityEngine;
using System.Collections;

public class MaineMenu : MonoBehaviour {

  enum MenuState { Splash, TrendSetter, Crook, Detective, Oddball }

  public Texture2D[] screens;
  private MenuState state;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  if (Input.GetButtonDown("P1_A"))
    {
     
      if(state == MenuState.Oddball){ 
        // CHANGE SCENES
        Application.LoadLevel("GameScene");
      } else {
        state++;
      }
    }
	}

  void OnGUI() {
    switch(state){
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
