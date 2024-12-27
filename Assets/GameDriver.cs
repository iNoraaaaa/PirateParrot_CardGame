using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;


public class GameDriver : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    
    private Camera mainCamera;
    
    public CardBank startingDeck;

    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();

    [Header("Managers")]
    [SerializeField] private CardManager cardManager;

    [SerializeField] private CardDisplayManager cardDisplayManager;

    [SerializeField] private CardDeckManager cardDeckManager;
    
    [SerializeField]  private EffectResolutionManager effectResolutionManager;
    [SerializeField]  private CardSelectionHasArrow cardSelectionHasArrow;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private EnemyAIManager enemyAIManager;
    [SerializeField] private PlayerManaManager playerManaManager;
    [SerializeField] private CharacterDeathManager characterDeathManager;
    
    private List<CardTemplate> _playerDeck = new List<CardTemplate>();

    [Header("Character pivots")] 
    [SerializeField]
    public Transform playerPivot;
    [SerializeField]
    public Transform enemyPivot;
    
    [Header("UI")]
    [SerializeField]
    private Canvas canvas;

    [SerializeField] private ManaWidget manaWidget;

    [SerializeField] private DeckWidget deckWidget;
    [SerializeField] private DiscardPileWidget discardPileWidget;
    

    [SerializeField] private AssetReference enemyTemplate;
    [SerializeField] private AssetReference playerTemplate;

    [SerializeField] private GameObject enemyHpWidget;
    [SerializeField] private GameObject playerHpWidget;
    [SerializeField] private GameObject enemyIntentWidget;
    [SerializeField] private GameObject playerStatusWidget;
    
    [SerializeField] private IntVariable enemyHp;
    [SerializeField] private IntVariable playerHp;
    
    [SerializeField] private IntVariable playerShield;
    [SerializeField] private IntVariable enemyShield;

    [SerializeField] private StatusVariable playerStatus;
    
    
    private void Start()
    {
        mainCamera = Camera.main;
        cardManager.Initialize();
        
        // 设置鼠标图标
        SetCursorTexture();

        CreatePlayer(playerTemplate);
        CreateEnemy(enemyTemplate);
    }

    private void SetCursorTexture()
    {
        float x, y;
        x = cursorTexture.width / 2.0f;
        y = cursorTexture.height / 2.0f;
        Cursor.SetCursor(cursorTexture, new Vector2(x, y), cursorMode);
    }

    private void CreatePlayer(AssetReference playerTemplateReference)
    {
        var handle = Addressables.LoadAssetAsync<HeroTemplate>(playerTemplateReference);
        handle.Completed += operationResult =>
        {
            var template = operationResult.Result;
            player = Instantiate(template.Prefab, playerPivot);
            Assert.IsNotNull(player);

            playerHp.Value = 20;
            playerShield.Value = 0;
            playerManaManager.SetDefaultMana(3);
            
            CreateHpWidget(playerHpWidget, player, playerHp, 30, playerShield);
            CreateStatusWidget(playerStatusWidget, player);
            
            manaWidget.Initialize(playerManaManager.playerManaVariable);
            
            foreach (var item in template.StartingDeck.Items)
            {
                for (int i = 0; i < item.Amount; i++)
                {
                    _playerDeck.Add(item.Card);
                }
            }

            var obj = player.GetComponent<CharacterObject>();
            obj.Template = template;
            obj.Character = new RuntimeCharacter()
            {
                Hp = playerHp, 
                Shield = playerShield,
                Mana = 100, 
                Status = playerStatus,
                MaxHp = 100
            };
            obj.Character.Status.Value.Clear();
            
            Initialize();
        };
    }

    private void CreateEnemy(AssetReference templateReference)
    {
        var handle = Addressables.LoadAssetAsync<EnemyTemplate>(templateReference);
        handle.Completed += operationResult =>
        {
            var pivot = enemyPivot;
            var template = operationResult.Result;
            var enemy = Instantiate(template.Prefab, pivot);

            Assert.IsNotNull(enemy);

            enemyHp.Value = 20;
            enemyShield.Value = 0;
            
            CreateHpWidget(enemyHpWidget, enemy, enemyHp, 20, enemyShield);
            CreateIntentWidget(enemyIntentWidget, enemy);
            
            var obj = enemy.GetComponent<CharacterObject>();
            obj.Template = template;
            obj.Character = new RuntimeCharacter 
            { 
                Hp = enemyHp, 
                Shield = enemyShield,
                Mana = 100, 
                MaxHp = 100
            };
            
            enemies.Add(enemy);
        };
    }

    public void Initialize()
    {
        cardDeckManager.Initialize(deckWidget, discardPileWidget);
        cardDeckManager.LoadDeck(_playerDeck);
        cardDeckManager.ShuffleDeck();

        cardDisplayManager.Initialize(cardManager, deckWidget, discardPileWidget);
        
        //cardDeckManager.DrawCardsFromDeck(5);

        var playerCharacter = player.GetComponent<CharacterObject>();
        var enemyCharacters = new List<CharacterObject>(enemies.Count);

        foreach (var enemy in enemies)
        {
            enemyCharacters.Add(enemy.GetComponent<CharacterObject>());
        }
        
        cardSelectionHasArrow.Initialize(playerCharacter, enemyCharacters);
        enemyAIManager.Initialize(playerCharacter, enemyCharacters);
        effectResolutionManager.Initialize(playerCharacter, enemyCharacters);
        characterDeathManager.Initialize(playerCharacter, enemyCharacters);
        
        turnManager.BeginGame();
    }

    private void CreateHpWidget(GameObject prefab, GameObject character, IntVariable hp, int maxHp, IntVariable shield)
    {
        var hpWidget = Instantiate(prefab, canvas.transform, false);
        var pivot = character.transform;
        var canvasPosition = mainCamera.WorldToViewportPoint(pivot.position + new Vector3(0.0f, -0.3f, 0.0f));
        hpWidget.GetComponent<RectTransform>().anchorMin = canvasPosition;
        hpWidget.GetComponent<RectTransform>().anchorMax = canvasPosition;
        hpWidget.GetComponent<HpWidget>().Initialize(hp, maxHp, shield);
    }

    private void CreateIntentWidget(GameObject prefab, GameObject character)
    {
        var widget = Instantiate(prefab, canvas.transform, false);
        var pivot = character.transform;
        var size = character.GetComponent<BoxCollider2D>().bounds.size;

        var canvasPosition = mainCamera.WorldToViewportPoint(
            pivot.position + new Vector3(0.2f, size.y + 0.7f, 0.0f)
        );
        
        widget.GetComponent<RectTransform>().anchorMin = canvasPosition;
        widget.GetComponent<RectTransform>().anchorMax = canvasPosition;
    }

    private void CreateStatusWidget(GameObject prefab, GameObject character)
    {
        var hpWidget = Instantiate(prefab, canvas.transform, false);
        var pivot = character.transform;
        var canvasPosition = mainCamera.WorldToViewportPoint(pivot.position + new Vector3(0.0f, -0.8f, 0.0f));
        hpWidget.GetComponent<RectTransform>().anchorMin = canvasPosition;
        hpWidget.GetComponent<RectTransform>().anchorMax = canvasPosition;
    }
}
