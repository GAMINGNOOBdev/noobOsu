using DiscordRPC;
using DiscordRPC.Message;
using System.Text;
using System.Threading;
using osu.Framework.Graphics;

namespace noobOsu.Game
{
    public partial class DiscordRichPresence : Component
    {
        private static string CLIENT_ID = "1072216999905722478";

        private DiscordRpcClient client;
        public RichPresence Presence = new RichPresence()
        {
            Assets = new Assets()
            {
                LargeImageKey = "noobOsu_logo",
                LargeImageText = "Playing the worst osu clone",
            }
        };

        public DiscordRichPresence()
        {
            Setup();
        }

        public void DisposePresence()
        {
            client?.Dispose();
        }

        private void Setup()
        {
            client = new DiscordRpcClient(CLIENT_ID);
            //If you are going to make use of the Join / Spectate buttons, you are required to register the URI Scheme with the client.
            client.RegisterUriScheme();

            //Register to the events we care about. We are registering to everyone just to show off the events

            client.OnReady += OnReady;                                      //Called when the client is ready to send presences
            client.OnClose += OnClose;                                      //Called when connection to discord is lost
            client.OnError += OnError;                                      //Called when discord has a error

            client.OnConnectionEstablished += OnConnectionEstablished;      //Called when a pipe connection is made, but not ready
            client.OnConnectionFailed += OnConnectionFailed;                //Called when a pipe connection failed.

            client.OnPresenceUpdate += OnPresenceUpdate;                    //Called when the Presence is updated

            client.OnSubscribe += OnSubscribe;                              //Called when a event is subscribed too
            client.OnUnsubscribe += OnUnsubscribe;                          //Called when a event is unsubscribed from.

            client.OnJoin += OnJoin;                                        //Called when the client wishes to join someone else. Requires RegisterUriScheme to be called.
            client.OnSpectate += OnSpectate;                                //Called when the client wishes to spectate someone else. Requires RegisterUriScheme to be called.
            client.OnJoinRequested += OnJoinRequested;                      //Called when someone else has requested to join this client.

            //Before we send a initial Presence, we will generate a random "game ID" for this example.
            // For a real game, this "game ID" can be a unique ID that your Match Maker / Master Server generates. 
            // This is used for the Join / Specate feature. This can be ignored if you do not plan to implement that feature.
            Presence.Secrets = new Secrets()
            {
                JoinSecret = "JOIN_definitely_not_a_used_game_id_for_noobOsu",
                SpectateSecret = "SPECTATE_definitely_not_a_used_game_id_for_noobOsu"
            };

            client.SetSubscription(EventType.Join | EventType.Spectate | EventType.JoinRequest);

            client.SetPresence(Presence);

            client.Initialize();
        }

        public void UpdatePresence()
        {
            if (client != null)
                client.SetPresence(Presence);
        }

        private void OnReady(object sender, ReadyMessage args)
        {
        }
        private void OnClose(object sender, CloseMessage args)
        {
        }
        private void OnError(object sender, ErrorMessage args)
        {
            if (client == null)
                return;

            try
            {
                client.Deinitialize();
            }
            catch (DiscordRPC.Exceptions.UninitializedException) {}

            client.Dispose();
            client = null;
        }

        private void OnConnectionEstablished(object sender, ConnectionEstablishedMessage args)
        {

        }
        private void OnConnectionFailed(object sender, ConnectionFailedMessage args)
        {
            if (client == null)
                return;

            try
            {
                client.Deinitialize();
            }
            catch (DiscordRPC.Exceptions.UninitializedException) {}

            client.Dispose();
            client = null;
        }

        private void OnPresenceUpdate(object sender, PresenceMessage args)
        {
        }

        private void OnSubscribe(object sender, SubscribeMessage args)
        {
        }
        private void OnUnsubscribe(object sender, UnsubscribeMessage args)
        {
        }
        
        private void OnJoin(object sender, JoinMessage args)
        {
            //Console.WriteLine("Joining Game '{0}'", args.Secret);
        }

        private void OnSpectate(object sender, SpectateMessage args)
        {
            //Console.WriteLine("Spectating Game '{0}'", args.Secret);
        }

        private void OnJoinRequested(object sender, JoinRequestMessage args)
        {
            // user info
            // Console.WriteLine("'{0}' has requested to join our game.", args.User.Username);
            // Console.WriteLine(" - User's Avatar: {0}", args.User.GetAvatarURL(User.AvatarFormat.GIF, User.AvatarSize.x2048));
            // Console.WriteLine(" - User's Descrim: {0}", args.User.Discriminator);
            // Console.WriteLine(" - User's Snowflake: {0}", args.User.ID);

            DiscordRpcClient client = (DiscordRpcClient)sender;
            client.Respond(args, true);
        }

    }
}
