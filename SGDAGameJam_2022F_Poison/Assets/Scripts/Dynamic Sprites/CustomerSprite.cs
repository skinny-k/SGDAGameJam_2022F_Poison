using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSprite : DynamicSprite
{
    [SerializeField] float _fadeOutSpeed = 2f;
    
    bool _fadeOut = false;
    SpriteRenderer _mySprite;
    
    void Awake()
    {
        _mySprite = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (_fadeOut && _mySprite != null)
        {
            Color colorThisFrame = _mySprite.color;
            colorThisFrame.a = Mathf.Clamp(_mySprite.color.a - (_fadeOutSpeed * Time.deltaTime), 0, 1);
            _mySprite.color = colorThisFrame;

            if (_mySprite.color.a == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Die()
    {
        _mySprite.color = new Color(1, 1, 1, 0.5f);
    }
    
    public void FadeOut()
    {
        _fadeOut = true;
    }
}
