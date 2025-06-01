using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using static AuthManager;

public class AuthManager : MonoBehaviour
{
    public bool isAuthenticated;

    [Header("Login Info")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;
    public TextMeshProUGUI loginMessage;

    public BoolPublisherSO updateUISO;
    public StringPublisherSO errorPublisher;

    //private static string apiUrl = "https://avnn-server.onrender.com/api/auth";

    private static string apiUrl = "http://localhost:5000/api/auth";
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
        string token = PlayerPrefs.GetString("token", "");
        if (!string.IsNullOrEmpty(token))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
        }
        else
        {
            Debug.Log("No token found in PlayerPrefs.");
        }
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
            if (response.user.isAdmin)
            {
                loginMessage.gameObject.SetActive(true);
                loginMessage.text = "Lỗi đăng nhập: Tài khoản không hợp lệ.";
                StartCoroutine(Logout());
            } else
            {
                Debug.Log("Login Successful! Token: " + response.token);
                PlayerPrefs.SetString("token", response.token);
                StartCoroutine(StartLoginCheck());
            }
        }
        else
        {
            loginMessage.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(request.downloadHandler.text))
            {
                try
                {
                    ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                    loginMessage.text = "Lỗi đăng nhập: " + errorResponse.message;
                }
                catch
                {
                    errorPublisher.RaiseEvent("Failed to login status: " + request.error);
                }
            }
            else
            {
                errorPublisher.RaiseEvent("Failed to login: " + request.error);
            }
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
            PlayerPrefs.DeleteKey("UID");
            PlayerPrefs.DeleteKey("token");
        }
        else
        {
            errorPublisher.RaiseEvent("Failed to logout: " + request.error);
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
        public bool isAdmin;
    }
    [System.Serializable]
    public class ErrorResponse
    {
        public string message;
    }
}
