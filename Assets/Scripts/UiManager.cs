using UnityEngine;

public enum UiPage
{
    Home,
    HomeOrPlay,
    Shop
}
public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public UiPage uiPage;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}