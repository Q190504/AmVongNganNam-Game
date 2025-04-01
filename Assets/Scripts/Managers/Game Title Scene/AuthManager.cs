using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class AuthManager : MonoBehaviour
{
    public bool isAuthenticated;

    [Header("Login Info")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;
    public TextMeshProUGUI loginMessage;

    public BoolPublisherSO updateUISO;
    public StringPublisherSO updateErrorTextSO;

    private static string apiUrl = "https://avnn-server.onrender.com/api/auth";

    void Start()
    {
        StartCoroutine(StartLoginCheck());
    }

    public void OnLoginButtonClick()
    {
        StartCoroutine(Login());
    }

    public void OnLogoutButtonClick()
    {
        StartCoroutine(Logout());
    }

    private IEnumerator StartLoginCheck()
    {
        yield return StartCoroutine(CheckLoginStatus());

        updateUISO.RaiseEvent(isAuthenticated);
    }

    public IEnumerator CheckLoginStatus()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl + "/me");
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            isAuthenticated = true;
            string responseJson = request.downloadHandler.text;
            UserResponse response = JsonUtility.FromJson<UserResponse>(responseJson);
            PlayerPrefs.SetString("UID", response.user.id);
            PlayerPrefs.SetString("name", response.user.name);
            Debug.Log("User is logged in..." + PlayerPrefs.GetString("UID"));
        }
        else
        {
            isAuthenticated = false;
            Debug.Log("User not logged in.");
        }
    }

    IEnumerator Login()
    {
        loginMessage.text = "Logging in...";

        LoginData loginData = new LoginData
        {
            email = loginEmail.text,
            password = loginPassword.text
        };

        
        string jsonData = JsonUtility.ToJson(loginData);
        Debug.Log(jsonData);
        UnityWebRequest request = new UnityWebRequest(apiUrl + "/login", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(responseJson);            
            Debug.Log("Login Successful! Token: " + response.token);
            StartCoroutine(StartLoginCheck());
        }
        else
        {
            loginMessage.gameObject.SetActive(true);
            loginMessage.text = "Error: " + request.error;
        }
    }

    IEnumerator Logout()
    {
        UnityWebRequest request = new UnityWebRequest(apiUrl + "/logout", "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Logout Successful!");
            StartCoroutine(StartLoginCheck());
            PlayerPrefs.DeleteKey("name");
            PlayerPrefs.DeleteKey("UIT");
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }


    [System.Serializable]
    public class LoginData
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    public class RegisterData
    {
        public string name;
        public string email;
        public string password;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string message;
        public User user;
        public string token;
    }

    [System.Serializable]
    public class UserResponse
    {
        public User user;
    }

    [System.Serializable]
    public class User
    {
        public string id;
        public string name;
        public string email;
    }
}
