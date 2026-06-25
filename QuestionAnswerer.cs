using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatBot
{
    public class QuestionAnswerer
    {
        private Dictionary<string, string> _questionAnswers;

        public QuestionAnswerer()
        {
            InitializeQuestionAnswers();
        }

        private void InitializeQuestionAnswers()
        {
            // Expanded curated question bank (now 100+ entries)
            _questionAnswers = new Dictionary<string, string>
            {
                // Phishing
                { "what is phishing", "Phishing is a social engineering attack where criminals impersonate trusted organizations by email, SMS, or websites to steal credentials, money, or personal data. Protect yourself by verifying senders, not clicking unknown links, and enabling 2FA." },
                { "how to avoid phishing", "Avoid phishing by checking sender addresses, not following links in unsolicited messages, hovering over links to see actual URLs, verifying requests by phone, and using email filters and 2FA." },
                { "spear phishing", "Spear phishing is a targeted phishing attack aimed at a specific person or organization, often using personal details. Be cautious with unexpected messages and verify requests independently." },
                { "phishing signs", "Common phishing signs: sender address mismatch, urgent language, unexpected attachments, poor grammar, suspicious URLs, and requests for credentials or money." },

                // Passwords
                { "what is password security", "Password security is the practice of protecting user accounts with strong, unique passwords and authentication methods. A strong password has 12+ characters with uppercase, lowercase, numbers, and symbols. Key practices: use a password manager for unique passwords on each site, enable 2FA for important accounts, avoid reusing passwords, never share passwords, and change them if you suspect a breach." },
                { "how to create a strong password", "Use long passphrases (12+ characters), combine upper/lowercase, numbers and symbols. Prefer randomly generated passwords from a manager and never reuse passwords across accounts." },
                { "password manager", "A password manager securely stores and generates unique passwords for each account. Use reputable managers (Bitwarden, 1Password) and protect them with a strong master password and 2FA." },
                { "password reuse", "Reusing passwords across sites is risky: one breach can expose multiple accounts. Use unique passwords and a manager to avoid reuse." },
                { "passphrase vs password", "Passphrases (multiple random words) are easier to remember and can be more secure than single-word passwords if long and random enough." },

                // Two-factor & authentication
                { "two-factor authentication", "Two-factor authentication (2FA) adds an extra verification step such as an app code or hardware key. Use authenticator apps or hardware tokens rather than SMS when possible." },
                { "mfa vs 2fa", "Multi-factor authentication (MFA) is a broader term; 2FA is a common form with two distinct factors (password + device code). Both increase account security significantly." },
                { "authenticator apps", "Authenticator apps like Google Authenticator, Authy, and Microsoft Authenticator provide time-based codes and are more secure than SMS." },

                // Malware & removal
                { "what is malware", "Malware is software designed to harm or compromise devices and data (viruses, trojans, worms, ransomware, spyware). Use updated AV, careful downloads, and backups to reduce risk." },
                { "remove malware", "To remove malware: disconnect from network, run reputable anti-malware scans in Safe Mode, quarantine/remove infected files, change credentials from a clean device, and restore from backups if needed." },
                { "antivirus recommendations", "Windows Defender is a good baseline; consider Malwarebytes or other reputable vendors for additional scanning. Keep definitions updated." },

                // Ransomware
                { "what is ransomware", "Ransomware encrypts files and demands payment for decryption. Prevention: backups, updates, endpoint protection, and user training. If infected, disconnect, report to authorities, and restore from backups if possible." },
                { "ransomware backup strategy", "Keep 3-2-1 backups: 3 copies, on 2 different media, with 1 offsite. Test restores regularly." },

                // Network & Wi-Fi
                { "public wi-fi", "Public Wi-Fi can be insecure; use a reputable VPN, prefer mobile data for sensitive tasks, and avoid logging into important accounts on unknown networks." },
                { "what is a vpn", "A VPN encrypts your internet traffic and masks your IP by routing through a secure server — useful for privacy on untrusted networks but not a substitute for antivirus or safe browsing." },
                { "wifi security", "Use WPA3 when available, unique SSID passwords, hide SSID if needed, keep router firmware updated, and change default admin credentials." },

                // Firewalls
                { "what is a firewall", "A firewall monitors and filters network traffic to prevent unauthorized connections. Keep firewalls enabled on devices and routers." },
                { "enable firewall", "Enable the built-in firewall on your OS and router; configure rules to block unnecessary inbound ports and use NAT where appropriate." },

                // Email security
                { "email attachments", "Attachments can carry malware or malicious macros. Verify unexpected attachments by contacting the sender and scan files before opening." },
                { "email authentication", "SPF, DKIM and DMARC are email authentication protocols that help reduce spoofing and phishing." },

                // Identity theft
                { "identity theft", "Identity theft occurs when someone uses your personal information without permission. Protect identity by limiting sharing, monitoring credit, and using strong authentication." },
                { "fraud alerts", "Place fraud alerts or credit freezes with credit bureaus if you suspect identity theft; monitor credit reports regularly." },

                // Social engineering
                { "social engineering", "Social engineering manipulates people into revealing sensitive information. Training and skepticism reduce risk; verify requests independently." },
                { "pretexting", "Pretexting is creating a false scenario to trick targets into revealing information. Verify credentials and contact people through official channels." },

                // Scams
                { "scam prevention", "Avoid scams by verifying contact methods, never sending money to unknown people, checking URLs, and asking trusted contacts before acting on large requests." },
                { "tech support scam", "Tech support scams claim your device is infected and ask for payment or remote access. Never grant remote access unless you verified the company through official channels." },

                // Browsing safety
                { "https", "HTTPS encrypts data between your browser and website using TLS; always prefer HTTPS for sensitive sites like banking or shopping." },
                { "site safety", "Check domain names, HTTPS, contact info, and reviews. Use reputation tools like VirusTotal when unsure." },

                // Software updates
                { "software updates", "Updates patch security flaws — enable automatic updates where practical and install critical patches promptly." },
                { "patch management", "For organizations, use centralized patch management tools and test updates in staging before broad deployment." },

                // Mobile security
                { "mobile security", "Protect phones with strong locks, keep OS/apps updated, only install from official stores, check app permissions, and backup data regularly." },
                { "sms phishing", "SMS phishing ('smishing') uses texts to trick users. Don't click links in unexpected texts and verify senders." },

                // Social media
                { "social media safety", "Limit public information, use privacy settings, enable 2FA, and be cautious about friend requests and shared content." },
                { "location privacy", "Be mindful of location sharing and geotagged posts — attackers can use these to target you physically or time attacks." },

                // Advanced threats
                { "zero day", "A zero-day exploit uses an unknown vulnerability before a patch exists. Defend with layered security, rapid patching, and monitoring." },
                { "botnet", "A botnet is a network of compromised machines used for attacks like DDoS. Protect devices to prevent botnet enrollment." },
                { "ddos", "DDoS attacks overwhelm services with traffic. Mitigate with specialized services, redundancy, and rate limiting." },
                { "mitm", "Man-in-the-middle attacks intercept communications; use HTTPS, VPNs, and certificate checks to reduce risk." },
                { "sql injection", "SQL injection exploits web inputs to run database commands. Web developers should use parameterized queries; users should prefer trusted sites." },
                { "xss", "Cross-site scripting allows attackers to run scripts in a victim's browser. Keep browsers updated and avoid untrusted links." },

                // Privacy & compliance
                { "gdpr", "GDPR is European data protection regulation giving users rights over personal data. For individuals: review and exercise your rights on services you use." },
                { "ccpa", "CCPA is a California privacy law providing data access and deletion rights for residents; check state resources for filing complaints." },

                // Cloud & backups
                { "cloud security", "Cloud security depends on provider controls and user configuration. Use strong auth, encryption, least privilege, and backups for critical data." },
                { "backups", "Keep regular backups, test restores, keep at least one offline copy, and use versioning to recover from ransomware." },

                // IoT & device hardening
                { "iot security", "Secure IoT by changing default passwords, keeping firmware updated, isolating devices on separate network segments, and disabling unused services." },

                // Developer & web app concepts (high-level)
                { "what is csrf", "CSRF (Cross-Site Request Forgery) tricks authenticated users into performing actions; developers should use anti-CSRF tokens and SameSite cookies." },
                { "cors", "CORS (Cross-Origin Resource Sharing) is a browser security policy; developers should configure allowed origins carefully." },

                // Authentication & keys
                { "hardware security key", "Hardware security keys (FIDO, YubiKey) provide phishing-resistant authentication and are recommended for high-value accounts." },
                { "webauthn", "WebAuthn is a modern web authentication standard that enables passwordless or phishing-resistant logins using public-key cryptography." },

                // Privacy tools & recommendations
                { "password manager recommendation", "Use a reputable password manager with strong encryption; enable its multi-device sync carefully and protect it with a strong master password and 2FA." },

                // Incident response
                { "incident response", "If a security incident occurs: contain the incident, preserve evidence, notify affected parties, patch vulnerabilities, and recover systems from clean backups." },

                // Parental & student safety
                { "student online safety", "Teach students to protect personal info, use privacy settings, report bullying, and ask adults before downloading apps or sharing sensitive data." },

                // Business & workplace
                { "workplace security", "Follow company policies, report suspicious emails, use company VPNs, and avoid storing sensitive data on personal devices unless authorized." },

                // Authentication attacks
                { "credential stuffing", "Credential stuffing uses leaked credentials to access other sites. Prevent with unique passwords and 2FA." },
                { "brute force", "Brute force attacks try password combinations; prevent with strong passwords, rate limiting, account lockouts, and 2FA." },

                // Device recovery
                { "factory reset safety", "Factory reset can remove malware from some devices — back up first and reinstall only trusted apps." },

                // Privacy-enhancing tech
                { "disk encryption", "Full-disk encryption (BitLocker, FileVault) protects data at rest if a device is lost or stolen; enable it on laptops and mobile devices." },

                // Email authentication protocols
                { "spf dkim dmarc", "SPF, DKIM and DMARC help email providers verify legitimate mail and reduce spoofing. They are technical controls implemented by domain owners." },

                // Help & Resources
                { "where can i get help", "Help Resources: FBI IC3 (ic3.gov), FTC (identitytheft.gov), local law enforcement, banks' fraud teams, and official vendor support pages. For training: SANS, OWASP, and vendor advisories." },
                { "where to report cybercrime", "Report cybercrime to relevant authorities: IC3 for federal complaints, local police, your bank for financial fraud, and platform providers for online abuse." },
                { "what are good cybersecurity resources", "CISA, OWASP, Have I Been Pwned, Malwarebytes Labs, Krebs on Security, and vendor security advisories are reliable starting points." },

                // Default fallback
                { "default_fallback", "I'm not sure about that specific question yet. Try asking about phishing, passwords, malware, privacy, VPNs, or how to report a scam. I rely on trusted cybersecurity guidance for accurate answers." }
            };
        }

        public string GetAnswer(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return null;

            string lowerQuestion = question.ToLower().Trim();

            // 1) Exact key match (highest priority)
            if (_questionAnswers.ContainsKey(lowerQuestion))
                return _questionAnswers[lowerQuestion];

            // 2) Substring match on keys (fast high-confidence)
            foreach (var kvp in _questionAnswers)
            {
                if (kvp.Key == "default_fallback")
                    continue;

                // Check if lowerQuestion contains key or key contains most of lowerQuestion
                if (lowerQuestion.Contains(kvp.Key) || kvp.Key.Contains(lowerQuestion))
                    return kvp.Value;
            }

            // 3) Token-based best-match scoring for flexible queries
            var queryWords = new HashSet<string>(lowerQuestion.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            string bestKey = null;
            double bestScore = 0.0;
            int bestMatches = 0;

            foreach (var key in _questionAnswers.Keys)
            {
                if (key == "default_fallback")
                    continue;

                var keyWords = key.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int matches = keyWords.Count(k => queryWords.Contains(k));

                if (matches == 0) continue;

                // Score based on proportion of key words matched
                double score = (double)matches / keyWords.Length;

                // Select best match: higher score, or same score but more matches
                if (score > bestScore || (Math.Abs(score - bestScore) < 1e-6 && matches > bestMatches))
                {
                    bestScore = score;
                    bestKey = key;
                    bestMatches = matches;
                }
            }

            // 4) Return best match if confidence is good (at least 40% key match or 2+ word matches)
            if (!string.IsNullOrEmpty(bestKey) && (bestScore >= 0.4 || bestMatches >= 2))
            {
                return _questionAnswers[bestKey];
            }

            // 5) No match found - return null and let ChatBot decide what to do
            return null;
        }

        public List<string> GetRelatedTopics(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<string>();

            string lowerInput = input.ToLower();
            var relatedTopics = new List<string>();

            foreach (var question in _questionAnswers.Keys)
            {
                if (question == "default_fallback")
                    continue;

                if (question.Contains(lowerInput) || lowerInput.Split().Any(word => question.Contains(word)))
                {
                    relatedTopics.Add(question);
                }
            }

            return relatedTopics.Take(5).ToList();
        }
    }
}
