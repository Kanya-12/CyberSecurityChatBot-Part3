using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CyberSecurityChatBot
{
    public class MemoryManager
    {
        private const string MemoryFile = "user_memory.json";

        // Track if this is a returning user in current session
        private bool _isReturningUser = false;
        private DateTime _previousLastVisit = DateTime.MinValue;

        public UserProfile Profile { get; private set; } = new UserProfile();

        // Property to check if user is returning in this session
        public bool IsReturningUser => _isReturningUser;
        public DateTime PreviousLastVisit => _previousLastVisit;

        public MemoryManager()
        {
            Load();
        }

        public void Load()
        {
            try
            {
                if (File.Exists(MemoryFile))
                {
                    string json = File.ReadAllText(MemoryFile);
                    var loadedProfile = JsonSerializer.Deserialize<UserProfile>(json) ?? new UserProfile();

                    // Detect if this is a returning user (has previous visits and username)
                    if (!string.IsNullOrWhiteSpace(loadedProfile.UserName) && 
                        loadedProfile.TotalVisits > 0 &&
                        loadedProfile.LastVisit > DateTime.MinValue)
                    {
                        _isReturningUser = true;
                        _previousLastVisit = loadedProfile.LastVisit;
                    }

                    Profile = loadedProfile;
                }
                else
                {
                    // First-time user
                    _isReturningUser = false;
                    Profile = new UserProfile();
                }
            }
            catch
            {
                // ignore corrupt memory file
                _isReturningUser = false;
                Profile = new UserProfile();
            }
        }

        public void Save()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(Profile, options);
                File.WriteAllText(MemoryFile, json);
            }
            catch
            {
                // non-fatal
            }
        }

        public void UpdateUserName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Profile.UserName = name.Trim();
                Save();
            }
        }

        /// <summary>
        /// Update visit tracking when user logs in
        /// </summary>
        public void UpdateVisitTracking()
        {
            Profile.LastVisit = DateTime.UtcNow;
            Profile.TotalVisits++;
            Save();
        }

        /// <summary>
        /// Get previous conversations for continuation
        /// </summary>
        public List<ConversationEntry> GetPreviousConversations(int lastCount = 10)
        {
            if (Profile.Conversations == null || Profile.Conversations.Count == 0)
                return new List<ConversationEntry>();

            int startIndex = Math.Max(0, Profile.Conversations.Count - lastCount);
            return Profile.Conversations.GetRange(startIndex, Profile.Conversations.Count - startIndex);
        }

        /// <summary>
        /// Get a summary of the last conversation
        /// </summary>
        public string GetConversationSummary(int lastCount = 5)
        {
            var lastMessages = GetPreviousConversations(lastCount);
            if (lastMessages.Count == 0)
                return "No previous conversation found.";

            var summary = new System.Text.StringBuilder();
            summary.AppendLine("Last conversation summary:");
            foreach (var msg in lastMessages.Where(m => m.Role == "user").TakeLast(3))
            {
                summary.AppendLine($"- You asked: {(msg.Message.Length > 50 ? msg.Message.Substring(0, 50) + "..." : msg.Message)}");
            }
            return summary.ToString();
        }

        public void AddConversationEntry(ConversationEntry entry)
        {
            if (Profile.Conversations == null)
                Profile.Conversations = new List<ConversationEntry>();

            Profile.Conversations.Add(entry);
            // Keep last 200 entries to avoid unbounded growth
            if (Profile.Conversations.Count > 200)
                Profile.Conversations.RemoveRange(0, Profile.Conversations.Count - 200);

            Save();
        }

        /// <summary>
        /// Update game scores
        /// </summary>
        public void UpdateGameScore(string gameName, int score)
        {
            if (Profile.GameScores == null)
                Profile.GameScores = new Dictionary<string, int>();

            Profile.GameScores[gameName] = Math.Max(Profile.GameScores.ContainsKey(gameName) ? Profile.GameScores[gameName] : 0, score);
            Profile.TotalGamesPlayed++;
            Save();
        }
    }
}
