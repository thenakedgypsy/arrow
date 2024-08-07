using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Rock
{
    public Vector2 Location;
    public Vector2 Direction;
    public float Size;
    public Rock(Vector2 loc, Vector2 dir, float size)
    {
        this.Location = loc;
        this.Direction = dir;
        this.Size = size;
    }
}