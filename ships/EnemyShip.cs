using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.ammunition;
using System;
using System.Collections.Generic;
using Pong.utils;
using System.Diagnostics;

namespace Pong.ships
{
    abstract class EnemyShip : Ships
    {
        private float incr { get; set; }
        private float rayon { get; set; }
        private float firstX { get; set; }

        public EnemyShip(int life, Vector2 position, Texture2D texture) : base(position, texture, life)
        {
        }

        public void ShipHandle(SpriteBatch batch)
        {
            if (isAlive()) Draw(batch);
        }

        public void enemyShoot(List<Ammunition> enemyAmmo, HeroShip heroShip)
        {
            for(int i = enemyAmmo.Count-1; i>=0; i--)
            {
                enemyAmmo[i].SetPosition(new Rectangle(enemyAmmo[i].GetPosition().X, enemyAmmo[i].GetPosition().Y + 5, enemyAmmo[i].GetPosition().Width, enemyAmmo[i].GetPosition().Height));
                if (enemyAmmo[i].GetPosition().Y - 69 > 800)
                {
                    enemyAmmo.Remove(enemyAmmo[i]);
                    continue;
                }
                if (enemyAmmo[i].GetPosition().Intersects(heroShip.GetPosition()))
                {
                    
                    heroShip.getHitWith(enemyAmmo[i]);
                    enemyAmmo[i].SetDamage(0);
                    enemyAmmo[i].SetTexture(null);
                }
            }
        }

        public abstract void Travel(GraphicsDeviceManager _graphics);
        public abstract List<Ammunition> GetAmmo();

        public class SmallEnemyShip : EnemyShip
        {
            private static int MAX_LIFE = 25;
            private List<Ammunition> wShipAmmos;

            override
            public List<Ammunition> GetAmmo()
            {
                return wShipAmmos;
            }

            public SmallEnemyShip(int pos, GraphicsDeviceManager _graphics, Texture2D texture) : base(MAX_LIFE, new Vector2(0, 0), texture)
            {
                if (pos == 1)
                {
                    rectangle = new Rectangle(_graphics.PreferredBackBufferWidth / 4 + 35 - texture.Width / 2, _graphics.PreferredBackBufferHeight - 150 - texture.Width - 400, rectangle.Width, rectangle.Height);
                    incr = 0;
                }
                else if (pos == 2)
                {
                    rectangle = new Rectangle((int)(float)(_graphics.PreferredBackBufferWidth - _graphics.PreferredBackBufferWidth / 3.5) - texture.Width / 2, _graphics.PreferredBackBufferHeight - 150 - texture.Width - 400, rectangle.Width, rectangle.Height);
                    incr = 90;
                }
                rayon = _graphics.PreferredBackBufferWidth / 4;
                firstX = rectangle.X;
                wShipAmmos = new List<Ammunition>();
            }

            override
            public void Travel(GraphicsDeviceManager _graphics)
            {
                incr += (float)(0.02 % (2 * Math.PI));


                if (isAlive())
                {
                    rectangle = new Rectangle((int)((float)(Math.Sin(incr) * rayon) + firstX), rectangle.Y, rectangle.Width, rectangle.Height);
                }
            }

            
            public void SpawnEnemyAmmo(Texture2D texture)
            {
                if (isAlive())
                {
                    WShipAmmo wShipAmmo = new WShipAmmo(texture);
                    wShipAmmo.SetPosition(new Rectangle(rectangle.X + rectangle.Width/2, rectangle.Y + rectangle.Height, texture.Width, texture.Height));
                    wShipAmmos.Add(wShipAmmo);
                }
            }
        }

        public class BigEnemyShip : EnemyShip
        {
            private static int MAX_LIFE = 100;
            private List<Ammunition> alienDroppins;

            override
            public List<Ammunition> GetAmmo()
            {
                return alienDroppins;
            }

            public BigEnemyShip(GraphicsDeviceManager _graphics, Texture2D texture) : base(MAX_LIFE, new Vector2(0, 0), texture)
            {
                rayon = _graphics.PreferredBackBufferWidth / 2 - texture.Width / 2;
                rectangle = new Rectangle(_graphics.PreferredBackBufferWidth / 2 - texture.Width / 2, 0, texture.Width, texture.Height);
                firstX = rectangle.X;
                alienDroppins = new List<Ammunition>();
            }

            override
            public void Travel(GraphicsDeviceManager _graphics)
            {
                incr += (float)(0.015 % (2 * Math.PI));
                rectangle = new Rectangle((int)((int)(float)(Math.Sin(incr) * rayon) + firstX), rectangle.Y, rectangle.Width, rectangle.Height);
            }

            
            public long SpawnEnemyAmmo(Texture2D texture)
            {
                if (isAlive())
                {
                    AlienDroppins alienDrop = new AlienDroppins(texture);
                    alienDrop.SetPosition(new Rectangle(rectangle.X + rectangle.Width/2, rectangle.Y + rectangle.Height, texture.Width, texture.Height));
                    alienDroppins.Add(alienDrop);
                }
                return TimeUtils.GetNanoSeconds();
            }
        }
    }
}