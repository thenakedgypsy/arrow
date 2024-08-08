//in game.cs:




  

//now some kind of rock fragmentation? in rock: 
//texture argument in constructor needs changing to be a setter! <--- DO THIS
//maybe the playerPos too? maybe a set this in spawn? give it a null value in the sig?
//call spawn in constructor ONLY if size is large
//else we will assign a direction ourselves
//also need to add a clean up rocks that move off screen
/*
public (Rock,Rock) Fragment()
{

    switch(this.Size)
    {
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
                Rock rock1 = new Rock(this.position,"mid");
                rock1.Position = new Vector2 (this.Position.X + 25, this.Position.Y);
                rock1.Direction = dir1;
                Rock rock2 = new Rock(this.position,"mid");
                rock2.Position = new Vector2 (this.Position.X - 25, this.Position.Y);
                rock2.Direction = dir2;
                return (rock1, rock2);

            case "mid":
                Rock rock1 = new Rock(this.position,"small");
                rock1.Position = new Vector2 (this.Position.X + 15, this.Position.Y);
                rock1.Direction = dir1;
                Rock rock2 = new Rock(this.position,"small");
                rock2.Position = new Vector2 (this.Position.X - 15, this.Position.Y);
                rock2.Direction = dir2;
                return (rock1, rock2);               
        }
    }
}
// going to need some timing for spawning waves
// also need to track some kind of score?

private void UpadteWave(GameTime gametime)
{    
    float currentTime = (float)gameTime.TotalGameTime.TotalSeconds; 
    if(currentTime - _lastWave >= _waveCD) //if CD has passed
    {
        _waveNumber++; //need a property in game.cs for this and _waveCD
        while(_rocks.Count < _waveNumber)
            Rock rock = new Rock(player.Position,"large");
            rock.Texture = largeRockTexture;
            _rocks.Add(rock);
    }
}

*/