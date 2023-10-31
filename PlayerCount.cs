using AMP;
using AMP.DedicatedServer;
using AMP.DedicatedServer.Plugins;
using AMP.Events;
using AMP.Logging;
using AMP.Network.Data;
using AMP.Network.Packets.Implementation;
using Netamite.Client.Implementation;
using Netamite.Network.Packet.Implementations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace Playercount
{
    public class Playercount : AMP_Plugin
    {
        public override string NAME => "playercount";
        public override string AUTHOR => "Flexhd";
        public override string VERSION => "0.1.3";

        private Coroutine updateCoroutine;
        PlayercountConfig config;

        internal class PlayercountConfig : PluginConfig
        {
            public string message = "Player count:";
        }


        public override void OnStart()
        {
            Log.Info(NAME, "player count plugin online have a cookie");
            // Start the player count display loop
            config = (PlayercountConfig)GetConfig();
            Thread playerCountThread = new Thread(PlayerCountDisplayLoop);
            playerCountThread.Start();
        }

        private void PlayerCountDisplayLoop()
        {
            while (true)
            {
                Vector3 position = Vector3.forward * 3;
                Vector3 upperRight = position + Vector3.up + Vector3.right;
                int playerCount = ModManager.serverInstance.connectedClients;
                string messagecount = $"{config.message}{playerCount}";
                ModManager.serverInstance.netamiteServer.SendToAll(
                    new DisplayTextPacket("players", messagecount, Color.yellow, upperRight, true, true, 10)
                );
                Thread.Sleep(10000);
            }
        }
    }
}
