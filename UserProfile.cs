using System;
using System.Collections.Generic;

namespace CyberSecurityChatBot
{
    public class UserProfile
    {
        public string UserName { get; set; } = "";
        public List<string> FavoriteTopics { get; set; } = new List<string>();
        public List<ConversationEntry> Conversations { get; set; } = new List<ConversationEntry>();
        public string ThemePreference { get; set; } = "Dark";

        // User Memory & Return Visit Tracking
        public DateTime FirstVisit { get; set; } = DateTime.UtcNow;
        public DateTime LastVisit { get; set; } = DateTime.UtcNow;
        public int TotalVisits { get; set; } = 1;
        public int ConversationIndexForContinuation { get; set; } = 0;

        // Game Statistics
        public int QuizHighScore { get; set; } = 0;
        public int TriviaCorrectAnswers { get; set; } = 0;
        public int TotalGamesPlayed { get; set; } = 0;
        public Dictionary<string, int> GameScores { get; set; } = new Dictionary<string, int>();
    }

    public class ConversationEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } // "user" or "bot"
        public string Message { get; set; }
    }
}
