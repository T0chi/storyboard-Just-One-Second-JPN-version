using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Drawing;
using System;

namespace StorybrewScripts
{
    public class Storyboard : StoryboardObjectGenerator
    {
        private int time;
        private KeyframedValue<Vector2> point1 = new KeyframedValue<Vector2>(InterpolatingFunctions.Vector2);
        private KeyframedValue<Vector2> point2 = new KeyframedValue<Vector2>(InterpolatingFunctions.Vector2);
        private KeyframedValue<float> point_rotate1 = new KeyframedValue<float>(InterpolatingFunctions.Float);
        private KeyframedValue<float> point_rotate2 = new KeyframedValue<float>(InterpolatingFunctions.Float);

        public override void Generate()
        {
            trademark(0);

            var sound2 = GetLayer("").CreateSample("sb/rain.ogg", 115219, 93);
            var sound3 = GetLayer("").CreateSample("sb/wind.ogg", 208167, 35);

            RotatingBackground();
            Sky();
            Destruction(208167, 252560, 800);
            Vignette();

            Pulse(8398);
            Pulse(30595);
            Pulse(52791);
            Pulse(74988);
            Pulse(97184);
            Pulse(119381);
            Pulse(141577);
            Pulse(163774);
            Pulse(185970);
            PulseReverse(208167, 3000);
            Pulse(208167);

            Rain();
            Rainbow(165161, 199150, 200);
            Clouds();
        }

        public void trademark(int startTime)
        {
            var delay = 0;

            var bitmap = GetMapsetBitmap("sb/p.png");
            var bitmapLogo = GetMapsetBitmap("sb/tochi.png");

            var bg = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(320, 240));
            var logo = GetLayer("").CreateSprite("sb/tochi.png", OsbOrigin.Centre);
            var logo2 = GetLayer("").CreateSprite("sb/tochi2.png", OsbOrigin.Centre);
            var mask = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre);
            var mask2 = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre);

            var sound = GetLayer("").CreateSample("sb/voicetag2.ogg", startTime - 10000, 90);

            bg.ScaleVec(startTime - 10000, 854.0f / bitmap.Width, 480.0f / bitmap.Height);
            bg.Fade(startTime - 10000, startTime - 9000, 0, 1);
            bg.Fade(startTime - 800, startTime, 1, 0);

            var position = new Vector2(320, 240);

            logo2.Move(startTime - delay - 7200 - delay, position);
            logo2.Color(startTime - delay - 7200 - delay, Color4.Black);
            logo2.Scale(OsbEasing.OutSine, startTime - delay - 7200, startTime - delay - 6300, 0.4f, 0.5f);
            logo2.Fade(startTime - delay - 7200, startTime - delay - 6800, 0, 0.4f);
            logo2.Fade(startTime - 6800, startTime - 6400, 0.4f, 0);

            logo.Move(startTime - delay - 7200, position);
            logo.Scale(OsbEasing.OutBack, startTime - delay - 7200, startTime - delay - 6300, 0.6f, 0.5f);
            logo.Fade(startTime - 7200 - delay, startTime - delay - 6300, 0, 1);
            logo.Fade(startTime - 1300, startTime - 800, 1, 0);

            // particles
            var sTime = startTime - 9000;
            var eTime = startTime - 800;
            var stop = startTime - delay - 7200;
            for (int i = 0; i < 100; i++)
            {
                var sprite = GetLayer("particles").CreateSprite("sb/c.png", OsbOrigin.Centre);

                int radiusStart = Random(600, 854);
                int radiusEnd = Random(200, 400);
                double angle = Random(0, Math.PI*2);

                Vector2 startPos = new Vector2(
                    (float)(320 + Math.Cos(angle) * radiusStart),
                    (float)(240 + Math.Sin(angle) * radiusStart));
                Vector2 center = new Vector2(
                    (float)(320 + Math.Cos(angle) * 1),
                    (float)(240 + Math.Sin(angle) * 1));
                Vector2 endPos = new Vector2(
                    (float)(320 + Math.Cos(angle) * radiusEnd),
                    (float)(240 + Math.Sin(angle) * radiusEnd));

                var Fade = Random(0.2, 1);
                var scale = Random(0.1, 0.5);
                var d = Random(0, 2000);
                var d2 = Random(sTime - 1000, stop);

                sprite.Scale(sTime - 1000, scale);
                sprite.Scale(sTime + d, stop, scale, scale * Random(1.5f, 4f));
                sprite.Scale(stop, scale);
                sprite.Color(sTime - 1000, Color4.Black);

                sprite.Move(OsbEasing.OutSine, sTime - 1000, d2, startPos, center);
                sprite.Move(OsbEasing.OutElastic, stop, stop + 2000, center, endPos);

                sprite.Fade(sTime - Random(500, 1000), sTime, 0, Fade);
                sprite.Fade(startTime - 1300, startTime - 800, Fade, 0);
            }

            // shine
            mask.Rotate(stop + 1400, MathHelper.DegreesToRadians(20));
            mask.ScaleVec(stop + 1400, 30, bitmapLogo.Height * 0.5f + 30);
            mask.Move(OsbEasing.InOutExpo, stop + 1400, stop + 2900, 320 + (bitmapLogo.Width / 2.7f), 235,
                                                320 - (bitmapLogo.Width / 2.7f), 235);

            mask2.Rotate(stop + 2500, MathHelper.DegreesToRadians(100));
            mask2.ScaleVec(stop + 2500, 30, bitmapLogo.Width * 0.5f + 30);
            mask2.Move(OsbEasing.InOutExpo, stop + 2500, stop + 4000, 320, 235 - (bitmapLogo.Height * 0.5f),
                                                320, 235 + (bitmapLogo.Height * 0.5f));
        }

        public void Sky()
        {
            var skyBitmap = GetMapsetBitmap("sb/sky.png");
            var skyWhiteBitmap = GetMapsetBitmap("sb/p.png");
            var sky = GetLayer("Sky").CreateSprite("sb/sky.png", OsbOrigin.Centre, new Vector2(320, 240));
            var sky2 = GetLayer("Sky 2").CreateSprite("sb/sky2.png", OsbOrigin.Centre, new Vector2(320, 240));
            var skyWhite = GetLayer("Sky White").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(320, 240));

            var startRotation = MathHelper.DegreesToRadians(0);
            var endRotation = MathHelper.DegreesToRadians(360);

            sky.Scale(0, 820.0f / skyBitmap.Width);
            sky.Fade(8398, 8398 + 1000, 0, 1);
            sky.Fade(321924, 326433, 1, 0);
            sky.Rotate(0, 338918, startRotation, endRotation);

            sky2.Scale(0, 820.0f / skyBitmap.Width);
            sky2.Fade(119381, 119381 + 1000, 0, 1);
            sky2.Fade(163774 - 1000, 163774, 1, 0);
            sky2.Rotate(0, 338918, startRotation, endRotation);

            skyWhite.ScaleVec(0, 854.0f / skyWhiteBitmap.Width, 480.0f / skyWhiteBitmap.Height);
            skyWhite.Fade(0, 8398, 0, 1);
            skyWhite.Fade(321924, 338918, 1, 0);


            // verse 1
            Lyrics(1, "新しい", 30595, new Vector2(-50, 50), new Vector2(0, 0));
            Lyrics(2, "朝が始まる", 35450, new Vector2(-70, 0), new Vector2(0, 0));
            Lyrics(3, "やわらかい", 41693, new Vector2(-50, 50), new Vector2(0, 0));
            Lyrics(4, "光の中で", 46548, new Vector2(-50, 50), new Vector2(0, 0));

            Lyrics(5, "美しい", 52791, new Vector2(50, -50), new Vector2(80, 0));
            Lyrics(6, "君のほほえみ", 57647, new Vector2(0, -100), new Vector2(0, 0));
            Lyrics(7, "夏の日の恋を", 63890, new Vector2(0, -100), new Vector2(0, 0));
            Lyrics(8, "予感させて", 68832, new Vector2(0, -100), new Vector2(0, 0));

            Lyrics(9, "夢にまで見た", 75768, new Vector2(80, -130), new Vector2(50, -50));
            Lyrics(10, "夢の続き", 78543, new Vector2(-20, -80), new Vector2(200, -50));
            Lyrics(11, "さまよい歩いて", 80710, new Vector2(0, -180), new Vector2(80, -50));
            Lyrics(12, "世界が消える", 83051, new Vector2(30, -130), new Vector2(20, -50));
            Lyrics(13, "その前に", 84352, new Vector2(20, -100), new Vector2(-30, 0));
            Lyrics(14, "世界が消える", 91982, new Vector2(130, -130), new Vector2(0, 0));
            Lyrics(15, "その前に", 93369, new Vector2(-20, -100), new Vector2(50, 0));

            Lyrics(16, "フィルムのような", 97965, new Vector2(10, -230), new Vector2(140, 50));
            Lyrics(17, "夢の続き", 100653, new Vector2(30, -100), new Vector2(60, 50));
            Lyrics(18, "何度も繰り返す", 103687, new Vector2(60, -110), new Vector2(40, 0));
            Lyrics(19, "二人だけのストーリー", 106982, new Vector2(118, -210), new Vector2(40, 0));
            Lyrics(20, "二人だけのストーリー", 113918, new Vector2(125, -270), new Vector2(80, -50));

            // chorus
            var color_chorus = Color4.Black;
            Lyrics(21, "新しい", 119294, new Vector2(80, -100), new Vector2(0, 0), color_chorus);
            Lyrics(22, "朝が始まる", 124150, new Vector2(80, -50), new Vector2(0, 0), color_chorus);
            Lyrics(23, "やわらかい", 130479, new Vector2(100, -50), new Vector2(0, 0), color_chorus);
            Lyrics(24, "光の中で", 135335, new Vector2(100, 0), new Vector2(50, 0), color_chorus);

            Lyrics(25, "美しい君の", 141317, new Vector2(100, -80), new Vector2(50, 0), color_chorus);
            Lyrics(26, "君のほほえみ", 146346, new Vector2(160, -20), new Vector2(50, 0), color_chorus);
            Lyrics(27, "夏の日の恋を", 152676, new Vector2(100, -20), new Vector2(100, 0), color_chorus);
            Lyrics(28, "予感させて", 157444, new Vector2(70, -35), new Vector2(100, 0), color_chorus);

            // verse 2
            Lyrics(29, "風に抱かれて二人", 164034, new Vector2(-100, 140), new Vector2(-150, 100));
            Lyrics(30, "あの空を", 169496, new Vector2(-100, 30), new Vector2(-100, 100));
            Lyrics(31, "ひとっ飛び", 171057, new Vector2(-100, 0), new Vector2(-100, 100));
            Lyrics(32, "もう一度", 175566, new Vector2(-100, 60), new Vector2(-100, 60));
            
            Lyrics(33, "世界が消える", 180681, new Vector2(-100, 60), new Vector2(-100, 60));
            Lyrics(34, "その前に", 182155, new Vector2(-100, 40), new Vector2(-100, 40));

            // main chorus
            var color_mainChorus1 = Color4.IndianRed;
            var color_mainChorus2 = Color4.DarkBlue;
            Lyrics(35, "時間を止めて", 208861, new Vector2(-100, -100), new Vector2(-100, -100), color_mainChorus1);
            Lyrics(36, "花のように", 211462, new Vector2(-100, -130), new Vector2(-100, -130), color_mainChorus2);
            Lyrics(37, "時間を止めて", 214410, new Vector2(-70, 20), new Vector2(-70, 20), color_mainChorus1);
            Lyrics(38, "瞬きを忘れて", 217011, new Vector2(-80, 0), new Vector2(-80, 0), color_mainChorus2);
            Lyrics(39, "時間を止めて", 219959, new Vector2(-80, 0), new Vector2(-80, 0), color_mainChorus1);
            Lyrics(40, "見つめていて", 222560, new Vector2(0, -130), new Vector2(-140, 20), color_mainChorus2);

            Lyrics(41, "時間を止めて", 231057, new Vector2(-100, -100), new Vector2(-100, -100), color_mainChorus1);
            Lyrics(42, "このまま", 233832, new Vector2(-100, -130), new Vector2(-100, -130), color_mainChorus2);
            Lyrics(43, "時間を止めて", 236606, new Vector2(-100, -70), new Vector2(-100, -70), color_mainChorus1);
            Lyrics(44, "ささやいている", 239207, new Vector2(-100, -70), new Vector2(-100, -70), color_mainChorus2);
            Lyrics(45, "時間を止めて", 242155, new Vector2(30, -120), new Vector2(30, -120), color_mainChorus1);
            Lyrics(46, "愛してる", 245103, new Vector2(-70, -130), new Vector2(-70, -120), color_mainChorus2);

            // verse 3
            Lyrics(47, "新しい", 252560, new Vector2(0, -100), new Vector2(0, -100));
            Lyrics(48, "朝が始まる", 257416, new Vector2(-30, -100), new Vector2(-30, -100));
            Lyrics(49, "やわらかい", 263658, new Vector2(-30, -100), new Vector2(-30, -100));
            Lyrics(50, "光の中で", 268514, new Vector2(70, -100), new Vector2(70, -100));
            Lyrics(51, "目を覚めて", 274757, new Vector2(70, -100), new Vector2(70, -100));
            Lyrics(52, "君がほほえむ", 279612, new Vector2(50, -100), new Vector2(50, -100));
            Lyrics(53, "美しい", 285855, new Vector2(0, -100), new Vector2(0, -100));
            Lyrics(54, "美しい世界", 289843, new Vector2(0, -110), new Vector2(0, -110));
        }

        public void Rain()
        {
            for (int i = 0; i < 60; i++)
            {
                var speed = Random(500, 2500);
                var fade = Random(0.1f, 0.8f);
                var extraScale = Random(2, 5);
                var scale = new Vector2((float)Random(0.2f, 0.5f), Random(100, 200));
                
                int delay = Random(0, speed);
                int startTime = 119381 + delay;
                int endTime = 163774 + delay;

                float startRadius = Random(0, 150);
                float endRadius = startRadius + Random(640, 1000);
                double angle = Random(0, Math.PI * 2);

                Vector2 startPos = new Vector2(
                    (float)(320 + Math.Cos(angle) * startRadius),
                    (float)(240 + Math.Sin(angle) * startRadius));

                Vector2 endPos_Rain1 = new Vector2(
                    (float)(320 + Math.Cos(angle) * endRadius),
                    (float)(240 + Math.Sin(angle) * endRadius));

                Vector2 endPos_Rain2 = new Vector2(
                    (float)(320 + Math.Cos(angle) * endRadius),
                    (float)(240 + Math.Sin(angle) * endRadius));

                // var startPos = new Vector2(320, 240);
                var endPosRain1 = new Vector2(endPos_Rain1.X, endPos_Rain1.Y);
                var endPosRain2 = new Vector2(endPos_Rain2.X, endPos_Rain2.Y);

                var rotationRain1 = Math.Atan2((endPosRain1.Y - startPos.Y), (endPosRain1.X - startPos.X)) + (Math.PI / 2);
                var rotationRain2 = Math.Atan2((endPosRain2.Y - startPos.Y), (endPosRain2.X - startPos.X)) + (Math.PI / 2);

                var rain1 = GetLayer("Rain").CreateSprite("sb/rain.png", OsbOrigin.TopLeft);
                
                rain1.StartLoopGroup(startTime, (endTime - startTime) / speed);

                rain1.Additive(0, speed);
                rain1.Rotate(0, rotationRain1);
                rain1.Fade(0, speed / 1.5f, fade, fade);
                rain1.MoveX(OsbEasing.In, 0, speed, startPos.X, endPosRain1.X);
                rain1.MoveY(OsbEasing.In, 0, speed, startPos.Y, endPosRain1.Y);
                rain1.ScaleVec(OsbEasing.In, 0, speed, scale.X, 0, scale.X + extraScale, scale.Y);
                rain1.Fade(speed / 1.5f, speed, fade, 0);

                rain1.EndGroup();

                var rain2 = GetLayer("Rain").CreateSprite("sb/rain.png", OsbOrigin.TopLeft);

                rain2.StartLoopGroup(startTime, (endTime - startTime) / speed);

                rain2.Additive(0, speed);
                rain2.Rotate(0, rotationRain2);
                rain2.Fade(0, speed / 1.5f, fade, fade);
                rain2.MoveX(OsbEasing.In, 0, speed, startPos.X, endPosRain2.X);
                rain2.MoveY(OsbEasing.In, 0, speed, startPos.Y, endPosRain2.Y);
                rain2.ScaleVec(OsbEasing.In, 0, speed, scale.X, 0, scale.X + extraScale, scale.Y);
                rain2.Fade(speed / 1.5f, speed, fade, 0);

                rain2.EndGroup();
            }
        }

        public void Rainbow(int startTime, int endTime, int y)
        {
            var rainbow = GetLayer("Rainbow").CreateSprite("sb/r.png", OsbOrigin.TopCentre);

            rainbow.Fade(startTime, startTime + 1000, 0, 0.2f);
            rainbow.Fade(endTime - 1000, endTime, 0.2f, 0);
            rainbow.Scale(startTime, 0.6f);
            rainbow.Move(startTime, 430, y);
            rainbow.Additive(startTime, endTime);
            rainbow.Rotate(startTime, endTime, MathHelper.DegreesToRadians(-10), MathHelper.DegreesToRadians(-30));
        }

        public void Clouds()
        {
            var clouds = new ParticleManager(this);
            clouds.GenerateFog(8398, 321924, 240, 200, 20, Color4.White, 1000, 30000, "Clouds");

            // intense
            var cloudsIntense = new ParticleManager(this);
            cloudsIntense.GenerateFog(119381, 163774, 240, 200, 20, Color4.White, 100, 10000, "Clouds");
            cloudsIntense.GenerateFog(208167, 258109, 240, 200, 20, Color4.White, 100, 10000, "Clouds");
        }

        public void RotatingBackground()
        {
            var bitmap = GetMapsetBitmap("sb/transparent.png");
            var topLeft = GetLayer("Rotating Background").CreateSprite("sb/transparent.png", OsbOrigin.BottomRight, new Vector2(320, 240));
            var topRight = GetLayer("Rotating Background").CreateSprite("sb/transparent.png", OsbOrigin.BottomLeft, new Vector2(320, 240));
            var bottomRight = GetLayer("Rotating Background").CreateSprite("sb/transparent.png", OsbOrigin.TopLeft, new Vector2(320, 240));
            var bottomLeft = GetLayer("Rotating Background").CreateSprite("sb/transparent.png", OsbOrigin.TopRight, new Vector2(320, 240));

            var outerTopLeft = GetLayer("Rotating Background").CreateSprite("bg.jpg", OsbOrigin.TopRight, new Vector2(320, 240));
            var outerTopRight = GetLayer("Rotating Background").CreateSprite("bg.jpg", OsbOrigin.TopLeft, new Vector2(320, 240));
            var outerBottomLeft = GetLayer("Rotating Background").CreateSprite("bg.jpg", OsbOrigin.TopRight, new Vector2(320, 240));
            var outerBottomRight = GetLayer("Rotating Background").CreateSprite("bg.jpg", OsbOrigin.TopLeft, new Vector2(320, 240));

            topLeft.FlipV(0, 338918);
            topRight.FlipV(0, 338918);
            topRight.FlipH(0, 338918);
            bottomRight.FlipH(0, 338918);

            outerTopLeft.FlipV(0, 338918);
            outerTopRight.FlipV(0, 338918);
            outerTopRight.FlipH(0, 338918);

            outerBottomLeft.FlipV(0, 338918);
            outerBottomRight.FlipV(0, 338918);
            outerBottomRight.FlipH(0, 338918);

            var width = 500.0f;
            var scale = width / bitmap.Width;

            topLeft.Scale(0, scale);
            topRight.Scale(0, scale);
            bottomRight.Scale(0, scale);
            bottomLeft.Scale(0, scale);
            
            outerTopLeft.Scale(0, scale);
            outerTopRight.Scale(0, scale);
            outerBottomLeft.Scale(0, scale);
            outerBottomRight.Scale(0, scale);

            topLeft.Fade(0, 8398, 0, 1);
            topLeft.Fade(OsbEasing.Out, 321924, 338918, 1, 0);
            topRight.Fade(0, 8398, 0, 1);
            topRight.Fade(OsbEasing.Out, 321924, 338918, 1, 0);
            bottomRight.Fade(0, 8398, 0, 1);
            bottomRight.Fade(OsbEasing.Out, 321924, 338918, 1, 0);
            bottomLeft.Fade(0, 8398, 0, 1);
            bottomLeft.Fade(OsbEasing.Out, 321924, 338918, 1, 0);

            outerTopLeft.Fade(0, 8398, 0, 1);
            outerTopLeft.Fade(OsbEasing.Out, 321924, 338918, 1, 0);
            outerTopRight.Fade(0, 8398, 0, 1);
            outerTopRight.Fade(OsbEasing.Out, 321924, 338918, 1, 0);
            outerBottomLeft.Fade(0, 8398, 0, 1);
            outerBottomLeft.Fade(OsbEasing.Out, 321924, 338918, 1, 0);
            outerBottomRight.Fade(0, 8398, 0, 1);
            outerBottomRight.Fade(OsbEasing.Out, 321924, 338918, 1, 0);

            var startRotation = MathHelper.DegreesToRadians(0);
            var endRotation = MathHelper.DegreesToRadians(360);

            topLeft.Rotate(0, 338918, startRotation, endRotation);
            topRight.Rotate(0, 338918, startRotation, endRotation);
            bottomRight.Rotate(0, 338918, startRotation, endRotation);
            bottomLeft.Rotate(0, 338918, startRotation, endRotation);



            // edges

            int startTime = -8500;
            int endTime = 338918;
            float timeStep = (float)Beatmap.GetTimingPointAt(8398).BeatDuration / 8;

            float radius = scale * bitmap.Height - 0.05f;
            float radius2 = scale * bitmap.Height * 0.7f;
            double angle = MathHelper.DegreesToRadians(-99.01f);
            double angleDelay = MathHelper.DegreesToRadians(90);
            float rotationShift = MathHelper.DegreesToRadians(-90);

            Vector2 positionTop = new Vector2(
                (float)(320 + Math.Cos(angle) * -radius),
                (float)(240 + Math.Sin(angle) * -radius)
            );
            Vector2 positionBottom = new Vector2(
                (float)(320 + Math.Cos(angle) * radius),
                (float)(240 + Math.Sin(angle) * radius)
            );

            Vector2 positionTop_point = new Vector2(
                (float)(320 + Math.Cos(angle + angleDelay) * -radius2),
                (float)(240 + Math.Sin(angle + angleDelay) * -radius2)
            );
            Vector2 positionBottom_point = new Vector2(
                (float)(320 + Math.Cos(angle + angleDelay) * radius2),
                (float)(240 + Math.Sin(angle + angleDelay) * radius2)
            );

            for (float time = startTime; time < endTime; time += timeStep)
            {
                var speed = (timeStep / (endTime - startTime)) * 6.44f;
                angle += speed;

                Vector2 nPositionTop = new Vector2(
                    (float)(320 + Math.Cos(angle) * -radius),
                    (float)(240 + Math.Sin(angle) * -radius)
                );
                Vector2 nPositionBottom = new Vector2(
                    (float)(320 + Math.Cos(angle) * radius),
                    (float)(240 + Math.Sin(angle) * radius)
                );

                Vector2 nPositionTop_point = new Vector2(
                    (float)(320 + Math.Cos(angle + angleDelay) * -radius2),
                    (float)(240 + Math.Sin(angle + angleDelay) * -radius2)
                );
                Vector2 nPositionBottom_point = new Vector2(
                    (float)(320 + Math.Cos(angle + angleDelay) * radius2),
                    (float)(240 + Math.Sin(angle + angleDelay) * radius2)
                );

                var RotationTop = Math.Atan2((positionTop.Y - nPositionTop.Y), (positionTop.X - nPositionTop.X)) - Math.PI / 2f;
                var RotationBottom = Math.Atan2((positionBottom.Y - nPositionBottom.Y), (positionBottom.X - nPositionBottom.X)) - Math.PI / 2f;

                point_rotate1.Add(time, (float)(Math.Atan2((positionTop.Y - nPositionTop.Y), (positionTop.X - nPositionTop.X)) - Math.PI / 2f) - rotationShift);
                point_rotate2.Add(time, (float)(Math.Atan2((positionBottom.Y - nPositionBottom.Y), (positionBottom.X - nPositionBottom.X)) - Math.PI / 2f) - rotationShift);
                
                point1.Add(time, positionTop_point);
                point2.Add(time, positionBottom_point);
                point1.Add(time + timeStep - 1, nPositionTop_point);
                point2.Add(time + timeStep - 1, nPositionBottom_point);

                if (time >= 0)
                {
                    // top
                    outerTopLeft.Move(time, time + timeStep - 1, positionTop, nPositionTop);
                    outerTopRight.Move(time, time + timeStep - 1, positionTop, nPositionTop);
                    outerTopLeft.Rotate(time, time + timeStep - 1, RotationTop - rotationShift, RotationTop - rotationShift);
                    outerTopRight.Rotate(time, time + timeStep - 1, RotationTop - rotationShift, RotationTop - rotationShift);

                    // bottom
                    outerBottomLeft.Move(time, time + timeStep - 1, positionBottom, nPositionBottom);
                    outerBottomRight.Move(time, time + timeStep - 1, positionBottom, nPositionBottom);
                    outerBottomLeft.Rotate(time, time + timeStep - 1, RotationBottom - rotationShift, RotationBottom - rotationShift);
                    outerBottomRight.Rotate(time, time + timeStep - 1, RotationBottom - rotationShift, RotationBottom - rotationShift);
                }

                positionTop = nPositionTop;
                positionBottom = nPositionBottom;

                positionTop_point = nPositionTop_point;
                positionBottom_point = nPositionBottom_point;
            }

            point_rotate1.Simplify1dKeyframes(0.1, x => x);
            point_rotate2.Simplify1dKeyframes(0.1, x => x);
            point1.Simplify2dKeyframes(0.5, x => x);
            point2.Simplify2dKeyframes(0.5, x => x);
            point1.SimplifyEqualKeyframes();
            point2.SimplifyEqualKeyframes();
        }

        public void Lyrics(int lyricsNumber, string text, int startTime, Vector2 customStartOffset, Vector2 customEndOffset,
            Color4? color = null)
        {
            this.time = startTime - 500;
            startTime = time;

            var font = LoadFont("sb/lyrics/" + lyricsNumber, new FontDescription()
            {
                FontPath = "font/KozMinPro-Light.otf",
                FontSize = 40,
                Color = Color4.White,
                Padding = Vector2.Zero,
                FontStyle = FontStyle.Bold,
                TrimTransparency = true,
                EffectsOnly = false,
                Debug = false,
            },
            new FontGlow()
            {
                Radius = true ? 0 : 0,
                Color = Color4.Cyan,
            },
            new FontOutline()
            {
                Thickness = 0,
                Color = Color4.Black,
            },
            new FontShadow()
            {
                Thickness = color != null ? 0 : 1,
                Color = new Color4(80, 80, 80, 255),
            });

            var Fade = 1;
            float FontScale = 0.5f;
            var LetterSpacing = 25;
            var timePerLetter = 50;
            var letterSpacing = LetterSpacing * FontScale;

            var i = 0;
            var offset = Vector2.Zero;
            var angle = 0f;

            foreach (var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var delay = 100;
                    var fadeTime = 800;
                    var duration = 15000;
                    var start = startTime - delay + timePerLetter * i;
                    var end = startTime + duration;

                    OsbSprite spriteBg = color != null ? GetLayer("Lyrics").CreateSprite("sb/p.png", OsbOrigin.Centre) : null;
                    var sprite = GetLayer("Lyrics").CreateSprite(texture.Path, OsbOrigin.Centre);

                    if (spriteBg != null) {
                        spriteBg.Fade(start, startTime + fadeTime - delay + timePerLetter * i, 0, Fade);
                        spriteBg.Fade((startTime + duration) - fadeTime - delay + timePerLetter * i, end, Fade, 0);
                    }
                    sprite.Fade(start, startTime + fadeTime - delay + timePerLetter * i, 0, Fade);
                    sprite.Fade((startTime + duration) - fadeTime - delay + timePerLetter * i, end, Fade, 0);
                    
                    if (letter == 'ー')
                    {
                        sprite.FlipV(start, end + delay * i);
                        if (spriteBg != null)
                            spriteBg.FlipV(start, end + delay * i);
                    }

                    var rot1 = point_rotate1.ValueAt(start + 30000);
                    var rot2 = point_rotate2.ValueAt(start + 30000);
                    var extraRotation = letter == 'ー' ? MathHelper.DegreesToRadians(-90) : 0;
                    var startRot = 0 + extraRotation;
                    var r = (start > 163080 ? rot1 : rot2) + Math.PI / 2;
                    
                    var d = true;
                    if (start >= 51491 && start < 140121) d = false;

                    var smth = d ? -10000 : 10000;
                    var smth2 = d ? -5000 : 5000;
                    var _point1 = (start > 163080 ? point2 : point1);
                    var _point2 = (start > 163080 ? point1 : point2);
                    var posStart = _point2.ValueAt(start + smth * (lyricsNumber % 5));
                    var posEnd = _point1.ValueAt(start + smth * (lyricsNumber % 5));

                    posStart += offset + customStartOffset;
                    posEnd += offset + customEndOffset;

                    var randomMove1 = Random(-20, 20);
                    var randomMove2 = Random(-25, 25);
                    var randomScale1 = Random(0, FontScale * 0.3f);
                    var randomScale2 = Random(0.5f, 0.1f);
                    sprite.Move(start, end, posStart, posEnd + new Vector2(randomMove1, randomMove2));
                    sprite.ScaleVec(OsbEasing.InOutSine, start + timePerLetter * i, end + timePerLetter * i, new Vector2(FontScale), new Vector2(FontScale, randomScale1) * randomScale2);

                    var endRot = Math.Atan2((posEnd.Y - posStart.Y), (posEnd.X - posStart.X)) + (Math.PI / 2) + extraRotation;
                    
                    if (start > 163080 && start < 199150)
                        endRot = endRot - (Math.PI * 2);
                    else if (start > 199150) endRot = endRot + (Math.PI * 0);
                    sprite.Rotate(OsbEasing.InOutSine, start + delay * i, end + delay * i, startRot, endRot);
                    
                    if (spriteBg != null)
                    {
                        var bgScale = 30;
                        spriteBg.Move(start, end, posStart, posEnd + new Vector2(randomMove1, randomMove2));
                        spriteBg.ScaleVec(OsbEasing.InOutSine, start + timePerLetter * i, end + timePerLetter * i, new Vector2(bgScale), new Vector2(bgScale, randomScale1) * randomScale2);
                        spriteBg.Rotate(OsbEasing.InOutSine, start + delay * i, end + delay * i, startRot, endRot);
                        if (color != null) spriteBg.Color(start, (Color4)color);
                    }
                    
                    angle = (float)(r + MathHelper.DegreesToRadians(60)); 
                    angle += d ? MathHelper.DegreesToRadians(10) : -MathHelper.DegreesToRadians(10);

                    i++;
                }
                offset += angleDistance(angle, texture.BaseWidth * FontScale + letterSpacing);
            }
        }
        
        private Vector2 angleDistance(float angle, float distance)
        {
            // Calculate the offset components using trigonometry
            float xOffset = distance * (float)Math.Cos(angle);
            float yOffset = distance * (float)Math.Sin(angle);

            // Return the offset as a Vector2
            return new Vector2(xOffset, yOffset);
        }

        public void Vignette()
        {
            var bitmap = GetMapsetBitmap("sb/v.png");
            var sprite = GetLayer("Vignette").CreateSprite("sb/v.png", OsbOrigin.Centre, new Vector2(320, 240));

            sprite.ScaleVec(0, 854.0f / bitmap.Width, 480.0f / bitmap.Height);
            sprite.Fade(0, 1000, 0, 0.8f);
            sprite.Fade(338918 - 1000, 338918, 0.8f, 0);
        }

        public void Destruction(int startTime, int endTime, int intensity)
        {
            for (int i = startTime; i < endTime; i += 20)
            {
                var sprite = GetLayer("Destruction").CreateSprite("sb/destruction/d" + Random(1, 9) + ".png", OsbOrigin.Centre);

                var startPos = new Vector2(-207, Random(0, 480));
                var RandomIntensity = Random(intensity, intensity * 5);
                var endPos = new Vector2(947, (float)Random(startPos.Y - 200, startPos.Y + 200));

                sprite.Scale(i, Random(0.01f, 0.6f));
                sprite.Move(i, i + RandomIntensity, startPos, endPos);
                sprite.Rotate(i, i + RandomIntensity, Random(MathHelper.DegreesToRadians(-180), MathHelper.DegreesToRadians(180)),
                                                      Random(MathHelper.DegreesToRadians(-180), MathHelper.DegreesToRadians(180)));
            }
        }

        public void Pulse(int startTime)
        {
            var bitmap = GetMapsetBitmap("sb/p.png");
            var sprite = GetLayer("Pulse").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(320, 240));

            sprite.ScaleVec(startTime, 854.0f / bitmap.Width, 480.0f / bitmap.Height);
            sprite.Fade(startTime, startTime + 3000, 0.1f, 0);
            sprite.Additive(startTime, startTime + 3000);
        }

        public void PulseReverse(int startTime, int duration)
        {
            var bitmap = GetMapsetBitmap("sb/p.png");
            var sprite = GetLayer("Pulse").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(320, 240));

            sprite.ScaleVec(startTime - duration, 854.0f / bitmap.Width, 480.0f / bitmap.Height);
            sprite.Fade(startTime - duration, startTime, 0, 0.2f);
            sprite.Fade(startTime, startTime + 3000, 0.2f, 0);
            sprite.Additive(startTime - duration, startTime + 3000);
        }
    }
}
