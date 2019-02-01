using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
  public List<GameObject> activate;
  public List<GameObject> deactivate;
  public void StartGame()
  {
    activate.ForEach(a => a.SetActive(true));
    deactivate.ForEach(a => a.SetActive(false));
  }

  public void Update()
  {
    if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton1))
    {
      Scene scene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(scene.name);
    }
  }
}
