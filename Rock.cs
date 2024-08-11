using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Rock
{
    public Vector2 Position;
    public Vector2 Direction;
    public string Size;
    public Texture2D Texture;
    public float Rotation;
    public Vector2 Origin;
    public float Speed;
    public int Score;


    public Rock(Vector2 playerPos, string size)
    {
        if(size == "large")
        {
            Score = 1;
            Spawn();
        }
        this.Direction = this.Position - playerPos;
        this.Size = size;
        Random random = new Random();
        int degrees = random.Next(0,361);
        Rotation = degrees * (float)Math.PI / 180.0f; //convert to radians
        Speed = (float)random.Next(1,4);
        
        
    }

    public Rock(string size)
    {
        this.Size = size;
        Random random = new Random();
        int degrees = random.Next(0,361);
        Rotation = degrees * (float)Math.PI / 180.0f; //convert to radians
        Speed = (float)random.Next(1,4);
    }

    public void Rotate(float increase)
    {
        float degrees = Rotation * 180.0f / (float)Math.PI; //convert to degrees
        degrees += increase;
        Rotation = degrees * (float)Math.PI / 180.0f; //convert back and adjust
    }

    public void Spawn()
    {
        Random random = new Random();
        int side = random.Next(1,5);
        switch(side)
        {
            case 1: //top
                this.Position = new Vector2(random.Next(2000),-50);    
                break;
            case 2: //right
                this.Position = new Vector2(2020,random.Next(1200));
                break;
            case 3: //bottom
                this.Position = new Vector2(random.Next(2000),2000);
                break;
            case 4: //left
                this.Position = new Vector2(-50,random.Next(1200));
                break;
            default:
                this.Position = new Vector2(0,0);
                break;
        }
    }

    public float GetSize()
    {
        if(this.Size == "large")
        {
            return 100f;
        }
        else if(this.Size == "mid")
        {
            return 60f;
        }
        else
        {
            return 25f;
        }
    }

    public List<Rock> Fragment()
    {
        List<Rock> rocks = new List<Rock>();
        float degrees = 30f;
        float radians = degrees * (MathF.PI / 180f);
        Vector2 nDirection = this.Direction;
        nDirection.Normalize();
     //new direction + degrees
        Vector2 dir1 = new Vector2(nDirection.X * MathF.Cos(radians) 
                                    - nDirection.Y * MathF.Sin(radians), 
                                   nDirection.X * MathF.Sin(radians) 
                                   + nDirection.Y * MathF.Cos(radians));
        //new directio - degrees
        Vector2 dir2 = new Vector2(nDirection.X * MathF.Cos(-radians) 
                                    - nDirection.Y * MathF.Sin(-radians),
                                    nDirection.X * MathF.Sin(-radians)
                                    + nDirection.Y * MathF.Cos(-radians));
        switch(this.Size)
        {
            case "large":
                Rock rock1 = new Rock(this.Position,"mid");
                rock1.Position = new Vector2 (this.Position.X + 25, this.Position.Y);
                rock1.Direction = dir1;
                Rock rock2 = new Rock(this.Position,"mid");
                rock2.Position = new Vector2 (this.Position.X - 25, this.Position.Y);
                rock2.Direction = dir2;
                rocks.Add(rock1);
                rocks.Add(rock2);
                return rocks;
         case "mid":
                Rock mRock1 = new Rock(this.Position,"small");
                mRock1.Position = new Vector2 (this.Position.X + 15, this.Position.Y);
                mRock1.Direction = dir1;
                Rock mRock2 = new Rock(this.Position,"small");
                mRock2.Position = new Vector2 (this.Position.X - 15, this.Position.Y);
                mRock2.Direction = dir2;
                rocks.Add(mRock1);
                rocks.Add(mRock2);
                return rocks;               
        }
        return rocks;
        
    }

    public List<Debris> Explode(Texture2D texture)
    {
        Random random = new Random();
        int deg = random.Next(0, 361);
        float degrees = deg;
        Vector2 nDir = new Vector2(0,-1);
        List<Debris> debris = new List<Debris>();
        
        if(this.Size == "large")
        {
            for(int i = 0; i < 51; i++)
            {   
                
                float radians = degrees * (MathF.PI / 180f);
                Vector2 dir = new Vector2(nDir.X * MathF.Cos(radians) 
                                        - nDir.Y * MathF.Sin(radians), 
                                       nDir.X * MathF.Sin(radians) 
                                       + nDir.Y * MathF.Cos(radians));
                dir = Vector2.Normalize(dir);
                Vector2 iPosition = this.Position + dir * 10f;
                Debris deb = new Debris(iPosition, dir, texture);
                degrees += 7.2f;
                debris.Add(deb);
            }
        }
        else if(this.Size == "mid")
        {
            for(int i = 0; i < 26; i++)
            {   
                
                float radians = degrees * (MathF.PI / 180f);
                Vector2 dir = new Vector2(nDir.X * MathF.Cos(radians) 
                                        - nDir.Y * MathF.Sin(radians), 
                                       nDir.X * MathF.Sin(radians) 
                                       + nDir.Y * MathF.Cos(radians));
                dir = Vector2.Normalize(dir);
                Vector2 iPosition = this.Position + dir * 10f;
                Debris deb = new Debris(iPosition, dir, texture);
                degrees += 14.4f;
                debris.Add(deb);
            }
        }
        else
        {
            for(int i = 0; i < 13; i++)
            {   
                
                float radians = degrees * (MathF.PI / 180f);
                Vector2 dir = new Vector2(nDir.X * MathF.Cos(radians) 
                                        - nDir.Y * MathF.Sin(radians), 
                                       nDir.X * MathF.Sin(radians) 
                                       + nDir.Y * MathF.Cos(radians));
                dir = Vector2.Normalize(dir);
                Vector2 iPosition = this.Position + dir * 10f;
                Debris deb = new Debris(iPosition, dir, texture);
                degrees += 28f;
                debris.Add(deb);
            }
        }
        return debris;
    }
    
}