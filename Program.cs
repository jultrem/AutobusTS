using MQTTnet;
using MQTTnet.Client;

namespace Autobus
{
    class Autobus
    {
        private bool _continuer = true;

        private readonly string CanalPosition;
        private readonly string CanalPassagers;
        private readonly int _id;
        
        private readonly IMqttClient _client;

        private static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("Ce programme doit recevoir un ID numérique pour s'exécuter");
                Environment.Exit(0);
            }

            var bus = new Autobus(args);
            bus.Lancer();
        }

        private Autobus(string[] args)
        {
            _id = int.Parse(args[0]);
            Console.WriteLine(_id);
            CanalPosition = "autobus/" + args[0] + "/position";
            CanalPassagers = "autobus/" + args[0] + "/passagers";

            var mqttFactory = new MqttFactory();
            Console.WriteLine("Allo");
            _client = mqttFactory.CreateMqttClient();
            Console.WriteLine("Bonjour");
        }
        
        private async void Lancer()
        {
            Random rd = new Random();
            int temps = rd.Next(1000, 2000);

            DateTime now = DateTime.Now;
            int heureCourante = now.Hour;
            int minuteCourante = now.Minute;
            string heure = $"{heureCourante}:{minuteCourante}";

            await Task.Run(async () =>
            {
                Console.WriteLine("Hello");
                await Connecter_Client_MQTT();
                Console.WriteLine("Hola");
            });
            // Initialisation

            //Faire les tâches d'envoi
            while (_continuer)
            {
                int[] arrets = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                foreach (int arret in arrets)
                {
                    Console.WriteLine($"L'autobus est à l'arrêt # {arret}!  {heure}");
                    await _client.PublishAsync(new MqttApplicationMessageBuilder()
                        .WithTopic(CanalPosition)
                        .WithPayload($"L'autobus est à l'arrêt # {arret}!  {heure}")
                        .Build());

                    Thread.Sleep(temps);
                }
            }

        }
        
        private async Task Connecter_Client_MQTT()
        {
            Console.WriteLine("Greetings");
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("127.0.0.1").Build();
            Console.WriteLine("salut");
            await _client.ConnectAsync(mqttClientOptions, CancellationToken.None);
            Console.WriteLine("Connecté à MQTT");
        }
        
    }
}