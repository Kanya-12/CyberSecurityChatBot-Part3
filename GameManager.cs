using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatBot
{
    /// <summary>
    /// Central manager for all games in the chatbot
    /// </summary>
    public class GameManager
    {
        private Random _random = new Random();
        private string _currentGameType = null;
        private int _currentScore = 0;
        private CybersecurityQuiz _quizGame;
        private CybersecurityTrivia _triviaGame;
        private HangmanGame _hangmanGame;

        public GameManager()
        {
            _quizGame = new CybersecurityQuiz();
            _triviaGame = new CybersecurityTrivia();
            _hangmanGame = new HangmanGame();
        }

        /// <summary>
        /// Get list of available games
        /// </summary>
        public List<string> GetAvailableGames()
        {
            return new List<string>
            {
                "Quiz",
                "Trivia",
                "Hangman",
                "Password Strength Checker"
            };
        }

        /// <summary>
        /// Start a specific game
        /// </summary>
        public string StartGame(string gameName)
        {
            _currentGameType = gameName.ToLower();
            _currentScore = 0;

            return gameName.ToLower() switch
            {
                "quiz" => _quizGame.StartQuiz(),
                "trivia" => _triviaGame.StartTrivia(),
                "hangman" => _hangmanGame.StartHangman(),
                "password strength checker" => StartPasswordStrengthChecker(),
                _ => "Unknown game. Available games: Quiz, Trivia, Hangman, Password Strength Checker"
            };
        }

        /// <summary>
        /// Process game input/answer
        /// </summary>
        public string ProcessGameInput(string input)
        {
            if (string.IsNullOrWhiteSpace(_currentGameType))
                return "No game in progress. Start a game first!";

            return _currentGameType switch
            {
                "quiz" => _quizGame.SubmitAnswer(input),
                "trivia" => _triviaGame.SubmitAnswer(input),
                "hangman" => _hangmanGame.GuessLetter(input),
                "password strength checker" => CheckPasswordStrength(input),
                _ => "Unknown game state"
            };
        }

        /// <summary>
        /// Get current game score
        /// </summary>
        public int GetCurrentScore()
        {
            return _currentGameType switch
            {
                "quiz" => _quizGame.GetScore(),
                "trivia" => _triviaGame.GetScore(),
                "hangman" => _hangmanGame.GetScore(),
                _ => 0
            };
        }

        /// <summary>
        /// Get current game status
        /// </summary>
        public string GetGameStatus()
        {
            return _currentGameType switch
            {
                "quiz" => _quizGame.GetStatus(),
                "trivia" => _triviaGame.GetStatus(),
                "hangman" => _hangmanGame.GetStatus(),
                _ => "No active game"
            };
        }

        /// <summary>
        /// Check if game is complete
        /// </summary>
        public bool IsGameComplete()
        {
            return _currentGameType switch
            {
                "quiz" => _quizGame.IsComplete,
                "trivia" => _triviaGame.IsComplete,
                "hangman" => _hangmanGame.IsGameOver,
                _ => false
            };
        }

        /// <summary>
        /// End current game
        /// </summary>
        public string EndGame()
        {
            if (string.IsNullOrWhiteSpace(_currentGameType))
                return "No game in progress";

            string result = $"Game ended. Final Score: {GetCurrentScore()}";
            _currentGameType = null;
            _currentScore = 0;
            return result;
        }

        /// <summary>
        /// Simple password strength checker game
        /// </summary>
        private string StartPasswordStrengthChecker()
        {
            return "Welcome to Password Strength Checker!\n\n" +
                   "I'll analyze the strength of passwords based on:\n" +
                   "- Length (8+ characters = 1 point, 12+ = 2 points)\n" +
                   "- Uppercase letters (1 point)\n" +
                   "- Lowercase letters (1 point)\n" +
                   "- Numbers (1 point)\n" +
                   "- Special characters (1 point)\n\n" +
                   "Type a password to check its strength (type 'quit' to exit)";
        }

        /// <summary>
        /// Check password strength
        /// </summary>
        private string CheckPasswordStrength(string password)
        {
            if (password.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                _currentGameType = null;
                return "Thanks for playing Password Strength Checker!";
            }

            int strength = 0;
            var feedback = new List<string>();

            // Length check
            if (password.Length >= 8) { strength++; feedback.Add("✓ Good length (8+)"); }
            if (password.Length >= 12) { strength++; feedback.Add("✓ Excellent length (12+)"); }
            else if (password.Length < 8) { feedback.Add("✗ Too short (less than 8 characters)"); }

            // Character type checks
            if (password.Any(char.IsUpper)) { strength++; feedback.Add("✓ Contains uppercase letters"); }
            else { feedback.Add("✗ No uppercase letters"); }

            if (password.Any(char.IsLower)) { strength++; feedback.Add("✓ Contains lowercase letters"); }
            else { feedback.Add("✗ No lowercase letters"); }

            if (password.Any(char.IsDigit)) { strength++; feedback.Add("✓ Contains numbers"); }
            else { feedback.Add("✗ No numbers"); }

            if (password.Any(c => !char.IsLetterOrDigit(c))) { strength++; feedback.Add("✓ Contains special characters"); }
            else { feedback.Add("✗ No special characters"); }

            string strengthLevel = strength switch
            {
                <= 2 => "🔴 Weak",
                <= 4 => "🟡 Moderate",
                _ => "🟢 Strong"
            };

            return $"{strengthLevel}\n\nAnalysis:\n" + string.Join("\n", feedback) + "\n\nScore: {strength}/7\n\nCheck another password (or type 'quit' to exit)";
        }
    }

    /// <summary>
    /// Cybersecurity Quiz Game
    /// </summary>
    public class CybersecurityQuiz
    {
        private List<QuizQuestion> _questions;
        private int _currentQuestionIndex = 0;
        private int _score = 0;
        public bool IsComplete { get; private set; } = false;

        public class QuizQuestion
        {
            public string Question { get; set; }
            public List<string> Options { get; set; }
            public int CorrectAnswerIndex { get; set; }
        }

        public CybersecurityQuiz()
        {
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            _questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What is the most important factor in password security?",
                    Options = new List<string>
                    {
                        "a) Length and complexity",
                        "b) Using your birthday",
                        "c) Reusing passwords",
                        "d) Writing it down"
                    },
                    CorrectAnswerIndex = 0
                },
                new QuizQuestion
                {
                    Question = "What does 2FA stand for?",
                    Options = new List<string>
                    {
                        "a) Two-Factor Authentication",
                        "b) Two-File Access",
                        "c) Two-Form Authority",
                        "d) Two-Firewall Algorithm"
                    },
                    CorrectAnswerIndex = 0
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string>
                    {
                        "a) A water sport",
                        "b) Social engineering attack to steal credentials",
                        "c) A type of computer virus",
                        "d) A firewall setting"
                    },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "What is ransomware?",
                    Options = new List<string>
                    {
                        "a) A type of antivirus",
                        "b) Malware that encrypts files and demands payment",
                        "c) A network security protocol",
                        "d) A type of backup system"
                    },
                    CorrectAnswerIndex = 1
                },
                new QuizQuestion
                {
                    Question = "What should you do if you suspect identity theft?",
                    Options = new List<string>
                    {
                        "a) Do nothing",
                        "b) Contact your bank and place fraud alerts",
                        "c) Change your password tomorrow",
                        "d) Close all social media accounts"
                    },
                    CorrectAnswerIndex = 1
                }
            };
        }

        public string StartQuiz()
        {
            _currentQuestionIndex = 0;
            _score = 0;
            IsComplete = false;
            return GetCurrentQuestion();
        }

        private string GetCurrentQuestion()
        {
            if (_currentQuestionIndex >= _questions.Count)
            {
                IsComplete = true;
                return $"Quiz Complete! Your Score: {_score}/{_questions.Count}";
            }

            var question = _questions[_currentQuestionIndex];
            return $"Question {_currentQuestionIndex + 1}/{_questions.Count}\n\n{question.Question}\n\n" +
                   string.Join("\n", question.Options) + "\n\nType a, b, c, or d to answer";
        }

        public string SubmitAnswer(string answer)
        {
            if (IsComplete)
                return "Quiz is complete. Start a new game to play again!";

            var question = _questions[_currentQuestionIndex];
            int answerIndex = answer.ToLower() switch
            {
                "a" => 0,
                "b" => 1,
                "c" => 2,
                "d" => 3,
                _ => -1
            };

            if (answerIndex == -1)
                return "Invalid answer. Please type a, b, c, or d";

            bool isCorrect = answerIndex == question.CorrectAnswerIndex;
            if (isCorrect)
            {
                _score++;
                var response = "✓ Correct!\n\n";
                _currentQuestionIndex++;
                response += GetCurrentQuestion();
                return response;
            }
            else
            {
                var response = $"✗ Incorrect. The correct answer is {(char)('a' + question.CorrectAnswerIndex)}\n\n";
                _currentQuestionIndex++;
                response += GetCurrentQuestion();
                return response;
            }
        }

        public int GetScore() => _score;
        public string GetStatus() => $"Quiz Progress: {_currentQuestionIndex}/{_questions.Count} | Score: {_score}";
    }

    /// <summary>
    /// Cybersecurity Trivia Game
    /// </summary>
    public class CybersecurityTrivia
    {
        private List<string> _trivia;
        private int _questionIndex = 0;
        private int _score = 0;
        public bool IsComplete { get; private set; } = false;
        private Random _random = new Random();

        public CybersecurityTrivia()
        {
            InitializeTrivia();
        }

        private void InitializeTrivia()
        {
            _trivia = new List<string>
            {
                "Did you know? A strong password should be at least 12 characters long!",
                "Fact: Enabling 2FA on important accounts reduces account compromise risk by 99%!",
                "Tip: Public WiFi networks are not secure for sensitive transactions!",
                "Alert: Malware can hide in email attachments, even from trusted senders!",
                "Security: Regular software updates patch critical security vulnerabilities!",
                "Warning: Reusing passwords across sites puts all your accounts at risk!",
                "Tip: HTTPS encrypts your connection to websites - always look for it!",
                "Fact: Social engineering attacks exploit human psychology, not technical vulnerabilities!",
                "Remember: Never click links from unsolicited emails - verify directly!",
                "Security: Biometric authentication (fingerprint, face) provides strong security!",
                "Tip: Keep your operating system and applications updated for security patches!",
                "Alert: VPNs encrypt your internet traffic and hide your IP address!",
                "Fact: Ransomware can encrypt your files and demand money for recovery!",
                "Warning: Your browser stores passwords - use a password manager instead!",
                "Security: Multi-factor authentication combines multiple verification methods!"
            };
        }

        public string StartTrivia()
        {
            _questionIndex = 0;
            _score = 0;
            IsComplete = false;
            return "Cybersecurity Trivia Started!\n\nAnswer 'true' or 'false' to each fact:\n\n" + GetNextTrivia();
        }

        private string GetNextTrivia()
        {
            if (_questionIndex >= 5)
            {
                IsComplete = true;
                return $"Trivia Complete! You learned 5 facts! Score: {_score}";
            }

            int index = _random.Next(_trivia.Count);
            return _trivia[index];
        }

        public string SubmitAnswer(string answer)
        {
            if (IsComplete)
                return "Trivia complete! Start a new game to play again!";

            bool isTrue = answer.Equals("true", StringComparison.OrdinalIgnoreCase);
            _score++;
            _questionIndex++;

            if (_questionIndex >= 5)
            {
                IsComplete = true;
                return $"Great! Fact learned!\n\nTrivia Complete! You learned 5 facts! Final Score: {_score}/5";
            }

            return $"Great! Fact learned!\n\nFact {_questionIndex}/5\n\n{GetNextTrivia()}";
        }

        public int GetScore() => _score;
        public string GetStatus() => $"Trivia Progress: {_questionIndex}/5 | Facts Learned: {_score}";
    }

    /// <summary>
    /// Cybersecurity-themed Hangman Game
    /// </summary>
    public class HangmanGame
    {
        private List<string> _words = new List<string>
        {
            "PHISHING", "RANSOMWARE", "MALWARE", "PASSWORD", "FIREWALL", 
            "ENCRYPTION", "AUTHENTICATION", "VULNERABILITY", "CYBERATTACK", 
            "VPNNETWORK", "TWOFACTOR", "BIOMETRIC", "CYBERSECURITY"
        };

        private string _currentWord;
        private HashSet<char> _guessedLetters = new HashSet<char>();
        private int _wrongGuesses = 0;
        private const int MaxWrongGuesses = 6;
        public bool IsGameOver { get; private set; } = false;
        private bool _isWon = false;

        public string StartHangman()
        {
            var random = new Random();
            _currentWord = _words[random.Next(_words.Count)];
            _guessedLetters.Clear();
            _wrongGuesses = 0;
            IsGameOver = false;
            _isWon = false;

            return "🎮 Hangman - Cybersecurity Edition!\n\n" +
                   "Guess letters to reveal the hidden cybersecurity word!\n\n" +
                   GetGameDisplay() + "\n\nGuess a letter (a-z):";
        }

        public string GuessLetter(string input)
        {
            if (IsGameOver)
                return "Game is over. Start a new game!";

            if (string.IsNullOrWhiteSpace(input) || input.Length != 1)
                return "Please enter a single letter!";

            char letter = char.ToUpper(input[0]);

            if (!char.IsLetter(letter))
                return "Please enter a valid letter!";

            if (_guessedLetters.Contains(letter))
                return $"Letter '{letter}' already guessed! Try another.";

            _guessedLetters.Add(letter);

            if (_currentWord.Contains(letter))
            {
                // Check if won
                if (IsWordGuessed())
                {
                    IsGameOver = true;
                    _isWon = true;
                    return GetGameDisplay() + $"\n\n🎉 You won! The word was: {_currentWord}";
                }
                return GetGameDisplay() + "\n\n✓ Good guess!";
            }
            else
            {
                _wrongGuesses++;
                if (_wrongGuesses >= MaxWrongGuesses)
                {
                    IsGameOver = true;
                    return GetGameDisplay() + $"\n\n💀 Game Over! The word was: {_currentWord}";
                }
                return GetGameDisplay() + "\n\n✗ Wrong guess!";
            }
        }

        private bool IsWordGuessed()
        {
            return _currentWord.All(c => _guessedLetters.Contains(c));
        }

        private string GetGameDisplay()
        {
            string display = $"Wrong Guesses: {_wrongGuesses}/{MaxWrongGuesses}\n\n";
            display += "Word: ";

            foreach (char c in _currentWord)
            {
                if (_guessedLetters.Contains(c))
                    display += c + " ";
                else
                    display += "_ ";
            }

            display += "\n\nGuessed Letters: " + string.Join(", ", _guessedLetters.OrderBy(x => x));
            return display;
        }

        public int GetScore() => _isWon ? 10 - _wrongGuesses : 0;
        public string GetStatus() => $"Hangman: {_wrongGuesses}/{MaxWrongGuesses} wrong | Word Progress: {string.Join(" ", _currentWord.Where(c => _guessedLetters.Contains(c)).Distinct())}";
    }
}
