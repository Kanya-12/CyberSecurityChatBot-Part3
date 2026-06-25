using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace CyberSecurityChatBot
{
    public class ChatBot
    {
        private GameManager _gameManager = new GameManager();
        private KeywordResponder _keywords;
        private SentimentDetector _sentiment;
        private MemoryStore _memory;
        private QuestionAnswerer _questionAnswerer;
        private bool _awaitingName = true;
        private string _lastTopic = "";
        // Venting / emotional support state
        private bool _inVentingMode = false;
        private List<string> _ventHistory = new List<string>();
        // Stress management state
        private bool _inStressMode = false;
        private List<string> _stressHistory = new List<string>();

        // Expose venting and stress state to the UI
        public bool IsInVentingMode { get { return _inVentingMode; } }
        public bool IsInStressMode { get { return _inStressMode; } }

        public ChatBot()
        {
            _gameManager = new GameManager();
            _keywords = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();
            _questionAnswerer = new QuestionAnswerer();
        }

        // Expose internal memory for UI/manager integration (read-only)
        public MemoryStore Memory => _memory;

        public string GetGreeting()
        {
            return "Hello there! What is your name?";
        }

        public string ProcessInput(string input)
        {

            if (string.IsNullOrWhiteSpace(input))
            {
                return "No worries. Please type a message or ask a question.";
            }

            input = input.Trim();
            string lowerInput = input.ToLower();

            // HIGHEST PRIORITY: Detect goodbye and exit all modes
            if (DetectGoodbye(lowerInput))
            {
                // Exit any active modes
                _inVentingMode = false;
                _inStressMode = false;
                _ventHistory.Clear();
                _stressHistory.Clear();

                // Return special goodbye message with user name
                return GetGoodbyeMessage();
            }

            // Step 1: If awaiting name, capture it
            if (_awaitingName)
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    _memory.UserName = CapitalizeName(input);
                    _awaitingName = false;
                    return $"Welcome {_memory.UserName}! I am KANYA SHIELD, your Cybersecurity Awareness Assistant. I'm here to help you stay safe online and learn about cybersecurity. What would you like to know?";
                }
                else
                {
                    return "Oops, I didn't catch your name. Please type your name so we can continue:";
                }
            }

            // Step 1.5: Handle explicit joke requests (PRIORITY)
            if (lowerInput.Contains("tell me a joke") || lowerInput == "joke" || lowerInput.Contains("make me laugh") || lowerInput.Contains("tell joke"))
            {
                return GetRandomJoke();
            }

            // Step 2: Handle follow-up phrases
            if (lowerInput.Contains("tell me more") || lowerInput.Contains("explain more") || lowerInput.Contains("more"))
            {
                if (!string.IsNullOrWhiteSpace(_lastTopic))
                {
                    string moreResponse = _keywords.GetResponse(_lastTopic);
                    if (!string.IsNullOrWhiteSpace(moreResponse))
                    {
                        return moreResponse;
                    }
                }
            }

            // Step 4: Detect sentiment
            Sentiment detectedSentiment = _sentiment.Detect(input);
            string sentimentOpener = _sentiment.GetSentimentResponse(detectedSentiment);

            // Step 4.2: Handle venting requests (NEW!)
            if (!_inVentingMode && (lowerInput.Contains("vent") || lowerInput.Contains("frustrated") || lowerInput.Contains("angry") || 
                lowerInput.Contains("upset") || lowerInput.Contains("stressed") || lowerInput.Contains("worried about") ||
                lowerInput.Contains("i hate") || (lowerInput.Contains("this is") && lowerInput.Contains("annoying"))))
            {
                _inVentingMode = true;
                _ventHistory.Clear();
                string ventingResponse = GetVentingSupport(input, detectedSentiment);
                return ventingResponse + " If you'd like to keep talking about how you feel, just go on — I'm listening. Say 'stop venting' when you want practical help.";
            }

            // Step 4.3: Handle stress management requests (NEW!)
            if (!_inStressMode && DetectStress(lowerInput))
            {
                _inStressMode = true;
                _stressHistory.Clear();
                string stressResponse = GetStressSupport(input, detectedSentiment);
                return stressResponse + " If you want more help managing stress, ask for: breathing exercises, grounding techniques, coping strategies, or progressive muscle relaxation. Say 'stop stress help' when you're ready to continue.";
            }

            // If already in stress mode, capture stress concerns and respond supportively
            if (_inStressMode)
            {
                _stressHistory.Add(input);
                string ongoingResponse = GetStressSupport(input, detectedSentiment);
                return ongoingResponse + " (You can ask for exercises, coping strategies, or say 'stop stress help' to switch to other topics.)";
            }

            // If already in venting mode, capture emotions and respond empathetically
            if (_inVentingMode)
            {
                _ventHistory.Add(input);
                string ongoingResponse = GetVentingSupport(input, detectedSentiment);
                return ongoingResponse + " (You can say 'stop venting' to switch to technical help or ask 'what can I do' for practical steps.)";
            }

            // Step 4.5: Check for detailed question answers BEFORE keyword matching
            string detailedAnswer = _questionAnswerer.GetAnswer(input);
            if (!string.IsNullOrWhiteSpace(detailedAnswer))
            {
                _memory.ExtractFavouriteTopic(input);
                _lastTopic = input;
                return sentimentOpener + detailedAnswer;
            }

            // Step 5: Run keyword detection
            string keywordResponse = _keywords.GetResponse(input);
            if (!string.IsNullOrWhiteSpace(keywordResponse))
            {
                _memory.ExtractFavouriteTopic(input);
                _lastTopic = input;
                return sentimentOpener + keywordResponse;
            }

            // Step 6: Handle special phrases
            if (lowerInput == "how are you")
            {
                return "I'm doing great and ready to help you stay safe online!";
            }
            if (lowerInput.Contains("stop venting") || lowerInput.Contains("i'm ready for help") || lowerInput.Contains("i want help") || lowerInput.Contains("what can i do"))
            {
                // User wants to exit venting/emotional support mode
                if (_inVentingMode)
                {
                    _inVentingMode = false;
                    string summary = SummarizeVentHistory();
                    return "Thank you for sharing — I hear you. " + summary + " If you want practical steps, ask me about passwords, 2FA, reporting scams, or say 'give me steps'.";
                }
            }
            if (lowerInput.Contains("stop stress help") || lowerInput.Contains("stress mode off"))
            {
                // User wants to exit stress management mode
                if (_inStressMode)
                {
                    _inStressMode = false;
                    string summary = SummarizeStressHistory();
                    return "I'm glad we talked about this. " + summary + " Remember: self-care is important. Feel free to reach out anytime you need support. What else can I help with?";
                }
            }

            if (lowerInput.Contains("what can you do") || lowerInput.Contains("what can i ask"))
            {
                var keywords = _keywords.GetAllKeywords();
                return "I can help you with questions about: " + string.Join(", ", keywords) + " and more! I also answer detailed questions about cybersecurity practices and threats.";
            }
            if (lowerInput.Contains("purpose"))
            {
                return "My purpose is to teach cybersecurity and help protect users from online threats. I provide detailed explanations and practical advice on staying safe online.";
            }

            // General conversational fallback (chat like ChatGPT/Copilot)
            string generalChat = GetGeneralChatResponse(input, detectedSentiment);
            if (!string.IsNullOrWhiteSpace(generalChat))
            {
                return generalChat;
            }

            // Step 7: Fallback - provide specific clarifying questions to help user
            // Instead of generic "I don't know" responses, ask what they want to learn about
            string[] clarifyingResponses = new string[]
            {
                "I'd like to help! Could you ask about one of these topics: phishing, passwords, malware, ransomware, two-factor authentication, privacy, VPNs, firewalls, social engineering, or scams?",
                "I specialize in cybersecurity topics. Would you like to know about: passwords, phishing, malware, privacy, 2FA, VPNs, firewalls, or how to report a scam?",
                "Let me help you with cybersecurity! I can answer questions about phishing, passwords, malware, privacy, VPNs, firewalls, ransomware, or security best practices. What interests you?",
                "I can assist with cybersecurity topics like: passwords, phishing, malware, two-factor authentication, VPNs, privacy, firewalls, social engineering, and scam reporting. Which would you like to explore?",
                "I'm here to help with cybersecurity! Ask me about passwords, phishing, malware, privacy, VPNs, firewalls, ransomware, identity theft, or how to stay safe online."
            };

            Random random = new Random();
            return clarifyingResponses[random.Next(clarifyingResponses.Length)];
        }

        // ================= NAME =================
        private string CapitalizeName(string name)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(name.Trim().ToLower());
        }

        // ================= VENTING SUPPORT (NEW!) =================
        private string GetVentingSupport(string input, Sentiment sentiment)
        {
            // Basic empathetic replies adjusted by detected sentiment
            List<string> supportive = new List<string>();

            if (sentiment == Sentiment.Frustrated || sentiment == Sentiment.Worried)
            {
                supportive.Add($"I hear you, {_memory.UserName}. That sounds really tough. Thank you for sharing that with me.");
                supportive.Add("It's understandable to feel overwhelmed. You're not alone in this.");
                supportive.Add("Taking a deep breath can help a bit — try breathing in for 4 seconds, hold 4, breathe out 6.");
                supportive.Add("Would you like some grounding techniques or practical steps to regain control?");
            }
            else if (sentiment == Sentiment.Neutral)
            {
                supportive.Add($"I'm listening, {_memory.UserName}. Tell me more if you'd like to keep talking.");
                supportive.Add("Sometimes talking through the details helps clarify next steps.");
                supportive.Add("Would you like to try a short breathing exercise or get practical cybersecurity steps?");
            }
            else // Positive or unknown (Happy)
            {
                supportive.Add($"I appreciate you sharing that, {_memory.UserName}. I'm here to support you.");
                supportive.Add("If you want, I can help with practical advice or we can just keep talking about how you feel.");
                supportive.Add("Would you like steps to protect your accounts right now?");
            }

            // If user asks for specific emotional help keywords, offer resources
            string lower = input.ToLower();
            if (lower.Contains("panic") || lower.Contains("anxiety") || lower.Contains("can't breathe") || lower.Contains("help me"))
            {
                supportive.Add("If you're feeling panicky or unsafe, please consider contacting local emergency services or a trusted person. If it's about privacy or fraud, I can show reporting resources.");
            }

            // If user requests breathing/grounding explicitly
            if (lower.Contains("breath") || lower.Contains("breathing") || lower.Contains("ground") || lower.Contains("grounding"))
            {
                supportive.Add(GetBreathingExercise());
                supportive.Add(GetGroundingExercise());
            }

            // Choose a combined response
            Random rand = new Random();
            string chosen = supportive[rand.Next(supportive.Count)];

            // Add gentle prompts and track in vent history
            _ventHistory.Add(input);

            string nextPrompts = " You can keep venting, ask for breathing or grounding techniques, or say 'stop venting' to switch to practical steps.";

            return chosen + nextPrompts;
        }

        private string SummarizeVentHistory()
        {
            if (_ventHistory.Count == 0)
                return "You shared some concerns.";

            // Summarize last two entries
            var last = _ventHistory.TakeLast(2).ToList();
            return $"I heard you describe: '{string.Join("; ", last)}'.";
        }

        // ================= COPING EXERCISES =================
        private string GetBreathingExercise()
        {
            return "Breathing exercise: Sit comfortably. Breathe in slowly for 4 seconds, hold for 4, breathe out for 6. Repeat 4 times. Notice how your body feels.";
        }

        private string GetGroundingExercise()
        {
            return "Grounding exercise (5-4-3-2-1): Name 5 things you can see, 4 you can touch, 3 you can hear, 2 you can smell, 1 you can taste. Use this to anchor yourself.";
        }

        private string GetCopingTips()
        {
            return "Coping tips: 1) Take short breaks and breathe. 2) Prioritize small tasks. 3) Reach out to someone you trust. 4) Use a password manager and enable 2FA to reduce worry about accounts.";
        }

        // ================= GENERAL CHAT =================
        // Constrained small-talk: only allow brief greetings, light humour, or enter venting mode.
        // Do NOT attempt to answer technical questions or invent facts here — those must come from
        // QuestionAnswerer or KeywordResponder.
        private string GetGeneralChatResponse(string input, Sentiment sentiment)
        {
            string lower = input.ToLower();


            if (lower.Contains("how are you") || lower.Contains("how's it going") || lower.Contains("what's up"))
            {
                return "I'm a virtual assistant — ready to help with cybersecurity questions or listen if you'd like to vent.";
            }

            // If user mentions stress/challenges explicitly outside venting mode, enter venting mode
            if (lower.Contains("stress") || lower.Contains("challenge") || lower.Contains("overwhelmed") || lower.Contains("struggling"))
            {
                _inVentingMode = true;
                _ventHistory.Clear();
                return "It sounds like you're dealing with some stress. I'm here to listen — tell me what's been going on, or say 'breathing' for a short exercise.";
            }

            // Reflective acknowledgements based on sentiment (non-technical)
            if (sentiment == Sentiment.Happy)
            {
                return "That's nice to hear — tell me more or ask a cybersecurity question.";
            }
            else if (sentiment == Sentiment.Curious)
            {
                return "I can help with facts from my cybersecurity topics — what would you like to learn about?";
            }
            else if (sentiment == Sentiment.Frustrated || sentiment == Sentiment.Worried)
            {
                return "I hear concern — I can listen or provide practical steps for security issues. Which would you prefer?";
            }

            // For anything else, return null to let the main flow give a grounded fallback
            return null;
        }

        // ================= JOKE BANK =================
        private string GetRandomJoke()
        {
            string[] jokes = new string[]
            {
                "Why did the computer show up at work late? It had a hard drive!",
                "Why do cybersecurity experts never get lonely? Because they're always making connections!",
                "Why did the password go to the doctor? Because it was feeling weak!",
                "How many cybersecurity professionals does it take to change a lightbulb? Three: one to change it and two to discuss how the old one could have been exploited.",
                "Why don't phishers ever play hide and seek? Because good malware is always seeking!",
                "I tried to make a joke about UDP, but I wasn't sure if you'd get it.",
                "Why did the hacker go to school? To improve their malware skills!",
                "What's a cybersecurity expert's favorite exercise? Running firewalls!",
                "Did you hear about the database that went on vacation? It needed to recharge its cache!",
                "Why do programmers always mix up Halloween and Christmas? Because Oct 31 = Dec 25!"
            };

            Random random = new Random();
            return jokes[random.Next(jokes.Length)];
        }

        // ================= START METHOD =================
        public void Start()
        {
            // Legacy console-based method. Use ProcessInput() in WPF instead.
            Console.WriteLine(GetGreeting());
        }

        // ================= GOODBYE DETECTION & HANDLING =================
        private bool DetectGoodbye(string lowerInput)
        {
            string[] goodbyeKeywords = new string[]
            {
                "bye", "goodbye", "see you", "farewell", "take care",
                "quit", "exit", "close", "logout", "leave",
                "bye bye", "see ya", "gotta go", "talk soon",
                "until later", "catch you", "later", "cya", "ttyl"
            };

            foreach (var keyword in goodbyeKeywords)
            {
                if (lowerInput.Contains(keyword))
                {
                    return true;
                }
            }
            return false;
        }

        private string GetGoodbyeMessage()
        {
            string userName = !string.IsNullOrWhiteSpace(_memory.UserName) ? _memory.UserName : "friend";

            string[] goodbyeMessages = new string[]
            {
                $"Goodbye, {userName}! It was great talking with you. Stay safe online and remember to keep your passwords strong. 🛡️",
                $"Take care, {userName}! Remember: good cybersecurity habits are the best defense. See you next time! 👋",
                $"Until next time, {userName}! Thanks for letting me help you today. Stay vigilant and stay safe! 🔐",
                $"Bye, {userName}! I hope our chat helped. Remember to enable 2FA and report suspicious activity. Be safe! 🌐",
                $"Goodbye, {userName}! Keep learning, stay curious, and protect your digital life. See you soon! 💪",
                $"Take care, {userName}! Thanks for spending time with KANYA SHIELD. Stay secure and be well! 🛡️"
            };

            Random random = new Random();
            return goodbyeMessages[random.Next(goodbyeMessages.Length)] + "\n\n[App will close in a moment...]";
        }

        // ================= STRESS MANAGEMENT =================
        private bool DetectStress(string lowerInput)
        {
            string[] stressKeywords = new string[]
            {
                "stress", "stressed", "overwhelm", "overwhelmed",
                "pressure", "anxious", "anxiety", "panic",
                "burned out", "burnout", "exhausted", "tired",
                "workload", "deadline", "deadline stress", "busy",
                "can't handle", "too much", "falling apart",
                "help me", "i need help", "can't cope", "struggling",
                "difficult time", "hard time", "tough time",
                "worried", "worry", "scared", "afraid",
                "tension", "tense", "anxious", "nervous"
            };

            foreach (var keyword in stressKeywords)
            {
                if (lowerInput.Contains(keyword))
                {
                    return true;
                }
            }
            return false;
        }

        private string GetStressSupport(string input, Sentiment sentiment)
        {
            List<string> supportive = new List<string>();

            // Empathetic opening based on sentiment
            if (sentiment == Sentiment.Frustrated || sentiment == Sentiment.Worried)
            {
                supportive.Add($"I hear you, {_memory.UserName}. Stress can feel overwhelming. That's completely normal, and I'm here to help.");
                supportive.Add("Stress is real, and acknowledging it is the first step. Let's work through this together.");
                supportive.Add($"{_memory.UserName}, it sounds like you're carrying a lot right now. You're not alone in feeling this way.");
            }
            else
            {
                supportive.Add($"I understand, {_memory.UserName}. Stress from daily pressures is common. Let's work on managing it together.");
                supportive.Add($"{_memory.UserName}, I'm here to support you. Tell me more about what's stressing you.");
                supportive.Add("Taking time to address stress shows strength. I'm here to help you find ways to cope.");
            }

            // Check for specific stress-related keywords and provide targeted advice
            string lower = input.ToLower();

            if (lower.Contains("work") || lower.Contains("job") || lower.Contains("deadline"))
            {
                supportive.Add("Work stress is common. Try breaking tasks into smaller steps, taking short breaks, and celebrating small wins.");
            }

            if (lower.Contains("sleep") || lower.Contains("can't sleep") || lower.Contains("insomnia"))
            {
                supportive.Add("Sleep is crucial for stress management. Try: avoiding screens 30 min before bed, keeping a consistent sleep schedule, and a calming bedtime routine.");
            }

            if (lower.Contains("relationship") || lower.Contains("friend") || lower.Contains("family"))
            {
                supportive.Add("Relationships can be stressful. Consider: open communication, setting boundaries, and reaching out to trusted people.");
            }

            if (lower.Contains("money") || lower.Contains("financial") || lower.Contains("bills"))
            {
                supportive.Add("Financial stress is tough. Consider: creating a budget, seeking financial advice, and taking it one step at a time.");
            }

            if (lower.Contains("health") || lower.Contains("sick") || lower.Contains("illness"))
            {
                supportive.Add("Health concerns are stressful. Remember to consult healthcare professionals and be patient with yourself during recovery.");
            }

            // Offer coping strategies
            if (lower.Contains("breath") || lower.Contains("breathing") || lower.Contains("exercise"))
            {
                supportive.Add(GetBreathingExercise());
            }

            if (lower.Contains("ground") || lower.Contains("grounding") || lower.Contains("5 senses"))
            {
                supportive.Add(GetGroundingExercise());
            }

            if (lower.Contains("muscle") || lower.Contains("relax") || lower.Contains("tension"))
            {
                supportive.Add(GetProgressiveMuscleRelaxation());
            }

            if (lower.Contains("tips") || lower.Contains("help") || lower.Contains("what can i do"))
            {
                supportive.Add(GetStressReliefTips());
            }

            // Add general stress relief prompt
            if (supportive.Count == 1)
            {
                supportive.Add("Would you like: breathing exercises, grounding techniques, muscle relaxation, coping tips, or just to talk about it?");
            }

            Random rand = new Random();
            string chosen = supportive[rand.Next(supportive.Count)];
            _stressHistory.Add(input);

            return chosen;
        }

        private string SummarizeStressHistory()
        {
            if (_stressHistory.Count == 0)
                return "You shared some important concerns.";

            // Summarize last concern
            string lastConcern = _stressHistory.LastOrDefault();
            if (!string.IsNullOrWhiteSpace(lastConcern))
            {
                return $"You talked about: '{lastConcern}'. I hope you found this supportive.";
            }
            return "I'm glad we talked about what's stressing you.";
        }

        // ================= STRESS RELIEF EXERCISES =================
        private string GetProgressiveMuscleRelaxation()
        {
            return "Progressive Muscle Relaxation (PMR): Starting with your toes, tense each muscle group for 5 seconds, then release. Work up: feet → legs → stomach → chest → arms → shoulders → neck → face. Do this slowly. Notice the difference between tension and relaxation. Takes 10-15 minutes.";
        }

        private string GetStressReliefTips()
        {
            return "Daily Stress Relief Tips:\n" +
                   "1) Exercise: Even a 10-minute walk reduces stress significantly.\n" +
                   "2) Hydration & Nutrition: Eat regular meals, drink water. Avoid excess caffeine.\n" +
                   "3) Sleep: 7-9 hours nightly. Consistent sleep schedule helps.\n" +
                   "4) Social Connection: Talk to friends, family, or a counselor.\n" +
                   "5) Hobbies: Do something you enjoy daily, even for 15 minutes.\n" +
                   "6) Meditation: Even 5 minutes daily can reduce anxiety.\n" +
                   "7) Limit News: Don't doom-scroll. Check news at set times.\n" +
                   "8) Set Boundaries: Learn to say 'no' to protect your time.";
        }

        private string GetStressManagementPlan()
        {
            return "Simple Stress Management Plan:\n" +
                   "• Identify: What's the main stressor? Be specific.\n" +
                   "• Prioritize: What can you control? What can't you? Focus on what you can.\n" +
                   "• Plan: Break the problem into small, manageable steps.\n" +
                   "• Support: Reach out to trusted friends, family, or professionals.\n" +
                   "• Self-care: Schedule time for activities that help you relax.\n" +
                   "• Review: Check progress weekly and adjust your plan as needed.";
        }
    }
}

