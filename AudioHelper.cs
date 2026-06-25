using System;
using System.IO;
using System.Media;
using System.Reflection;

namespace CyberSecurityChatBot
{
    public class AudioHelper
    {
        private static readonly string GreetingAudioFileName = "Chatbot greeting.wav";
        private static string GetAudioFilePath()
        {
            // Get the directory where the application is running
            string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(appDirectory, GreetingAudioFileName);
        }

        /// <summary>
        /// Play the greeting audio file when app starts.
        /// </summary>
        public static void PlayGreeting()
        {
            try
            {
                string audioPath = GetAudioFilePath();
                System.Console.WriteLine($"Looking for audio file at: {audioPath}");

                if (File.Exists(audioPath))
                {
                    System.Console.WriteLine($"Audio file found! Playing: {audioPath}");
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.PlaySync(); // Play synchronously so greeting finishes before UI is ready
                    }
                    System.Console.WriteLine("Audio playback completed.");
                }
                else
                {
                    System.Console.WriteLine($"Greeting audio file not found at: {audioPath}");
                    // List files in the directory for debugging
                    string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (Directory.Exists(appDirectory))
                    {
                        var wavFiles = Directory.GetFiles(appDirectory, "*.wav");
                        System.Console.WriteLine($"WAV files found in {appDirectory}:");
                        foreach (var file in wavFiles)
                        {
                            System.Console.WriteLine($"  - {Path.GetFileName(file)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error playing greeting audio: {ex.Message}");
                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
                // Silently fail - don't interrupt app startup
            }
        }

        /// <summary>
        /// Convert text to speech and play it aloud.
        /// Note: .NET 10 doesn't have System.Speech in Windows SDK. 
        /// This is a placeholder for future TTS implementation.
        /// For now, we silently skip TTS to maintain compatibility.
        /// </summary>
        public static void Speak(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            try
            {
                // Placeholder for TTS implementation
                // In a production app, you could use:
                // - Windows TTS APIs via P/Invoke
                // - Third-party TTS services (Azure Cognitive Services, Google, etc.)
                // - System.Speech on Windows Desktop Framework

                System.Console.WriteLine($"[TTS Placeholder] Bot would say: {text}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error in text-to-speech: {ex.Message}");
                // Silently fail - don't interrupt chat
            }
        }

        /// <summary>
        /// Stop any ongoing speech.
        /// </summary>
        public static void StopSpeaking()
        {
            try
            {
                // Placeholder for stop functionality
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error stopping speech: {ex.Message}");
            }
        }

        /// <summary>
        /// Set the speech rate (-10 to 10, where -10 is very slow and 10 is very fast).
        /// </summary>
        public static void SetSpeechRate(int rate)
        {
            try
            {
                // Placeholder for rate setting
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error setting speech rate: {ex.Message}");
            }
        }

        /// <summary>
        /// Set the speech volume (0 to 100).
        /// </summary>
        public static void SetVolume(int volume)
        {
            try
            {
                // Placeholder for volume setting
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error setting volume: {ex.Message}");
            }
        }
    }
}
