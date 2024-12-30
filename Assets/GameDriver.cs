using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;

public class GameDriver : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;

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
    
    [SerializeField] private List<AssetReference> enemyTemplates;
    private int currentLevel = 0;
    
    private void Start()
    {
        mainCamera = Camera.main;
        cardManager.Initialize();
        
        // Set cursor texture
        SetCursorTexture();

        CreatePlayer(playerTemplate);
        CreateEnemy(enemyTemplate);

        // 初始化 characterDeathManager
        if (characterDeathManager == null)
        {
            characterDeathManager = GetComponent<CharacterDeathManager>();
            if (characterDeathManager == null)
            {
                Debug.LogError("CharacterDeathManager reference not found!");
                return;
            }
        }
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

            playerHp.Value = 50;
            playerShield.Value = 0;
            playerManaManager.SetDefaultMana(3);
            
            CreateHpWidget(playerHpWidget, player, playerHp, 50, playerShield);
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

            enemyHp.Value = 50;
            enemyShield.Value = 10;
            
            CreateHpWidget(enemyHpWidget, enemy, enemyHp, 50, enemyShield);
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

    private void StartBossFight()
    {
        if (currentLevel < 0 || currentLevel >= enemyTemplates.Count)
        {
            Debug.LogError("Invalid currentLevel index: " + currentLevel);
            return;
        }

        var bossTemplateRef = enemyTemplates[currentLevel];
        bossTemplateRef.LoadAssetAsync<EnemyTemplate>().Completed += handle =>
        {
            var template = handle.Result;
            var enemy = Instantiate(template.Prefab, enemyPivot);
            
            enemyHp.Value = 100;
            enemyShield.Value = 0;
            
            CreateHpWidget(enemyHpWidget, enemy, enemyHp, 100, enemyShield);
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
            Initialize();
        };
    }

        public void OnEnemyDefeated()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
        
        currentLevel++;
        if (currentLevel < enemyTemplates.Count)
        {
            StartNextEnemy();
        }
        else
        {
            Debug.Log("Game Won!");
            if (characterDeathManager != null)
            {
                characterDeathManager.EndGame(false);
            }
            else
            {
                Debug.LogError("CharacterDeathManager reference is null!");
            }
        }
    }

    private void StartNextEnemy()
    {
        if (currentLevel < 0 || currentLevel >= enemyTemplates.Count)
        {
            Debug.LogError("Invalid currentLevel index: " + currentLevel);
            return;
        }

        var enemyTemplateRef = enemyTemplates[currentLevel];
        enemyTemplateRef.LoadAssetAsync<EnemyTemplate>().Completed += handle =>
        {
            var template = handle.Result;
            var enemy = Instantiate(template.Prefab, enemyPivot);
            
            enemyHp.Value = 100; // 增加敌人的血量
            enemyShield.Value = 20; // 增加敌人的护盾
            
            CreateHpWidget(enemyHpWidget, enemy, enemyHp, 100, enemyShield);
            CreateIntentWidget(enemyIntentWidget, enemy);
            
            var obj = enemy.GetComponent<CharacterObject>();
            obj.Template = template;
            obj.Character = new RuntimeCharacter 
            { 
                Hp = enemyHp, 
                Shield = enemyShield,
                Mana = 10, // 增加敌人的行动点
                MaxHp = 100
            };
            
            enemies.Add(enemy);
            Initialize();
        };
    }
}