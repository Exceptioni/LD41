using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {

    public static Utility Instance;

    public List<Sprite> CardType = new List<Sprite>();
    public List<Sprite> CardCost = new List<Sprite>();

    public Sprite CardBack;


    void Awake () {
        Instance = this;
	}
}
