using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.ammunition;
using System.Diagnostics;
using System.Collections.Generic;

namespace Pong.ships
{
    class HeroShip : Ships
    {
        private List<Bullets> bullets;

        public List<Bullets> GetBullets()
        {
            return bullets;
        }

        public HeroShip(Vector2 position, Texture2D texture, int life) : base(position, texture, life)
        {
            bullets = new List<Bullets>();
        }

        public void Move(Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (key == Keys.Left) rectangle = new Rectangle(rectangle.X - 20, rectangle.Y, rectangle.Width, rectangle.Height);
                if (key == Keys.Right) rectangle = new Rectangle(rectangle.X + 20, rectangle.Y, rectangle.Width, rectangle.Height);
                if (key == Keys.Down) rectangle = new Rectangle(rectangle.X, rectangle.Y + 20, rectangle.Width, rectangle.Height);
                if (key == Keys.Up) rectangle = new Rectangle(rectangle.X, rectangle.Y - 20, rectangle.Width, rectangle.Height);
            }
        }

        public void MoveMouse(int x, int y) => rectangle = new Rectangle(x - 100 / 2, y - 136 / 2, rectangle.Width, rectangle.Height);

        public void PositionTest()
        {
            if (rectangle.X > 1280 - 100) rectangle = new Rectangle(1280 - 100, rectangle.Y, rectangle.Width, rectangle.Height);
            if (rectangle.X < 0) rectangle = new Rectangle(0, rectangle.Y, rectangle.Width, rectangle.Height);
            if (rectangle.Y > 800 - 136) rectangle = new Rectangle(rectangle.X, 800 - 136, rectangle.Width, rectangle.Height);
            if (rectangle.Y < 550) rectangle = new Rectangle(rectangle.X, 550, rectangle.Width, rectangle.Height);
        }

        public void SpawnHeroAmmo(Texture2D texture)
        {
            if (isAlive())
            {
                Bullets bullet = new Bullets(texture);
                bullet.SetPosition(new Rectangle(rectangle.X + rectangle.Width / 2 - texture.Width, rectangle.Y, texture.Width, texture.Height));
                bullets.Add(bullet);
            }
        }

        public void HeroShoot(List<EnemyShip> enemyShips)
        {
            for(int i = bullets.Count - 1; i>=0; i--)
            {
                bullets[i].SetPosition(new Rectangle(bullets[i].GetPosition().X, (int)(bullets[i].GetPosition().Y - 25), bullets[i].GetPosition().Width, bullets[i].GetPosition().Height));
                if (bullets[i].GetPosition().Y + 25 > 800) bullets.Remove(bullets[i]);
                foreach(EnemyShip enemyShip in enemyShips)
                {
                    if (bullets[i].GetPosition().Intersects(enemyShip.GetPosition())){
                        enemyShip.getHitWith(bullets[i]);
                        bullets[i].SetDamage(0);
                        bullets[i].SetTexture(null);
                    }
                }
            }

        }
    }
}
