# ÂM VỌNG NGÀN NĂM (Echoes of A Thousand Years)
Âm Vọng Ngàn Năm (Echoes of a Thousand Years) is a rhythm game that brings Vietnam’s traditional folk music to life in a fresh, exciting way. While many young people today drift toward pop music and global trends, this game turns ancient sounds and instruments like the đàn bầu and sáo trúc into interactive, engaging experiences. By blending cultural heritage with modern gameplay, Âm Vọng Ngàn Năm sparks curiosity and pride, helping a new generation rediscover and protect the soul of Vietnamese music.

## Functions
### RHYTHM MODE “PHIÊU LƯU”
Dive into fast-paced stages where each level is a Vietnamese folk song turned into a thrilling one-key rhythm game. **Tap the spacebar** to the beat, master the rhythm, and unlock new songs.

### FREE-PLAY MODE “DIỄN TẤU”
Unleash your inner musician with traditional instruments! Play freely across three octaves, experiment with melodies, and explore the sounds of Vietnam’s musical heritage in your own style.

### LIBRARY MODE “THƯ VIỆN”
Go beyond the music - every instrument and song you unlock reveals its story. Blend your gameplay with meaningful knowledge!

## Before running
- In `Assets/`, create a new folder `_env`
- Add a new class `ENV_CONFIG` and modify it as below:
```
public class ENV_CONFIG
{
    public static string AUTH_URL = "http://localhost:5000/api/auth"; //link to your game auth API

    public static string SONG_URL = "http://localhost:5000/api/songs"; //link to your game song API

    public static string STAT_URL = "http://localhost:5000/api/game-status"; //link to your game status API

    public static string SIGN_UP_URL = "http://localhost:3000/signup"; //link to your sign up UI
}
```
## Running
- Clone and set up the backend repository [here](https://github.com/DanLinhHuynh-Niwashi/AmVongNganNam-Admin).
- Run the backend program by `npm start`. Please ensure that the links are set correctly.
- In Unity Editor, run your game **from GameTitleScene**.

