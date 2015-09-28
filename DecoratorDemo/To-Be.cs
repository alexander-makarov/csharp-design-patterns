// Okay, let's add some spell casting here.
// Rules are for one knight might be casted more than one spell at a time.
// Let's add next spells:
// DoubleDamage (double the damage caused by a knight) - that lasts for one turn, 
// Dexterity (give 70% probability for getting off the damage) - lasts for three turns
// DamageReturn (for any damage taken by knight, send 50% of the damage back to the enemy it caused) - lasts for one turn
// Weakness (halve the damage of knight that has been casted on) - lasts for one turn
// 
// Below the implementation by using decorator patterns. Notice we are not changing any of the old code, only adding new functionality.

using System;

namespace DecoratorDemo
{
    public abstract class SpellDecorator : Knight
    {
        protected Knight _spelledKnight;
        protected int _lastingTimeTurnsLeft;

        protected SpellDecorator(Knight toCastSpellOn)
        {
            _spelledKnight = toCastSpellOn;
        }
        public override int LifeBar 
        {
            get { return _spelledKnight.LifeBar; }
            protected internal set { _spelledKnight.LifeBar = value; }
        }
        public override int Damage
        {
            get { return _spelledKnight.Damage; }
            protected internal set { _spelledKnight.Damage = value; }
        }
        public override bool Attack(Knight enemy)
        {
            return _spelledKnight.Attack(enemy);
        }

        public override bool Defend(Knight enemy)
        {
            return _spelledKnight.Defend(enemy);
        }

        public override string ToString()
        {
            return _spelledKnight.ToString();
        }
    }

    public class DoubleDamageSpell : SpellDecorator
    {
        public DoubleDamageSpell(Knight toCastSpellOn)
            :base(toCastSpellOn)
        {
            _lastingTimeTurnsLeft = 1;
        }

        public override bool Attack(Knight enemy)
        {
            if (_lastingTimeTurnsLeft > 0) // while spell is not expired
            {
                Console.WriteLine("DoubleDamageSpell has been casted on {0}:", this);
                base.Attack(enemy); // do double damage (attack two times per turn)
                _lastingTimeTurnsLeft--;
            }
            return base.Attack(enemy);
        }
    }

    public class DamageReturnSpell : SpellDecorator
    {
        public DamageReturnSpell(Knight toCastSpellOn)
            : base(toCastSpellOn)
        {
            _lastingTimeTurnsLeft = 1;
        }

        public override bool Defend(Knight enemy)
        {
            if (_lastingTimeTurnsLeft > 0) // while spell is not expired
            {
                Console.WriteLine("DamageReturnSpell has been casted on {0}:", this);
                enemy.LifeBar -= enemy.Damage;
                Console.WriteLine("{0} gets damage back, lifebar={1} now.", enemy, enemy.LifeBar);
                _lastingTimeTurnsLeft--;
            }
            return base.Defend(enemy);
        }
    }


    /// <summary>
    /// Battling of two knights
    /// </summary>
    public static class ToBeDemo
    {
        public static void Run()
        {
            Knight darkKnight = new DarkKnight();
            darkKnight = new DamageReturnSpell(darkKnight);
            Knight whiteKnight = new WhiteKnight();
            whiteKnight = new DoubleDamageSpell(whiteKnight);
            whiteKnight = new DoubleDamageSpell(whiteKnight);

            var knightToAttack = darkKnight; // first knight to attack will be a dark knight
            var knightToDefend = whiteKnight; // correspodingly white knight is first to defend itself

            // let's battle now
            int turnNumber = 1;
            Console.WriteLine("Turn #{0}",turnNumber++);
            while (!knightToAttack.Attack(knightToDefend)) // till any of them will be defeated
            {
                // switch knights for the next turn
                var tmp = knightToAttack;
                knightToAttack = knightToDefend;
                knightToDefend = tmp;
                Console.WriteLine("\r\nTurn #{0}", turnNumber++);
            }

            // find out the winner:
            Console.WriteLine("The winner is {0}. {1} has been defeated.", knightToAttack, knightToDefend);
        }
    }
}
