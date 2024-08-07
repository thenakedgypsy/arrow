using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


public class Bullet
{
    public Vector2 Direction;
    public Vector2 Location;
    public Bullet(Vector2 dir, Vector2 spawn)
    {
        this.Direction = dir;
        this.Location = spawn;
    }
}