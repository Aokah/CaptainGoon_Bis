using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.ammunition;
using System.Diagnostics;

namespace Pong.ships
{
    abstract class Ships
    {
        protected Rectangle rectangle { get; set; }
        protected Texture2D texture { get; set; }
        protected int life { get; set; }

        public Rectangle GetPosition()
        {
            return rectangle;
        }

        public Ships(Vector2 position, Texture2D texture, int life)
        {
            Debug.WriteLine(texture.Width + " " + texture.Height);
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            Debug.WriteLine(rectangle.Width + " " + rectangle.Height);
            this.texture = texture;
            this.life = life;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, rectangle, new Color(255, 255, 255, 255)); //changer blue et green en fonction de la vie pour plus ou moins de rouge 
        }

        public bool isAlive()
        {
            return (life > 0);
        }

        public void getHitWith(Ammunition ammo)
        {
            life = (life - ammo.GetDamage() <= 0) ? 0 : (life - ammo.GetDamage());
        }

        public void DeathHandle()
        {
            rectangle = new Rectangle(-100, -100, 0, 0);
        }
    }
}
