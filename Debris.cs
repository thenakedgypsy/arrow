using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Debris
{
    public Vector2 Direction;
    public Vector2 Position;
    public Texture2D Texture;

    public Debris( Vector2 pos, Vector2 dir, Texture2D texture)
    {
        this.Direction = dir;
        this.Position = pos;
        this.Texture = texture;
    }
}