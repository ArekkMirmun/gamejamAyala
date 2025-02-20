using System;
using UnityEngine;
using UnityEngine.InputSystem;

//Enum for the different weapon types
public enum WeaponType
{
    Hand,
    Sword,
    Gun,
    Rifle
}

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance;
    
    public int playerCurrentHP;
    public int maxHP = 5;
    public int currentLevel = 1;
    public int currentHeals = 1;
    public int enemiesToLevelUp = 3;
    
    //Player prefabs for different weapons
    public GameObject playerPrefabBase;
    public GameObject playerPrefabSword;
    public GameObject playerPrefabGun;
    public GameObject playerPrefabRifle;
    
    //Enemy prefabs
    public GameObject enemyPrefabSword;
    public GameObject enemyPrefabGun;
    public GameObject enemyPrefabRifle;
    public GameObject enemyPrefabBoss;
    
    //Weapons prefabs
    public GameObject weaponPrefabSword;
    

    public WeaponType currentWeapon;
    public GameObject currentPlayerPrefab;
    private bool _swordPicked;
    
    public PlayerInput playerInput;
    
    public SceneManager sceneManager;

    public void Start()
    {
        Instance = this;
        ChangeWeapon(WeaponType.Hand);
        currentPlayerPrefab = playerPrefabBase;
    }
    
    public bool ShouldEquipWeapon(WeaponType weapon)
    {
        // Check if the new weapon has a higher priority than the current weapon
        return weapon > currentWeapon;
    }

    public void ChangeWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;
        switch (weapon)
        {
            case WeaponType.Hand:
                currentPlayerPrefab = playerPrefabBase;
                break;
            case WeaponType.Sword:
                currentPlayerPrefab = playerPrefabSword;
                break;
            case WeaponType.Gun:
                currentPlayerPrefab = playerPrefabGun;
                break;
            case WeaponType.Rifle:
                currentPlayerPrefab = playerPrefabRifle;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        //if current HP is max HP, increase max HP by 5, and set current HP to max HP
        //else increase current HP by 3
        maxHP += 5;

        if (playerCurrentHP == maxHP)
        {
            playerCurrentHP = maxHP;
        }
        else
        {
            playerCurrentHP += 3;
        }
    }
    
    public void HealMax()
    {
        playerCurrentHP = maxHP;
    }
    
    
    public void EnemyDefeated(GameObject enemy)
    {
        //If there is one enemy left to level up and the enemy defeated is sword type
        //, instantiate a gun on enemy position
        if (enemiesToLevelUp == 1 && enemy.GetComponent<EnemyInfo>().enemyType == EnemyType.Sword && !_swordPicked)
        {
            Instantiate(weaponPrefabSword, enemy.transform.position, Quaternion.identity);
            _swordPicked = true;
        }
        Destroy(enemy);

        if (enemy.GetComponent<EnemyInfo>().enemyType == EnemyType.Boss)
        {
            sceneManager.LoadScene("GameWin");

        }
        
        //Enable player input
        playerInput.GetComponent<CharacterMovement>().UnfreezeMovement();
        //Decrease enemies to level up
        enemiesToLevelUp--;
        if (enemiesToLevelUp <= 0)
        {
            LevelUp();
            enemiesToLevelUp = 3;
        }
    }
    
    public void PickUpHeal()
    {
        currentHeals++;
    }
    
    public int GetCurrentHeals()
    {
        return currentHeals;
    }
    
    public void EnemyEncountered(GameObject enemy)
    {
        //Get the enemy type from the EnemyInfo component
        EnemyType enemyType = enemy.GetComponent<EnemyInfo>().enemyType;
        //Disable player input with FreezeMovement method
        playerInput.GetComponent<CharacterMovement>().FreezeMovement();
        
        //Start battle with enemy using BattleSystem instance
        switch (enemyType)
        {
            case EnemyType.Sword:
                BattleSystem.Instance.StartBattle(enemy, enemyPrefabSword);
                break;
            case EnemyType.Gun:
                BattleSystem.Instance.StartBattle(enemy, enemyPrefabGun);
                break;
            case EnemyType.Rifle:
                BattleSystem.Instance.StartBattle(enemy, enemyPrefabRifle);
                break;
            case EnemyType.Boss:
                BattleSystem.Instance.StartBattle(enemy, enemyPrefabBoss);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void FreezeMovement()
    {
        playerInput.GetComponent<CharacterMovement>().FreezeMovement();
        TimeChangeScript.Instance.PauseTime();
    }
    public void UnfreezeMovement()
    {
        playerInput.GetComponent<CharacterMovement>().UnfreezeMovement();
        TimeChangeScript.Instance.ResumeTime();
    }
}
