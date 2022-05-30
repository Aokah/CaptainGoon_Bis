using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.ammunition
{
    class Ammunition
    {
        private int damage;

        public int GetDamage()
        {
            return damage;
        }

        public void SetDamage(int value)
        {
            damage = value;
        }

        private Rectangle position;

        public Rectangle GetPosition()
        {
            return position;
        }

        public void SetPosition(Rectangle value)
        {
            position = value;
        }

        private Texture2D texture;

        public Texture2D GetTexture()
        {
            return texture;
        }

        public void SetTexture(Texture2D value)
        {
            texture = value;
        }

        public Ammunition(int damage)
        {
            this.SetDamage(damage);
        }
    }

    class Bullets : Ammunition
    {
        public static string texture = "assets/sprites/ammo/bullets/glowtube_small";
        public Bullets(Texture2D texture) : base(10)
        {
            SetPosition(new Rectangle());
            SetTexture(texture);
        }
    }

    class WShipAmmo : Ammunition
    {
        public static string texture = "assets/sprites/ammo/wship-4";
        public WShipAmmo(Texture2D texture) : base(10)
        {
            SetPosition(new Rectangle());
            SetTexture(texture);
        }
    }

    class AlienDroppins : Ammunition
    {
        public static string texture = "assets/sprites/ammo/aliendropping_small";
        public AlienDroppins(Texture2D texture) : base(25)
        {
            SetPosition(new Rectangle());
            SetTexture(texture);
        }
    }
}
