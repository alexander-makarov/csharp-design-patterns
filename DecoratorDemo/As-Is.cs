// Here we go. To illustrate the using of pattern Decorator
// we start, for example, with some old inherited components 
// written to model battling knights for some kind of computer game.
// We have DarkKnight and WhiteKnight classes defined two type of knights
// within game universe. Both classes were well-debugged, written by some other guys
// or whatever else - the point is we are not allowed to make any changes in these clasess.
// Same goes for Battle class that knows how to execute the fair competition between a given pair of knights.
// 
// The goal is bring some new functionality - spells, into the game universe. 
// So there must be some spells players will be allowed to cast for 
// their knights before the battle begins.
// 
// Below the system with automatic two knights battling the way it is, before our new required functionality being added.

using System;

namespace DecoratorDemo
{
    #region current-system-classes-we-are-not-allowed-to-change
    public abstract class Knight
    {
        /// <summary>
        /// Lifebar, vitality points of the knight
        /// </summary>
        public virtual int LifeBar { get; protected internal set; }
        /// <summary>
        /// Damage caused by the knight when it attacks
        /// </summary>
        public virtual int Damage { get; protected internal set; }

        /// <summary>
        /// Attack other knight
        /// </summary>
        /// <param name="enemy">other knight, the enemy</param>
        /// <returns>true only when enemy is defeated</returns>
        public virtual bool Attack(Knight enemy)
        {
            Console.WriteLine("{0} attacks!", this);
            var isDefeated = !enemy.Defend(this);
            //Console.WriteLine("{0} lifebar={1} now.", enemy, enemy.LifeBar);
            //Console.WriteLine("{0} attacks! {1} lifebar={2} now.", this, enemy, enemy.LifeBar);
            return isDefeated;
        }

        public virtual bool Defend(Knight enemy)
        {
            Console.WriteLine("{0} defends!", this);
            LifeBar -= enemy.Damage; // get a damage
            LifeBar = LifeBar < 0 ? 0 : LifeBar; // minimum lifebar value is zero.
            return LifeBar > 0; // return if knight is still alive
        }
    }

    public class DarkKnight : Knight
    {
        public DarkKnight()
        {
            LifeBar = 100;
            Damage = 25;
        }
        public override string ToString()
        {
            return "DarkKnight";
        }
    }

    public class WhiteKnight : Knight
    {
        public WhiteKnight()
        {
            LifeBar = 100;
            Damage = 20;
        }

        public override string ToString()
        {
            return "WhiteKnight";
        }
    }

    /// <summary>
    /// Battling of two knights implementation
    /// </summary>
    public class Battle
    {
        public Knight Kn1 { get; private set; }
        public Knight Kn2 { get; private set; }

        public Battle(Knight kn1, Knight kn2)
        {
            Kn1 = kn1;
            Kn2 = kn2;
        }

        public void BeginBattle()
        {
            Console.WriteLine("{0} [Lifebar={1} Damage={2}]\r\nVS\r\n{3} [Lifebar={4} Damage={5}]\r\n", 
                Kn1, Kn1.LifeBar, Kn1.Damage, Kn2, Kn2.LifeBar, Kn2.Damage);

            var knightToAttack = Kn1; // first knight to attack will be a dark knight
            var knightToDefend = Kn2; // correspodingly white knight is first to defend itself

            // let's battle now
            int turnNumber = 1;
            Console.WriteLine("Turn #{0}", turnNumber++);
            while (!knightToAttack.Attack(knightToDefend)) // till any of them will be defeated
            {
                // switch knights for the next turn
                var tmp = knightToAttack;
                knightToAttack = knightToDefend;
                knightToDefend = tmp;
                Console.WriteLine("\r\n{0}.Lifebar={1}\t{2}.Lifebar={3}", Kn1, Kn1.LifeBar, Kn2, Kn2.LifeBar);
                Console.WriteLine("\r\nTurn #{0}", turnNumber++);
            }
            Console.WriteLine("\r\n{0}.Lifebar={1}\t{2}.Lifebar={3}", Kn1, Kn1.LifeBar, Kn2, Kn2.LifeBar);

            // find out the winner:
            Console.WriteLine("\r\nThe winner is {0}. {1} has been defeated.", knightToAttack, knightToDefend);
        }
    }
    #endregion current-system-classes-we-are-not-allowed-to-change

    /// <summary>
    /// Simple two knights battling example
    /// </summary>
    public static class AsIsDemo
    {
        public static void Run()
        {
            // having two knights about to battle:
            Knight darkKnight = new DarkKnight();
            Knight whiteKnight = new WhiteKnight();

            // let the battle begin:
            var battle = new Battle(darkKnight, whiteKnight);
            battle.BeginBattle();
        }
    }
}
