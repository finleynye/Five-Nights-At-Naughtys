using System;
using Discord;
using UnityEngine;
using UnityEngine.UI;

public class DiscordController : MonoBehaviour
{
    public long applicationID;
    private static bool _instanceExists;
    private Discord.Discord discord;
    private NightCycle _nightCycle;

    public string details = $"Surviving the night";
    public string state = $"Night: ";

    public string gameLogo;
    public string titleText = $"Five Nights At Naughtys";

    private long _time;

    private void Awake()
    {
        if (!_instanceExists)
        {
            _instanceExists = true;
            DontDestroyOnLoad(this.gameObject);
        }
        else if(FindObjectsOfType(GetType()).Length > 1)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
        _nightCycle = GetComponent<NightCycle>();
        _time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        
        UpdateStatus();
    }

    private void Update()
    {
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(this.gameObject);
        }
    }

    private void LateUpdate()
    {
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        try
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = details,
                State = $"{state}{_nightCycle.nightCount}",
                Assets =
                {
                    LargeImage = gameLogo,
                    LargeText = titleText
                },
                Timestamps =
                {
                    Start = _time
                }
            };
            
            activityManager.UpdateActivity(activity, (res) =>
            {
                if(res != Discord.Result.Ok) Debug.LogWarning("Failed to connect to Discord!");
            });
        }
        catch 
        {
            Destroy(this.gameObject);
        }
    }
}
