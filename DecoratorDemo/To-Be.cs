// Okay, let's add some spell casting here.
// Rules are for one knight might be casted more than one spell at a time.
// Let's add next spells:
// DoubleDamage (double the damage caused by a knight) - that lasts for one turn, 
// Dexterity (give 70% probability for getting off the damage, to dodge) - lasts for three turns
// DamageReturn (for any damage taken by knight, send 50% of the damage back to the enemy it caused) - lasts for one turn
// Weakness (halve the damage of knight that has been casted on) - lasts for one turn
// 
// Below the implementation by using decorator pattern. Notice we are not changing any of the old code, 
// only adding new functionality, i.e. following the Open/Closed Principle.

using System;

namespace DecoratorDemo
{
    #region bringing-new-functionality-to-the-system
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
                var fullStengthDamage = _spelledKnight.Damage;
                _spelledKnight.Damage = _spelledKnight.Damage * 2; // damage is being doubled
                var result = base.Attack(enemy); // attack doubled damage
                _lastingTimeTurnsLeft--;

                Console.WriteLine("\t>>> DoubleDamageSpell has been casted on {0} ({1} turns left)", this, _lastingTimeTurnsLeft);
                Console.WriteLine("\t\t{0}.Damage={1} now.", this, Damage);

                _spelledKnight.Damage = fullStengthDamage; // restore normal damage for the knight
                return result;
            }
            return base.Attack(enemy);
        }
    }

    public class WeaknessSpell : SpellDecorator
    {
        public WeaknessSpell(Knight toCastSpellOn)
            : base(toCastSpellOn)
        {
            _lastingTimeTurnsLeft = 1;
        }

        public override bool Attack(Knight enemy)
        {
            if (_lastingTimeTurnsLeft > 0) // while spell is not expired
            {
                var fullStengthDamage = _spelledKnight.Damage;
                _spelledKnight.Damage = _spelledKnight.Damage/2; // being weak
                var result = base.Attack(enemy); // attack being weak  with halved damage
                _lastingTimeTurnsLeft--;

                Console.WriteLine("\t>>> WeaknessSpell has been casted on {0} ({1} turns left)", this, _lastingTimeTurnsLeft);
                Console.WriteLine("\t\t{0}.Damage={1} now.", this, Damage);

                _spelledKnight.Damage = fullStengthDamage; // restore normal damage for the knight
                return result;
            }
            return base.Attack(enemy);
        }
    }

    public class DamageReturnSpell : SpellDecorator
    {
        public DamageReturnSpell(Knight toCastSpellOn)
            : this(toCastSpellOn, 1)
        {
        }

        public DamageReturnSpell(Knight toCastSpellOn, int lastingTimeTurnsLeft)
            : base(toCastSpellOn)
        {
            _lastingTimeTurnsLeft = lastingTimeTurnsLeft;
        }

        public override bool Defend(Knight enemy)
        {
            if (_lastingTimeTurnsLeft > 0) // while spell is not expired
            {
                enemy.LifeBar -= enemy.Damage / 2;
                var result = base.Defend(enemy);
                _lastingTimeTurnsLeft--;

                Console.WriteLine("\t>>> DamageReturnSpell has been casted on {0} ({1} turns left)", this, _lastingTimeTurnsLeft);
                Console.WriteLine("\t\t{0} gets 50% damage back, lifebar={1} now.", enemy, enemy.LifeBar);
                return result;
            }
            return base.Defend(enemy);
        }
    }

    public class DexteritySpell : SpellDecorator
    {
        private Random _r;
        public DexteritySpell(Knight toCastSpellOn)
            : base(toCastSpellOn)
        {
            _lastingTimeTurnsLeft = 3; 
            _r = new Random(DateTime.Now.Millisecond);
        }

        public override bool Defend(Knight enemy)
        {
            if (_lastingTimeTurnsLeft > 0) // while spell is not expired
            {
                bool result = true; // by default knight is still alive
                
                var takeChanceToDodge = _r.Next(10); // range is 0..9
                if (takeChanceToDodge < 7) // 70% chance to dodge
                {
                    //var nonDodgedDamage = enemy.Damage;
                    //enemy.Damage = 0; // set enemy.Damage to zero just to have a consistent call to Defend method
                    //result = base.Defend(enemy);
                    //enemy.Damage = nonDodgedDamage; // restore normal damage for an enemy
                    Console.WriteLine("{0} dodges!", this);
                }
                else
                {
                    result = base.Defend(enemy);
                }
                
                _lastingTimeTurnsLeft--;

                Console.WriteLine("\t>>> DexteritySpell has been casted on {0} ({1} turns left)", this, _lastingTimeTurnsLeft);
                return result;
            }
            return base.Defend(enemy);
        }
    }

    #endregion bringing-new-functionality-to-the-system

    /// <summary>
    /// Now we can cast spells onto knights that are about to battle!
    /// 
    /// *note: Combination of spells given below  provide both knights with an oportunity to win.
    /// On every running you will get different results, 
    /// including a possible draw when both knights lifebar 
    /// points are equal to zero. However the old system code 
    /// which is responsible for battle does not have a draw option
    /// and will name a winner the knight that happened to be last one to attack :)
    /// </summary>
    public static class ToBeDemo
    {
        public static void Run()
        {
            // decorated darkKnight:
            Knight darkKnight = new DarkKnight();
            darkKnight = new DamageReturnSpell(darkKnight, 4); // cast damage return spell for 4 turns 

            // decorated whiteKnight:
            Knight whiteKnight = new WhiteKnight();
            whiteKnight = new DoubleDamageSpell(new DoubleDamageSpell(whiteKnight)); // cast 4x damage, doubled twice damage
            whiteKnight = new DexteritySpell(new WeaknessSpell(whiteKnight)); // two more spells casted, weakness and dexteriry

            // let the battle begin:
            var battle = new Battle(darkKnight, whiteKnight);
            battle.BeginBattle();
        }
    }
}
