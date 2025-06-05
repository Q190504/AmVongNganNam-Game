using UnityEngine;

public class SignUpURLClickHandler : URLClickHandler
{
    private void Awake()
    {
        url = ENV_CONFIG.SIGN_UP_URL;
    }
}
