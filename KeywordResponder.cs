using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatBot
{
    public class KeywordResponder
    {
        private Dictionary<string, List<string>> _responses;
        private Random _random = new Random();

        public KeywordResponder()
        {
            InitializeResponses();
        }

        private void InitializeResponses()
        {
            _responses = new Dictionary<string, List<string>>
            {
                {
                    "password",
                    new List<string>
                    {
                        "Strong passwords are your first line of defense against hackers. Use at least 12 characters with uppercase, lowercase, numbers, and symbols.",
                        "Never reuse passwords across different accounts. Consider using a password manager to keep track securely.",
                        "Change your password immediately if you suspect it's been compromised.",
                        "Avoid using personal information like birthdays or names in your password."
                    }
                },
                {
                    "phishing",
                    new List<string>
                    {
                        "Phishing emails try to trick you into revealing sensitive information. Always verify the sender's email address.",
                        "Never click links in suspicious emails. Instead, navigate to the official website directly.",
                        "Look for spelling errors and suspicious requests for personal information - these are red flags.",
                        "Report phishing emails to your email provider and the organization being impersonated."
                    }
                },
                {
                    "privacy",
                    new List<string>
                    {
                        "Review your privacy settings on social media regularly. Limit what personal information you share publicly.",
                        "Be careful about what you post online - it can be screenshot and shared permanently.",
                        "Use privacy-focused search engines and browsers if you want more anonymity.",
                        "Understand what data companies collect about you and opt out when possible."
                    }
                },
                {
                    "scam",
                    new List<string>
                    {
                        "Common scams include fake lottery winnings, romance scams, and tech support fraud.",
                        "If it sounds too good to be true, it probably is. Be skeptical of unsolicited offers.",
                        "Never send money to verify eligibility for prizes or to claim winnings.",
                        "Report scams to the FTC and local law enforcement to help protect others."
                    }
                },
                {
                    "malware",
                    new List<string>
                    {
                        "Malware includes viruses, trojans, and worms that can damage your system or steal data.",
                        "Keep your antivirus software updated and run regular scans.",
                        "Only download software from official sources and trusted websites.",
                        "Be cautious when opening email attachments, especially from unknown senders."
                    }
                },
                {
                    "firewall",
                    new List<string>
                    {
                        "A firewall acts as a barrier between your computer and the internet, blocking unauthorized access.",
                        "Make sure your firewall is enabled on your device and network router.",
                        "Firewalls work best when combined with other security measures like antivirus software.",
                        "Some firewalls can be configured to allow or block specific applications."
                    }
                },
                {
                    "vpn",
                    new List<string>
                    {
                        "A VPN (Virtual Private Network) encrypts your internet traffic and masks your IP address.",
                        "Use a reputable VPN provider with a no-logs policy to protect your privacy.",
                        "VPNs are especially important when using public Wi-Fi networks.",
                        "A VPN won't protect you from malware or phishing attacks - use it alongside other security measures."
                    }
                },
                {
                    "two-factor",
                    new List<string>
                    {
                        "Two-factor authentication (2FA) requires two forms of verification to access your account.",
                        "Use authenticator apps instead of SMS when possible, as they're more secure.",
                        "Enable 2FA on all your important accounts like email and banking.",
                        "Save your backup codes in a safe place in case you lose access to your authentication device."
                    }
                },
                {
                    "ransomware",
                    new List<string>
                    {
                        "Ransomware encrypts your files and demands payment for their release.",
                        "Back up your important files regularly to an external drive or cloud storage.",
                        "Never pay ransoms, as this encourages criminals and doesn't guarantee file recovery.",
                        "Keep your operating system and software patched to prevent ransomware infections."
                    }
                }
            };
        }

        public string GetResponse(string input)
        {
            string lowerInput = input.ToLower();

            foreach (var keyword in _responses.Keys)
            {
                if (lowerInput.Contains(keyword))
                {
                    List<string> responseList = _responses[keyword];
                    int randomIndex = _random.Next(responseList.Count);
                    return responseList[randomIndex];
                }
            }

            return null;
        }

        public List<string> GetAllKeywords()
        {
            return _responses.Keys.ToList();
        }
    }
}
