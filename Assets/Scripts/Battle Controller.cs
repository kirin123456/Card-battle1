using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    private void Awake()
    {
        instance = this;
    }


    public int startingMana = 4, maxMana = 12;
    public int playerMana, enemyMana;
    private int currentPlayerMaxMana, currentEnemyMaxMana, currentMaxMana;



    public int startingCardsAmount = 5;
    public int cardsToDrawPerTurn = 2;

    public enum TurnOrder { playerActive, playerCardAttacks, enemyActive, enemyCardAttacts}
    public TurnOrder currentPhase;

    public Transform discardPoint;

    public int playerHealth, enemyHealth;

    public bool battleEnded;

    public float resultScreenDelyTime = 1f;

    [Range(0f, 1f)]

    public float playerFirstChance = .5f;

    // Start is called before the first frame update
    void Start()
    {
        //playerMana = startingMana;
        //UIController.instance.SetPlayerManaText(playerMana);
        currentPlayerMaxMana = startingMana;
        FillPlayerManan();

        DeckController.instance.DrawMultipleCards(startingCardsAmount);
        UIController.instance.SetPlayerHealthText(playerHealth);
        UIController.instance.SetEnemyHealthText(enemyHealth);

        currentEnemyMaxMana = startingMana;
        FillEnemyManan();

        if (Random.value > playerFirstChance)
        {
            currentPhase = TurnOrder.playerCardAttacks;
            AdvanceTurn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            AdvanceTurn();
        }
        
    }

    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana = playerMana - amountToSpend;

        if(playerMana < 0)
        {
            playerMana = 0;
        }


        UIController.instance.SetPlayerManaText(playerMana);

    }

    public void FillPlayerManan()
    {
        //playerMana = startingMana;
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void SpendEnemyMana(int amountToSpend)
    {
        enemyMana -= amountToSpend;

        if (enemyMana < 0)
        {
            enemyMana = 0;
        }


        UIController.instance.SetEnemyManaText(enemyMana);

    }

    public void FillEnemyManan()
    {
        enemyMana = currentEnemyMaxMana;
        UIController.instance.SetEnemyManaText(enemyMana);
    }

    public void AdvanceTurn()
    {
        if (battleEnded == false)
        {


            currentPhase++;

            if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
            {
                currentPhase = 0;
            }


            switch (currentPhase)
            {
                case TurnOrder.playerActive:


                    UIController.instance.endTurnButton.SetActive(true);
                    UIController.instance.drawCardButton.SetActive(true);

                    if (currentPlayerMaxMana < maxMana)
                    {
                        currentPlayerMaxMana++;
                    }

                    FillPlayerManan();

                    DeckController.instance.DrawMultipleCards(cardsToDrawPerTurn);

                    break;



                case TurnOrder.playerCardAttacks:

                    //Debug.Log("skipping player card attacks");
                    //AdvanceTurn();

                    CardPointsController.instance.PlayerAttack();
                    break;



                case TurnOrder.enemyActive:

                    //Debug.Log("skipping enemy actions");
                    //AdvanceTurn();

                    if (currentEnemyMaxMana < maxMana)
                    {
                        currentEnemyMaxMana++;
                    }

                    FillEnemyManan();

                    EnemyController.instance.StartAction();

                    break;

                case TurnOrder.enemyCardAttacts:

                    //Debug.Log("skipping enemy card attacks");
                    //AdvanceTurn();

                    CardPointsController.instance.EnemyAttack();
                    break;

            }
        }
    }

    public void EndPlayerTurn()
    {
        UIController.instance.endTurnButton.SetActive(false);
        UIController.instance.drawCardButton.SetActive(false);
        AdvanceTurn();
    }

    public void DamagePlayer(int damageAmount)
    {
        if(playerHealth > 0 || !battleEnded)
        {
            playerHealth -= damageAmount;

            if(playerHealth <= 0)
            {
                playerHealth = 0;

                //End Battle
                EndBattle();
            }

            UIController.instance.SetPlayerHealthText(playerHealth);

            UIDamageindicator damageClone = Instantiate(UIController.instance.playerDamage, UIController.instance.playerDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }

    public void DamageEnemy(int damageAmount)
    {
        if (enemyHealth > 0 || battleEnded == false)
        {
            enemyHealth -= damageAmount;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;

                //End Battle
                EndBattle();
            }

            UIController.instance.SetEnemyHealthText(enemyHealth);

            UIDamageindicator damageClone = Instantiate(UIController.instance.enemyDamage, UIController.instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }

    void EndBattle()
    {
        battleEnded = true;

        HandController.instance.EmptyHand();

        if(enemyHealth <= 0)
        {
            UIController.instance.battleResultText.text = "YOU WON!";

            foreach(CardPlacePoint point in CardPointsController.instance.enemyCardPoints)
            {
                if(point.activeCard != null)
                {
                    point.activeCard.MoveToPoint(discardPoint.position, point.activeCard.transform.rotation);
                }
            }
        }else
        {
            UIController.instance.battleResultText.text = "YOU LOSE!";

            foreach (CardPlacePoint point in CardPointsController.instance.playerCardPoints)
            {
                if (point.activeCard != null)
                {
                    point.activeCard.MoveToPoint(discardPoint.position, point.activeCard.transform.rotation);
                }
            }

        }

        StartCoroutine(ShowResultCo());
    }

    IEnumerator ShowResultCo()
    {
        yield return new WaitForSeconds(resultScreenDelyTime);

        UIController.instance.battleEndScreen.SetActive(true);
    }
}
