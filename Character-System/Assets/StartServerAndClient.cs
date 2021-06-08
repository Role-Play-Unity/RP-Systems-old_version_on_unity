using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class StartServerAndClient : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.StartHost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
