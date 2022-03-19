# Command Mod for Spaceflight Simulator
_A simple command console for Spaceflight Simulator and API for modders._

## How to install
It is necessary to install modloader, you can download it at this [link](https://github.com/105-Code/SFS-Modloader/releases).
This mod is easy to install, all you need to do is go to [release](https://github.com/dani0105/CommandMod/releases) and download the version you need, then download `CommandMod.zip` and put it in the `MODS` folder where you have SFS installed, then unzip your file and you will get something like `CommandMod` folder with `CommandMod.dll` file inside. Now run your game and if you press `P` the console will appear.

## How to Use
To use this mod, you just need to press `P` and type the command you need. If you need to know what commands are installed, type `/help`, this command will show you all installed commands.

## How to Implement on my mod
I will attach an example of how to register commands from your mod.

```c#
using ModLoader;
using SFS.Parts.Modules;
using SFS.World;
using System;
using UnityEngine;
using CommandMod;
using CommandMod.Commands;
using System.Collections.Generic;

namespace MyMod
{
  public class Main : SFSMod
  {
    public Main() : base(
        "mymodid", // mod id
        "My Mod Name", // Mod name
        "author",  // author
        "v1.1.x", // min modloader version  require
        "v1.0.0", // mod version
        "Mod Description", // description
        null, // assets file name if you need
        new Dictionary<string, string[]> { { "commandMod", new string[] { "v1.0.0"}  } } //add Command Mod as dependency
        ) 
    {
    }

    public override void load()
    {
      // get command mod instance
      CommandMod.CommandMod commandMod = (CommandMod.CommandMod) Loader.main.getMod("commandMod");
      // create my command
      ConsoleCommand helloWorld = new ConsoleCommand("helloworld","Print 'Hello World!'", "/helloworld",this.helloWorld);
      // register my command
      commandMod.registerCommand(helloWorld);
    }

    // command function
    private string helloWorld(string[] args)
    {
      // do things here
      return "Hello World!";
    }

    public override void unload()
    {
      throw new System.NotImplementedException();
    }
  }
}
```


