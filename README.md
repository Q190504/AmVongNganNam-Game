# Before running
- In `Assets/`, create a new folder `_env`
- Add a new class `ENV_CONFIG` and modify it as below:
```
public class ENV_CONFIG
{
    public static string AUTH_URL = "http://localhost:5000/api/auth"; //link to your game auth API

    public static string SONG_URL = "http://localhost:5000/api/songs"; //link to your game song API

    public static string STAT_URL = "http://localhost:5000/api/game-status"; //link to your game status API
}
```
