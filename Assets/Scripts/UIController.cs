using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }


    public TMP_Text playerManaText, playerHealthText, enemyHealthText, enemyManaText;

    public GameObject manaWarning;
    public float manaWarningTime;
    private float manaWariningCounter;

    public GameObject drawCardButton, endTurnButton;

    public UIDamageindicator playerDamage, enemyDamage;

    public GameObject battleEndScreen;
    public TMP_Text battleResultText;

    public string mainMenuScene, battleSelectScene;

    public GameObject pauseScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(manaWariningCounter > 0)
        {
            manaWariningCounter -= Time.deltaTime;

            if(manaWariningCounter <= 0)
            {
                manaWarning.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
        
    }

    public void SetPlayerManaText(int manaAmount)
    {
        
        playerManaText.text = "Mana:" + manaAmount;

    }


    public void SetEnemyManaText(int manaAmount)
    {

        enemyManaText.text = "Mana:" + manaAmount;

    }

    public void SetPlayerHealthText(int healthAmount)
    {
        playerHealthText.text = "Player Health:" + healthAmount;
    }

    public void SetEnemyHealthText(int healthAmount)
    {
        enemyHealthText.text = "Enemy Health:" + healthAmount;
    }

    public void ShowManaWarning()
    {
        manaWarning.SetActive(true);
        manaWariningCounter = manaWarningTime;
    }

    public void DrawCard()
    {
        DeckController.instance.DrawCardForMana();


        AudioManager.instansce.PlaySFX(0);

    }

    public void EndPlayerTurn()
    {


        BattleController.instance.EndPlayerTurn();

        AudioManager.instansce.PlaySFX(0);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;

        AudioManager.instansce.PlaySFX(0);
    }

    public void RestartMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;

        AudioManager.instansce.PlaySFX(0);
    }

    public void ChoosseNewBattle()
    {
        SceneManager.LoadScene(battleSelectScene);

        Time.timeScale = 1f;

        AudioManager.instansce.PlaySFX(0);
    }

    public void PauseUnpause()
    {
        if(pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;

        }else
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }

        AudioManager.instansce.PlaySFX(0);
    }
}

