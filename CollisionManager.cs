using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class CollisionManager
{
    private Vector2 Obj1TL;
    private Vector2 Obj1BR;
    private Vector2 Obj2TL;
    private Vector2 Obj2BR;

    public CollisionManager()
    {

    }

    public bool CollideCheck(Texture2D o1Texture, Vector2 o1Position, Texture2D o2Texture, Vector2 o2Position)
    {
        Obj1TL = new Vector2(o1Position.X - (o1Texture.Width / 2), o1Position.Y - (o1Texture.Height / 2));
        Obj1BR = new Vector2(o1Position.X + (o1Texture.Width / 2), o1Position.Y - (o1Texture.Height / 2));

        Obj2TL = new Vector2(o2Position.X - (o2Texture.Width / 2), o2Position.Y - (o2Texture.Height / 2));
        Obj2BR = new Vector2(o2Position.X + (o2Texture.Width / 2), o2Position.Y + (o2Texture.Height / 2));

        if (Obj1BR.X < Obj2TL.X || Obj2BR.X < Obj1TL.X)
        {
            return false; //is to the left / right
        }
        if (Obj1BR.Y < Obj2TL.Y || Obj2BR.Y < Obj1TL.Y)
        {
            return false;  // is above / below
        }
        return true;    
    }





}