using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//create all predetermined battle states
public enum BattleState { START, PLAYERTURN, PLAYERATTACK, ENEMYTURN, WON, LOST, RUN }

public class BattleSystem : MonoBehaviour
{
    //create all required objects
    public GameObject playerPrefab;

    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject bossPrefab1;
    public GameObject bossPrefab2;
    public GameObject bossPrefab3;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public HealthBar playerHealthBar;
    public ManaBar playerManaBar;
    public EnergyBar playerEnergyBar;
    public LogicScriptPlayerInfo logic;

    public HealthBar enemyHealthBar;

    public string attackName;

    public bool enemyIsAlive;
    public bool playerIsAlive;

    public bool playerHasMana;
    public bool playerHasEnergy;

    //turned this into public for game.cs to use
    [SerializeField] 
    public int enemyID;

    //need to reference player in maze
    [SerializeField]
    Player player;

    [SerializeField]
    Game game;

    GameObject playerGO;

    GameObject enemyGO;

    [SerializeField]
    CameraTracking cameratracking;

    //turns this off when game starts
    void Start()
    {
        gameObject.SetActive(false);
    }

    //rewrote this from start (that is now for turning this off at start) to a method that activates this whenever a battle needs to start
    public void StartBattle()
    {
        //turn on the game object(it holds the all the battle related objects as a parent)
        //set battle state to start, and start the setup battle script
        gameObject.SetActive(true);
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    //creates the player and enemy, as well as set everything up to be battle ready
    IEnumerator SetupBattle()
    {
        //create the player
        playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        //based on the enemyID, spawn a different enemy
        switch (enemyID)
        {
            case 0:
                enemyGO = Instantiate(enemyPrefab1, enemyBattleStation);
                enemyUnit = enemyGO.GetComponent<Unit>();
                break;
            case 1:
                enemyGO = Instantiate(enemyPrefab2, enemyBattleStation);
                enemyUnit = enemyGO.GetComponent<Unit>();
                break;
            case 2:
                enemyGO = Instantiate(enemyPrefab3, enemyBattleStation);
                enemyUnit = enemyGO.GetComponent<Unit>();
                break;
            case 3:
                enemyGO = Instantiate(bossPrefab1, enemyBattleStation);
                enemyUnit = enemyGO.GetComponent<Unit>();
                break;
            case 4:
                enemyGO = Instantiate(bossPrefab2, enemyBattleStation);
                enemyUnit = enemyGO.GetComponent<Unit>();
                break;
            case 5:
                enemyGO = Instantiate(bossPrefab3, enemyBattleStation);
                enemyUnit = enemyGO.GetComponent<Unit>();
                break;
        }

        dialogueText.text = "A " + enemyUnit.unitName + " approaches!";

        //set all required UI elements
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        playerHealthBar.SetMaxHealth(playerUnit.maxHP);
        playerManaBar.SetMaxMana(playerUnit.maxMana);
        playerEnergyBar.SetMaxEnergy(playerUnit.maxEnergy);
        playerHealthBar.SetHealth(playerUnit.maxHP);
        playerManaBar.SetMana(playerUnit.maxMana);
        playerEnergyBar.SetEnergy(playerUnit.maxEnergy);
        logic.changeHealthValue(playerUnit.currentHP);

        enemyHealthBar.SetMaxHealth(enemyUnit.maxHP);
        enemyHealthBar.SetHealth(enemyUnit.maxHP);

        //wait 2 seconds, then start the player's turn
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }

    //script to run when the end turn button is pressed
    IEnumerator EndTurn()
    {
        dialogueText.text = "You have ended your turn.";

        //change the battle state to enemy turn
        state = BattleState.ENEMYTURN;

        //wait 2 seconds, then start the enemy turn script
        yield return new WaitForSeconds(1f);

        StartCoroutine(EnemyTurn());
    }

    //script to run when it is the players turn
    //waits for any action from the player
    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(1f);

        dialogueText.text = "Pick an action.";
    }

    IEnumerator EnemyTurn()
    {
        //lets the enemy attack
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        playerIsAlive = playerUnit.TakeDamage(enemyUnit.damage, 3);

        playerHealthBar.SetHealth(playerUnit.currentHP);
        logic.changeHealthValue(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        //checks to see if the player is alive
        if (playerIsAlive)
        {
            //if it is, regen 10 mana and set energy to max
            playerUnit.currentMana += 10;
            if (playerUnit.currentMana > 100)
            {
                playerUnit.currentMana = 100;
            }
            playerManaBar.SetMana(playerUnit.currentMana);
            playerUnit.currentEnergy = playerUnit.maxEnergy;
            playerEnergyBar.SetEnergy(playerUnit.currentEnergy);

            state = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        } else
        {
            //if not, set battle state to lost and end the battle
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
    }

    public void onEndTurnButton()
    {
        //if it is not the players turn, don't do anything, if it is, start the end turn script
        if (state != BattleState.PLAYERTURN)
        {
            return;
        } else
        {
            StartCoroutine(EndTurn());
        }
    }

    //runs when the battle ends, no matter what(win, lose, run)
    IEnumerator EndBattle()
    {
        //checks what battle state it is, and runs the correct text 
        yield return new WaitForSeconds(1f);
        if (state == BattleState.WON)
        {
            dialogueText.text = "Enemy Conquered";
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You Have Been Conquered";
            //runs the necessary end game scripts
            this.game.isAlive = false;
            this.game.EndGame();
            this.cameratracking.On();
            gameObject.SetActive(false);
            Destroy(playerGO);
            Destroy(enemyGO);
            yield break;
        } else if (state == BattleState.RUN)
        {
            dialogueText.text = "You have run away from battle.";
        }
        //checks to see if the enemy slain is a normal enemy or a boss
        //0, 1, 2 enemy ID's are regular enemies
        //3, 4, 5 enemy ID's are bosses
        if(enemyID < 3)
        {
            this.player.OutBattle();
            cameratracking.On();
            gameObject.SetActive(false);
        }
        else
        {
            this.game.EndGame();
            this.cameratracking.On();
            gameObject.SetActive(false);
        }
        Destroy(playerGO);
        Destroy(enemyGO);
    }

    //required because there were too many attacks
    IEnumerator attackDecoder(int attack)
    {
        //checks to see if it is actually the player's turn
        if (state == BattleState.PLAYERTURN)
        {
            state = BattleState.PLAYERATTACK;
        } else
        {
            yield break;
        }

        int attackResult = 0;

        //based on the input, run a different attack
        switch (attack)
        {
            case 0:
                attackResult = playerAttack(2 * this.game.floorNumber, 0, 2);
                attackName = "Slash";
                break;
            case 1:
                attackResult = playerAttack(4 * this.game.floorNumber, 10, 3);
                attackName = "Fireball";
                break;
            case 2:
                attackResult = playerAttack(4 * this.game.floorNumber, 10, 3);
                attackName = "Icicle";
                break;
            case 3:
                attackResult = playerAttack(4 * this.game.floorNumber, 10, 3);
                attackName = "Wind Cutter";
                break;
            case 4:
                attackResult = playerAttack(4 * this.game.floorNumber, 10, 3);
                attackName = "Stone Blast";
                break;
            case 5:
                attackResult = playerAttack(7 * this.game.floorNumber, 0, 8);
                attackName = "Headbutt";
                break;
            case 6:
                attackResult = playerAttack(1 * this.game.floorNumber, 0, 1);
                attackName = "Punch";
                break;
            case 7:
                attackResult = playerAttack(4 * this.game.floorNumber, 0, 4);
                attackName = "Kick";
                break;

        }

        playerManaBar.SetMana(playerUnit.currentMana);
        playerEnergyBar.SetEnergy(playerUnit.currentEnergy);

        successfulAttack(attackResult);

        yield return new WaitForSeconds(1f);

        enemyHealthBar.SetHealth(enemyUnit.currentHP);

        if (attackResult == 0)
        {
            dialogueText.text = "The " + attackName.ToLower() + " attack was successful!";
        }

        //checks if the enemy is alive
        if (enemyIsAlive)
        {
            state = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
        else
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
    }

    //functions for the buttons to activate on click
    public void slashAttack()
    {
        StartCoroutine(attackDecoder(0));
    }

    public void fireballAttack()
    {
        StartCoroutine(attackDecoder(1));
    }

    public void icicleAttack()
    {
        StartCoroutine(attackDecoder(2));
    }

    public void windCutterAttack()
    {
        StartCoroutine(attackDecoder(3));
    }

    public void stoneBlastAttack()
    {
        StartCoroutine(attackDecoder(4));
    }

    public void headbuttAttack()
    {
        StartCoroutine(attackDecoder(5));
    }

    public void punchAttack()
    {
        StartCoroutine(attackDecoder(6));
    }

    public void kickAttack()
    {
        StartCoroutine(attackDecoder(7));
    }

    public void runButton()
    {
        if (enemyID > 2)
        {
            dialogueText.text = "You cannot run from a boss!";
        } else
        {
            state = BattleState.RUN;
            StartCoroutine(EndBattle());
        }
    }

    //displays the correct text depending on what happened during the attack
    void successfulAttack(int option)
    {
        if (option == 0)
        {
            dialogueText.text = "You have used " + attackName.ToLower() + " !";
        } else if (option == 1)
        {
            dialogueText.text = "You don't have enough mana.";
        } else if (option == 2)
        {
            dialogueText.text = "You don't have enough energy.";
        }
    }

    //checks if the player has enough mana and energy to perform the attack
    int playerAttack(int damageMulti, int manaChange, int energyChange)
    {
        playerHasMana = playerUnit.changeMana(manaChange);
        playerHasEnergy = playerUnit.changeEnergy(energyChange);
        if (playerHasMana == false)
        {
            playerUnit.currentMana += manaChange;
            playerUnit.currentEnergy += energyChange;
            return 1;
        }
        else if (playerHasEnergy == false)
        {
            playerUnit.currentMana += manaChange;
            playerUnit.currentEnergy += energyChange;
            return 2;
        }
        else
        {
            enemyIsAlive = enemyUnit.TakeDamage(playerUnit.damage, damageMulti);
            return 0;
        }
    }
}
