using System;
using System.Collections.Generic;

namespace CyberSecurityChatBot
{
    public enum Sentiment { Neutral, Worried, Curious, Frustrated, Happy }

    public class SentimentDetector
    {
        private Dictionary<Sentiment, List<string>> _sentimentTriggers;
        private Dictionary<Sentiment, List<string>> _sentimentResponses;

        public SentimentDetector()
        {
            InitializeTriggers();
            InitializeResponses();
        }

        private void InitializeTriggers()
        {
            _sentimentTriggers = new Dictionary<Sentiment, List<string>>
            {
                {
                    Sentiment.Worried,
                    new List<string>
                    {
                        "worried", "scared", "afraid", "anxious", "nervous", "unsafe",
                        "concerned", "uneasy", "panic", "threat", "danger", "threat"
                    }
                },
                {
                    Sentiment.Curious,
                    new List<string>
                    {
                        "curious", "wondering", "interested", "want to know", "how does", "explain",
                        "tell me", "what is", "how can", "learn", "know more", "understand"
                    }
                },
                {
                    Sentiment.Frustrated,
                    new List<string>
                    {
                        "frustrated", "annoyed", "confused", "don't understand", "stuck",
                        "lost", "help me", "problem", "issue", "difficult", "complicated"
                    }
                },
                {
                    Sentiment.Happy,
                    new List<string>
                    {
                        "great", "thanks", "helpful", "awesome", "love it", "excellent",
                        "brilliant", "perfect", "amazing", "thank you", "appreciate"
                    }
                }
            };
        }

        private void InitializeResponses()
        {
            _sentimentResponses = new Dictionary<Sentiment, List<string>>
            {
                {
                    Sentiment.Worried,
                    new List<string>
                    {
                        "I understand you're concerned about your security. Let me help ease your worries with some practical advice.",
                        "It's natural to feel worried about cyber threats. You're taking the right step by learning about them.",
                        "Don't worry - with the right knowledge and tools, you can protect yourself effectively."
                    }
                },
                {
                    Sentiment.Curious,
                    new List<string>
                    {
                        "Great question! I'm glad you're interested in learning more about cybersecurity.",
                        "Your curiosity about online safety is excellent. Let me explain this in detail.",
                        "I love your interest in understanding security better. Here's what you need to know."
                    }
                },
                {
                    Sentiment.Frustrated,
                    new List<string>
                    {
                        "I can see you're feeling frustrated. Let me break this down into simpler terms.",
                        "No worries if this seems complicated - cybersecurity can be confusing at first. Let me clarify.",
                        "I understand your frustration. Let me help you understand this better."
                    }
                },
                {
                    Sentiment.Happy,
                    new List<string>
                    {
                        "I'm thrilled you found that helpful! Your proactive approach to security is wonderful.",
                        "Your enthusiasm for security is fantastic! Keep up this mindset.",
                        "I'm so glad I could help! You're making great strides in your security awareness."
                    }
                }
            };
        }

        public Sentiment Detect(string input)
        {
            string lowerInput = input.ToLower();

            // Check in order of priority (Happy and Frustrated might have overlaps with other sentiments)
            foreach (var sentiment in new[] { Sentiment.Happy, Sentiment.Frustrated, Sentiment.Worried, Sentiment.Curious })
            {
                if (_sentimentTriggers.ContainsKey(sentiment))
                {
                    foreach (var trigger in _sentimentTriggers[sentiment])
                    {
                        if (lowerInput.Contains(trigger))
                        {
                            return sentiment;
                        }
                    }
                }
            }

            return Sentiment.Neutral;
        }

        public string GetSentimentResponse(Sentiment sentiment)
        {
            if (sentiment == Sentiment.Neutral || !_sentimentResponses.ContainsKey(sentiment))
            {
                return "";
            }

            Random random = new Random();
            var responseList = _sentimentResponses[sentiment];
            int randomIndex = random.Next(responseList.Count);
            return responseList[randomIndex] + " ";
        }
    }
}
