using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;
    [SerializeField]
    private TMP_Text _readyUpText;
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI roomID;
    [SerializeField]
    private TextMeshProUGUI playerID;

    private List<PlayerListing> _listing = new List<PlayerListing>();
    private RoomsCanvases _roomsCanvases;

    int playerNum;

    public override void OnEnable()
    {
        base.OnEnable();
        GetCurrentRoomPlayers();

        roomID.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
        playerID.text = "Your ID: " + PhotonNetwork.NickName;

        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            button.gameObject.SetActive(false);
        } else {
            button.gameObject.SetActive(true);
        }
    }

    //Fix bug duplicate player  
    public override void OnDisable()
    {
          base.OnDisable();
          for (int i = 0; i <_listing.Count; i++)
              Destroy(_listing[i].gameObject);
          _listing.Clear();
    }

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public override void OnLeftRoom()
    {
        _content.DestroyChildren();
    }

    public void GetCurrentRoomPlayers()
    {
        if(!PhotonNetwork.IsConnected)
        {
            return;
        }
        if(PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
        {
            return;
        }
        foreach (KeyValuePair<int, Player> PlayerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(PlayerInfo.Value);
        }

    }

    private void AddPlayerListing(Player player)
    {
        int index = _listing.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _listing[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = (PlayerListing)Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listing.Add(listing);
            }
        }

    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listing.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listing[index].gameObject);
            _listing.RemoveAt(index);
        }
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel(2);
            }
        }
    }

}
