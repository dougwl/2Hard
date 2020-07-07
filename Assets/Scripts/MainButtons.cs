using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtons : MonoBehaviour
{

    public CanvasGroup board;
    public CanvasGroup darkfilter;
    public CanvasGroup fading;
    public InitMotion tween;
    public Transform settings;
    public Transform credits;
    public Transform progress;
    public bool lerp = false;
    public bool open = true;
    public bool openWait = true;

    private TrailRenderer[] tr;
    private ParticleSystem[] ps;
    private Transform page;
    private Transform pageWait;
    private GameManager GM;
    private AudioManager AM;

    private void Start()
    {
        GM = GameManager.GM;
        AM = AudioManager.AM;
        pageWait = null;
        if (GM.GameState == GameState.MainMenu)
        {
            settings.localPosition = new Vector3(GM.screenWidth, 0);
            credits.localPosition = new Vector3(GM.screenWidth, 0);
            progress.localPosition = new Vector3(GM.screenWidth, 0);
        }
    }

    public void PlayGame()
    {
        AM.MusicBeforePlay();
        StartCoroutine(tween.Moveout());
    }

    public void RestartGame()
    {
        AM.MusicBeforePlay();
        StartCoroutine(Game());
    }

    public void OpenMenu()
    {
        AM.MusicMainMenu();
        StartCoroutine(Menu());
    }

    public void OpenSettings()
    {
        if (!lerp)
        {
            page = settings;
            lerp = true;
            open = true;
            GM.backButtonState = BackState.Settings;
        }
        else if (open == false)
        {
            pageWait = settings;
            openWait = true;
        }
    }

    public void ExitSettings()
    {
        if (!lerp || page == settings)
        {
            page = settings;
            lerp = true;
            open = false;
            GM.backButtonState = BackState.None;
        }
        else if (open == false)
        {
            pageWait = settings;
            openWait = false;
            GM.backButtonState = BackState.None;
        }
    }

    public void OpenCredits()
    {
        if (!lerp)
        {
            page = credits;
            lerp = true;
            open = true;
            GM.backButtonState = BackState.Credits;
        }
        else if (open == true)
        {
            pageWait = credits;
            openWait = true;
        }
    }

    public void ExitCredits()
    {
        if (!lerp || page == credits)
        {
            page = credits;
            lerp = true;
            open = false;
            GM.backButtonState = BackState.Settings;
        }
    }

    public void OpenProgress()
    {
        if (!lerp || page == progress)
        {
            page = progress;
            lerp = true;
            open = true;
            GM.backButtonState = BackState.Progress;
        }
        else if (open == false)
        {
            pageWait = progress;
            openWait = true;
        }
    }

    public void ExitProgress()
    {
        if (!lerp || page == progress)
        {
            page = progress;
            lerp = true;
            open = false;
            GM.backButtonState = BackState.None;
        }
    }

    private IEnumerator Game()
    {
        board.blocksRaycasts = false;

        float decelerate = 0f;
        float aux = 1;
        if (GM.GameState == GameState.InGame) aux = 2;
        while (darkfilter.alpha != 1f)
        {
            decelerate += Time.deltaTime;

            darkfilter.alpha = Mathf.Lerp(darkfilter.alpha, 1f, decelerate * aux);

            if (darkfilter.alpha > 0.95f)
            {
                darkfilter.alpha = 1;
                GM.StartGame();
            }
            yield return null;
        }
    }

    private IEnumerator Menu() // Some changes need to be done after unifying scenes.
    {
        board.blocksRaycasts = false;

        float decelerate = 0f;
        while (fading.alpha != 1f)
        {
            decelerate += Time.deltaTime;

            fading.alpha = Mathf.Lerp(fading.alpha, 1f, decelerate);

            if (fading.alpha > 0.95f)
            {
                fading.alpha = 1f;
                GM.StartMenu();
            }
            yield return null;
        }
    }

    private void Update() //Change behaviour to Event/Ienumerator
    {
        if (GM.GameState == GameState.MainMenu)
        {

            if (lerp && open)
            {
                float decelerate = Mathf.Min(10 * Time.deltaTime, 1f);

                page.localPosition = Vector2.Lerp(page.localPosition, Vector3.zero, decelerate);

                if (page.localPosition.x < 2f)
                {
                    page.localPosition = Vector3.zero;
                    lerp = false;
                }
            }
            if (lerp && !open)
            {
                float decelerate = Mathf.Min(10 * Time.deltaTime, 1f);

                page.localPosition = Vector2.Lerp(page.localPosition, new Vector3(GM.screenWidth, 0), decelerate);

                if (new Vector3(GM.screenWidth, 0).x - page.localPosition.x < 25f)
                {
                    page.localPosition = new Vector3(GM.screenWidth, 0);
                    if (pageWait != null)
                    {
                        page = pageWait;
                        open = openWait;
                        if (open) { GM.backButtonState = BackState.None;
                            print("Update = " + page.gameObject.name); }
                        pageWait = null;
                    }
                    else lerp = false;
                }
            }
        }
    }
}
