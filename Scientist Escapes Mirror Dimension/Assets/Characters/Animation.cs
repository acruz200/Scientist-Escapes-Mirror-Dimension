using UnityEngine;

public class Animation : MonoBehaviour {
    private Animator anim;

    void Start() {
        // Get an instance of the Animator component attached to the character.
        anim = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Set the trigger value to True for the parameter Dance.
            anim.SetTrigger("Dance");
        }
    }
}