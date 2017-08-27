using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace Multibuff
{
	[ApiVersion(2, 1)]

	public class MultiBuff : TerrariaPlugin
	{
		public override Version Version
		{
			get { return new Version(1, 0); }
		}

		public override string Name
		{
			get { return "Multibuff"; }
		}

		public override string Author
		{
			get { return "TheOldPlayeR"; }
		}

		public override string Description
		{
			get { return "A plugin that makes it easier to give multiple buffs!"; }
		}

		public MultiBuff(Main game)
			: base(game)
		{
			Order = +4;
		}

		public override void Initialize()
		{
			Commands.ChatCommands.Add(new Command("multibuff.buffs", mbuffs, "multibuff", "mbuff"));
		}

		void mbuffs(CommandArgs args)
		{
			if (args.Parameters.Count != 2)
			{
				args.Player.SendErrorMessage("Invalid Syntax");
				args.Player.SendInfoMessage("Example: /mbuff TheOldPlayeR \"Meh.txt\".\nThat would give TheOldPlayeR buffs from \"Meh.txt\".");
				return;
			}
			var foundplr = TShock.Utils.FindPlayer(args.Parameters[0]);
			if (foundplr.Count == 0)
			{
				args.Player.SendErrorMessage("Invalid player!");
				return;
			}
			else {
				string txt = args.Parameters[1];
				string path = Path.Combine(TShock.SavePath, "MultiBuff", txt);
				bool flag3 = File.Exists(path);
				if (flag3)
				{
					var array2 = File.ReadAllLines(path);
					for (int j = 0; j < array2.Length; j++)
					{
						string cmd2 = array2[j];
						Commands.HandleCommand(TSPlayer.Server, "/gbuff " + args.Parameters[0] + " " + cmd2);
					}
					var iplayer = foundplr[0];
					iplayer.SendMessage(args.Player.Name + " has buffed you with buffs from " + txt + "!", Color.Cyan);
					args.Player.SendMessage(iplayer.Name + " has received the buffs!", Color.Cyan);
				}
				else
				{
					File.Create(path);
					args.Player.SendMessage("There was no file called \"" + txt + "\".\n" + txt + " has been created for you.", Color.Cyan);
					args.Player.SendMessage("To add buffs to the file open it with a txt editor and add \"(buff name) (buff time)\".\nExample: \"Shine 9000\"", Color.Cyan);
				}
			}
		}
	}
}

