using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace ws_afk_timer
{
    public class AfkMain : BaseScript
    {
        int timer;

        Vector3 current_position;
        Vector3 previous_position;

        public AfkMain()
        {
            BeginChecking();
        }

        async void BeginChecking()
        {
            while (true)
            {
                int player_ped = API.GetPlayerPed(-1);
                current_position = API.GetEntityCoords(player_ped, true);


                if (current_position == previous_position)
                {
                    if (timer > 10)
                    {
                        SubtractFromTimer(false);
                    }

                    if (timer < 11)
                    {
                        SubtractFromTimer(true);
                    }
                    if (timer == 0)
                    {
                        ServerFunc.KickPlayer();
                        //Debug.WriteLine("Kicked");
                    }
                }

                else
                {
                    ResetTimer();
                }

                previous_position = current_position;
                //Debug.WriteLine("Current_Position: "+ current_position + "||Timer: " + timer );
                await Delay(5000);
            }
        }

        void ResetTimer()
        {
            timer = 15;
        }

        void SubtractFromTimer(bool with_warning)
        {
            if (with_warning)
            {
                SendChatMessage("Warning", "You will be kicked in " + timer + " seconds for being AFK", 250, 0, 0);
            }
            timer = timer - 1;
        }

        public static void SendChatMessage(string title, string message, int r, int g, int b)
        {
            var msg = new Dictionary<string, object>
            {
                ["color"] = new[] { r, g, b },
                ["args"] = new[] { title, message }
            };
            TriggerEvent("chat:addMessage", msg);
        }

    }
}
