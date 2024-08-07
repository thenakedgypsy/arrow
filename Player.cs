using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Player
{
    public float Rotation;
    public Vector2 MoveDirection;
    public float Speed;
    public Vector2 Origin;
    public Vector2 Position;
    public Texture2D Texture;

    public Player(Texture2D texture)
    {
        Texture = texture;
        Speed = 2.0f;
        Position = new Vector2(960,540);
        Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
    }

    public void UpdateRotation(Vector2 mousePos)
    {
        MoveDirection = mousePos - Position;
        Rotation = (float)Math.Atan2(MoveDirection.Y, MoveDirection.X);  
    }


}