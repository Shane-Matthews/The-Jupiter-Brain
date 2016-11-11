using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Listener : MonoBehaviour
{
    public NotificationsManager Notifications = null;

    void Start() { 

        if(Notifications!=null)
            Notifications.AddListener(this, "KillBarrierDeath");
    }
    public void KillBarrierDeath(Component Sender)
    {
        Debug.Log("ded");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}