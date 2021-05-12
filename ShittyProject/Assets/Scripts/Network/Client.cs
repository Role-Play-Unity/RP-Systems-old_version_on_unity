using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Client : MonoBehaviour
{
    public Transform Player;
    public Transform PlayerPrefab;

    public int PlayerID = 0;
    public string PlayerName = "{ NAME }";

    private IPEndPoint _iPEndPointPort;
    private Vector3 _oldPosition;

    UdpClient client = new UdpClient();

    private Thread _connectThread;

    private Vector3 _playerPosition = Vector3.one;
    private Vector3 _newPlayerPosition = Vector3.zero;
    private string _newPlayerName = "";

    void Start()
    {
        System.Random rnd = new System.Random();
        PlayerID = rnd.Next(1, 99999999);
        PlayerName = rnd.Next(1, 99999999).ToString();
        Player.name = PlayerName;

        _oldPosition = Vector3.zero;
        _newPlayerName = "";
    }

    private void OnApplicationQuit()
    {
        client.Close();
        _connectThread.Abort();
    }

    private void Update()
    {
        _playerPosition = Player.position;

        lock (_newPlayerName)
            if (_newPlayerName != "")
                InstantiateNewPlayer(_newPlayerPosition, _newPlayerName);
    }

    [Obsolete]
    private void InstantiateNewPlayer(Vector3 position, string Name)
    {
        print(Name + " " + GameObject.Find(Name));

        GameObject plr = GameObject.Find(Name);

        if (Name != PlayerName)
        {
            if (!GameObject.Find(Name))
            {
                GameObject obj = Instantiate(PlayerPrefab, position, Quaternion.identity).gameObject;
                obj.name = _newPlayerName;
            }
            else
            {
                GameObject obj = GameObject.Find(_newPlayerName).gameObject;
                obj.transform.position = position;
            }
        }
    }

    public bool CreateConnect(string Ip, int Port)
    {
        System.Random rnd = new System.Random();
        PlayerID = rnd.Next(1, 99999999);
        PlayerName = rnd.Next(1, 99999999).ToString();

        Player.name = PlayerName;

        _oldPosition = Vector3.zero;
        _newPlayerName = "";
        _playerPosition = Player.position;

        _connectThread = new Thread(() => Connect(Ip, Port));
        _connectThread.Start();

        return true;
    }

    public void Connect(string Ip, int Port)
    {
        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(Ip), Port);
        client.Connect(serverEP);

        //[] receivedData = client.Receive(ref ep);
        //Console.WriteLine("receive data from " + ep.ToString());

        //PlayerData receivedPData = new PlayerData();
        //receivedPData = JsonUtility.FromJson<PlayerData>(ep.ToString());

        Vector3 playerPosition = new Vector3((float)Math.Round(_playerPosition.x, 2),
                                             (float)Math.Round(_playerPosition.y, 2),
                                             (float)Math.Round(_playerPosition.z, 2));


        PlayerData pdata = new PlayerData();
        pdata.Id = PlayerID;
        pdata.Name = PlayerName;
        pdata.Position = playerPosition;

        string jsonData = JsonUtility.ToJson(pdata);

        byte[] connectedMessage = Encoding.ASCII.GetBytes(jsonData);
        client.Send(connectedMessage, connectedMessage.Length);

        //PlayerID = int.Parse(ep.ToString());

        while (true)
        {
            if (!client.Client.Connected)
                return;

            playerPosition = new Vector3((float)Math.Round(_playerPosition.x, 2),
                                         (float)Math.Round(_playerPosition.y, 2),
                                         (float)Math.Round(_playerPosition.z, 2));

            PlayerData d = new PlayerData();
            d.Position = playerPosition;
            d.Name = PlayerName;
            d.Id = PlayerID;

            string json = JsonUtility.ToJson(d);

            byte[] msg = Encoding.ASCII.GetBytes(json);
            byte[] nothing = Encoding.ASCII.GetBytes(" ");

            if (playerPosition != _oldPosition)
                client.Send(msg, msg.Length);
            else
                client.Send(nothing, nothing.Length);

            _oldPosition = playerPosition;

            if (client.Available > 0)
            {
                byte[] receivedData = client.Receive(ref serverEP);

                print(Encoding.ASCII.GetString(receivedData));

                if (Encoding.ASCII.GetString(receivedData) != " ")
                {
                    PlayerData receivedPData = new PlayerData();
                    try
                    {
                        receivedPData = JsonUtility.FromJson<PlayerData>(Encoding.ASCII.GetString(receivedData));

                        lock (_newPlayerName)
                        {
                            _newPlayerName = receivedPData.Name;
                        }

                        _newPlayerPosition = receivedPData.Position;
                    }
                    catch { }

                    //print("Player: " + receivedPData.Name + " Vector3: " + receivedPData.Position.ToString());

                    /*if (!transform.parent.Find(receivedPData.Name))
                    {
                        GameObject obj = Instantiate(PlayerPrefab, receivedPData.Position, Quaternion.identity).gameObject;
                        obj.name = receivedPData.Name;
                    }
                    else
                    {
                        GameObject obj = transform.parent.Find(receivedPData.Name).gameObject;
                        obj.transform.position = receivedPData.Position;
                    }*/
                }

                //print(receivedData);
            }

            Thread.Sleep(10);
        }

        client.Close();
    }
}

[Serializable]
public class PlayerData
{
    public Vector3 Position;
    public string Name;
    public int Id;
} 