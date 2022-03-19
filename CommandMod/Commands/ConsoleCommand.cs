using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandMod.Commands
{
    public class ConsoleCommand : BaseCommand
    {
        private Func<string[], string> _function;

        public ConsoleCommand(string id, string description, string format, Func<string[], string> function) : base(id, description, format)
        {
            this._function = function;
        }

        public override string invoke(string[] args)
        {
            return _function.Invoke(args);
        }
    }
}
