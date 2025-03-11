using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace MazeG
{
    public abstract class Player
    {
        public string Name { get; }
        public string Emoji { get; }
        public int LifePoints { get; set; }
        public int Speed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<StatusEffect> Effects { get; } = new List<StatusEffect>();

        // Nueva propiedad para definir el color usando markup de Spectre.Console.
        public string ColorMarkup { get; }

        public Player(string name, string emoji, int lifePoints, int speed, int x, int y, string colorMarkup)
        {
            Name = name;
            Emoji = emoji;
            LifePoints = lifePoints;
            Speed = speed;
            X = x;
            Y = y;
            ColorMarkup = colorMarkup;
        }

        public abstract void UseSkill();

        public void ProcessEffects()
        {
            foreach (var effect in Effects.ToList())
            {
                effect.ApplyEffect(this);
                if (effect.Duration <= 0)
                    Effects.Remove(effect);
            }
        }
    }
}