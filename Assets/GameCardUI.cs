using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Card Card;

    public Image Background;
    public Image Image;
    public Text Name;
    public Image Cost;


    bool hovering = false;
    private void Update()
    {
        Name.text = Card.Name;
        Cost.sprite = Utility.Instance.CardCost[Card.ManaCost];
        Background.sprite = Utility.Instance.CardType[(int)Card.Type];
        Image.sprite = Card.GetSprite();

        if (hovering)
        {
            GameUI.Instance.Hover(Card.Name + " (" + Card.Type.ToString() + ")", Card.EffectText);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}
