using UnityEngine;

public class animateOffset : MonoBehaviour
{
    private SpriteRenderer sr;
    public float offsetRate;
    private float offset = 0;

    // Use this for initialization
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (offset < 0 || offset > sr.bounds.size.x)
        {
            offsetRate *= -1;
        }
        offset += offsetRate;
        sr.material.mainTextureOffset = new Vector2(3, 0);
    }
}