using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameLibrary.Helpers.Debug
{
    public class DebugCommandManager : ConcurrentBag<DebugCommand>
    {
        public DebugCommandManager(World world, params DebugCommand[] commands)
            : base(commands)
        {
            this._World = world;
        }

        #region Functioning Loop

        public void Execute(string input)
        {
            List<string> commandInput = new List<string>(input.Split(' ')); //Get a list of all of the words entered
            if (commandInput.Count > 0 && !string.IsNullOrEmpty(commandInput[0]))
            {
                string command = commandInput[0];
                commandInput.RemoveAt(0); //Take the command out of the command string

                //Check to see if command exists
                if (this.Where(x => x.Command.Equals(command)).Count() <= 0)
                    this.Where(x => x.Command.Equals("ex")).Single().Execute(_World, input);
                else  //If there are matching commands, run them.
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Title += " - " + command;
                    foreach (DebugCommand c in this.Where(x => x.Command.Equals(command)))
                    {
                        c.Execute(_World, commandInput.ToArray());
                    }
                }
            }
        }

        #endregion Functioning Loop

        #region Fields

        private World _World;

        #endregion Fields

        #region Helpers

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(String.Format("{0,-12} | {1,-5} | {2}", "Command", "#Args", "Description"));
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (DebugCommand c in this)
                Console.WriteLine(c.ToString());
        }

        public void Clear()
        {
            while (!this.IsEmpty)
            {
                DebugCommand result;
                this.TryTake(out result);
            }
        }

        #endregion Helpers
    }
}