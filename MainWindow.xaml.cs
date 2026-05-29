using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
// sellwane khampepe st10485259 - Prog6221 Part 2 POE Submission

namespace PrincessCyberGuide
{
    // ============================================================
    //  AUDIO CLASS
    // ============================================================
    public class Audio
    {
        public void GreetingVoice()
        {
            try { new SoundPlayer("CHATSOUND.wav").PlaySync(); }
            catch { }
        }
    }

    // ============================================================
    //  CHATBOT CLASS
    // ============================================================
    public class ChatBot
    {
        private Random _rng = new Random();
        private string _userFullName = "";
        private List<string> _userInterests = new List<string>();        // List
        private string _lastTopic = "";

        // Dictionary: Keyword → Topic
        private Dictionary<string, string> _keywordToTopic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"password", "password"}, {"passwd", "password"}, {"2fa", "password"},
            {"phishing", "phishing"}, {"phish", "phishing"}, {"fake email", "phishing"},
            {"scam", "scam"}, {"fraud", "scam"},
            {"attack", "attack"}, {"hack", "attack"}, {"ransomware", "attack"}, {"malware", "attack"},
            {"browsing", "browsing"}, {"safe browsing", "browsing"}, {"https", "browsing"},
            {"privacy", "privacy"}, {"private", "privacy"},
            {"vpn", "vpn"}, {"firewall", "firewall"}, {"encryption", "encryption"},
            {"antivirus", "antivirus"}, {"virus", "antivirus"}, {"breach", "breach"}
        };

        // Dictionary of Arrays: Topic → Responses
        private Dictionary<string, string[]> _responses = new Dictionary<string, string[]>();

        public ChatBot()
        {
            // Arrays of responses for each topic
            _responses["password"] = new[]
            {
                "🔐 Use strong unique passwords with at least 12 characters.",
                "🔐 Never reuse the same password on different websites.",
                "🔐 Enable Two-Factor Authentication (2FA) on all accounts."
            };

            _responses["phishing"] = new[]
            {
                "🎣 Always hover over links before clicking them.",
                "🎣 Real companies never ask for your password via email.",
                "🎣 Be careful with suspicious or urgent messages."
            };

            _responses["scam"] = new[]
            {
                "⚠️ If it sounds too good to be true, it probably is.",
                "⚠️ Never send money to someone you met online."
            };

            _responses["attack"] = new[]
            {
                "🛡️ Keep regular backups using the 3-2-1 rule.",
                "🛡️ Use a VPN on public Wi-Fi."
            };

            _responses["browsing"] = new[]
            {
                "🌐 Always check for the HTTPS padlock icon.",
                "🌐 Use an ad-blocker like uBlock Origin."
            };

            _responses["privacy"] = new[]
            {
                "🔒 Review privacy settings on your social media accounts.",
                "🔒 Be careful about app permissions."
            };

            _responses["vpn"] = new[] { "🛡️ A VPN protects your data on public networks." };
            _responses["antivirus"] = new[] { "🛡️ Keep your antivirus updated to detect threats." };
        }

        private string DetectTopic(string message)
        {
            foreach (var pair in _keywordToTopic)
            {
                if (message.ToLower().Contains(pair.Key))
                    return pair.Value;
            }
            return "";
        }

        if (msg.Contains("data breach") || msg.Contains("breach")) return "breach";
        private void AddInterest(string topic)
        {
            string interest = topic switch
            {
                "password" => "Password Safety",
                "phishing" => "Phishing",
                "scam" => "Scams",
                "attack" => "Cyber Attacks",
                "browsing" => "Safe Browsing",
                "privacy" => "Privacy",
                "vpn" => "VPN",
                "antivirus" => "Antivirus",
                _ => topic
            };

            if (!string.IsNullOrEmpty(interest) && !_userInterests.Contains(interest))
                _userInterests.Add(interest);
        }

        private string GetRandomResponse(string topic)
        {
            if (!_responses.ContainsKey(topic))
                return "Let me share a helpful tip...";

            var answers = _responses[topic];
            return answers[_rng.Next(answers.Length)] + "\n\n💡 Type 'tell me more' for another tip.";
        }

        public string Bot(string message)
        {
            message = message.ToLower().Trim();

            if (message == "exit" || message == "end")
                return "EXIT";

            // Name Collection
            if (string.IsNullOrEmpty(_userFullName))
            {
                if (message.Contains("hi") || message.Contains("hello"))
                    return "👑 Hello! I'm Princess CyberGuide.\n\nHow are you today?";

                if (message.Contains("good") || message.Contains("fine") || message.Contains("great"))
                    return "✨ Glad to hear!\n\nMay I know your full name?";

                var words = message.Split(' ');
                if (words.Length >= 2)
                {
                    _userFullName = char.ToUpper(words[0][0]) + words[0].Substring(1).ToLower() + " " +
                                   char.ToUpper(words[1][0]) + words[1].Substring(1).ToLower();

                    return $"👑 Nice to meet you, **{_userFullName}**!\n\n" +
                           "You can ask me about: passwords, phishing, scams, attacks, safe browsing, privacy, VPN, antivirus, and more.\n\nWhat would you like to know?";
                }
            }

            // Follow-up
            if (message.Contains("tell me more") || message.Contains("another tip"))
            {
                return string.IsNullOrEmpty(_lastTopic)
                    ? "Sure! What topic would you like more info on?"
                    : GetRandomResponse(_lastTopic);
            }

            // Memory Recall
            if (message.Contains("recall") || message.Contains("memory"))
                return GetMemorySummary();

            // Main Topic Detection
            string topic = DetectTopic(message);
            if (!string.IsNullOrEmpty(topic))
            {
                _lastTopic = topic;
                AddInterest(topic);
                return GetRandomResponse(topic);
            }


          // Error Handling - Requirement met
        return "🤷 I am not sure I understand. Please try to rephrase your question.\n\n" +
       "You can ask me about passwords, phishing, scams, attacks, privacy, VPN, etc.";
        }

        private string GetMemorySummary()
        {
            return $"🧠 Memory for {_userFullName}\n\n" +
                   "Interests: " + (_userInterests.Count > 0 ? string.Join(", ", _userInterests) : "None yet");
        }
    }

    // ============================================================
    //  MAIN WINDOW
    // ============================================================
    public partial class MainWindow : Window
    {
        private readonly ChatBot _bot = new ChatBot();
        private readonly Audio _audio = new Audio();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _audio.GreetingVoice();
            AddAsciiArt();
          AddMessage("Bot", "👑 Welcome to the Royal Cybersecurity Court!\n\nI am Princess CyberGuide.\n\nHow are you today? Feel free to ask me anything about cybersecurity!", Colors.Magenta);
        }

        private void AddAsciiArt()
        {
            // Your ASCII art here...
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) => SendUserMessage();
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendUserMessage();
        }

        private void SendUserMessage()
        {
            string input = InputBox.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AddMessage("You", input, Colors.Cyan);

            string response = _bot.Bot(input);

            if (response == "EXIT")
            {
                AddMessage("Bot", "Thank you for chatting! Stay safe online. 👑", Colors.Magenta);
                MessageBox.Show("Thank you for using Princess CyberGuide!", "Goodbye", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }
            else
            {
                AddMessage("Bot", response, Colors.Magenta);
            }

            InputBox.Clear();
            ChatScrollViewer.ScrollToEnd();
        }

        private void AddMessage(string sender, string message, Color color)
        {
            var border = new Border
            {
                Background = sender == "You"
                    ? new SolidColorBrush(Color.FromArgb(80, 0, 180, 255))
                    : new SolidColorBrush(Color.FromArgb(60, 192, 38, 211)),
                CornerRadius = new CornerRadius(18),
                Margin = new Thickness(8, 10, 8, 10),
                Padding = new Thickness(15),
                HorizontalAlignment = sender == "You" ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 700
            };

            border.Child = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 15.5,
                Foreground = new SolidColorBrush(color)
            };

            ChatPanel.Children.Add(border);
        }
    }
}
