using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(InputField))]
public class PlayerTeamInputField : MonoBehaviour
{
    #region Private Constants

    // Store the PlayerPref Key to avoid typos
    const string playerTeamPrefKey = "1";

    #endregion

    #region MonoBehaviour CallBacks

    // Start is called before the first frame update
    void Start()
    {
        string defaultTeam = "1";
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerTeamPrefKey))
            {
                defaultTeam = PlayerPrefs.GetString(playerTeamPrefKey);
                _inputField.text = defaultTeam;
            }
        }


        //PhotonNetwork.PlayerTeam = defaultTeam;
    }

    #endregion

    #region Public Methods

    /// Sets the team of the player, and save it in the PlayerPrefs for future sessions.
    /// <param team="value">Player's team number</param>
    public void SetPlayerTeam(string value)
    {
        //string defaultTeam = "1";
        // #Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Team is null or empty");
            return;
        }
        Hashtable playerProperties = new Hashtable();
        playerProperties.Add("PlayerTeam", value);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        //PhotonNetwork.LocalPlayer.PlayerTeam = value;
        //PhotonNetwork.NickName = value;


        PlayerPrefs.SetString(playerTeamPrefKey, value);
        Debug.Log("Player team: " + PhotonNetwork.LocalPlayer.CustomProperties["PlayerTeam"]);
    }

    #endregion
}
