using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameController : MonoBehaviour
{
    Deck playerDeck;
    CardIndex cardIndex;
    Deck savedCards;
    bool newGame;
    public PlayerCharacter character;
    GameState gameState;
    BattleManager battleManager;
    [SerializeField] GameObject cardPrefab, enemyPrefab, playerPrefab, buttonPrefab;
    [SerializeField] Button endTurnButton;
    [SerializeField] TMPro.TextMeshProUGUI gameOverText;
    [SerializeField] private Transform backgroundParent;
    BattleConfig battleConfig;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartGame();
    }
    public void StartGame()
    {

        character = PlayerCharacter.Hero;
        battleConfig = new BattleConfig(cardPrefab,
            enemyPrefab,
            playerPrefab,
            buttonPrefab,
            endTurnButton,
            character,
            gameOverText,
            backgroundParent);
        battleManager = gameObject.AddComponent<BattleManager>();
        battleManager.CreateGame(battleConfig);



    }
    private void OnDisable()
    {
        Debug.Log("Deleted GC");
    }
    void Update()
    {
        if(gameState == GameState.Menu)
        {
            //Implement pause or menuing
        }
    }
    
}


public enum GameState { Menu, Play, Pause }