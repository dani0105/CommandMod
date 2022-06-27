using ModLoader;
using SFS.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommandMod
{
	public class KeySettings : Mod_Keybindings
	{
		public static KeySettings Main;
		public static void Setup()
		{
			Main = SetupKeybindings<KeySettings>(CommandMod.Main);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000C74C File Offset: 0x0000A94C
		public override void CreateUI()
		{
			KeySettings keySettings = new KeySettings(); // default keybindings
			base.CreateUI_Text("CommandMod");
			base.CreateUI_Keybinding(this.Toggle_Console, keySettings.Toggle_Console, "Toggle Console");
		}


		public KeybindingsPC.Key Toggle_Console = KeyCode.T;
	}
}
