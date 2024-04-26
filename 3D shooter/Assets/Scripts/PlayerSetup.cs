using UnityEngine;
using UnityEngine.Networking;
using Mirror;


[RequireComponent (typeof (Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private string remoteLayer = "RemotePlayer";
    private Camera sceneCamera;

  
    [SerializeField]
    Behaviour[] componentsToDisable;

    //Отключение компонентов: слои (организация объектов в иерархии и обработка столкновений), камера (присваевает игрока иммено его камеру)

    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
                componentsToDisable[i].enabled = false;
            gameObject.layer = LayerMask.NameToLayer(remoteLayer); //Слои

        }
        else
        {
            
            sceneCamera = Camera.main; // Камера
            if (sceneCamera != null)
                sceneCamera.gameObject.SetActive(false);
        }

        

    }


    public override void OnStartClient ()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString(); //Добавление тега каждому игроку
        Player player = GetComponent<Player>();
        string playerName = "Player " + netID;
        
        gameObject.name = playerName;
        GameManager.RegisterPlayer(netID, player, playerName);



    }

    void OnDisable()
    {
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);  //Отключение стартовой камеры (меню подключений) после запуска игры

        GameManager.UnRegisterPlayer(transform.name);

    }
}
