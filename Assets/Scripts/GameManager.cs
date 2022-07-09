using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private SceneFader fader;

    private List<Orb> orbs;

    private Door lockedDoor;

    private float gameTime;

    private bool gameIsOver;

    public int deathNum;

    public Vector2 FirstPosition;

    private void Awake()
    {
        if (instance != null)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        orbs = new List<Orb>();
        Object.DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (!gameIsOver)
        {
            gameTime += Time.deltaTime;
            UIManager.UpdateTimeUI(gameTime);
        }
    }

    public static void RegisterDoor(Door door)
    {
        instance.lockedDoor = door;
    }

    public static void RegisterSceneFader(SceneFader obj)
    {
        instance.fader = obj;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (!(instance == null))
        {
            if (!instance.orbs.Contains(orb))
            {
                instance.orbs.Add(orb);
            }
            UIManager.UpdateOrbUI(instance.orbs.Count);
        }
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (instance.orbs.Contains(orb))
        {
            instance.orbs.Remove(orb);
            if (instance.orbs.Count == 0)
            {
                instance.lockedDoor.Open();
            }
            UIManager.UpdateOrbUI(instance.orbs.Count);
        }
    }

    public static void PlayerWon()
    {
        instance.gameIsOver = true;
        UIManager.DisplayGameOver();
        AudioManager.PlayerWonAudio();
    }

    public static bool GameOver()
    {
        return instance.gameIsOver;
    }

    public static void PlayerDied()
    {
        instance.fader.FadeOut();
        instance.deathNum++;
        UIManager.UpdateDeathUI(instance.deathNum);
        instance.Invoke("RestartScene", 1.5f);
    }

    private void RestartScene()
    {
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
