using System;
using System.Threading;

namespace GameLibrary.Helpers.Debug
{
    public class DebugCommand
    {
        #region Constructors

        /// <summary>
        /// Creates a console command
        /// </summary>
        /// <param name="commandKey">The key that, when pressed, that will activate the command</param>
        /// <param name="description">The description of command.</param>
        public DebugCommand(string command, string description)
            : this(command, description,
            (w, args) => Console.WriteLine("Command not implemented."), 0, "")
        {
        }

        /// <summary>
        /// Builds a full command
        /// </summary>
        /// <param name="commandString">The command</param>
        /// <param name="description">Description</param>
        /// <param name="runCommand">The delegate called when command invoked</param>
        /// <param name="requiredArgs">The # of required args</param>
        /// <param name="usage">The usage string</param>
        public DebugCommand(string commandString, string description, ExecutionDelegate runCommand, int requiredArgs, string usage)
        {
            this.Command = commandString;
            this.Description = description;
            this._Execute = runCommand;
            this.NumRequiredArgs = requiredArgs;
            this.Usage = usage;
        }

        public DebugCommand()
            : this("", "")
        {
        }

        #endregion Constructors

        #region Functioning Loop

        /// <summary>
        /// Calls the run command delegate and provides a method which developers can override when the command is ran.
        /// </summary>
        /// <param name="World"></param>
        /// <param name="args"></param>
        public virtual void Execute(World World, params string[] args)
        {
            if (NumRequiredArgs > 0 && (args == null || args.Length != NumRequiredArgs))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Required arguments(" + NumRequiredArgs + ") not present. For usage type \"help <command>\"");
            }
            else //Run the command
            {
                Thread execution = new Thread(new ThreadStart(() => { _Execute(World, args); }));
                execution.Start();
                var quit = new ConsoleCancelEventHandler((s, a) =>
                {
                    a.Cancel = true;
                    execution.Abort();
                });
                Console.CancelKeyPress += quit;
                while (execution.IsAlive) ;
                Console.WriteLine();
            }
        }

        /// <summary>
        /// The delegate called when the command is ran.
        /// </summary>
        protected ExecutionDelegate _Execute;

        #endregion Functioning Loop

        #region Properties

        /// <summary>
        /// The key that, when pressed, that will activate the command
        /// </summary>
        public string Command
        {
            get;
            set;
        }

        /// <summary>
        /// The description of command.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        public string Usage
        {
            set;
            get;
        }

        public int NumRequiredArgs
        {
            set;
            get;
        }

        #endregion Properties

        #region Helpers

        /// <summary>
        /// Creates a command information string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0,-12} | {1,-5} | {2}", this.Command.ToUpper(), this.NumRequiredArgs, this.Description);
        }

        /// <summary>
        /// Prints out everything about a command
        /// </summary>
        /// <returns></returns>
        public string Info()
        {
            return "=== " + this.Command.ToUpper() + " ==="
                + ("\nDescription:\n"
                    + this.Description.AddLeadingString(3, "")
                    + "\n#ArgsRequired:\n"
                    + this.NumRequiredArgs.ToString().AddLeadingString(3, "")
                    + "\nUsage:\n"
                    + this.Usage.AddLeadingString(3, "")
                ).AddLeadingString(2, "");
        }

        #endregion Helpers

        /// <summary>
        /// Runs a command in the given world regarding the world's entities.
        /// </summary>
        /// <param name="World"></param>
        /// <param name="Entities"></param>
        public delegate void ExecutionDelegate(World World, params string[] args);
    }
}