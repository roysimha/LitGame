using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MainCharAnimationController : MonoBehaviour {
    // Use this for initialization

    private const int NUMBEROFSPRITES= 9;
    private Sprite [] spriteArray;
    private SpriteRenderer sr;
    private Image ig;
	void Start () {
        try
        {
            sr = GetComponent<SpriteRenderer>();
        }
        catch (System.Exception)
        {
        }

        try
        {
            ig = GetComponent<Image>();
        }
        catch (System.Exception)
        {

        }
        spriteArray = new Sprite[NUMBEROFSPRITES];
            spriteArray = Resources.LoadAll<Sprite>("SpriteSheet_mainCharacter");
       ChangeToAnimation(1);
	}

	public void ChangeToAnimation(int i_index)
    {
        if (sr)
        {
            sr.sprite = spriteArray[i_index];
        }
        if(ig)
        {
            ig.sprite = spriteArray[i_index];
        }
    }
	// Update is called once per frame
	void Update ()
    {
	
	}
}
