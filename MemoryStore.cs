using System;
using System.Collections.Generic;

namespace CyberSecurityChatBot
{
    public class MemoryStore
    {
        private Dictionary<string, string> _memory;

        public string UserName { get; set; } = "";
        public string FavouriteTopic { get; set; } = "";

        public MemoryStore()
        {
            _memory = new Dictionary<string, string>();
        }

        public void Store(string key, string value)
        {
            if (_memory.ContainsKey(key))
            {
                _memory[key] = value;
            }
            else
            {
                _memory.Add(key, value);
            }
        }

        public string Recall(string key)
        {
            if (_memory.ContainsKey(key))
            {
                return _memory[key];
            }
            return null;
        }

        public string GetPersonalisedOpener()
        {
            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(FavouriteTopic))
            {
                return $"As someone interested in {FavouriteTopic}, {UserName}, ";
            }
            else if (!string.IsNullOrWhiteSpace(UserName))
            {
                return $"{UserName}, ";
            }
            else if (!string.IsNullOrWhiteSpace(FavouriteTopic))
            {
                return $"Since you're interested in {FavouriteTopic}, ";
            }

            return "";
        }

        public void ExtractFavouriteTopic(string input)
        {
            string lowerInput = input.ToLower();
            string[] topics = { "password", "phishing", "privacy", "scam", "malware", "firewall", "vpn", "two-factor", "ransomware" };

            foreach (var topic in topics)
            {
                if (lowerInput.Contains("interested in " + topic) || lowerInput.Contains("like " + topic))
                {
                    FavouriteTopic = topic;
                    break;
                }
            }
        }
    }
}
