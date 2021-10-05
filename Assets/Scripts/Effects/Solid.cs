using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Shader material;
    public Color _color;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = Shader.Find("GUI/Text Shader");
    }

    void ColorSprite()
    {
        spriteRenderer.material.shader = material;
        spriteRenderer.color = _color;
    }

    // Update is called once per frame
    void Update()
    {
        ColorSprite();   
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (PoolManager.Instance != null)
            PoolManager.Instance.CoolObject(this.gameObject, PoolObjectType.Shadow);
        else
            Destroy(gameObject);        
    }
}
