// Here we go. To illustrate the using of pattern Decorator
// we start, for example, with some old inherited components 
// written to model battling knights for some kind of computer game.
// We have DarkKnight and WhiteKnight classes defined two type of knights
// within game universe. Both classes were well-debugged, written by some other guys
// or whatever else - the point is we are not allowed to make any changes in these clasess.
// 
// The goal is bring some new functionality - spells, into game universe. 
// So there must be some spells players will be allowed to cast for 
// their knights before the battle begins.
// 
// Below the system with automatic two knights battling the way it is, before our new required functionality being added.

using System;

namespace DecoratorDemo
{
    public abstract class Knight
    {
        /// <summary>
        /// Lifebar, vitality points of the knight
        /// </summary>
        public int LifeBar { get; protected set; }
        /// <summary>
        /// Damage caused by the knight when it attacks
        /// </summary>
        public int Damage { get; protected set; }

        /// <summary>
        /// Attack other knight
        /// </summary>
        /// <param name="enemy">other knight, the enemy</param>
        /// <returns>true only when enemy is defeated</returns>
        public virtual bool Attack(Knight enemy)
        {
            var isDefeated = !enemy.Defend(this);
            Console.WriteLine("{0} attacks! {1} lifebar={2} now.", this, enemy, enemy.LifeBar);
            return isDefeated;
        }

        public virtual bool Defend(Knight enemy)
        {
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
    /// Battling of two knights
    /// </summary>
    public static class AsIsDemo
    {
        public static void Run()
        {
            Knight darkKnight = new DarkKnight();
            Knight whiteKnight = new WhiteKnight();

            var knightToAttack = darkKnight; // first knight to attack will be a dark knight
            var knightToDefend = whiteKnight; // correspodingly white knight is first to defend itself

            // let's battle now
            while (!knightToAttack.Attack(knightToDefend)) // till any of them will be defeated
            {
                // switch knights for the next turn
                var tmp = knightToAttack;
                knightToAttack = knightToDefend;
                knightToDefend = tmp;
            }

            // find out the winner:
            Console.WriteLine("The winner is {0}. {1} has been defeated.", knightToAttack, knightToDefend);
        }
    }
}
