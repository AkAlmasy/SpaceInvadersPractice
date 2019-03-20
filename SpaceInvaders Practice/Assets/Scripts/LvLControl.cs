using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvLControl : MonoBehaviour {

    [SerializeField] float loadDelayTime = 2f;


    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<GameSession>().GameReset();
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitBeforeLoadGameOver());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitBeforeLoadGameOver()
    {
        yield return new WaitForSeconds(loadDelayTime);
        SceneManager.LoadScene("Game Over");
    }
}
