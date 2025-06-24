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
    PlayerCharacter character;
    BattleManager battleManager;
    [SerializeField]
    GameObject cardPrefab, enemyPrefab, playerPrefab, buttonPrefab;
    [SerializeField]
    Button endTurnButton;
    public TMPro.TextMeshProUGUI gameOverText;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartGame();
    }
    public void StartGame()
    {

        character = PlayerCharacter.Hero;

        battleManager = gameObject.AddComponent<BattleManager>();
        battleManager.CreateGame(savedCards, character, cardPrefab, enemyPrefab, playerPrefab, buttonPrefab);



    }
    private void OnDisable()
    {
        Debug.Log("Deleted GC");
    }
    void Update()
    {
        
    }
    
}
public enum GameState { Menu, GameOver, Battle, PrePostBattle }