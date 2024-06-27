using Newtonsoft.Json;

namespace TypingIndicator
{
    [Serializable]
    public class Configuration
    {
        public Dictionary<string, string> Localizations = new()
        {
            {"en", "Typing..."},
            // {"nl", "Typen..."},
            // {"ar", "الكتابة"},
            // {"it", "Digitando..."},
            // {"fr", "Dactylographie..."},
            // {"de", "Tippen..."},
            // {"ru", "Ввод текста..."},
            // {"ja", "タイピング"},
            // {"es-es", "Mecanografía..."}
        };

        public float Timeout = 5;
        public int MaxRange = 50;
        public string READONLY_CreatedWithTypingIndicatorVersion;

        public Configuration(){}

        public Packet_ServerConfig ToServerConfigPacket(string playerLanguageCode)
        {
            return new Packet_ServerConfig()
            {
                ServerLocalizations = Localizations,
                MaxTimeout = Timeout,
                MaxRange = MaxRange,
                PlayerLanguageCode = playerLanguageCode
            };
        }
    }
}