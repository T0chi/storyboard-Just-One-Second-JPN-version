using System;
using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;

public class ParticleManager
{
    private StoryboardObjectGenerator generator;
    public ParticleManager(StoryboardObjectGenerator generator)
    {
        this.generator = generator;
    }
    public void GenerateFairy(double startTime, Vector2 position, int durationMin = 5000, int durationMax = 10000)
    {
        for(int i = 0; i < 20; i++)
        {
            double angle = generator.Random(0, Math.PI*2);
            var radius = generator.Random(10, 50);

            var endPosition = new Vector2(
                (float)(position.X + Math.Cos(angle) * radius),
                (float)(position.Y + Math.Sin(angle) * radius)
            );

            var particleDuration = generator.Random(durationMin, durationMax);
            var sprite = generator.GetLayer("PARTICLES").CreateSprite("sb/d.png");
            sprite.Fade(startTime, startTime + particleDuration, 1, 0);
            sprite.Scale(startTime, startTime + particleDuration, radius*0.001, 0);
            sprite.Move(OsbEasing.OutExpo, startTime, startTime + particleDuration, position, endPosition);
            sprite.Additive(startTime, startTime + particleDuration);
        }
    }
    public void GenerateFog(int startTime, int endTime, int posY, int stroke, int quantity, Color4 color, int minSpeed, int maxSpeed, string layer = "PARTICLES")
    {
        for(int i = 0; i < quantity; i++)
        {
            int firstTimeDuration = generator.Random(1000, 30000);
            int posX = generator.Random(-107, 830);
            int endX = 800;
            int distance = posX - -150;
            int elementStartTime = startTime;
            double fade = generator.Random(0.1, 0.5);

            // for(int p = 0; p < 2; p++)
            // {
            //     var particle = generator.GetLayer(layer).CreateSprite("sb/p.png");
            //     particle.MoveX(startTime, startTime + firstTimeDuration, posX, endX);
            //     particle.StartLoopGroup(startTime + firstTimeDuration, 3);
            //     particle.MoveX(0, 0 + generator.Random(15000, 50000), -107, endX);
            //     particle.EndGroup();
            //     particle.MoveY(startTime, generator.Random(posY - stroke, posY + stroke));     
            //     particle.Fade(startTime, startTime + 1000, 0, 1);
            //     particle.Fade(endTime, endTime + 1000, 1, 0);
            //     particle.Scale(startTime, generator.Random(0.008, 0.015));
            //     particle.Color(startTime, color);
            //     particle.Additive(startTime, endTime);
            // }

            var sprite = generator.GetLayer(layer).CreateSprite($"sb/clouds/s{generator.Random(0, 9)}.png");
            sprite.MoveX(startTime, startTime + firstTimeDuration, posX, endX);
            sprite.Fade(startTime, startTime + 1000, 0, fade);
            sprite.Fade(endTime, endTime + 1000, fade, 0);
            sprite.Color(startTime, color);
            sprite.Scale(startTime, generator.Random(0.4, 1));
                
            elementStartTime += firstTimeDuration;
            while(elementStartTime < endTime)
            {          
                int newDuration = generator.Random(15000, 50000);
                sprite.MoveX(elementStartTime, elementStartTime + newDuration, -150, endX);       
                sprite.MoveY(elementStartTime, generator.Random(posY - stroke, posY + stroke));
                elementStartTime += newDuration;
            }
        }
    }
    public void GenerateParticlesMoveUp(int startTime, int endTime)
    {
        for(int i = 0; i < 300; i++)
        {  
            var duration = generator.Random(5000, 20000);
            int posX = generator.Random(-107, 747);
            var sprite = generator.GetLayer("PARTICLES").CreateSprite("sb/s.png");
            sprite.Scale(startTime, generator.Random(0.01, 0.05));
            sprite.MoveX(startTime, generator.Random(-107, 747));
            sprite.StartLoopGroup(startTime + generator.Random(0, 2000), (endTime-startTime)/duration);
            sprite.MoveX(OsbEasing.InOutSine, 0, duration/2, posX - 10, posX + 10);
            sprite.MoveX(OsbEasing.InOutSine, duration/2, duration, posX + 10, posX - 10);
            sprite.MoveY(OsbEasing.OutSine, 0, duration, 480, generator.Random(0, 300));
            sprite.Fade(0, duration, 0.2, 0);
            sprite.EndGroup();                
        }
    }
    public void GenerateCircleParticles(int startMove, int startTime, int endTime, int endMove)
    {
        for(int i = 0; i < 200; i++)
        {
            var angle = generator.Random(0, Math.PI*2);
            var radius = generator.Random(200, 600);

            var startPosition = new Vector2(
                (float)(320 + Math.Cos(angle) * 500),
                (float)(240 + Math.Sin(angle) * 500)
            );

            var endPosition = new Vector2(
                (float)(320 + Math.Cos(angle) * radius),
                (float)(240 + Math.Sin(angle) * radius)
            );

            var sprite = generator.GetLayer("PARTICLES").CreateSprite("sb/d.png", OsbOrigin.Centre, startPosition);
            sprite.Move(OsbEasing.OutBack, startMove, startTime, startPosition, endPosition);
            sprite.Move(OsbEasing.InBack, endTime, endMove, endPosition, startPosition);
            sprite.Fade(startMove, startTime, 0, 1);
            sprite.Fade(endTime, endMove, 1, 0);
            sprite.Scale(startMove, radius*0.00005);
        }
    }
    public void GenerateDirectionalCross(int startTime, int endTime, int speed, int spawnDelay)
    {
        Vector2 basePosition = new Vector2(320, 240);
        for(int i = 0; i < 4; i++)
        {
            double angle = (Math.PI/2) * i;
            for(int sTime = startTime; sTime < endTime; sTime += spawnDelay)
            {
                var endPosition = new Vector2(
                    (float)(320 + Math.Cos(angle) * 450),
                    (float)(240 + Math.Sin(angle) * 450)
                );

                var sprite = generator.GetLayer("PARTICLES").CreateSprite("sb/p.png", OsbOrigin.Centre);
                sprite.Move(OsbEasing.OutSine, sTime, sTime + speed, basePosition, endPosition);
                sprite.Fade(sTime + speed/6, sTime + speed/2, 0, 1);
                sprite.ScaleVec(sTime, sTime + speed, 10, 1, 10, 0);
                sprite.Rotate(OsbEasing.InSine, sTime, sTime + speed, angle, angle - 1.5);

                angle += Math.PI/60;
            }
        }
    }
    public void GenerateLinesPlane(int startTime, Vector2 position, bool direction)
    {
        var line = generator.GetLayer("PARTICLES").CreateSprite("sb/pl.png", OsbOrigin.CentreRight);
        line.Fade(startTime, startTime + 2000, 1, 0);
        line.ScaleVec(OsbEasing.OutExpo, startTime, startTime + 2000, 3, 0.2, 0, 0);
        line.MoveY(startTime, position.Y);
        line.MoveX(OsbEasing.OutExpo, startTime, startTime + 500, direction == true ? -107 : 745, direction == true ? 1000 : -300);
        
        if(!direction)
            line.Rotate(startTime, Math.PI);

        line.Additive(startTime, startTime + 2000);

        var hl = generator.GetLayer("PARTICLES").CreateSprite("sb/hl.png", OsbOrigin.Centre, position);
        hl.Fade(startTime, startTime + 1000, 1, 0);
        hl.Scale(OsbEasing.OutExpo, startTime, startTime + 1000, 0.2, 0.25);
        hl.Additive(startTime, startTime + 1000);

        var circle = generator.GetLayer("PARTICLES").CreateSprite("sb/c2.png", OsbOrigin.Centre, position);
        circle.Fade(startTime, startTime + 1000, 1, 0);
        circle.Scale(OsbEasing.OutExpo, startTime, startTime + 1000, 0.3, 0.35);
        circle.Additive(startTime, startTime + 1000);

        GenerateFairy(startTime, position, 1000, 3000);
    }
    public void GenerateRain(int startTime, int endTime, int intensity)
    {
        var duration = endTime - startTime;
        for(int i = 0; i < intensity * 7.5; i++)
        {
            int particleSpeed = generator.Random(70000, 100000)/(intensity*10);
            int posX = generator.Random(-107, 747);
            int endX = generator.Random(posX - 20, posX + 20);

            double angle = Math.Atan2(680 - 0, endX - posX);

            var sprite = generator.GetLayer("PARTICLES").CreateSprite("sb/pl.png", OsbOrigin.Centre, new Vector2(generator.Random(-107, 747), 0));
            sprite.StartLoopGroup(startTime + (i * 100), duration/particleSpeed);
            sprite.MoveY(0, particleSpeed, -100, 580);
            sprite.MoveX(0, particleSpeed, posX, endX);
            sprite.Rotate(0, particleSpeed, Math.PI/2, angle);
            sprite.EndGroup();
            sprite.Fade(startTime, generator.Random(0.1, 0.4));
            sprite.Scale(startTime, (1f/particleSpeed)*20 );

            var splash = generator.GetLayer("PARTICLES").CreateSprite("sb/d.png", OsbOrigin.Centre, new Vector2(posX, 480));
            splash.StartLoopGroup(startTime + (i * 100) + particleSpeed, duration/particleSpeed);
            splash.MoveY(OsbEasing.OutExpo, 0, 300, 480, generator.Random(400, 450));
            splash.Fade(OsbEasing.OutExpo, 0, particleSpeed, 1, 0);
            splash.Scale(OsbEasing.OutExpo, 0, particleSpeed, 0.05, 0);
            splash.EndGroup();
        }
    }
    public void GenerateMovingLights(int startTime, int endTime)
    {
        for(int i = startTime; i < endTime; i += generator.Random(20, 300))
        {
            int duration = generator.Random(5000, 8000);
            double angle = generator.Random(0, Math.PI*2);
            float radius = generator.Random(20, 500);
            var startPos = new Vector2(generator.Random(-107, 747), generator.Random(0, 480));
            var endPos = new Vector2(
                (float)(startPos.X + Math.Cos(angle) * radius),
                (float)(startPos.Y + Math.Sin(angle) * radius)
            );
            var sprite = generator.GetLayer("Particles").CreateSprite("sb/hl.png", OsbOrigin.Centre, startPos);
                
            sprite.Move(OsbEasing.InOutSine, i, i + duration, startPos, endPos);
            sprite.Scale(i, generator.Random(0.5, 1));
            sprite.Fade(i, i + 1000, 0, 0.01);
            sprite.Fade(i + duration - 1000, i + duration, 0.01, 0);
            sprite.Additive(i, i + duration);   
        }
    }
    public void GenerateCircularParticles(int startTime, int endTime)
    {
        for(int i = 0; i < 600; i++)
        {
            int radius = generator.Random(200, 450);
            double angle = generator.Random(0, Math.PI*2);
            double scale = generator.Random(0.02, 0.04);
            int flashTime = generator.Random(1000, 5000);
            int duration = endTime - startTime;

            Vector2 position = new Vector2(
                (float)(320 + Math.Cos(angle) * radius),
                (float)(240 + Math.Sin(angle) * radius)
            );

            double nPosX = (position.X - 320) * 2;
            double nPosY = (position.Y - 240) * 2;

            var sprite = generator.GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre);
            sprite.Fade(startTime, startTime + generator.Random(1000, 3000), 0, 1);

            sprite.StartLoopGroup(startTime, duration/flashTime);
            sprite.Scale(0, flashTime, scale, 0);
            sprite.EndGroup();

            for(int time = startTime; time < endTime; time += 1000)
            {
                angle+=0.03;

                Vector2 nPosition = new Vector2(
                    (float)(320 + Math.Cos(angle) * radius),
                    (float)(240 + Math.Sin(angle) * radius)
                );

                sprite.Move(time, time + 1000, position, nPosition);

                position = nPosition;
            }
        }      
    }
    public void GenerateCircularMovingParticles(int startTime, int endTime)
    {
        for(int i = 0; i < 400; i++)
        {
            int radius = generator.Random(280, 450);
            double angle = generator.Random(0, Math.PI*2);
            double scale = generator.Random(0.02, 0.04);
            int flashTime = generator.Random(1000, 5000);
            int duration = endTime - startTime;

            Vector2 position = new Vector2(
                (float)(320 + Math.Cos(angle) * radius),
                (float)(240 + Math.Sin(angle) * radius)
            );

            double nPosX = (position.X - 320) * 2;
            double nPosY = (position.Y - 240) * 2;

            var sprite = generator.GetLayer("").CreateSprite("sb/d.png", OsbOrigin.Centre);
            sprite.Fade(startTime, startTime + generator.Random(1000, 3000), 0, 1);

            sprite.StartLoopGroup(startTime, duration/flashTime);
            sprite.Scale(0, flashTime, scale, 0);
            sprite.EndGroup();

            for(int time = startTime; time < endTime; time += 1000)
            {
                angle+=0.03;

                Vector2 nPosition = new Vector2(
                    (float)(320 + Math.Cos(angle) * radius),
                    (float)(240 + Math.Sin(angle) * radius)
                );

                sprite.Move(time, time + 1000, position, nPosition);

                position = nPosition;
            }
        }
    }
}