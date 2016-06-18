using UnityEngine;
using UnityEditor;
using System.Collections;

public class MainCharAnimationController : MonoBehaviour {
    // Use this for initialization

    private const int NUMBEROFSPRITES= 9;
    private Sprite [] spriteArray;
    private SpriteRenderer sr;
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        spriteArray = new Sprite[NUMBEROFSPRITES];
            spriteArray = Resources.LoadAll<Sprite>("SpriteSheet_mainCharacter");
       ChangeToAnimation(1);
	}

	public void ChangeToAnimation(int i_index)
    {
        sr.sprite = spriteArray[i_index];
    }
	// Update is called once per frame
	void Update ()
    {
	
	}
}
