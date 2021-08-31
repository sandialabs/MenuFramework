using Microsoft.MixedReality.Toolkit.Audio;
using UnityEngine;

public class FeedBackSet : MonoBehaviour
{
    private TextToSpeech textToSpeech;
    public string speakText;

    // Start is called before the first frame update
    void Start()
    {
        textToSpeech = GetComponent<TextToSpeech>();
    }


    public void BeenClicked()
    {
        var msg = string.Format(speakText, textToSpeech.Voice.ToString());
            textToSpeech.StartSpeaking(msg);
 
    }
} 