﻿using UnityEngine;
using nullbloq.Noodles;

public class GameManager : MonoBehaviour
{
    SaveData gameData;

    [Header("Save Components: ")]
    [SerializeField] Noodler noodler = null;
    [SerializeField] StoryManager storyManager = null;
    [SerializeField] ActionController actionController = null;
    [SerializeField] BackgroundController backgroundController = null;
    [SerializeField] DecisionCheckController decisionCheckController = null;
    [SerializeField] DialogueController dialogueController = null;
    [SerializeField] FilterController filterController = null;
    [SerializeField] MusicController musicController = null;

    void Awake()
    {
        SetGameData();
    }

    void Start()
    {
        SoundManager.Get().PlaySong(SoundManager.Songs.Protagonist);
    }

    void OnEnable()
    {
        UIManager_Gameplay.OnGameSave += SaveGameData;
        StoryManager.OnNoScenesRemaining += GoToMainMenu;
    }

    void OnDisable()
    {
        UIManager_Gameplay.OnGameSave -= SaveGameData;
        StoryManager.OnNoScenesRemaining -= GoToMainMenu;
    }

    void SetGameData()
    {
        gameData = SaveManager.Get().LoadFile();

        storyManager.SetLoadedData(gameData);
        actionController.SetLoadedData(gameData);
        backgroundController.SetLoadedData(gameData);
        decisionCheckController.SetLoadedData(gameData);
        dialogueController.SetLoadedData(gameData);
        filterController.SetLoadedData(gameData);
        musicController.SetLoadedData(gameData);
    }

    void UpdateGameData()
    {
        gameData.routeExecutionStarted = storyManager.RouteExecutionStarted;
        gameData.lastDecisionGood = decisionCheckController.LastDecisionGood;
        gameData.currentNodeGUID = noodler.CurrentNode.GUID;
        gameData.routeSceneIndex = storyManager.RouteSceneIndex;
        gameData.currentRoute = storyManager.CurrentRoute;
        gameData.currentFilter = filterController.CurrentFilter;
        gameData.backgroundData = backgroundController.CurrentBackgroundData;
        gameData.logData = dialogueController.GetLogData();


        gameData.currentDialogueStripIndex = dialogueController.CurrentDialogueStripIndex - 1;
        if (gameData.currentDialogueStripIndex < 0) gameData.currentDialogueStripIndex = 0;

        gameData.charactersInScene = actionController.CharactersInScene;

        gameData.musicData = new SaveData.MusicData()
        {
            musicPlaying = musicController.MusicPlaying,
            songTitle = musicController.CurrentSong
        };
    }
    
    void SaveGameData(bool saveAsJson)
    {
        UpdateGameData();
        SaveManager.Get().SaveFile(gameData, saveAsJson);
    }

    void GoToMainMenu()
    {
        SceneLoadManager.Get().LoadMainMenu();
    }
}