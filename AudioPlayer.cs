using System;
using System.Media;

namespace CyberSecurityChatBot
{
    public class AudioPlayer
    {
        public static void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("C:\\Users\\kanya kapo\\Downloads\\Cybersecurity ChatBot\\Chatbot greeting.wav");
                player.PlaySync();
            }
            catch
            {
                Console.WriteLine("C:\\Users\\kanya kapo\\Downloads\\Cybersecurity ChatBot\\Chatbot greeting.wav");
            }
        }
    }
}