# CyberSecurityChatBot-Part3
## POE 

# KanyaShield Chatbot  

---

## Overview  
KanyaShield is a **Cybersecurity Awareness Chatbot** developed in C#.  
The chatbot is designed to educate users about online safety, cyber threats, and best security practices.  

This project evolved across three parts:  
- **Part 1:** Console chatbot with voice greeting, ASCII art, and keyword responses.  
- **Part 2:** WPF GUI chatbot with sentiment detection, memory, and follow‑up flow.  
- **Part 3:** Final GUI chatbot with Task Assistant, Quiz, NLP simulation, and Activity Log.  

---

## Features  

### Part 1  
- Voice greeting using audio playback  
- ASCII art logo display  
- Interactive chatbot with user input  
- 29 cybersecurity awareness questions  
- Typing animation for better user experience  
- Input validation and error handling  

### Part 2  
- GUI interface built with WPF  
- Sentiment detection (e.g., “I am worried about phishing”)  
- Automatic tips based on detected sentiment  
- Memory feature (remembers user’s name)  
- Follow‑up flow (“tell me more”)  

### Part 3  
- **Task Assistant with Reminders**  
  - Add, view, complete, and delete tasks  
  - Tasks stored in `tasks.json` using JSON  
  - Reminder system integrated into chatbot flow  
- **Cybersecurity Quiz Mini‑Game**  
  - 10+ questions covering phishing, passwords, safe browsing, social engineering  
  - Immediate feedback after each answer  
  - Final score and message at the end  
- **NLP Simulation**  
  - Flexible keyword detection using varied phrasing  
  - Handles natural requests like “remind me to update my password tomorrow”  
- **Activity Log**  
  - Records chatbot actions with timestamps  
  - Shows last 5–10 entries with “show more” option  

---

## Purpose  
The purpose of this chatbot is to:  
- Educate users about cybersecurity  
- Promote safe online behavior  
- Help prevent cyber threats such as phishing, malware, and scams  

---

## Technologies Used  
- C#  
- .NET 8.0  
- WPF / WinForms (GUI)  
- Newtonsoft.Json (NuGet package)  
- GitHub (Version Control)  
- GitHub Actions (CI/CD)  

---

## How to Run  
1. Clone the repository:
   ```bash
git clone https://github.com/Kanya-12/CyberSecurityChatBot-Part3

2. Open the solution in Visual Studio 2022.
3. Install Newtonsoft.Json via NuGet:
    - Right‑click project → Manage NuGet Packages → Search “Newtonsoft.Json” → Install.
4. Place greeting.wav in the project root.
5. Build and run the project.
6. tasks.json will be auto‑created when the first task is added

## Example Topics Covered
- Phishing attacks
  
- Strong passwords
  
- Malware & ransomware
  
- Safe browsing
  
- Social engineering
  
- Data privacy
  
- Two‑factor authentication
  
- Privacy settings
  
- Data backup

## Author
Kanya Kapo  
Student Number: ST10490015

## GitHub Commits (Project Development)
1. Initial project setup

2. Added voice greeting

3. Added ASCII art

4. Implemented user input

5. Added chatbot responses

6. Improved UI and validation

7. Added JSON task storage with TaskStorageHelper and CyberTask model

8. Added Task Assistant panel with add, view, complete, and delete via JSON file

9. Added cybersecurity quiz mini‑game with 10+ questions and immediate feedback

10. Added activity log with timestamps, 10 entry limit, and show more option

11. Expanded NLP to detect task, quiz, and log intents from varied phrasings

12. Final integration test passed, README updated with setup instructions

## Releases
- v3.0 – Task Assistant + JSON storage

- v3.1 – Quiz + Activity Log

- v3.2 – Final integrated version

## Continuous Integration
This project uses GitHub Actions to automatically build and verify the code.

## Video Presentation
Unlisted YouTube link: [ ]

Demonstrates:

- Voice greeting + ASCII art

- Keyword responses, sentiment detection, memory, follow‑up flow

- Task Assistant CRUD operations with JSON

- Quiz mini‑game with feedback and final score

- NLP intent detection with varied phrasing

- Activity log with timestamps and “show more”

- Code walkthrough (TaskStorageHelper, QuizManager, ActivityLogger, ChatBot/KeywordResponder)

- GitHub commit history, 3 releases, and Actions green tick
