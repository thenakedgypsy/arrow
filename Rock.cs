using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Rock
{
    public Vector2 Location;
    public Vector2 Direction;
    public string Size;
    public Texture2D Texture;
    public float Rotation;
    public Vector2 Origin;


    public Rock(Vector2 dir, string size, Texture2D texture)
    {
        Spawn();
        this.Direction = dir;
        this.Size = size;
        this.Texture = texture;
        Random random = new Random();
        int degrees = random.Next(0,361);
        Rotation = degrees * (float)Math.PI / 180.0f; //convert to radians
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
                this.Location = new Vector2(random.Next(1920),0);    
                break;
            case 2: //right
                this.Location = new Vector2(1920,random.Next(1080));
                break;
            case 3: //bottom
                this.Location = new Vector2(random.Next(1920),1080);
                break;
            case 4: //left
                this.Location = new Vector2(0,random.Next(1080));
                break;
            default:
                this.Location = new Vector2(0,0);
                break;
        }
    }

}