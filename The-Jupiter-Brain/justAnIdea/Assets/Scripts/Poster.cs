using UnityEngine;
using System.Collections;

public class Poster : MonoBehaviour
{
    public NotificationsManager Notifications = null;

    void Update()
    {
        if (Input.anyKeyDown && Notifications != null)
            Notifications.PostNotification(this, "OnKeyboardInput");
    }
}
