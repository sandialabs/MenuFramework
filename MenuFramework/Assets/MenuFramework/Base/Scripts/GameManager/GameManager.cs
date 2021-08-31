using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
public class GameManager : MonoBehaviour
{

    [HideInInspector] public GameObject backPanel, backBackPanel, backMinPanel, topBackPanel, warningBackPanel, proxBackPanel;
    [HideInInspector] public int currentTab;

    private string currentLevelName = string.Empty;
    List<AsyncOperation> loadOperations;
    [HideInInspector] public TextMeshPro helpText, followText;
    [HideInInspector] public List<string> sceneNames;
    [HideInInspector]
    [Tooltip("Timeout in seconds before new scene is loaded.")]
    public float waitTimeInSecBeforeLoading = 0.25f;
    [HideInInspector] public AudioFeedBack audioFeedBack;
    [HideInInspector] public Camera cam;
    private bool multiScene, allowMenuToggleOnSceneStart, menuFollowProximity, isSceneActive, isHelpCommandsOn;
    [HideInInspector] public RadialView radialViewMainMenu, radialViewMinMenu;
    [HideInInspector] public GameObject mainMenu, minimizeMenu;
    ///*[HideInInspector]*/ //public VuforiaBehaviour vb;
    private string userName, labelText;
    [HideInInspector] public Text mainMenuLabel;
    [HideInInspector] public List<GameObject> helpMenus;
    [HideInInspector] public bool helpMenuFoldout, sceneListFoldout;
    private int helpMenuCounter;
    [HideInInspector] public List<SceneModel> sceneList;
    [HideInInspector] public GameObject trainingButtons;
    [HideInInspector] public string frameworkGreeting, loginGreetingBegin;



    //Instantiate the list of AsyncOperations
    //**DontDestroyOnLoad(gameObject) will keep the GameManager persistant
    private void Start()
    {
        userName = "default";
        //IMPORTANT
        //Objects and types need to be instantiated if the item is not found
        if (mainMenu == null) { mainMenu = GameObject.Find("MenuSystem"); }
        if (minimizeMenu == null) { minimizeMenu = GameObject.Find("MinimizeMenu"); }
        if (helpMenus == null) { helpMenus = new List<GameObject>(); }
        if (mainMenuLabel == null) { mainMenuLabel = FindObjectOfType<Text>(); }
        if (trainingButtons == null) { trainingButtons = GameObject.Find("TrainingButtons"); }

        helpMenuCounter = 0;
        isHelpCommandsOn = false;
        menuFollowProximity = true;
        allowMenuToggleOnSceneStart = true;
        multiScene = false;
        DontDestroyOnLoad(gameObject);
        loadOperations = new List<AsyncOperation>();
        sceneNames = new List<string>();
        resetMenuCamera();

        ApplicationsBeginSpeech();
    }

    /// <summary>
    /// Update is a default Unity method that will update continously for check changes
    ///  - Example: Checks camera position to menu, changes if needed
    /// </summary>
    void Update()
    {
        if (mainMenu != null)
        {
            if (mainMenu.activeSelf)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(mainMenu.transform.position);
                if (menuFollowProximity == true)
                {
                    if (mainMenu.activeSelf && screenPos.z > 2.0)
                    {
                        radialViewMainMenu.enabled = true;
                        ToggleFollowText();
                    }
                }
            }
            if (minimizeMenu.activeSelf)
            {
                radialViewMinMenu.enabled = true;
            }
        }
    }


    /// <summary>
    /// TransparentBacking will make the background of the menuFramework inactive so the menu looks see through
    /// </summary>
    bool backing = false;
    public void TransparentBacking()
    {
        backing = !backing;
        if (backing)
        {
            audioFeedBack.CustomSpeak("Transparent On");
            backPanel.SetActive(false);
            backBackPanel.SetActive(false);
            backMinPanel.SetActive(false);
            topBackPanel.SetActive(false);
            warningBackPanel.SetActive(false);
            proxBackPanel.SetActive(false);
        }
        else
        {
            audioFeedBack.CustomSpeak("Transparent Off");
            backPanel.SetActive(true);
            backBackPanel.SetActive(true);
            backMinPanel.SetActive(true);
            topBackPanel.SetActive(true);
            warningBackPanel.SetActive(true);
            proxBackPanel.SetActive(true);
        }
    }


    /// <summary>
    /// ApplicationBeginSpeech will set the beginning speech when the application starts and then play out through voice feedback
    /// </summary>
    public void ApplicationsBeginSpeech()
    {
        if (frameworkGreeting != null && frameworkGreeting != "")
        {
            audioFeedBack.CustomSpeak(frameworkGreeting);
        }
    }

    /// <summary>
    /// resetMenuCamera is used to reset the location of the camera if it ever gets messed up in use
    /// </summary>
    public void resetMenuCamera()
    {
        //Reset the camera orientation
        InputTracking.Recenter();
        //XRInputSubsystem xRInput = new XRInputSubsystem();
        //xRInput.TryRecenter();
    }

    /// <summary>
    /// setMainMenuLabel is used to set the main menu label when ever tracing back or main menu is hit
    /// </summary>
    public void setMainMenuLabel()
    {
        if (labelText != null)
        {
            if (mainMenuLabel != null)
            {
                mainMenuLabel.text = labelText;
            }
        }
        else
        {
            mainMenuLabel.text = labelText;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string startingTitle()
    {
        mainMenuLabel.text = frameworkGreeting;
        return frameworkGreeting;
    }

    /// <summary>
    /// Login Greeting is used to set the mainmenu label to the given name at the beginning
    /// </summary>
    /// <param name="userText"></param>
    public void loginGreeting(string userText)
    {
        userName = userText;
        if (userText != null)
        {
            if (mainMenuLabel != null)
            {
                mainMenuLabel.text = "Welcome " + userName + ", Select a training";
            }
        }
        else
        {
            mainMenuLabel.text = "Welcome, Select a training";
        }
        labelText = mainMenuLabel.text;
    }

    /// <summary>
    /// ToggleHelpCommands method this will check to see if the user wants the help menu on or off
    /// </summary>
    public void ToggleHelpCommands()
    {
        isHelpCommandsOn = !isHelpCommandsOn;
        if (audioFeedBack != null)
        {
            if (isHelpCommandsOn == true)
            {
                audioFeedBack.HelpCommandsOnSpeak();
                helpText.text = "Say \"Help Off\"";
            }
            else
            {
                audioFeedBack.HelpCommandsOffSpeak();
                helpText.text = "Say \"Help On\"";
            }
        }
    }

    /// <summary>
    /// ToggleFollowText will change the text for the follow button
    /// </summary>
    public void ToggleFollowText()
    {
        followText.text = "Say \"Follow Off\"";
    }

    /// <summary>
    /// MenuProximityToggle method this will toggle if the menu will be using the warning menu or automatically follow.
    /// </summary>
    public void MenuProximityToggle()
    {
        menuFollowProximity = !menuFollowProximity;
        if (menuFollowProximity == true)
        {
            audioFeedBack.CustomSpeak("Menu Proximity On");
        }
        else
        {
            audioFeedBack.CustomSpeak("Menu Proximity Off");
        }
    }

    /// <summary>
    /// ResetMenuSystem this method will set the popUpMenu active or inactive
    /// </summary>
    public void resetMenuSystem()
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
        }
    }

    /// <summary>
    /// backStChildToInactive will set all of the object under back to inactive
    /// </summary>
    public void backSetChildToInactive()
    {
        trainingButtons.SetChildrenActive(false);
    }

    /// <summary>
    /// OnLoadOperationComplete controls and checks when an async scene has been added to the list
    /// </summary>
    /// <param name="ao"></param>
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (loadOperations.Contains(ao))
        {
            loadOperations.Remove(ao);
        }
        Debug.Log("Load Complete");
    }

    /// <summary>
    /// OnUnloadOperationComplete controls the check if an async scene have been successfully unloaded
    /// </summary>
    /// <param name="ao"></param>
    void OnUnLoadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("UnLoad Complete");
    }

    /// <summary>
    /// checkSceneLoaded this method will take in a scene name and check to see then toggle a scene if already loaded
    /// </summary>
    /// <param name="sceneName"></param>
    public void checkSceneLoaded(string sceneName)
    {
        isSceneActive = false;
        foreach (var scene in sceneList)
        {
            if (scene.sceneName == sceneName && scene.isOn == false)
            {
                scene.toggleObject.SetActive(true);
                scene.isOn = true;

                isSceneActive = true;
                ToggleMenuSetting();
            }
        }
    }

    /// <summary>
    /// LoadScene this method will take in a sceneName parameter
    ///  - Checks to see if miltiScene is on. If not then it will toggle off all of the various trainings that are on.
    ///  - Checks to see if the scene has already been added is not then the scene will be loaded in
    ///  - Checks if seen is already on and load then it will not do anything
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        if (sceneName != null)
        {
            isSceneActive = false;
            if (multiScene == false)
            {
                ClearLevels();
            }
            foreach (var scene in sceneNames)
            {
                if (scene == sceneName)
                {
                    isSceneActive = true;
                }
            }
            if (isSceneActive == true)
            {
                checkSceneLoaded(sceneName);
            }
            else if (!string.IsNullOrWhiteSpace(sceneName) && isSceneActive == false)
            {
                StartCoroutine(LoadLevel(sceneName));
            }
            else
            {
                Debug.Log($"Unsupported or Already Loaded scene name: {sceneName}");
                ToggleMenuSetting();
            }
        }
        else
        {
            Debug.Log("SceneName is not enter in the inspector!");
        }
    }

    public static string lastSceneLoaded = "";
    /// <summary>
    /// Loadlevel will will take in the scene name and allow for the scene to be async loaded
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadLevel(string sceneName)
    {
        if (multiScene == false)
        {
            // Let's wait in case we don't want to switch scenes too abruptly 
            yield return new WaitForSeconds(waitTimeInSecBeforeLoading);
            //ClearLevels();
        }
        if (isSceneActive != true)
        {
            //Create Model Object
            SceneModel sceneModel = new SceneModel();

            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            if (ao == null)
            {
                Debug.LogError("[GameManager] Unable to load level " + sceneName);
            }
            //ListenToSceneTransition();
            //Add all of the scene items into the sceneModel Object
            sceneModel.toggleObject = GameObject.FindGameObjectWithTag("Toggle" + sceneName);
            sceneModel.sceneName = sceneName;
            sceneModel.isOn = true;

            sceneList.Add(sceneModel);

            ao.completed += OnLoadOperationComplete;
            sceneNames.Add(sceneName);
            loadOperations.Add(ao);
            currentLevelName = sceneName;
            InputTracking.Recenter();
            ToggleMenuSetting();
        }
        else
        {
            Debug.Log($"Scene is already active: {sceneName}");
        }
    }

    /// <summary>
    /// ForceClose this method will close all currently running scenes
    /// </summary>
    /// <param name="levelName"></param>
    public void ForceClose(string sceneName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + sceneName);
            return;
        }
        ao.completed += OnUnLoadOperationComplete;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneName"></param>
    public void ForceCloseSpecific(string sceneName)
    {
        ForceClose(sceneName);
        foreach (var scene in sceneList)
        {
            if (scene.sceneName == sceneName)
            {
                sceneList.Remove(scene);
            }
        }
        foreach (var scene in sceneNames)
        {
            if (scene == sceneName)
            {
                sceneNames.Remove(scene);
            }
        }
    }

    /// <summary>
    /// ForceCloseAll this will completely close out scenes and clear them out of all arrays and lists
    /// </summary>
    public void ForceCloseAll()
    {
        foreach (var scene in sceneNames)
        {
            ForceClose(scene);
        }
        sceneList.Clear();
        sceneNames.Clear();
        loadOperations.Clear();
        ToggleOpenMenu();
    }

    /// <summary>
    /// ClearLevels will toggle all scenes that are running
    /// </summary>
    public void ClearLevels()
    {
        if (sceneList.Count > 0)
        {
            foreach (var scene in sceneList)
            {
                if (scene.isOn == true)
                {
                    scene.toggleObject = GameObject.FindGameObjectWithTag("Toggle" + scene.sceneName);
                    scene.toggleObject.SetActive(false);
                    scene.isOn = false;
                }
            }
            ToggleOpenMenu();
        }
    }

    /// <summary>
    /// ToggleMenuEnableDisable change the feedback depending on if it is following or not
    /// </summary>
    public void ToggleMenuEnableDisable()
    {
        allowMenuToggleOnSceneStart = !allowMenuToggleOnSceneStart;
        if (audioFeedBack != null)
        {
            if (allowMenuToggleOnSceneStart == true)
            {
                audioFeedBack.MenuToggleOnSpeak();
            }
            else
            {
                audioFeedBack.MenuToggleOffSpeak();
            }
        }
    }

    /// <summary>
    /// ToggleMenuSetting this method will stop the menu from being toggling going into a scene
    /// </summary>
    public void ToggleMenuSetting()
    {
        if (allowMenuToggleOnSceneStart == true)
        {
            mainMenu.SetActive(false);
            minimizeMenu.SetActive(true);
        }
    }

    /// <summary>
    /// ToggleOpenMenu will make it so the menu will not close when starting a training
    /// </summary>
    public void ToggleOpenMenu()
    {
        mainMenu.SetActive(true);
        minimizeMenu.SetActive(false);
    }

    /// <summary>
    /// EnableMultiScene will allow the user to run multiple scenes at once
    /// </summary>
    public void EnableMultiScene()
    {
        if (audioFeedBack != null)
        {
            if (multiScene == false)
            {
                multiScene = true;
                audioFeedBack.MultiSceneOnSpeak();
            }
            else
            {
                multiScene = false;
                audioFeedBack.MultiSceneOffSpeak();
            }
        }
    }

    /// <summary>
    /// NextHelpMenu this function will change the help menu displayed
    /// </summary>
    public void NextHelpMenu()
    {
        if (helpMenus.Count > 0)
        {
            if (helpMenus.Count - 1 == helpMenuCounter)
            {
                helpMenus[helpMenuCounter].SetActive(false);
                helpMenuCounter = 0;
                helpMenus[helpMenuCounter].SetActive(true);
            }
            else if (helpMenus.Count > helpMenuCounter)
            {
                helpMenus[helpMenuCounter].SetActive(false);
                helpMenuCounter += 1;
                helpMenus[helpMenuCounter].SetActive(true);
            }
            else
            {
                Debug.LogError("Help Menu List out of bounds!");
            }
        }
    }


    /// <summary>
    /// ExitApplication this method will close the application completely
    /// </summary>
    public void ExitApplication()
    {
        Application.Quit();
    }
}
