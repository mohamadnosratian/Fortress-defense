using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;
public class LoginManager : MonoBehaviour
{
    public TMP_InputField username, password;
    public Button submit, showPassword;
    public Toggle rememberMe;
    public Sprite On, Off;
    public GameObject login;
    // Start is called before the first frame update
    async void Start()
    {
        submit.onClick.AddListener(submitListener);
        showPassword.onClick.AddListener(showPasswordListener);
        await Task.Delay(1);
        await auth_with_token();
        // StartCoroutine(LoadAsynchronously("Download"));
    }
    public void OpenSignUpLink()
    {
        Application.OpenURL("https://atlasgames.org/");
    }
    void showPasswordListener()
    {
        if (password.contentType == TMP_InputField.ContentType.Standard)
        {
            password.contentType = TMP_InputField.ContentType.Password;
            showPassword.image.sprite = On;
        }
        else
        {
            password.contentType = TMP_InputField.ContentType.Standard;
            showPassword.image.sprite = Off;
        }
        password.ForceLabelUpdate();
    }
    async void submitListener()
    {
        await auth_with_userpass();
    }
    public async Task auth_with_token()
    {
        UserResponse auth_result = null;
        try
        {
            auth_result = await APIManager.instance.check_token();
        }
        catch (System.Net.WebException)
        {
            login.SetActive(true);
        }
        if (auth_result != null)
        {
            User.UserProfile = auth_result;
            StartCoroutine(APIManager.instance.LoadAsynchronously("Download"));
        }
    }
    public async Task auth_with_userpass()
    {
        submit.interactable = false;
        Authentication auth = new Authentication { username = username.text, password = password.text };
        AuthenticationResponse auth_result = null;
        try
        {
            auth_result = await APIManager.instance.authenticate(auth);
        }
        catch (System.Net.WebException)
        {
            submit.interactable = true;
        }
        submit.interactable = true;
        if (auth_result != null)
        {
            if (!rememberMe.isOn)
                StartCoroutine(APIManager.instance.LoadAsynchronously("Download"));
            else
            {
                User.Token = auth_result.token;
                await auth_with_token();
            }
        }
    }


}
