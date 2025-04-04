using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public bool isLoginSuccess;

    [Header("Login Info")]
    public TMP_InputField userNameInput;
    public TMP_InputField passwordInput;

    public BoolPublisherSO updateUISO;
    public StringPublisherSO updateErrorTextSO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TO DO: Check & set isloginSuccess
        updateUISO.RaiseEvent(isLoginSuccess);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckValidLoginInfo()
    {
        //TO DO: check info
        updateUISO.RaiseEvent(isLoginSuccess);

        //TO DO: check what error
        if (!isLoginSuccess)
            updateErrorTextSO.RaiseEvent("Error");
    }
}
