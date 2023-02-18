using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ship{
    public ShipType shipType;
    public float shipDamage;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer fire1;
    public SpriteRenderer fire2;
    public Sprite estado1;
    public Sprite estado2;
    public Sprite estado3;
    public Sprite estado4;
}

public enum ShipType{
    Default,
    Shooter,
    Chaser
}

public enum BulletModel{
    Default,
    Player,
    Enemy
}
