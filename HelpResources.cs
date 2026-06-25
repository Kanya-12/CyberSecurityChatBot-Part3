using System;
using System.Collections.Generic;

namespace CyberSecurityChatBot
{
    public class HelpResources
    {
        private static Dictionary<string, ResourceInfo> _resources;

        public HelpResources()
        {
            InitializeResources();
        }

        private static void InitializeResources()
        {
            _resources = new Dictionary<string, ResourceInfo>
            {
                // Government & Official Resources
                {
                    "cisa", new ResourceInfo
                    {
                        Name = "CISA (Cybersecurity & Infrastructure Security Agency)",
                        Url = "https://www.cisa.gov/",
                        Description = "U.S. government agency providing cybersecurity alerts, advisories, and resources. Visit for threat updates and incident reporting.",
                        Category = "Government"
                    }
                },
                {
                    "nist", new ResourceInfo
                    {
                        Name = "NIST Cybersecurity Framework",
                        Url = "https://www.nist.gov/cyberframework",
                        Description = "National Institute of Standards and Technology provides guidelines and best practices for cybersecurity.",
                        Category = "Government"
                    }
                },
                {
                    "ic3", new ResourceInfo
                    {
                        Name = "IC3 (Internet Crime Complaint Center)",
                        Url = "https://www.ic3.gov/",
                        Description = "FBI's official platform for reporting cybercrimes and fraud. File complaints about online scams and attacks.",
                        Category = "Reporting"
                    }
                },
                {
                    "fbi", new ResourceInfo
                    {
                        Name = "FBI Cyber Division",
                        Url = "https://www.fbi.gov/investigate/cyber",
                        Description = "FBI's cybercrime investigation resources and incident reporting channels.",
                        Category = "Reporting"
                    }
                },

                // Privacy & Rights
                {
                    "ftc", new ResourceInfo
                    {
                        Name = "FTC Privacy Resources",
                        Url = "https://www.consumer.ftc.gov/privacy",
                        Description = "Federal Trade Commission guidance on privacy rights and how to protect personal information.",
                        Category = "Privacy"
                    }
                },
                {
                    "identitytheft", new ResourceInfo
                    {
                        Name = "IdentityTheft.gov",
                        Url = "https://www.identitytheft.gov/",
                        Description = "Federal government site for reporting and recovering from identity theft. Get recovery plan and resources.",
                        Category = "Reporting"
                    }
                },

                // Education & Training
                {
                    "sans", new ResourceInfo
                    {
                        Name = "SANS Institute",
                        Url = "https://www.sans.org/",
                        Description = "Leading cybersecurity training and certification provider. Offers courses and resources for all skill levels.",
                        Category = "Training"
                    }
                },
                {
                    "coursera", new ResourceInfo
                    {
                        Name = "Coursera Cybersecurity Courses",
                        Url = "https://www.coursera.org/browse/computer-science/cybersecurity",
                        Description = "Free and paid cybersecurity courses from universities and industry experts.",
                        Category = "Training"
                    }
                },
                {
                    "comptia", new ResourceInfo
                    {
                        Name = "CompTIA Security+",
                        Url = "https://www.comptia.org/certifications/security",
                        Description = "Popular entry-level cybersecurity certification and learning path.",
                        Category = "Training"
                    }
                },
                {
                    "udemy", new ResourceInfo
                    {
                        Name = "Udemy Cybersecurity Courses",
                        Url = "https://www.udemy.com/courses/security-cybersecurity/",
                        Description = "Affordable cybersecurity courses covering various topics and skill levels.",
                        Category = "Training"
                    }
                },

                // Threat Monitoring & Intelligence
                {
                    "virustotal", new ResourceInfo
                    {
                        Name = "VirusTotal",
                        Url = "https://www.virustotal.com/",
                        Description = "Free service to scan files and URLs for malware and threats. Check suspicious downloads here.",
                        Category = "Tools"
                    }
                },
                {
                    "shodan", new ResourceInfo
                    {
                        Name = "Shodan",
                        Url = "https://www.shodan.io/",
                        Description = "Search engine for finding internet-connected devices. Useful for security research and understanding exposure.",
                        Category = "Tools"
                    }
                },
                {
                    "haveibeenpwned", new ResourceInfo
                    {
                        Name = "Have I Been Pwned",
                        Url = "https://haveibeenpwned.com/",
                        Description = "Check if your email/password was exposed in a data breach. Essential for assessing personal risk.",
                        Category = "Tools"
                    }
                },
                {
                    "abuse", new ResourceInfo
                    {
                        Name = "AbuseIPDB",
                        Url = "https://www.abuseipdb.com/",
                        Description = "Report and check suspicious IP addresses for malicious activity.",
                        Category = "Tools"
                    }
                },

                // Security News & Updates
                {
                    "krebs", new ResourceInfo
                    {
                        Name = "Krebs on Security",
                        Url = "https://krebsonsecurity.com/",
                        Description = "Respected cybersecurity journalist Brian Krebs covers breaking security news and incidents.",
                        Category = "News"
                    }
                },
                {
                    "darknetdiaries", new ResourceInfo
                    {
                        Name = "Darknet Diaries Podcast",
                        Url = "https://darknetdiaries.com/",
                        Description = "Popular podcast covering true cybercrime stories and security topics in an engaging way.",
                        Category = "News"
                    }
                },
                {
                    "thehackernews", new ResourceInfo
                    {
                        Name = "The Hacker News",
                        Url = "https://thehackernews.com/",
                        Description = "Daily cybersecurity news and threat intelligence coverage.",
                        Category = "News"
                    }
                },

                // Community & Support
                {
                    "reddit", new ResourceInfo
                    {
                        Name = "r/cybersecurity Reddit",
                        Url = "https://www.reddit.com/r/cybersecurity/",
                        Description = "Active community discussing cybersecurity topics, sharing resources, and answering questions.",
                        Category = "Community"
                    }
                },
                {
                    "stack", new ResourceInfo
                    {
                        Name = "Security Stack Exchange",
                        Url = "https://security.stackexchange.com/",
                        Description = "Q&A community for security professionals and enthusiasts. Ask and answer detailed security questions.",
                        Category = "Community"
                    }
                },
                {
                    "isaca", new ResourceInfo
                    {
                        Name = "ISACA (Cybersecurity Professionals)",
                        Url = "https://www.isaca.org/",
                        Description = "Professional association for IT audit, governance, and security professionals.",
                        Category = "Community"
                    }
                },

                // Password & 2FA Tools
                {
                    "bitwarden", new ResourceInfo
                    {
                        Name = "Bitwarden Password Manager",
                        Url = "https://bitwarden.com/",
                        Description = "Free, open-source password manager to securely store and manage strong passwords.",
                        Category = "Tools"
                    }
                },
                {
                    "authy", new ResourceInfo
                    {
                        Name = "Authy 2FA",
                        Url = "https://authy.com/",
                        Description = "Free two-factor authentication app for securing online accounts with TOTP codes.",
                        Category = "Tools"
                    }
                },
                {
                    "google", new ResourceInfo
                    {
                        Name = "Google Authenticator",
                        Url = "https://support.google.com/accounts/answer/1066447",
                        Description = "Free 2FA app from Google for generating one-time verification codes.",
                        Category = "Tools"
                    }
                },

                // Phishing & Social Engineering Protection
                {
                    "phishtank", new ResourceInfo
                    {
                        Name = "PhishTank",
                        Url = "https://phishtank.org/",
                        Description = "Community-driven phishing database. Report and check suspected phishing URLs.",
                        Category = "Tools"
                    }
                },
                {
                    "reportphishing", new ResourceInfo
                    {
                        Name = "Report Phishing to Anti-Phishing Working Group",
                        Url = "https://www.antiphishing.org/report-phishing/",
                        Description = "Official channel to report phishing emails and help protect others.",
                        Category = "Reporting"
                    }
                },

                // VPN & Network Security
                {
                    "vpn", new ResourceInfo
                    {
                        Name = "ProPublica VPN Comparison",
                        Url = "https://www.propublica.org/article/should-you-use-a-vpn",
                        Description = "Detailed guide on VPNs: what they do, what they don't, and how to choose one.",
                        Category = "Tools"
                    }
                },
                {
                    "tor", new ResourceInfo
                    {
                        Name = "Tor Project",
                        Url = "https://www.torproject.org/",
                        Description = "Free software for anonymous internet browsing and communication.",
                        Category = "Tools"
                    }
                },

                // Incident Response
                {
                    "incident", new ResourceInfo
                    {
                        Name = "SANS Incident Handler's Handbook",
                        Url = "https://www.sans.org/reading-room/whitepapers/incident/",
                        Description = "Comprehensive guide for handling security incidents step-by-step.",
                        Category = "Training"
                    }
                },
                {
                    "nistcsf", new ResourceInfo
                    {
                        Name = "NIST Incident Response",
                        Url = "https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-61r3.pdf",
                        Description = "NIST Computer Security Incident Handling Guide - standard for IR procedures.",
                        Category = "Training"
                    }
                }
            };
        }

        /// <summary>
        /// Get a specific resource by key (e.g., "cisa", "haveibeenpwned").
        /// </summary>
        public ResourceInfo GetResource(string key)
        {
            if (_resources.TryGetValue(key.ToLower(), out var resource))
            {
                return resource;
            }
            return null;
        }

        /// <summary>
        /// Get all resources in a category (e.g., "Government", "Tools", "Training").
        /// </summary>
        public List<ResourceInfo> GetResourcesByCategory(string category)
        {
            var results = new List<ResourceInfo>();
            foreach (var kvp in _resources)
            {
                if (kvp.Value.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(kvp.Value);
                }
            }
            return results;
        }

        /// <summary>
        /// Get all available categories.
        /// </summary>
        public List<string> GetAllCategories()
        {
            var categories = new HashSet<string>();
            foreach (var resource in _resources.Values)
            {
                categories.Add(resource.Category);
            }
            return new List<string>(categories);
        }

        /// <summary>
        /// Get all resources.
        /// </summary>
        public List<ResourceInfo> GetAllResources()
        {
            return new List<ResourceInfo>(_resources.Values);
        }

        /// <summary>
        /// Search resources by keyword in name or description.
        /// </summary>
        public List<ResourceInfo> SearchResources(string keyword)
        {
            var results = new List<ResourceInfo>();
            string lowerKeyword = keyword.ToLower();
            foreach (var resource in _resources.Values)
            {
                if (resource.Name.ToLower().Contains(lowerKeyword) || 
                    resource.Description.ToLower().Contains(lowerKeyword))
                {
                    results.Add(resource);
                }
            }
            return results;
        }

        /// <summary>
        /// Resource info container.
        /// </summary>
        public class ResourceInfo
        {
            public string Name { get; set; }
            public string Url { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }

            public override string ToString()
            {
                return $"{Name}\nURL: {Url}\n{Description}\nCategory: {Category}";
            }
        }
    }
}
