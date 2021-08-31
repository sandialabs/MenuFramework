using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScripting : MonoBehaviour
{ 
    // Assuming here this script is attached to the same object as the Animation component
    // Ofcourse you can also keep it public and reference the component yourself
    public Animation newAnimation;

    public List<AnimationClip> clips;

    //private void Awake()
    //{
    //    newAnimation = GetComponent<Animation>();
    //}

    private void Start()
    {
        //if(newAnimation == null) { newAnimation = new List<AnimationClip>(); } 
        StartCoroutine(PlayAll());
    }

    private IEnumerator PlayAll()
    {
        foreach (var clip in clips)
        {
            newAnimation.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }
    }


}
