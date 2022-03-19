using CommandMod.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandMod.Commands
{

    public abstract class BaseCommand
    {
        private string _id;
        private string _description;
        private string _format;

        public string Id
        {
            get
            {
                return this._id;
            }
        }
        public string Description
        {
            get
            {
                return this._description;
            }
        }

        public string Format
        {
            get
            {
                return this._format;
            }
        }

        public BaseCommand(string id, string description, string format)
        {
            this._id = id;
            this._description = description;
            this._format = format;
        }

        /// <summary>
        /// write the code that you want to execute when the user type your command
        /// </summary>
        public abstract string invoke(string[] args);
    }
    
}
