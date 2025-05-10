
using System;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem Instance { get; private set; }
    
    private const string CHANNEL_ID = "game_channel";
    private const string PERMISSION_KEY = "notifications_enabled";

    public static event Action<bool> OnNotificationPermissionChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeNotifications();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeNotifications()
    {
        bool hasPermission = Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS");
        EnableNotifications(hasPermission);

        if (!hasPermission)
        {
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += (permission) => EnableNotifications(true);
            callbacks.PermissionDenied += (permission) => EnableNotifications(false);
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS", callbacks);
        }

        CreateNotificationChannel();
    }


    private void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = CHANNEL_ID,
            Name = "Game Notifications",
            Importance = Importance.High,
            Description = "Game notifications channel"
        };
        
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void SendNotification(string title, string text, int fireTimeInSeconds)
    {

        if (NotificationsEnabled() && Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            var notification = new AndroidNotification
            {
                Title = title,
                Text = text,
                FireTime = DateTime.Now.AddSeconds(fireTimeInSeconds)
            };
            AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
        }
    }

    public void EnableNotifications(bool enable)
    {
        PlayerPrefs.SetInt(PERMISSION_KEY, enable ? 1 : 0);
        OnNotificationPermissionChanged?.Invoke(enable);
    }

    public bool NotificationsEnabled()
    {
        return PlayerPrefs.GetInt(PERMISSION_KEY, 0) == 1;
    }
}
