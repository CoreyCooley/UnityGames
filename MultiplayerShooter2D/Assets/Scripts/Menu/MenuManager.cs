using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{

    // Menu Screens
    [SerializeField]
    private GameObject playerNameScreen;
    [SerializeField]
    private GameObject connectScreen;

    [SerializeField]
    private GameObject createPlayerNameButton;

    // Input Fields
    [SerializeField]
    private InputField playerNameInput;
    [SerializeField]
    private InputField createRoomInput;
    [SerializeField]
    private InputField joinRoomInput;

    private const int GAME_SCENE = 1;
    private const int MAX_PLAYERS = 4;

    private void Awake() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        Debug.Log("Got connected to master!");

        // Join default lobby... once photon connects to a lobby it calls OnJoinedLobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby(){
        Debug.Log("Joined lobby");
        playerNameScreen.SetActive(true);
    }

    public override void OnJoinedRoom(){
        Debug.Log("Joined room");

        // Load game scene using Photon
        PhotonNetwork.LoadLevel(GAME_SCENE);
    }

    #region UIMethods

    // Set Player IGN
    public void OnClick_CreatePlayerNameButton()
    {
        string playerName = playerNameInput.text;

        PhotonNetwork.NickName = playerName;
        Debug.Log($"Player Name {PhotonNetwork.NickName}");

        playerNameScreen.SetActive(false);
        connectScreen.SetActive(true);
    }

    // Hide / Show Create Player Name Button
    public void OnChanged_PlayerNameInput()
    {
        string playerName = playerNameInput.text;

        if(playerName.Length > 1)
            createPlayerNameButton.SetActive(true);
        else
            createPlayerNameButton.SetActive(false);
    }

    // Join Room Button
    public void OnClick_JoinRoomButton()
    {
        RoomOptions roomOpts = new RoomOptions();
        string roomName = joinRoomInput.text;

        roomOpts.MaxPlayers = MAX_PLAYERS;
        // OnJoinedRoom will be called
        if(roomName.Length > 1)
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOpts, TypedLobby.Default);
    }
    // Create Room Button
    public void OnClick_CreateRoomButton()
    {
        RoomOptions roomOpts = new RoomOptions();
        string roomName = joinRoomInput.text;

        roomOpts.MaxPlayers = MAX_PLAYERS;

        if(roomName.Length > 1)
            PhotonNetwork.CreateRoom(roomName, roomOpts, null);
    }
    #endregion
}
