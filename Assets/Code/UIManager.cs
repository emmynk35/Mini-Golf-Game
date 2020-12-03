using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    public static Transform canvas;
    private StartMenu _start;
    private PauseMenu _pause;
    private RestartMenu _restart;

    public UIManager()
    {
        canvas = GameObject.Find("Canvas").transform;
    }

    public void ShowStartMenu()
    {
        _start = new StartMenu();
        _start.Show();
    }

    public void ShowPauseMenu()
    {
        _pause = new PauseMenu();
        _pause.Show();
        Time.timeScale = 0f;
    }

    public void ShowRestartMenu()
    {
        _restart = new RestartMenu();
        _restart.Show();
        Time.timeScale = 0f;
    }

    public void HideStartMenu()
    {
        _start.Hide();
        _start = null;
    }

    public void HidePauseMenu()
    {
        _pause.Hide();
        _pause = null;
    }

    public void HideRestartMenu()
    {
        _restart.Hide();
        _restart = null;
    }

    private abstract class GeneralMenu
    {
        protected GameObject obj;

        public virtual void Show()
        {
            obj.SetActive(true);
        }

        public virtual void Hide()
        {
            GameObject.Destroy(obj);
        }

        protected void InitializeQuitButton()
        {
            obj.transform.Find("Quit").GetComponent<Button>().onClick.AddListener(() =>
            {
                Game.Ctx.QuitGame();
            });
        }
    }

    private class StartMenu : GeneralMenu
    {
        private readonly GameObject start;
        protected readonly Transform canvas;

        public StartMenu()
        {
            start = (GameObject) Resources.Load("Menus/Start Menu");
            canvas = GameObject.Find("Canvas").transform;
            this.obj = GameObject.Instantiate(start, canvas);
            this.InitializeQuitButton();
            this.obj.transform.Find("Start Game").GetComponent<Button>().onClick.AddListener(() =>
            {
                Game.Ctx.StartGame();
            });
        }
    }
    
    private class PauseMenu : GeneralMenu
    {
        private readonly GameObject pause;
        protected readonly Transform canvas;
        
        public PauseMenu()
        {
            pause = (GameObject) Resources.Load("Menus/Pause Menu");
            canvas = GameObject.Find("Canvas").transform;
            this.obj = GameObject.Instantiate(pause, canvas);
            this.InitializeQuitButton();
            this.obj.transform.Find("Resume").GetComponent<Button>().onClick.AddListener(() =>
            {
                Game.Ctx.ResumeGame();
            });
        }
    }
    
    private class RestartMenu : GeneralMenu
    {
        private readonly GameObject restart;
        protected readonly Transform canvas;

        public RestartMenu()
        {
            restart = (GameObject) Resources.Load("Menus/Restart Menu");
            canvas = GameObject.Find("Canvas").transform;
            this.obj = GameObject.Instantiate(restart, canvas);
            this.InitializeQuitButton();
            this.obj.transform.Find("Restart").GetComponent<Button>().onClick.AddListener(() =>
            {
                Game.Ctx.RestartGame();
            });
        }
    }
}