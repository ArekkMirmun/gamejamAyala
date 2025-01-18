using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


public enum BattleState {WAIT, START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{

    public GameObject combatHUD;
    public bool instaWin = false;
    public static BattleSystem Instance;
    
    public BattleState state;
    
    private GameObject _playerPrefab;
    private GameObject _enemyPrefab;

    private GameObject _currentEnemy;
    
    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    
    Unit playerUnit;
    Unit enemyUnit;
    
    PlayerInfo playerInfo;
    
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    
    public TextMeshProUGUI dialogueText;
    public float dialogueSpeedSeconds = 2f;
    public SceneManager sceneManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        state = BattleState.WAIT;
        combatHUD.SetActive(false);
    }
    
    //Starts the combat
    public void StartBattle(GameObject enemyHitted, GameObject enemyCombatPrefab)
    {
        _currentEnemy = enemyHitted;
        playerInfo = PlayerInfo.Instance;
        _playerPrefab = playerInfo.currentPlayerPrefab;
        _enemyPrefab = enemyCombatPrefab;
        combatHUD.SetActive(true);
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(_playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        
        playerUnit.unitLevel = playerInfo.currentLevel;
        playerUnit.currentHP = playerInfo.playerCurrentHP;
        playerUnit.maxHP = playerInfo.maxHP;
        
        
        GameObject enemyGO = Instantiate(_enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        
        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";
        
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        
        yield return new WaitForSeconds(dialogueSpeedSeconds);



        if (instaWin)
        {
            //Inssta kill enemy
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    private void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }
    
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerAttack());
    }
    
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerHeal());
    }

    private IEnumerator PlayerHeal()
    {
        if (playerInfo.currentHeals <= 0)
        {
            
            dialogueText.text = "You are out of heals!";
            yield return new WaitForSeconds(dialogueSpeedSeconds);
        }
        else
        {
            playerInfo.currentHeals--;
                    playerUnit.Heal(enemyUnit.damage + 2);
                    
                    playerHUD.SetHP(playerUnit.currentHP);
                    dialogueText.text = "You feel renewed strength!";
                    yield return new WaitForSeconds(dialogueSpeedSeconds);
        
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator PlayerAttack()
    {
        int damage = playerUnit.damage;
        //calculate critic change 1/5
        bool ifCrit = Random.Range(0, 5) == 0;
        if (ifCrit)
        {
            damage *= 2;
            dialogueText.text = "Critical hit!";
            yield return new WaitForSeconds(dialogueSpeedSeconds);

        }

        bool isDead = enemyUnit.TakeDamage(damage);
        
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack was successful!";
        
        yield return new WaitForSeconds(dialogueSpeedSeconds);
        
        if (isDead)
        {
            state = BattleState.WON;
            //Checks if the player is going to level up
            if (playerInfo.enemiesToLevelUp == 1)
            {
                //Show text that the player is going to level up
                dialogueText.text = "You are going to level up!";
                yield return new WaitForSeconds(dialogueSpeedSeconds);
                //Show text that says "Your level is now: " + playerInfo.currentLevel +1
                dialogueText.text = "Your level is now: " + (playerInfo.currentLevel + 1);
                
            }
            
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";
        
        yield return new WaitForSeconds(dialogueSpeedSeconds);
        
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerInfo.playerCurrentHP = playerUnit.currentHP;
        playerHUD.SetHP(playerUnit.currentHP);
        
        yield return new WaitForSeconds(dialogueSpeedSeconds / 2f);
        
        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    private IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
            playerInfo.EnemyDefeated(_currentEnemy);
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated!";
            yield return new WaitForSeconds(dialogueSpeedSeconds);
            sceneManager.LoadGameOver();
        }

        yield return new WaitForSeconds(dialogueSpeedSeconds);
        combatHUD.SetActive(false);
    }
}
