﻿using UnityEngine;
using System.Collections;

public class CustomCursor : MonoBehaviour 
{
	public Texture2D cursorImage;
	
	private int cursorWidth = 32;
	private int cursorHeight = 32;
	
	void Start()
	{
		Cursor.visible = false;
	}
	
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(Input.mousePosition.x - 2, Screen.height - Input.mousePosition.y - 2, cursorWidth, cursorHeight), cursorImage);
	}
}