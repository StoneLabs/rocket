using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Rocket
{
    public enum Powerups
    {
        dipple,
        doppleDamage,
        speedPlus,

        speedMinus,
        topAlltime,
        single
    }

    public class Bullet
    {
        public Vector2 position;
        public Vector2 direction;
        public Texture2D texture;

        public Bullet(Vector2 POSITION, Vector2 DIRECTION, Texture2D TEXTURE)
        {
            position = POSITION;
            direction = DIRECTION;
            texture = TEXTURE;
        }

        public void Update()
        {
            position -= direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 8, 8), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
        }
    }

    public class Powerup
    {
        public Vector2 position;
        public Texture2D texture;
        public Powerups type;

        public Powerup(Vector2 POSITION, Texture2D TEXTURE, Powerups TYPE)
        {
            position = POSITION;
            texture = TEXTURE;
            type = TYPE;
        }

        public void Update()
        {
            position.Y += 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X, position.Y), Color.White);
        }
    }

    public class Star
    {
        public Vector2 position;
        public Texture2D texture;
        public float rotation;
        public int speed;
        public float scaleFac;

        public Star(Vector2 POSITION, float ROTATION, Texture2D TEXTURE, int SPEED, float SCALEFAC)
        {
            position = POSITION;
            rotation = ROTATION;
            texture = TEXTURE;
            speed = SPEED;
            scaleFac = SCALEFAC;
        }

        public void Update()
        {
            position.Y += speed;
            rotation += speed / 100;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, Color.White, rotation, Vector2.Zero, scaleFac / 50, SpriteEffects.None, 0.2f);
        }
    }

    public class Meteor
    {
        public Vector2 position;
        public float rotation;
        public float scaleFac;
        public int life;
        public Texture2D texture;
        public int punkte;
        public float speed;

        public int startState = 50000;

        public Meteor(Vector2 POSITION, float ROTATION, float SCALEFAC, int LIFE, Texture2D TEXTURE, int PUNKTE, float SPEED)
        {
            position = POSITION;
            rotation = ROTATION;
            scaleFac = SCALEFAC;
            life = LIFE;
            texture = TEXTURE;
            punkte = PUNKTE;
            speed = SPEED;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)scaleFac, (int)scaleFac), null, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0.2f);
        }
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SoundEffectInstance sound;

        float positionX;
        float positionY;

        int punkte = 0;
        int lifes = 10;
        float spawnrate = 100;
        int spawnNOT = 0;
        float levelprogress = 0;
        int startState = 100000;
        string middleText = "";
        int fireRate = 5;
        int shootTick = 0;
        bool firstloopDraw = true;
        bool showExtra;
        int rigthBoxHeight = 0;
        bool middleTextShacke = true;
        bool extraWeapon = false;
        bool createNoPowerUp = false;
        string cheat = "";
        bool cheatLines = false;
        bool pause = false;

        KeyboardState oks;

        int powerUp1T = 0;
        int powerUp2T = 0;
        int powerUp3T = 0;
        int powerUp4T = 0;
        int powerUp5T = 0;
        int powerUp6T = 0;

        Texture2D rocket;
        Texture2D bullet;
        Texture2D stern1;
        Texture2D stern2;
        Texture2D stern3;
        Texture2D meteor1;
        Texture2D meteor2;
        Texture2D meteor3;
        Texture2D meteor4;
        Texture2D powerUp0;
        Texture2D powerUp1;
        Texture2D powerUp2;
        Texture2D powerUp3;
        Texture2D powerUp4;
        Texture2D powerUp5;
        Texture2D powerUp6;
        Texture2D life;
        Texture2D progressBox;
        SpriteFont font;

        bool fire = false;
        List<Bullet> bullets = new List<Bullet>();
        List<Meteor> meteors = new List<Meteor>();
        List<Star> stars = new List<Star>();
        List<Powerup> Powerups = new List<Powerup>();

        Vector2 screenSize;
        Vector2 oldScreenSize;

        Random random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            int ScreenHeight = 0;
            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int ScreenWidth = 0;
            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;


            graphics.PreferredBackBufferHeight = ScreenHeight - 100;
            graphics.PreferredBackBufferWidth = ScreenWidth - 50;

            if (ScreenWidth - 50 > 1200)
                graphics.PreferredBackBufferWidth = 1200;

            if (graphics.PreferredBackBufferHeight > graphics.PreferredBackBufferWidth)
                graphics.PreferredBackBufferHeight = graphics.PreferredBackBufferWidth;

            if (graphics.PreferredBackBufferHeight > ScreenWidth - 100)
                graphics.PreferredBackBufferHeight = ScreenWidth - 100;

            graphics.ApplyChanges();



            screenSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            oldScreenSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            Window.AllowUserResizing = false;

            positionX = graphics.PreferredBackBufferWidth / 2 - 50;
            positionY = graphics.PreferredBackBufferHeight - 100;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            rocket = Content.Load<Texture2D>("TEXTURES\\ROCKET\\rocket");
            bullet = Content.Load<Texture2D>("TEXTURES\\BULLET\\bullet");
            meteor1 = Content.Load<Texture2D>("TEXTURES\\METEOR\\meteor1");
            meteor2 = Content.Load<Texture2D>("TEXTURES\\METEOR\\meteor2");
            meteor3 = Content.Load<Texture2D>("TEXTURES\\METEOR\\meteor3");
            meteor4 = Content.Load<Texture2D>("TEXTURES\\METEOR\\meteor4");
            stern1 = Content.Load<Texture2D>("TEXTURES\\STAR\\star1");
            stern2 = Content.Load<Texture2D>("TEXTURES\\STAR\\star2");
            stern3 = Content.Load<Texture2D>("TEXTURES\\STAR\\star3");
            powerUp0 = Content.Load<Texture2D>("TEXTURES\\POWERUP\\pwu0");
            powerUp1 = Content.Load<Texture2D>("TEXTURES\\POWERUP\\GOOD\\pwu1");
            powerUp2 = Content.Load<Texture2D>("TEXTURES\\POWERUP\\GOOD\\pwu2");
            powerUp3 = Content.Load<Texture2D>("TEXTURES\\POWERUP\\GOOD\\pwu3");
            powerUp4 = Content.Load<Texture2D>("TEXTURES\\POWERUP\\EVIL_MUHAHAHAH\\pwu4");
            powerUp5 = Content.Load<Texture2D>("TEXTURES\\POWERUP\\EVIL_MUHAHAHAH\\pwu5");
            powerUp6 = Content.Load<Texture2D>("TEXTURES\\POWERUP\\EVIL_MUHAHAHAH\\pwu6");
            life = Content.Load<Texture2D>("TEXTURES\\LEVEL\\life");
            progressBox = Content.Load<Texture2D>("TEXTURES\\LEVEL\\progressBox");
            font = Content.Load<SpriteFont>("FONT\\Font");

            sound = Content.Load<SoundEffect>("SOUNDS\\MUSIC\\music").CreateInstance();
            sound.Volume = 0.1f;
            sound.IsLooped = true;
            sound.Play();
            if (ScreenHeight < 600 || ScreenWidth < 600)
            {
                System.Windows.Forms.MessageBox.Show("We are very sorry, but to play the game you need at least 600x600 pixel screen resolution.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                Exit();
            }
            this.Window.Title = "Rocket | A Game by Stone | LICENS CC BY-NC-ND";
        }

        public void rebootGame()
        {


            startState = 50000;

            positionX = graphics.PreferredBackBufferWidth / 2 - 50;
            positionY = graphics.PreferredBackBufferHeight - 100;

            if (sound.State == SoundState.Playing)
                sound.Stop();
            sound = Content.Load<SoundEffect>("SOUNDS\\MUSIC\\music").CreateInstance();
            sound.Volume = 0.05f;
            sound.IsLooped = true;
            sound.Play();

            punkte = 0;
            lifes = 10;
            spawnrate = 100;
            spawnNOT = 0;
            levelprogress = 0;
            startState = 0;
            middleText = "";
            fireRate = 5;
            firstloopDraw = true;
            showExtra = false;
            middleTextShacke = true;
            extraWeapon = false;
            createNoPowerUp = false;
            cheat = "";
            cheatLines = false;
            pause = false;

            powerUp1T = 0;
            powerUp2T = 0;
            powerUp3T = 0;
            powerUp4T = 0;
            powerUp5T = 0;
            powerUp6T = 0;

            fire = false;
            bullets = new List<Bullet>();
            meteors = new List<Meteor>();
            stars = new List<Star>();
            Powerups = new List<Powerup>();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                rebootGame();

            screenSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            if (oldScreenSize != screenSize)
            {
                oldScreenSize = screenSize;
                graphics.PreferredBackBufferWidth = (int)screenSize.X;
                graphics.PreferredBackBufferHeight = (int)screenSize.Y;
                graphics.ApplyChanges();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F10))
            {
                graphics.IsFullScreen = false;
                graphics.ApplyChanges();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                showExtra = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                showExtra = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                if (sound.State == SoundState.Playing)
                {
                    sound.Stop();
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                if (sound.State == SoundState.Stopped)
                {
                    sound.Play();
                }
            }

            if (startState > 0)
            {
                middleText = "";
                if (startState > 90000 && startState < 100000)
                {
                    middleText = "A Game by Levy Ehrstein";
                }
                if (startState > 80000 && startState < 90000)
                {
                    middleText = "Music by Xythe";
                }
                if (startState > 70000 && startState < 80000)
                {
                    middleText = "Textures by Levy Ehrstein";
                }
                if (startState > 60000 && startState < 70000)
                {
                    middleText = "Code by Levy Ehrstein";
                }
                if (startState > 50000 && startState < 60000)
                {
                    middleText = "Move with the arrow keys!";
                }
                if (startState > 40000 && startState < 50000)
                {
                    middleText = "Press space to shoot!";
                }
                if (startState > 30000 && startState < 40000)
                {
                    middleText = "Press Esc to restart!";
                }
                if (startState > 20000 && startState < 30000)
                {
                    middleText = "Press m to stop music or n to start!";
                }
                if (startState > 0 && startState < 20000)
                {
                    middleText = "More at www.stone-apps.de";
                }
                if (startState > 100)
                {
                    if (random.Next(0, 10) == 1)
                        middleText = "";
                }
            }
            else
            {
                if (!pause)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.P) && !oks.IsKeyDown(Keys.P))
                        pause = true;

                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                        if (Keyboard.GetState().GetPressedKeys().Length != oks.GetPressedKeys().Length)
                            cheat += Keyboard.GetState().GetPressedKeys()[0];

                    if (cheat.Contains("INFINITY"))
                    {
                        lifes = int.MaxValue;
                        cheat = "";
                    }

                    if (cheat.Contains("MORELINES"))
                    {
                        cheatLines = true;
                        cheat = "";
                    }

                    middleText = "Have fun :D";
                    if (startState < -10000)
                        middleText = "";
                    if (!(lifes < 1) && !(levelprogress > 99.99999f))
                    {
                        UpdateRocket();
                        UpdateMeteor();
                        UpdateBullet();
                        UpdateStars();
                        UpdatePowerUp();

                        if (powerUp1T > 0)
                            powerUp1T -= 10;
                        if (powerUp2T > 0)
                            powerUp2T -= 10;
                        if (powerUp3T > 0)
                            powerUp3T -= 10;

                        if (powerUp4T > 0)
                            powerUp4T -= 10;
                        if (powerUp5T > 0)
                            powerUp5T -= 10;
                        if (powerUp6T > 0)
                            powerUp6T -= 10;

                        punkte++;
                        shootTick++;
                        spawnNOT--;

                        if (spawnrate > 30)                            
                            spawnrate -= 0.0025f;
                        levelprogress += 0.0025f;

                        if ((levelprogress > 80 && levelprogress < 81) || (levelprogress > 81 && levelprogress < 82))
                        {
                            spawnNOT = 10;
                            middleTextShacke = false;
                            middleText = "Houston: We have discovered a large wave in front of you!";
                            createNoPowerUp = true;
                        }

                        if ((levelprogress > 89 && levelprogress < 90))
                        {
                            spawnNOT = 10;
                            middleTextShacke = false;
                            middleText = "Houston: We have discovered a very very very large wave in front of you!";
                        }

                        if ((levelprogress > 90 && levelprogress < 91))
                        {
                            spawnNOT = 10;
                            middleTextShacke = false;
                            middleText = "Rocket: Roger! Wish us luck!";
                        }

                        if ((levelprogress > 91 && levelprogress < 92))
                        {
                            spawnNOT = 10;
                            middleTextShacke = false;
                            middleText = "Rocket Capitain to Weapons officer: All weapons maximal energy!";
                            fireRate = 2;
                        }

                        if ((levelprogress > 95 && levelprogress < 95.25f))
                        {
                            middleTextShacke = false;
                            middleText = "Rocket Capitain to Weapons officer: MORE ENERGY!";
                            extraWeapon = true;
                        }

                        if (levelprogress > 80)
                            spawnrate = 999999;
                        if (levelprogress > 81)
                            spawnrate = 25;

                        if (levelprogress > 88)
                            spawnrate = 999999;
                        if (levelprogress > 91)
                        {
                            spawnrate = 2;
                            powerUp1T = 10000;
                            powerUp2T = 10000;
                            powerUp3T = 10000;
                        }
                        if (levelprogress > 95)
                            spawnrate = 1;

                        if (levelprogress > 98.75f)
                            spawnNOT = int.MaxValue;
                    }
                    else
                    {
                        middleText = "";
                        positionY -= 10;
                        UpdateBullet();
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.P) && !oks.IsKeyDown(Keys.P))
                        pause = false;
                }
            }

            startState -= 50;

            oks = Keyboard.GetState();
            base.Update(gameTime);
        }

        public void UpdatePowerUp()
        {
            if (random.Next(1, 1500) == 1)
            {
                if (!createNoPowerUp)
                    createPowerUp();
            }

            List<int> haveToRemove = new List<int>();

            for (int i = 0; i < Powerups.Count; i++)
            {
                Powerups[i].Update();
                foreach (Bullet b in bullets)
                {
                    if (new Rectangle((int)Powerups[i].position.X, (int)Powerups[i].position.Y, 16, 16).Contains((int)b.position.X, (int)b.position.Y))
                    {
                        haveToRemove.Add(i);
                        if (Powerups[i].type == Rocket.Powerups.dipple)
                        {
                            powerUp1T += 10000;
                            powerUp5T = 0;
                        }
                        if (Powerups[i].type == Rocket.Powerups.doppleDamage)
                        {
                            powerUp2T += 10000;
                        }
                        if (Powerups[i].type == Rocket.Powerups.speedPlus)
                        {
                            powerUp3T += 10000;
                            powerUp6T = 0;
                        }
                        if (Powerups[i].type == Rocket.Powerups.topAlltime)
                        {
                            powerUp4T += 5000;
                        }
                        if (Powerups[i].type == Rocket.Powerups.single)
                        {
                            powerUp5T += 5000;
                            powerUp1T = 0;
                        }
                        if (Powerups[i].type == Rocket.Powerups.speedMinus)
                        {
                            powerUp6T += 5000;
                            powerUp3T = 0;
                        }
                    }
                }
            }

            for (int i = Powerups.Count; i >= 0; i--)
            {
                if (haveToRemove.Contains(i))
                    Powerups.RemoveAt(i);
            }
        }

        public void UpdateRocket()
        {
            int leftrightspeed = 10;
            if (powerUp3T > 0)
                leftrightspeed += 10;

            if (powerUp6T > 0)
                leftrightspeed -= 7;

            if (powerUp4T > 0)
                positionY = graphics.PreferredBackBufferHeight - 400;

            if (Keyboard.GetState().IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                if (positionX - 25 > -50)
                    positionX -= leftrightspeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                if (positionX + 25 < graphics.PreferredBackBufferWidth-50)
                    positionX += leftrightspeed;

            if (Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                if (positionY - 10 > graphics.PreferredBackBufferHeight - 350)
                    positionY -= 10;
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                if (positionY + 10 < graphics.PreferredBackBufferHeight - 80)
                    positionY += 10;
        }

        public void UpdateBullet()
        {
            fire = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                //if (rotation <= 0.025 && rotation >= -0.025)
                    fire = true;

            if (fire)
            {
                if (shootTick%fireRate == 1)
                {
                    if (!(powerUp5T > 0))
                    {
                        bullets.Add(new Bullet(new Vector2(positionX + 35, positionY + 25), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 55, positionY + 25), new Vector2(0, 15), bullet));
                    }
                    if (powerUp1T > 0 || powerUp5T > 0)
                        bullets.Add(new Bullet(new Vector2(positionX + 45, positionY + 25), new Vector2(0, 15), bullet));

                    if (extraWeapon)
                    {
                        bullets.Add(new Bullet(new Vector2(positionX + 20, positionY + 40), new Vector2(5, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 75, positionY + 40), new Vector2(-5, 15), bullet));
                    }
                    if (cheatLines)
                    {
                        bullets.Add(new Bullet(new Vector2(positionX + 0, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 5, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 10, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 15, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 20, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 25, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 30, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 35, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 40, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 45, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 50, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 55, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 60, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 65, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 70, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 75, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 80, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 85, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 90, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 95, positionY + 40), new Vector2(0, 15), bullet));
                        bullets.Add(new Bullet(new Vector2(positionX + 100, positionY + 40), new Vector2(0, 15), bullet));
                    }
                }
            }

            List<int> haveToRemove = new List<int>();

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();

                if (!new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight).Contains(new Point((int)bullets[i].position.X, (int)bullets[i].position.Y)))
                {
                    haveToRemove.Add(i);
                }
            }

            for (int i = bullets.Count; i >= 0; i--)
            {
                if (haveToRemove.Contains(i))
                    bullets.RemoveAt(i);
            }
        }

        public void UpdateStars()
        {
            if (random.Next(0, 10) == 1)
                createStar();

            List<int> haveToRemove = new List<int>();
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].Update();

                if (!new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight).Contains(new Point((int)stars[i].position.X, (int)stars[i].position.Y)))
                    haveToRemove.Add(i);
            }

            for (int i = stars.Count; i >= 0; i--)
            {
                if (haveToRemove.Contains(i))
                    stars.RemoveAt(i);
            }
        }

        public void UpdateMeteor()
        {
            if (!(spawnNOT > 0))
            {
                if (random.Next(0, (int)spawnrate) == 0)
                    createMeteor();
            }

            List<int> haveToRemove = new List<int>();
            List<int> haveToRemoveBullets = new List<int>();
            for (int i = 0; i < meteors.Count; i++)
            {
                meteors[i].position.Y += meteors[i].speed;

                for (int f = 0; f < bullets.Count; f++)
                {
                    if (new Rectangle((int)meteors[i].position.X, (int)meteors[i].position.Y, (int)meteors[i].scaleFac, (int)meteors[i].scaleFac).Intersects(new Rectangle((int)bullets[f].position.X, (int)bullets[f].position.Y, 8, 8)) || new Rectangle((int)meteors[i].position.X, (int)meteors[i].position.Y, (int)meteors[i].scaleFac, (int)meteors[i].scaleFac).Contains(new Rectangle((int)bullets[f].position.X, (int)bullets[f].position.Y, 8, 8)))
                    {
                        meteors[i].life--;
                        if (powerUp2T > 0)
                            meteors[i].life--;
                        haveToRemoveBullets.Add(f);
                    }
                }

                if (meteors[i].life < 1)
                {
                    punkte += meteors[i].punkte;
                    haveToRemove.Add(i);
                }

                if (meteors[i].position.Y > graphics.PreferredBackBufferHeight)
                {
                    haveToRemove.Add(i);
                    lifes--;
                }
            }

            for (int i = meteors.Count; i >= 0; i--)
            {
                if (haveToRemove.Contains(i))
                {
                    meteors.RemoveAt(i);
                }
            }

            for (int i = haveToRemoveBullets.Count; i >= 0; i--)
            {
                if (haveToRemoveBullets.Contains(i))
                    bullets.RemoveAt(i);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0, 0, 0));

            spriteBatch.Begin();

            foreach (Star s in stars)
            {
                s.Draw(spriteBatch);
            }

            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }

            foreach (Meteor m in meteors)
            {
                m.Draw(spriteBatch);
            }

            foreach (Powerup p in Powerups)
            {
                p.Draw(spriteBatch);
            }


            spriteBatch.Draw(rocket, new Rectangle((int)positionX, (int)positionY, 100, 100), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            for (int i = 0, o = 0; o < lifes && o < 15; i += graphics.PreferredBackBufferWidth / 10)
            {
                o++;
                Color col = Color.Black;
                if (lifes > 0)
                    col = Color.DarkRed;
                if (lifes > 3)
                    col = Color.Blue;
                if (lifes > 7)
                    col = Color.Green;
                spriteBatch.Draw(life, new Rectangle(i, 0, graphics.PreferredBackBufferWidth / 10, 50), col);
            }

            if (powerUp1T > 0)
            {
                spriteBatch.Draw(powerUp1, new Rectangle(graphics.PreferredBackBufferWidth - 32, graphics.PreferredBackBufferHeight - 171, 32, 32), Color.White);
                //spriteBatch.DrawString(font, powerUp1T.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 128, graphics.PreferredBackBufferHeight - 146), Color.White);
            }
            if (powerUp2T > 0)
            {
                spriteBatch.Draw(powerUp2, new Rectangle(graphics.PreferredBackBufferWidth - 32, graphics.PreferredBackBufferHeight - 114, 32, 32), Color.White);
                //spriteBatch.DrawString(font, powerUp2T.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 128, graphics.PreferredBackBufferHeight - 89), Color.White);
            }
            if (powerUp3T > 0)
            {
                spriteBatch.Draw(powerUp3, new Rectangle(graphics.PreferredBackBufferWidth - 32, graphics.PreferredBackBufferHeight - 57, 32, 32), Color.White);
                //spriteBatch.DrawString(font, powerUp3T.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 128, graphics.PreferredBackBufferHeight - 32), Color.White);
            }
            if (powerUp4T > 0)
            {
                spriteBatch.Draw(powerUp4, new Rectangle(0, graphics.PreferredBackBufferHeight - 171, 32, 32), Color.White);
                //spriteBatch.DrawString(font, powerUp1T.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 128, graphics.PreferredBackBufferHeight - 146), Color.White);
            }
            if (powerUp5T > 0)
            {
                spriteBatch.Draw(powerUp5, new Rectangle(0, graphics.PreferredBackBufferHeight - 114, 32, 32), Color.White);
                //spriteBatch.DrawString(font, powerUp2T.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 128, graphics.PreferredBackBufferHeight - 89), Color.White);
            }
            if (powerUp6T > 0)
            {
                spriteBatch.Draw(powerUp6, new Rectangle(0, graphics.PreferredBackBufferHeight - 57, 32, 32), Color.White);
                //spriteBatch.DrawString(font, powerUp3T.ToString(), new Vector2(graphics.PreferredBackBufferWidth - 128, graphics.PreferredBackBufferHeight - 32), Color.White);
            }

            if (lifes < 1)
            {
                spriteBatch.DrawString(font, "You lose! Earth destroyed!", new Vector2(graphics.PreferredBackBufferWidth / 2 - font.MeasureString("You lose! Earth destroyed!").X / 2, graphics.PreferredBackBufferHeight / 2 - font.MeasureString("You lose! Earth destroyed!").Y / 2 - font.MeasureString("Points: " + punkte).Y), Color.Red, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Points: " + punkte, new Vector2(graphics.PreferredBackBufferWidth / 2 - font.MeasureString("Points: " + punkte).X / 2, graphics.PreferredBackBufferHeight / 2 - font.MeasureString("Points: " + punkte).Y / 2), Color.Red, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            }
            if (levelprogress > 99.99999f)
            {
                spriteBatch.DrawString(font, "You win! Earth saved!", new Vector2(graphics.PreferredBackBufferWidth / 2 - font.MeasureString("You win! Earth saved!").X / 2, graphics.PreferredBackBufferHeight / 2 - font.MeasureString("You win! Earth saved!").Y / 2 - font.MeasureString("Points: " + punkte).Y), Color.Red, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Points: " + punkte, new Vector2(graphics.PreferredBackBufferWidth / 2 - font.MeasureString("Points: " + punkte).X / 2, graphics.PreferredBackBufferHeight / 2 - font.MeasureString("Points: " + punkte).Y / 2), Color.Red, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            }
            
            int fortschritt = 50 + (int)(((graphics.PreferredBackBufferHeight - 250) / 100 * levelprogress) *-1 + ((graphics.PreferredBackBufferHeight - 250) / 100 * 100));
            spriteBatch.Draw(rocket, new Rectangle(graphics.PreferredBackBufferWidth - 34, fortschritt, 32, 32), Color.Yellow);

            if (firstloopDraw)
                rigthBoxHeight = fortschritt;
            spriteBatch.Draw(progressBox, new Rectangle(graphics.PreferredBackBufferWidth - 36, 50, 35, rigthBoxHeight), Color.White);

            if (showExtra)
            {
                spriteBatch.DrawString(font, "SpawnNot: " + spawnNOT, new Vector2(0, graphics.PreferredBackBufferHeight - 100), Color.Red, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Lifes: " + lifes, new Vector2(0, graphics.PreferredBackBufferHeight - 75), Color.Red, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Level: " + Math.Round(levelprogress, 2) + "%", new Vector2(0, graphics.PreferredBackBufferHeight - 50), Color.Red, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "SR: " + spawnrate, new Vector2(0, graphics.PreferredBackBufferHeight - 25), Color.Red, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            }

            if (middleTextShacke)
                spriteBatch.DrawString(font, middleText, new Vector2((graphics.PreferredBackBufferWidth / 2 - font.MeasureString(middleText).X / 2) + random.Next(-5, 5), (graphics.PreferredBackBufferHeight / 2 - font.MeasureString(middleText).Y / 2) + random.Next(-5, 5)), new Color(random.Next(0, 10), random.Next(240, 255), random.Next(0, 10)));
            else
                spriteBatch.DrawString(font, middleText, new Vector2((graphics.PreferredBackBufferWidth / 2 - font.MeasureString(middleText).X / 2), (graphics.PreferredBackBufferHeight / 2 - font.MeasureString(middleText).Y / 2)), new Color(random.Next(0, 10), random.Next(240, 255), random.Next(0, 10)));
            spriteBatch.End();

            firstloopDraw = false;

            base.Draw(gameTime);
        }

        public void createMeteor()
        {
            if (random.Next(0, (spawnrate < 27 ? 99999999 : 500)) == 1)
            {
                spawnNOT = 1500;

                Meteor met = new Meteor(
                                new Vector2(50, -graphics.PreferredBackBufferWidth),
                                0,
                                graphics.PreferredBackBufferWidth,
                                random.Next(400, 750),
                                meteor4,
                                100000,
                                0.3f
                                );
                meteors.Add(met);
            }
            else
            {
                int type = random.Next(1, 4);
                if (type > 3 || type < 1)
                {
                    System.Windows.Forms.MessageBox.Show("A unexpected exception has occurred:\n\nMissing logic!\nNo logic found.", "XNA Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    Exit();
                }
                switch (type)
                {
                    case 1:
                        {
                            Meteor met = new Meteor(
                                new Vector2(random.Next(50, graphics.PreferredBackBufferWidth - 50), -500),
                                0,
                                random.Next(25, 50),
                                5,
                                meteor1,
                                200,
                                random.Next(1, 4)
                                );
                            meteors.Add(met);
                            break;
                        }
                    case 2:
                        {
                            Meteor met = new Meteor(
                                new Vector2(random.Next(50, graphics.PreferredBackBufferWidth - 50), -500),
                                0,
                                random.Next(100, 150),
                                levelprogress  > 90 ? 20 : 50,
                                meteor2,
                                600,
                                random.Next(1, 2)
                                );
                            meteors.Add(met);
                            break;
                        }
                    case 3:
                        {
                            Meteor met = new Meteor(
                                new Vector2(random.Next(50, graphics.PreferredBackBufferWidth - 50), -500),
                                0,
                                random.Next(150, 200),
                                levelprogress > 90 ? 20 : 25,
                                meteor3,
                                1000,
                                random.Next(3, 6)
                                );
                            meteors.Add(met);
                            break;
                        }
                }
            }
        }

        public void createStar()
        {
            int type = random.Next(1, 4);
            if (type > 3 || type < 1)
            {
                System.Windows.Forms.MessageBox.Show("A unexpected exception has occurred:\n\nMissing logic!\nNo logic found.", "XNA Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Exit();
            }
            switch (type)
            {
                case 1:
                    {
                        Star star = new Star(
                            new Vector2(random.Next(0, graphics.PreferredBackBufferWidth - 30), -0),
                            random.Next(0, 360),
                            stern1,
                            random.Next(1, 10),
                            random.Next(6, 12)
                            );
                        stars.Add(star);
                        break;
                    }
                case 2:
                    {
                        Star star = new Star(
                            new Vector2(random.Next(0, graphics.PreferredBackBufferWidth - 30), -0),
                            random.Next(0, 360),
                            stern2,
                            random.Next(1, 10),
                            random.Next(6, 12)
                            );
                        stars.Add(star);
                        break;
                    }
                case 3:
                    {
                        Star star = new Star(
                            new Vector2(random.Next(0, graphics.PreferredBackBufferWidth - 30), -0),
                            random.Next(0, 360),
                            stern3,
                            random.Next(1, 10),
                            random.Next(6, 12)
                            );
                        stars.Add(star);
                        break;
                    }
            }
        }

        public void createPowerUp()
        {
            int type = 0;
            if (random.Next(0, 5) == 0)
                type = random.Next(4, 7);
            else
                type = random.Next(1, 4);
            
            if (type > 6 || type < 1)
            {
                System.Windows.Forms.MessageBox.Show("A unexpected exception has occurred:\n\nMissing logic!\nNo logic found.", "XNA Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Exit();
            }
            switch (type)
            {
                case 1:
                    {
                        Powerup pw = new Powerup(
                            new Vector2(random.Next(0, graphics.PreferredBackBufferWidth - 16), -16),
                            random.Next(1, 3) == 1 ? powerUp0 : powerUp1,
                            Rocket.Powerups.dipple
                            );
                        Powerups.Add(pw);
                        break;
                    }
                case 2:
                    {
                        Powerup pw = new Powerup(
                            new Vector2(random.Next(0, graphics.PreferredBackBufferWidth - 16), -16),
                            random.Next(1, 3) == 1 ? powerUp0 : powerUp2,
                            Rocket.Powerups.doppleDamage
                            );
                        Powerups.Add(pw);
                        break;
                    }
                case 3:
                    {
                        Powerup pw = new Powerup(
                            new Vector2(random.Next(0, graphics.PreferredBackBufferWidth - 16), -16),
                            random.Next(1, 3) == 1 ? powerUp0 : powerUp3,
                            Rocket.Powerups.speedPlus
                            );
                        Powerups.Add(pw);
                        break;
                    }
                case 4:
                    {
                        Powerup pw = new Powerup(
                            new Vector2(random.Next(0, graphics.PreferredBackBufferWidth - 16), -16),
                            random.Next(1, 5) == 1 ? powerUp0 : powerUp4,
                            Rocket.Powerups.topAlltime
                            );
                        Powerups.Add(pw);
                        break;
                    }
                case 5:
                    {
                        Powerup pw = new Powerup(
                            new Vector2(random.Next(0, graphics.PreferredBackBufferWidth - 16), -16),
                            random.Next(1, 5) == 1 ? powerUp0 : powerUp5,
                            Rocket.Powerups.single
                            );
                        Powerups.Add(pw);
                        break;
                    }
                case 6:
                    {
                        Powerup pw = new Powerup(
                            new Vector2(random.Next(0, graphics.PreferredBackBufferWidth - 16), -16),
                            random.Next(1, 5) == 1 ? powerUp0 : powerUp6,
                            Rocket.Powerups.speedMinus
                            );
                        Powerups.Add(pw);
                        break;
                    }
            }
        }
    }
}
