using UnityEngine;
using System.Collections;
[RequireComponent(typeof(NotificationsManager))]

public class GameManager : MonoBehaviour {
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("GameManager").AddComponent<GameManager>();
            return instance;
        }
    }

    public static NotificationsManager Notifications
    {
        get
        {
            if (notifications == null) notifications = instance.GetComponent<NotificationsManager>();
            return notifications;
        }
    }

    private static GameManager instance = null;

    private static NotificationsManager notifications = null;

    void Awake()
    {
        if((instance) && (instance.GetInstanceID() != GetInstanceID()))
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
