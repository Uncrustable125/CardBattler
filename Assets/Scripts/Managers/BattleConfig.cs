using UnityEngine;

public class BattleConfig
{
    public GameObject cardPrefab;
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public GameObject buttonPrefab;
    public Button endTurnButton;
    public PlayerCharacter character;
    public TMPro.TextMeshProUGUI gameOverText;
    public Transform cameraParent;


    public BattleConfig(
    GameObject cardPrefab,
    GameObject enemyPrefab,
    GameObject playerPrefab,
    GameObject buttonPrefab,
    Button endTurnButton,
    PlayerCharacter character,
    TMPro.TextMeshProUGUI gameOverText,
    Transform backgroundParent)
    {
        this.cardPrefab = cardPrefab;
        this.enemyPrefab = enemyPrefab;
        this.playerPrefab = playerPrefab;
        this.buttonPrefab = buttonPrefab;
        this.endTurnButton = endTurnButton;
        this.character = character;
        this.gameOverText = gameOverText;
        this.cameraParent = backgroundParent;
    }

}
