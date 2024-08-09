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
    public bool Alive;

    public Player(Texture2D texture)
    {
        Texture = texture;
        Speed = 2.0f;
        Position = new Vector2(960,540);
        Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
        Alive = true;
    }

    public void UpdateRotation(Vector2 mousePos)
    {
        MoveDirection = mousePos - Position;
        Rotation = (float)Math.Atan2(MoveDirection.Y, MoveDirection.X);  
    }

    public List<Debris> Explode(Texture2D texture)
    {
        Random random = new Random();
        int deg = random.Next(0, 361);
        float degrees = deg;
        Vector2 nDir = new Vector2(0,-1);
        List<Debris> debris = new List<Debris>();
            for(int i = 0; i < 180; i++)
            {   
                
                float radians = degrees * (MathF.PI / 180f);
                Vector2 dir = new Vector2(nDir.X * MathF.Cos(radians) 
                                        - nDir.Y * MathF.Sin(radians), 
                                       nDir.X * MathF.Sin(radians) 
                                       + nDir.Y * MathF.Cos(radians));
                dir = Vector2.Normalize(dir);
                Vector2 iPosition = this.Position + dir * 10f;
                Debris deb = new Debris(iPosition, dir, texture);
                degrees += 2f;
                debris.Add(deb);
            }
        return debris;
    }
}