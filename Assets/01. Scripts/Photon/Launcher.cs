using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Launcher : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
}
