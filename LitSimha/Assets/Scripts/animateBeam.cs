using UnityEngine;
using System.Collections;

public class animateBeam : MonoBehaviour {
public float _uvTieX = 1;
	public float _uvTieY = 1;
	public float _fps = 10;
 
	private Vector2 _size;
	private LineRenderer _myRenderer;
	private float _lastIndex = -1;
 
	void Start () 
	{
		_size = new Vector2 ((1.0f / _uvTieX)*0.001f, 1.0f / _uvTieY);
		_myRenderer = GetComponent<LineRenderer>();
		if(_myRenderer == null)
			enabled = false;
	}
	// Update is called once per frame
	void Update()
	{
		// Calculate index
		float index = (Time.timeSinceLevelLoad * _fps) % (_uvTieX * _uvTieY);
    	if(index != _lastIndex)
		{
			// split into horizontal and vertical index
			float uIndex = index % _uvTieX;
			float vIndex = index / _uvTieY;
 
			// build offset
			// v coordinate is the bottom of the image in opengl so we need to invert.
			Vector2 offset = new Vector2 (uIndex * _size.x, 0);
 
			_myRenderer.material.SetTextureOffset ("_MainTex", offset);
			_myRenderer.material.SetTextureScale ("_MainTex", _size);
 
			_lastIndex = index;
		}
	}
}
