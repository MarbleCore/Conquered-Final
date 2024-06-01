using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, PLAYERATTACK, ENEMYTURN, WON, LOST, RUN }

public class BattleSystem : MonoBehaviour
{
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
        gameObject.SetActive(true);
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

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

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        playerHealthBar.SetMaxHealth(playerUnit.maxHP);
        playerManaBar.SetMaxMana(playerUnit.maxMana);
        playerEnergyBar.SetMaxEnergy(playerUnit.maxEnergy);
        playerHealthBar.SetHealth(playerUnit.maxHP);
        playerManaBar.SetMana(playerUnit.maxMana);
        playerEnergyBar.SetEnergy(playerUnit.maxEnergy);

        enemyHealthBar.SetMaxHealth(enemyUnit.maxHP);
        enemyHealthBar.SetHealth(enemyUnit.maxHP);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator EndTurn()
    {
        dialogueText.text = "You have ended your turn.";
        state = BattleState.ENEMYTURN;

        yield return new WaitForSeconds(1f);

        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(1f);

        dialogueText.text = "Pick an action.";
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        playerIsAlive = playerUnit.TakeDamage(enemyUnit.damage, 3);

        playerHealthBar.SetHealth(playerUnit.currentHP);
        logic.changeHealthValue(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (playerIsAlive)
        {
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
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
    }

    public void onEndTurnButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        } else
        {
            StartCoroutine(EndTurn());
        }
    }

    IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(1f);
        if (state == BattleState.WON)
        {
            dialogueText.text = "Enemy Conquered";
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "You Have Been Conquered";
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
        //few lines jiaming put here
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

    IEnumerator attackDecoder(int attack)
    {
        if (state == BattleState.PLAYERTURN)
        {
            state = BattleState.PLAYERATTACK;
        } else
        {
            yield break;
        }

        int attackResult = 0;

        switch (attack)
        {
            case 0:
                attackResult = playerAttack(2, 0, 2);
                attackName = "Slash";
                break;
            case 1:
                attackResult = playerAttack(3, 10, 3);
                attackName = "Fireball";
                break;
            case 2:
                attackResult = playerAttack(3, 10, 3);
                attackName = "Icicle";
                break;
            case 3:
                attackResult = playerAttack(3, 10, 3);
                attackName = "Wind Cutter";
                break;
            case 4:
                attackResult = playerAttack(3, 10, 3);
                attackName = "Stone Blast";
                break;
            case 5:
                attackResult = playerAttack(7, 0, 8);
                attackName = "Headbutt";
                break;
            case 6:
                attackResult = playerAttack(1, 0, 1);
                attackName = "Punch";
                break;
            case 7:
                attackResult = playerAttack(4, 0, 5);
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
        state = BattleState.RUN;
        StartCoroutine(EndBattle());
    }

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
