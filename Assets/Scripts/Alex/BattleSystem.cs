using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


public enum BattleState {WAIT, START, PLAYERTURN, INPROGRESS, ENEMYTURN, RUNAWAY, WON, LOST }
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
    
    GameObject playerGO;
    Animator playerAnimator;
    AudioSource playerAudioSource; 
    AudioSource enemyAudioSource;
    GameObject enemyGO;
    Animator enemyAnimator;
    
    public AudioSource normalBackgroundMusic;
    public AudioSource battleBackgroundMusic;
    public AudioSource bossIntroMusic;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        state = BattleState.WAIT;
        combatHUD.SetActive(false);
        normalBackgroundMusic.Play();
    }
    
    //Starts the combat
    public void StartBattle(GameObject enemyHitted, GameObject enemyCombatPrefab)
    {
        normalBackgroundMusic.Pause();
        battleBackgroundMusic.Play();
        PlayerInfo.Instance.FreezeMovement();
        _currentEnemy = enemyHitted;
        playerInfo = PlayerInfo.Instance;
        _playerPrefab = playerInfo.currentPlayerPrefab;
        _enemyPrefab = enemyCombatPrefab;
        combatHUD.SetActive(true);
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        //Pause daynight timer
        TimeChangeScript.Instance.PauseTime();
        
        playerGO = Instantiate(_playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        
        //Set player animator with child of player prefab
        playerAnimator = playerGO.transform.GetChild(0).GetComponent<Animator>();
        
        //Set player sound
        playerAudioSource = playerGO.GetComponent<AudioSource>();
        
        playerUnit.unitLevel = playerInfo.currentLevel;
        playerUnit.currentHP = playerInfo.playerCurrentHP;
        playerUnit.maxHP = playerInfo.maxHP;
        
        
        enemyGO = Instantiate(_enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        
        //Set enemy sound
        enemyAudioSource = enemyGO.GetComponent<AudioSource>();
        
        //Set enemy animator with child of enemy prefab
        enemyAnimator = enemyGO.transform.GetChild(0).GetComponent<Animator>();
        
        
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
            PlayerTurn();
        }
    }

    private void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        dialogueText.text = "Choose an action:";
    }
    
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        
        state = BattleState.INPROGRESS;
        
        StartCoroutine(PlayerAttack());
    }
    
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        
        state = BattleState.INPROGRESS;

        
        StartCoroutine(PlayerHeal());
    }
    
    public void OnRunButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        
        state = BattleState.RUNAWAY;
        
        StartCoroutine(RunAway());
    }

    private IEnumerator RunAway()
    {
        //Run away from the enemy with a chance of 80%
        if (Random.Range(0, 5) != 3)
        {
            dialogueText.text = "You ran away!";
            StartCoroutine(EndBattle());
        }
        else
        {
            dialogueText.text = "You couldn't run away!";
            yield return new WaitForSeconds(dialogueSpeedSeconds);
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator PlayerHeal()
    {
        if (playerInfo.currentHeals <= 0)
        {
            
            dialogueText.text = "You are out of heals!";
            yield return new WaitForSeconds(dialogueSpeedSeconds);
            PlayerTurn();
        }
        else
        {
            //checks if the player is max HP already
            if (playerUnit.currentHP == playerUnit.maxHP)
            {
                dialogueText.text = "You are already at max HP!";
                yield return new WaitForSeconds(dialogueSpeedSeconds);
                PlayerTurn();

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
    }

    private IEnumerator PlayerAttack()
    {
        //Attack enemy triggering the attack animation
        playerAnimator.SetTrigger("Attack");
        dialogueText.text = "You attack!";
        playerAudioSource.Play();
        yield return new WaitForSeconds(dialogueSpeedSeconds);
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
        dialogueText.text = "The attack made " + damage + " damage!";
        
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
                yield return new WaitForSeconds(dialogueSpeedSeconds);

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
        //Enemy attacks player triggering the attack animation
        enemyAnimator.SetTrigger("Attack");
        enemyAudioSource.Play();
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
        //Resume time
        TimeChangeScript.Instance.ResumeTime();
        
        PlayerInfo.Instance.UnfreezeMovement();
        normalBackgroundMusic.Play();
        battleBackgroundMusic.Stop();
        
        Destroy(playerGO);
        Destroy(enemyGO);
        state = BattleState.WAIT;
        combatHUD.SetActive(false);
    }
    
    public void PlayBossMusic()
    {
        normalBackgroundMusic.Pause();
        battleBackgroundMusic.Stop();
        bossIntroMusic.Play();
    }
}
