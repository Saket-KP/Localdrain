using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Niantic.Lightship.SharedAR.Colocalization;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartGameAR : MonoBehaviour
{
    [SerializeField] private SharedSpaceManager _sharedSpaceManager;
    private const int MAX_AMOUNT_CLIENTS_ROOM = 2;

    [SerializeField] private Texture2D _targetImage;
    [SerializeField] private float _targetImageSize;
    private string roomName = "TestRoom";

    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button CreateRoomButton;
    [SerializeField] private Button JoinRoomButton;
    private bool isHost;

    public static event Action OnStartSharedSpaceHost;
    public static event Action OnStartSharedSpaceClient;
    public static event Action OnStartGame;
    public static event Action OnStartSharedSpace;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _sharedSpaceManager.sharedSpaceManagerStateChanged += SharedSpaceManagerOnsharedSpaceManagerStateChanged;

        StartGameButton.onClick.AddListener(StartGame);
        CreateRoomButton.onClick.AddListener(CreateGameHost);
        JoinRoomButton.onClick.AddListener(JoinGameClient);

        StartGameButton.interactable = false;

    }

    private void SharedSpaceManagerOnsharedSpaceManagerStateChanged(SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs obj)
    {
        if (obj.Tracking)
        {
            StartGameButton.interactable = true;
            CreateRoomButton.interactable = false;
            JoinRoomButton.interactable = false;

        }
    }

    void StartGame()
    {
        OnStartGame?.Invoke();


        if (isHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }
    }

    void StartSharedSpace()
    {
        OnStartSharedSpace?.Invoke();

        if (_sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.MockColocalization)
        {
            var mockTrackingArgs = ISharedSpaceTrackingOptions.CreateMockTrackingOptions();
            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomName,
                capacity: MAX_AMOUNT_CLIENTS_ROOM,
                description: "MockColonizationDemo"
            );

            _sharedSpaceManager.StartSharedSpace(mockTrackingArgs, roomArgs);
            return;
        }
        
        if (_sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.ImageTrackingColocalization)
        {
            var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                _targetImage, _targetImageSize
            );

            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomName,
                capacity:MAX_AMOUNT_CLIENTS_ROOM,
                description:"ImageColonization"
            );

            _sharedSpaceManager.StartSharedSpace(imageTrackingOptions,roomArgs);
            return;
        }

    }

    void CreateGameHost()
    {
        isHost = true;
        OnStartSharedSpaceHost?.Invoke();
        StartSharedSpace();
    }

    void JoinGameClient()
    {
        isHost = false;
        OnStartSharedSpaceClient?.Invoke();
        StartSharedSpace();
    }



}
