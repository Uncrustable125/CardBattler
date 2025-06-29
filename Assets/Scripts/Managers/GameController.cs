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
    GameState gameState;
    BattleManager battleManager;
    public GameObject cardPrefab, enemyPrefab, playerPrefab, buttonPrefab;
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
        battleManager.CreateGame(character, this);



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