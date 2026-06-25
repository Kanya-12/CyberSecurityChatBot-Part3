using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CyberSecurityChatBot
{
    public partial class MainWindow : Window
    {
        private ChatBot _chatBot;
        private HelpResources _helpResources;
        private MemoryManager _memoryManager;
        private GameManager _gameManager;
        private List<string> _quickTabQuestions;
        private bool _isTyping = false;
        private bool _awaitingContinuationResponse = false;
        private bool _isGoodbyeTriggered = false;

        public MainWindow()
        {
            InitializeComponent();
            _chatBot = new ChatBot();
            _helpResources = new HelpResources();
            _memoryManager = new MemoryManager();
            _gameManager = new GameManager();

            // Initialize quick tab questions
            InitializeQuickTabs();

            // Hide venting panel initially
            VentingActionsPanel.Visibility = Visibility.Collapsed;

            // Check for returning user
            HandleReturningUser();

            // Update venting UI state
            UpdateVentingUI();
        }

        /// <summary>
        /// Handle returning user logic
        /// </summary>
        private void HandleReturningUser()
        {
            if (_memoryManager.IsReturningUser && !string.IsNullOrWhiteSpace(_memoryManager.Profile.UserName))
            {
                // Returning user
                string welcomeBack = $"Welcome back, {_memoryManager.Profile.UserName}! 👋\n\n" +
                                    $"Last visit: {_memoryManager.Profile.LastVisit:MMM dd, yyyy 'at' HH:mm}\n" +
                                    $"Total visits: {_memoryManager.Profile.TotalVisits}\n\n";

                AppendBotMessage(welcomeBack);

                // Check if there's conversation history
                var previousConversations = _memoryManager.GetPreviousConversations(5);
                if (previousConversations.Count > 0)
                {
                    string continuationOffer = _memoryManager.GetConversationSummary(3) + "\n\n" +
                        "Would you like to continue where you left off? (Type 'yes' or 'no')";
                    AppendBotMessage(continuationOffer);
                    _awaitingContinuationResponse = true;
                }

                // Update visit tracking
                _memoryManager.UpdateVisitTracking();
            }
            else
            {
                // First-time user or new profile
                AppendBotMessage(_chatBot.GetGreeting());
            }
        }

        /// <summary>
        /// Initialize quick tab buttons with common questions
        /// </summary>
        private void InitializeQuickTabs()
        {
            _quickTabQuestions = new List<string>
            {
                "What is Password Security?",
                "What is Phishing?",
                "What is Two-Factor Authentication?",
                "How to Report a Scam",
                "What is Privacy?",
                "What is Malware?",
                "I need to vent",
                "I'm stressed",
                "Help with stress",
                "Coping strategies",
                "Play Quiz Game",
                "Play Trivia Game",
                "Play Hangman Game"
            };

            // Create buttons for quick tabs
            foreach (string question in _quickTabQuestions)
            {
                Button quickTab = new Button
                {
                    Content = question,
                    Margin = new Thickness(5),
                    Padding = new Thickness(12, 8, 12, 8),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E2E4F")),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E5FF")),
                    FontWeight = FontWeights.Normal,
                    FontSize = 11,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E5FF"))
                };

                quickTab.Tag = question;
                quickTab.Click += QuickTab_Click;
                quickTab.MouseEnter += (s, e) => 
                {
                    (s as Button).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E5FF"));
                    (s as Button).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B1020"));
                };
                quickTab.MouseLeave += (s, e) => 
                {
                    (s as Button).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E2E4F"));
                    (s as Button).Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E5FF"));
                };

                QuickTabsPanel.Children.Add(quickTab);
            }
        }

        /// <summary>
        /// Handle quick tab button clicks
        /// </summary>
        private void QuickTab_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag is string question)
            {
                // Check if this is a game request
                if (question.Contains("Game"))
                {
                    HandleGameRequest(question);
                }
                else
                {
                    UserInput.Text = question;
                    SendMessage();
                }
            }
        }

        /// <summary>
        /// Handle game requests
        /// </summary>
        private void HandleGameRequest(string gameRequest)
        {
            string gameName = gameRequest.Replace("Play ", "").Replace(" Game", "");
            string gameStart = _gameManager.StartGame(gameName);
            AppendBotMessage(gameStart);
        }

        // Sidebar quick buttons handler
        private void SidebarQuick_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.Content is string text)
            {
                UserInput.Text = text;
                SendMessage();
            }
        }

        /// <summary>
        /// Handle Help Resources button click
        /// </summary>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            ShowHelpWindow();
        }

        /// <summary>
        /// Display help resources in a separate window
        /// </summary>
        private void ShowHelpWindow()
        {
            HelpWindow helpWindow = new HelpWindow(_helpResources);
            helpWindow.Owner = this;
            helpWindow.Show();
        }

        // Event handler for Send button click
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
            // Update venting UI after processing input
            UpdateVentingUI();
        }

        // Event handler for Enter key in TextBox
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        // Main method to process and send user message
        private async void SendMessage()
        {
            string userInput = UserInput.Text.Trim();

            // If input is empty, don't process
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return;
            }

            // Display user message as a card
            AppendUserMessage(userInput);

            // Clear the input box
            UserInput.Clear();
            UserInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
            UserInput.Text = "Type your message here...";

            // Show typing indicator with animation
            ShowTypingIndicator();

            // Non-blocking thinking delay for realism
            await System.Threading.Tasks.Task.Delay(800);

            string botResponse;

            // Check if awaiting conversation continuation response
            if (_awaitingContinuationResponse)
            {
                _awaitingContinuationResponse = false;
                if (userInput.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    botResponse = "Great! Here's a summary of your last 3 questions:\n\n" + 
                                 _memoryManager.GetConversationSummary(3) + 
                                 "\n\nWhat would you like to know today?";
                }
                else if (userInput.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    botResponse = "No problem! Let's start fresh. What would you like to learn about?";
                }
                else
                {
                    botResponse = "Please answer 'yes' or 'no' to continue your previous conversation.";
                }
            }
            // Check if currently in a game
            else if (!string.IsNullOrWhiteSpace(_gameManager.GetGameStatus()) && !_gameManager.GetGameStatus().Contains("No active game"))
            {
                // Process game input
                botResponse = _gameManager.ProcessGameInput(userInput);

                // Check if game is complete
                if (_gameManager.IsGameComplete())
                {
                    int finalScore = _gameManager.GetCurrentScore();
                    _memoryManager.UpdateGameScore(_gameManager.GetGameStatus().Split(':')[0], finalScore);
                    botResponse += "\n\nGame stats saved! Want to play another game or learn more? (Try 'Play Quiz Game', 'Play Trivia Game', 'Play Hangman Game')";
                }
            }
            // Check if user wants to see games
            else if (userInput.Equals("games", StringComparison.OrdinalIgnoreCase) || userInput.Contains("play game"))
            {
                var games = _gameManager.GetAvailableGames();
                botResponse = "Available games:\n\n" + string.Join("\n", games.Select((g, i) => $"{i + 1}. {g}")) + 
                             "\n\nYou can click the game buttons above or type the game name!";
            }
            // Check if user wants to see stats
            else if (userInput.Equals("stats", StringComparison.OrdinalIgnoreCase) || userInput.Contains("my stats"))
            {
                botResponse = $"Your Stats:\n\n" +
                             $"Name: {_memoryManager.Profile.UserName}\n" +
                             $"Total Visits: {_memoryManager.Profile.TotalVisits}\n" +
                             $"First Visit: {_memoryManager.Profile.FirstVisit:MMM dd, yyyy}\n" +
                             $"Last Visit: {_memoryManager.Profile.LastVisit:MMM dd, yyyy}\n" +
                             $"Total Games Played: {_memoryManager.Profile.TotalGamesPlayed}\n" +
                             $"Total Conversations: {_memoryManager.Profile.Conversations.Count}";
            }
            else
            {
                // Process input with ChatBot on background thread to keep UI responsive
                botResponse = await System.Threading.Tasks.Task.Run(() => _chatBot.ProcessInput(userInput));
            }

            // Hide typing indicator
            HideTypingIndicator();

            // Persist user message to MemoryManager
            _memoryManager.AddConversationEntry(new ConversationEntry
            {
                Role = "user",
                Message = userInput,
                Timestamp = DateTime.UtcNow
            });

            // Display bot response as a card
            AppendBotMessage(botResponse);

            // Persist bot response to MemoryManager
            _memoryManager.AddConversationEntry(new ConversationEntry
            {
                Role = "bot",
                Message = botResponse,
                Timestamp = DateTime.UtcNow
            });

            // Save to persistent storage
            _memoryManager.Save();

            // Update venting UI state (in case mode changed)
            UpdateVentingUI();

            // Check for goodbye and schedule app closure
            if (botResponse.Contains("[App will close in a moment...]"))
            {
                _isGoodbyeTriggered = true;
                // Schedule app closure after 2 seconds to let user see the goodbye message
                Dispatcher.BeginInvoke(new System.Action(() =>
                {
                    Application.Current.Shutdown();
                }), System.Windows.Threading.DispatcherPriority.Normal, 
                new object[] { });

                // Set a timer to close the app after 2 seconds
                System.Threading.Timer closeTimer = new System.Threading.Timer(
                    (state) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Application.Current.Shutdown();
                        });
                    },
                    null,
                    TimeSpan.FromSeconds(2),
                    Timeout.InfiniteTimeSpan
                );
            }

            // Scroll to bottom
            ScrollToBottom();
        }

        /// <summary>
        /// Show typing indicator while AI is processing
        /// </summary>
        private void ShowTypingIndicator()
        {
            // Create a typing bubble card
            Border typingBubble = CreateTypingBubble();
            typingBubble.Name = "TypingBubble";
            ChatMessagesPanel.Children.Add(typingBubble);
            ScrollToBottom();
        }

        /// <summary>
        /// Hide the typing indicator
        /// </summary>
        private void HideTypingIndicator()
        {
            // Remove typing bubble if present
            for (int i = ChatMessagesPanel.Children.Count - 1; i >= 0; i--)
            {
                var child = ChatMessagesPanel.Children[i];
                if (child is Border br && br.Name == "TypingBubble")
                {
                    ChatMessagesPanel.Children.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Create a typing bubble with animated dots
        /// </summary>
        private Border CreateTypingBubble()
        {
            Border bubble = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#141D33")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E2E4F")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(15),
                Margin = new Thickness(10, 10, 150, 10),
                MaxWidth = 400,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            StackPanel panel = new StackPanel { Orientation = Orientation.Horizontal };
            panel.Margin = new Thickness(0,0,0,0);
            // We'll use small margins on children instead of Spacing property.

            // Create three animated dots with scale animation
            for (int i = 0; i < 3; i++)
            {
                Ellipse dot = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FF66")),
                    Opacity = 0.9,
                    RenderTransform = new ScaleTransform(1,1)
                };

                // Scale animation to create a pulsing effect
                DoubleAnimation scaleAnim = new DoubleAnimation
                {
                    From = 0.6,
                    To = 1.2,
                    Duration = TimeSpan.FromSeconds(0.6),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true,
                    BeginTime = TimeSpan.FromSeconds(i * 0.2)
                };

                ScaleTransform transform = new ScaleTransform(1,1);
                dot.RenderTransform = transform;
                transform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
                transform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);

                dot.Margin = new Thickness(0,0,5,0);
                panel.Children.Add(dot);
            }

            TextBlock text = new TextBlock
            {
                Text = " AI is thinking...",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0A0A0")),
                FontSize = 11,
                VerticalAlignment = VerticalAlignment.Center,
                FontStyle = FontStyles.Italic
            };
            panel.Children.Add(text);

            bubble.Child = panel;
            return bubble;
        }

        /// <summary>
        /// Append user message as a modern card
        /// </summary>
        private void AppendUserMessage(string message)
        {
            // Create message card container
            Border messageCard = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E5FF")),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(15),
                Margin = new Thickness(10, 10, 50, 10),
                MaxWidth = 500,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            TextBlock messageText = new TextBlock
            {
                Text = message,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0B1020")),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.Normal
            };

            messageCard.Child = messageText;
            ChatMessagesPanel.Children.Add(messageCard);
        }

        /// <summary>
        /// Append bot message as a modern card with avatar
        /// </summary>
        private void AppendBotMessage(string message)
        {
            // Create a container for bot avatar + message
            Grid messageContainer = new Grid();
            messageContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
            messageContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            messageContainer.Margin = new Thickness(10, 10, 150, 10);

            // Bot avatar
            Ellipse avatar = new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E5FF")),
                Opacity = 0.3,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(avatar, 0);
            messageContainer.Children.Add(avatar);

            // Message bubble
            Border messageBubble = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#141D33")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E2E4F")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(15),
                Margin = new Thickness(10, 0, 0, 0)
            };

            TextBlock messageText = new TextBlock
            {
                Text = message,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20
            };

            messageBubble.Child = messageText;
            Grid.SetColumn(messageBubble, 1);
            messageContainer.Children.Add(messageBubble);

            ChatMessagesPanel.Children.Add(messageContainer);

            // Use text-to-speech to read the bot's response aloud
            AudioHelper.Speak(message);
        }

        /// <summary>
        /// Scroll chat to the bottom
        /// </summary>
        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToEnd();
        }

        /// <summary>
        /// Update venting indicator and quick action buttons based on ChatBot state
        /// </summary>
        private void UpdateVentingUI()
        {
            if (_chatBot.IsInStressMode)
            {
                VentingStatusText.Text = "Stress Management Mode: I'm here to support you";
                VentingActionsPanel.Visibility = Visibility.Visible;
            }
            else if (_chatBot.IsInVentingMode)
            {
                VentingStatusText.Text = "Venting mode: I'm here to listen";
                VentingActionsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                VentingStatusText.Text = "";
                VentingActionsPanel.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Handle TextBox focus to clear placeholder
        /// </summary>
        private void UserInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UserInput.Text == "Type your message here...")
            {
                UserInput.Text = "";
                UserInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            }
        }

        /// <summary>
        /// Handle TextBox lose focus to show placeholder
        /// </summary>
        private void UserInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserInput.Text))
            {
                UserInput.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666"));
                UserInput.Text = "Type your message here...";
            }
        }

        // Quick action buttons for venting
        private void BreathingButton_Click(object sender, RoutedEventArgs e)
        {
            // Call ChatBot by simulating user input "breathing"
            string response = _chatBot.ProcessInput("breathing");
            AppendBotMessage(response);
            UpdateVentingUI();
            ScrollToBottom();
        }

        private void GroundingButton_Click(object sender, RoutedEventArgs e)
        {
            string response = _chatBot.ProcessInput("grounding");
            AppendBotMessage(response);
            UpdateVentingUI();
            ScrollToBottom();
        }

        private void StopVentingButton_Click(object sender, RoutedEventArgs e)
        {
            string response = _chatBot.ProcessInput("stop venting");
            AppendBotMessage(response);
            UpdateVentingUI();
            ScrollToBottom();
        }
    }
}
