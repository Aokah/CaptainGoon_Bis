using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.ships;
using Pong.ammunition;
using System.Collections.Generic;
using Pong.utils;

namespace Pong
{
    public class Game1 : Game
    {
        private const int SCREEN_WIDTH = 1280;
        private const int SCREEN_HEIGHT = 800;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Rectangle rect;
        private Texture2D rectTexture;
        private HeroShip ship;
        private EnemyShip.SmallEnemyShip smol1;
        private EnemyShip.SmallEnemyShip smol2;
        private EnemyShip.BigEnemyShip big1;
        private List<EnemyShip.SmallEnemyShip> smallShips;
        private List<EnemyShip> enemyShips;
        private List<Ships> allShips;
        private long lastShootTime;
        private Texture2D bulletTexture;
        private Texture2D wShipTexture;
        private Texture2D alienDropTexture;
        private bool moveWithMouse;
        private bool hasShot;
       

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.ApplyChanges();

            rect = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
            ship = new HeroShip(new Vector2(0, 0), Content.Load<Texture2D>("assets/sprites/ships/blueships1_small"), 100);
            moveWithMouse = true;
            hasShot = false;

            smol1 = new EnemyShip.SmallEnemyShip(1, _graphics, Content.Load<Texture2D>("assets/sprites/ships/roundysh_small"));
            smol2 = new EnemyShip.SmallEnemyShip(2, _graphics, Content.Load<Texture2D>("assets/sprites/ships/roundysh_small"));
            big1 = new EnemyShip.BigEnemyShip(_graphics, Content.Load<Texture2D>("assets/sprites/ships/spco_small"));

            smallShips = new List<EnemyShip.SmallEnemyShip>();
            enemyShips = new List<EnemyShip>();
            allShips = new List<Ships>();
            ListsHandle();

            lastShootTime = big1.SpawnEnemyAmmo(Content.Load<Texture2D>(AlienDroppins.texture));
            smol1.SpawnEnemyAmmo(Content.Load<Texture2D>(WShipAmmo.texture));
            smol2.SpawnEnemyAmmo(Content.Load<Texture2D>(WShipAmmo.texture));

            base.Initialize();
        }

        protected void ListsHandle()
        {
            smallShips.Add(smol1);
            smallShips.Add(smol2);

            enemyShips.Add(smol1);
            enemyShips.Add(smol2);
            enemyShips.Add(big1);

            allShips.Add(ship);
            allShips.Add(smol1);
            allShips.Add(smol2);
            allShips.Add(big1);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rectTexture = Content.Load<Texture2D>("assets/backgrounds/Planets");
            bulletTexture = Content.Load<Texture2D>(Bullets.texture);
            wShipTexture = Content.Load<Texture2D>(WShipAmmo.texture);
            alienDropTexture = Content.Load<Texture2D>(AlienDroppins.texture);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            MoveHandle();
            ShipsHandle();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !hasShot)
            {
                ship.SpawnHeroAmmo(bulletTexture);
                hasShot = true;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released) hasShot = false;

            ship.HeroShoot(enemyShips);
          
            if(TimeUtils.GetNanoSeconds() - lastShootTime > 1000000000)
            {
                lastShootTime = big1.SpawnEnemyAmmo(alienDropTexture);
                smol1.SpawnEnemyAmmo(wShipTexture);
                smol2.SpawnEnemyAmmo(wShipTexture);
            }


            foreach(EnemyShip enemyShip in enemyShips)
            {
                if (enemyShip.GetAmmo().Count != 0) enemyShip.enemyShoot(enemyShip.GetAmmo(), ship);
            }

            base.Update(gameTime);
        }

        protected void MoveHandle()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) ToggleMove(moveWithMouse);
            if (!moveWithMouse)
            {
                ship.Move(Keyboard.GetState().GetPressedKeys());
            }
            else
            {
                ship.MoveMouse(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }

        protected void ShipsHandle()
        {
            if (ship.isAlive())
            {
                ship.PositionTest();
            }

            foreach(Ships ship in allShips)
            {
                if (!ship.isAlive())
                {
                    ship.DeathHandle();
                }
            }

            foreach(EnemyShip enemyShip in enemyShips)
            {
                if (enemyShip.isAlive())
                {
                    enemyShip.Travel(_graphics);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(rectTexture, rect, Color.White);

            DrawShips();
            DrawAmmo();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void DrawShips()
        {
            foreach(Ships ship in allShips)
            {
                if (ship.isAlive())
                {
                    ship.Draw(_spriteBatch);
                }
            }
        }

        protected void DrawAmmo()
        {
            foreach (Bullets bullet in ship.GetBullets())
            {
                if (bullet.GetTexture() != null) _spriteBatch.Draw(bullet.GetTexture(), new Vector2(bullet.GetPosition().X, bullet.GetPosition().Y), Color.White);
            }
           
            foreach(EnemyShip enemy in enemyShips)
            {
                foreach(Ammunition ammo in enemy.GetAmmo())
                {
                    if (ammo.GetTexture() != null) _spriteBatch.Draw(ammo.GetTexture(), new Vector2(ammo.GetPosition().X, ammo.GetPosition().Y), Color.White);
                }
            }
        }

        private void ToggleMove(bool move)
        {
            moveWithMouse = !move;
        }
    }
}
