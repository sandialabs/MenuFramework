using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public Camera cam = new Camera();
    public GameObject contentQuad, contentBackPlate, backPlate, warningWindowBack, minPanelBack;
    public bool toggleTransparentBackGround = false;


    public Text mainMenuLabel, MainDescription;
    public TextMeshPro helpText = null, followText = null;
    public BoundingBox menuholderBounding, descriptionBounding, menuBounding;
    public ManipulationHandler menuholderMH, descriptionMH, menuMH;

    //Used for manipulation of the menu layout
    public GameObject descriptionBox, menuHolder;
    private bool proximityToggle = true;
    private AudioFeedBack audioFeedBack;

    //Variables for menu control
    [Tooltip("Add more menus to the menu system")]
    public List<GameObject> helpMenus = new List<GameObject>();

    //Variables for menu control
    private bool multiScene = false, allowMenuToggleOnSceneStart = true, isHelpCommandsOn = false;
    private RadialView radialViewMainMenu;
    private GameObject mainMenu, minimizeMenu, trainingButtons = null;

    //private VuforiaBehaviour vb;
    private string userName, userLabelText, startPhrase;
    private int helpMenuCounter = 0;

    [SerializeField] private string startSpeech;
    [SerializeField] private string loginSpeech;
    [SerializeField] string currentTrainingTime;

    private string currentSequenceItem = string.Empty;


    private Quaternion OriginalMenuRotationValue, OriginalDescriptionRotationValue;
    private Vector3 OriginalMenuScaleValue, OriginalMenuPositionValue, OriginalDescriptionScaleValue, OriginalDescriptionPositionValue;

    //Instantiate the list of AsyncOperations
    //**DontDestroyOnLoad(gameObject) will keep the GameManager persistant
    private void Start()
    {
        //IMPORTANT
        //Objects and types need to be instantiated if the item is not found
        cam = GameObject.Find("Camera").GetComponent(typeof(Camera)) as Camera;
        if (!audioFeedBack) { audioFeedBack = GameObject.Find("AudioManager").GetComponent(typeof(AudioFeedBack)) as AudioFeedBack; }
        if (!mainMenu) { mainMenu = GameObject.Find("MenuSystem"); }
        if (!radialViewMainMenu) { radialViewMainMenu = GameObject.Find("MenuSystem1").GetComponent(typeof(RadialView)) as RadialView; }
        if (!mainMenuLabel) { mainMenuLabel = FindObjectOfType<Text>(); }
        if (loginSpeech == null || loginSpeech == "") { loginSpeech = "select a training to get started"; }
        if (startSpeech == null) { startSpeech = ""; }

        OriginalMenuRotationValue = menuHolder.transform.localRotation;
        OriginalMenuScaleValue = menuHolder.transform.localScale;
        OriginalMenuPositionValue = menuHolder.transform.localPosition;

        OriginalDescriptionRotationValue = descriptionBox.transform.localRotation;
        OriginalDescriptionScaleValue = descriptionBox.transform.localScale;
        OriginalDescriptionPositionValue = descriptionBox.transform.localPosition;

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
                if (proximityToggle)
                {
                    Vector3 screenPos = cam.WorldToScreenPoint(mainMenu.transform.position);

                    if (mainMenu.activeSelf && screenPos.z > 3.0)
                    {
                        radialViewMainMenu.enabled = true;
                        followText.text = "Say \"Follow Off\"";
                    }
                }
            }
        }
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
                mainMenuLabel.text = "Welcome " + userName + ", " + loginSpeech;
            }
        }
        else
        {
            if (mainMenuLabel != null)
            {
                mainMenuLabel.text = loginSpeech;
            }
        }
        userLabelText = mainMenuLabel.text;
    }

    /// <summary>
    /// TransparentBackGroundOn will turn on the transparent background making the back panels active false
    /// </summary>
    public void TransparentBackGroundOn()
    {
        audioFeedBack.CustomSpeak("Transparent On");
        contentQuad.SetActive(false);
        contentBackPlate.SetActive(false);
        backPlate.SetActive(false);
        warningWindowBack.SetActive(false);
        minPanelBack.SetActive(false);
    }

    /// <summary>
    /// TransparentBackGroundOff will get the background panels active to false making the invisible
    /// </summary>
    public void TransparentBackGroundOff()
    {
        audioFeedBack.CustomSpeak("Transparent Off");
        contentQuad.SetActive(true);
        contentBackPlate.SetActive(true);
        backPlate.SetActive(true);
        warningWindowBack.SetActive(true);
        minPanelBack.SetActive(true);
    }


    public void SetDescriptionText(string description)
    {
        MainDescription.text = description;
    }

    public void ResetMenuEdit()
    {
        menuHolder.transform.localRotation = Quaternion.Slerp(transform.localRotation, OriginalMenuRotationValue, Time.deltaTime);
        menuHolder.transform.localScale = Vector3.Lerp(transform.localScale, OriginalMenuScaleValue, Time.deltaTime);
        menuHolder.transform.localPosition = Vector3.Lerp(transform.localPosition, OriginalMenuPositionValue, Time.deltaTime);

        descriptionBox.transform.localRotation = Quaternion.Slerp(transform.localRotation, OriginalDescriptionRotationValue, Time.deltaTime);
        descriptionBox.transform.localScale = Vector3.Lerp(transform.localScale, OriginalDescriptionScaleValue, Time.deltaTime);
        descriptionBox.transform.localPosition = Vector3.Lerp(transform.localPosition, OriginalDescriptionPositionValue, Time.deltaTime);
    }


    /// <summary>
    /// TransparentBackGround will toggle between making the menu system transparent or not for visual aid.
    /// </summary>
    public void TransparentBackGround()
    {
        toggleTransparentBackGround = !toggleTransparentBackGround;
        if (toggleTransparentBackGround)
            TransparentBackGroundOn();
        else
            TransparentBackGroundOff();
    }


    /// <summary>
    /// TransparentBackGround will toggle between making the menu system transparent or not for visual aid.
    /// </summary>
    bool toggleMenuEdit = false;
    public void MenuEdit()
    {
        toggleMenuEdit = !toggleMenuEdit;
        if (toggleMenuEdit)
        {
            menuholderBounding.enabled = true;
            menuholderMH.enabled = true;
            descriptionBounding.enabled = true;
            descriptionMH.enabled = true;
            audioFeedBack.CustomSpeak("Menu Edit On");
        }
        else
        {
            menuholderBounding.enabled = false;
            menuholderMH.enabled = false;
            descriptionBounding.enabled = false;
            descriptionMH.enabled = false;
            audioFeedBack.CustomSpeak("Menu Edit Off");
        }
    }

    /// <summary>
    /// startingTitle will take in a startTitle string then set it into 
    /// the seqmanager variable and display it out to the user
    /// </summary>
    public void startingTitle(string startTitle)
    {
        userName = startTitle;
        mainMenuLabel.text = startTitle;
    }

    /// <summary>
    /// setDescription this will set the description text in the menu window
    /// </summary>
    public void SetDescription()
    {
        MainDescription.text = " ";
    }


    /// <summary>
    /// Sets the children items in the object to inactive when clicking the back button
    /// </summary>
    public void BackButtonSetChildrenInactive()
    {
        trainingButtons.SetChildrenActive(false);
    }

    /// <summary>
    /// setMainMenuLabel is used to set the main menu label when ever tracing back or main menu is hit
    /// </summary>
    public void ResetMainMenuLabel()
    {
        if (userName != null)
        {
            if (mainMenuLabel != null)
            {
                mainMenuLabel.text = userName + " " + loginSpeech;
            }
        }
        else
        {
            mainMenuLabel.text = "Select a training to get started";
        }
    }

    /// <summary>
    /// This method will set the main menu label
    /// </summary>
    /// <param name="sceneName"></param>
    public void SetMainMenuLabel(string sceneName)
    {
        currentSequenceItem = sceneName;
        mainMenuLabel.text = sceneName;
    }

    /// <summary>
    /// ResetMenuSystem this method will set the popUpMenu active or inactive
    /// </summary>

    public void ResetMenuSystem()
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
        }
    }

    /// <summary>
    /// ToggleProximity change the feedback depending on if it is following or not
    /// </summary>
    public void ToggleProximity()
    {
        proximityToggle = !proximityToggle;
        if (audioFeedBack != null)
        {
            if (proximityToggle)
            {
                audioFeedBack.CustomSpeak("Proximity On");
            }
            else
            {
                audioFeedBack.CustomSpeak("Proximity Off");
            }
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
                audioFeedBack.MenuToggleOnSpeak();
            else
                audioFeedBack.MenuToggleOffSpeak();
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
    /// ToggleOpenMenu will onpen the menu and close the minimize menu
    /// </summary>
    public void ToggleOpenMenu()
    {
        mainMenu.SetActive(true);
        minimizeMenu.SetActive(false);
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
    /// EnableMultiScene will allow the user to run multiple scenes at once
    /// </summary>
    public void EnableBacking()
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
                Debug.LogError("Help Menu is out of scope!");
            }
        }
    }
}
