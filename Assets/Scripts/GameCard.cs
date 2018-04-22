using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameCard : MonoBehaviour 
{

    private Card Card = new Card_SpellFireball();
    public bool FaceUp
    {
        get
        {
            if (Card.Position == CardPosition.Deck) return false;
            if (Card.Position == CardPosition.Discard) return false;
            return true;
        }
    }
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public Collider2D CardCollider;
    public CircleCollider2D SpellHitbox;
    public SpriteRenderer ManaText;
    public TextMesh NameText;
    public TextMesh TypeText;
    public TextMesh EffectText;
    public SpriteRenderer BackgroundImage;
    public SpriteRenderer CardImage;

    public void OnHitEnemy(Enemy enemy)
    {
        Card.OnHitEnemy(enemy);
    }

    public Transform Parent
    {
        get
        {
            switch (Card.Position)
            {
                case CardPosition.Deck:
                    return GameUI.Instance.Deck;
                case CardPosition.Hand:
                    return GameUI.Instance.Hand;
                case CardPosition.Discard:
                    return GameUI.Instance.Discard;
                case CardPosition.Field:
                case CardPosition.Travelling:
                    return GameUI.Instance.Field;
                default:
                    return GameUI.Instance.CardDump;

            }
        }
    }

    public bool IsInField
    {
        get
        {
            return Card.Position == CardPosition.Travelling || Card.Position == CardPosition.Field;
        }
    }
    private float castDist = 0f;
    private Vector2 castDir;
    public TextMesh RuneTimerText;
    private void Update()
    {
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }
        bool shouldhavebg = !Card.Casting && Card.Position != CardPosition.Field && Card.Position != CardPosition.Travelling;
        Color BGcolor = shouldhavebg ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0);
        BackgroundImage.color = Color.Lerp(BackgroundImage.color, BGcolor, 10f * Time.deltaTime);
        CardCollider.enabled = shouldhavebg;
        ManaText.enabled = shouldhavebg && Card.Position == CardPosition.Hand;

        if (shouldhavebg && !Card.CanCast() && Card.Position == CardPosition.Hand)
        {
            Color DisabledColor = new Color(0.25f, 0.25f, 0.25f, 1f);
            BackgroundImage.color = Color.Lerp(BackgroundImage.color, DisabledColor, 10f * Time.deltaTime);
            ManaText.color = Color.Lerp(ManaText.color, DisabledColor, 10f * Time.deltaTime);
            CardImage.color = Color.Lerp(CardImage.color, DisabledColor, 10f * Time.deltaTime);
        }
        else if(shouldhavebg)
        {
            Color EnabledColor = new Color(1, 1, 1, 1f);
            BackgroundImage.color = Color.Lerp(BackgroundImage.color, EnabledColor, 10f * Time.deltaTime);
            ManaText.color = Color.Lerp(ManaText.color, EnabledColor, 10f * Time.deltaTime);
            CardImage.color = Color.Lerp(CardImage.color, EnabledColor, 10f * Time.deltaTime);
        }

        SpellHitbox.radius = Card.HitboxSize;

        CardImage.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        RuneTimerText.text = "";
        if (Card.Type == CardType.Rune && Card.Position == CardPosition.Field)
        {
            Card.RuneTimer -= Time.deltaTime;
            RuneTimerText.text = Card.RuneTimer < 3 ? Mathf.CeilToInt(Card.RuneTimer).ToString() : "";
        }

        if (Card.Casting)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            castDist = Vector2.Distance(transform.position, target);
            castDir = (target - transform.position).normalized;
            transform.position = Vector2.Lerp(transform.position, target, 20f * Time.deltaTime);

            if (Input.GetMouseButtonUp(0))
            {
                StopCasting();
            }
            if (Input.GetMouseButtonUp(1))
            {
                CancelCasting();
            }
        }
        else
        {
            if (transform.parent != Parent)
            {
                transform.parent = Parent;
            }
            switch (Card.Position)
            {
                case CardPosition.Discard:
                case CardPosition.Deck:
                    transform.localPosition = Vector3.zero;
                    break;
                case CardPosition.Field:
                    if (Card.Type == CardType.Spell)
                    {
                        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 20f * Time.deltaTime);
                    }
                    else
                    {
                        transform.position = Card.RunePosition;

                        if(Card.RuneTimer <= 0)
                        {
                            ClearEffects();
                            Card.SendTo(CardPosition.Discard);
                        }
                    }
                    break;

                case CardPosition.Travelling:
                    castDir.x = Mathf.Clamp(castDir.x, -0.075f, 0.075f);
                    castDir.y = Mathf.Clamp(castDir.y, -0.075f, 0.075f);
                    transform.Translate(castDir * (Mathf.Clamp(castDist, 0.75f, 5f)) * Time.deltaTime * 100f * Card.SpeedMultiplier);

                    if(transform.position.y > 21 || transform.position.y < -3 || transform.position.x > -10 || transform.position.x < -30)
                    {
                        ClearEffects();
                        Card.SendTo(CardPosition.Discard);
                    }

                    float rot_z = Mathf.Atan2(castDir.y, castDir.x) * Mathf.Rad2Deg;
                    CardImage.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                    break;

                case CardPosition.Removed:
                    transform.localPosition = new Vector3(0, -1000, 0);
                    break;
                case CardPosition.Hand:

                    break;
            }
        }
        if (FaceUp)
        {
            ManaText.gameObject.SetActive(true);
            CardImage.gameObject.SetActive(true);

            ManaText.sprite = Utility.Instance.CardCost[Card.ManaCost];
            CardImage.sprite = Card.GetSprite();
            BackgroundImage.sprite = Utility.Instance.CardType[(int)Card.Type];
        }
        else
        {
            CardImage.gameObject.SetActive(false);
            BackgroundImage.sprite = Utility.Instance.CardBack;
        }
    }

    public Card GetCard()
    {
        return Card;
    }

    public void SetCard(Card newCard)
    {
        if (Card != null)
        {
            Card.GC = null;
        }
        Card = newCard;
        Card.GC = this;
    }

    public void OnMouseDown()
    {
        switch (Card.Position)
        {
            case CardPosition.Hand:
                if (Card.CanCast())
                {
                    Card.Casting = true;
                    StartEffect(Card.GetCastEffect(), EffectType.Cast);
                }
                break;
        }
    }

    public void OnMouseOver()
    {
        if (Card.Position != CardPosition.Hand) return;
        if (GameManager.Instance.Player.Hand.Any(m => m.Casting)) return;
        GameUI.Instance.Hover(Card.Name + " (" + Card.Type.ToString() + ")", Card.EffectText);
    }

    public void StopCasting()
    {
        ClearEffects();

        if (Card.Type == CardType.Spell)
        {
            if (transform.position.y - GameManager.Instance.PlayerTransform.position.y > GameManager.CastRange || castDir.magnitude < 0.04f)
            {
                // Cancel Cast
                CancelCasting();
            }
            else
            {
                Card.Cast();
                StartEffect(Card.GetTravelEffect(), EffectType.Travel);
            }
        }
        else
        {
            if (transform.position.y - GameManager.Instance.PlayerTransform.position.y < GameManager.CastRange)
            {
                // Cancel Cast
                CancelCasting();
            }
            else
            {
                Card.Cast();
                StartEffect(Card.GetAfterEffect(), EffectType.After);
            }
        }
    }

    public void CancelCasting()
    {
        Card.Casting = false;
        Card.SendTo(CardPosition.Hand);
        ClearEffects();
    }

    public void StartEffect(GameObject ef, EffectType et)
    {
        if (et == EffectType.Cast || et == EffectType.Travel)
        {
            ClearEffects();
        }
        GameObject go = GameObject.Instantiate(ef, transform);
        go.transform.localPosition = Vector3.zero;
        Effects.Add(go);
    }

    private List<GameObject> Effects = new List<GameObject>();

    public void ClearEffects()
    {
        foreach(GameObject go in Effects)
        {
            Destroy(go);
        }

        Effects.Clear();
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(Card.Type == CardType.Spell && Card.Position == CardPosition.Travelling)
        {
            if(c.tag == "Spell")
            {
                if(c.GetComponent<GameCard>().Card.Type == CardType.Rune && c.GetComponent<GameCard>().Card.Position == CardPosition.Field)
                {
                    c.GetComponent<GameCard>().Card.RuneEffect(Card);

                    if(Card.Name == "Shadowsurge")
                    {
                        c.GetComponent<GameCard>().Card.RuneEffect(Card);
                    }
                }
            }
        }
    }
}

public enum EffectType
{
    Cast,
    Travel,
    After
}