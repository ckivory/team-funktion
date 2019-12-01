using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public GameObject;
private Renderer quadRenderer;

float scrollSpeed = 0.5f;

void Start()
{
    quadRenderer = quadGameObject.GetComponent<Renderer>();
}

void Update()
{
    Vector2 textureOffset = new Vector2(Time.time * scrollSpeed, 0);
    quadRenderer.material.mainTextureOffset = textureOffset;
}