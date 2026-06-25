# CyberSecurityChatBot-Part3
POE

## Project Overview
This is the final submission for **PROG6221 – Programming 2A**.  
It extends the chatbot built in Part 1 (console) and Part 2 (WPF GUI) with **Part 3 features**:

- Task Assistant with Reminders (JSON storage, CRUD operations)  
- Cybersecurity Quiz Mini‑Game (10+ questions, immediate feedback, final score)  
- NLP Simulation (flexible phrasing detection)  
- Activity Log (records actions, shows last 5–10 entries, “show more” option)  

All features from Parts 1 and 2 remain functional: voice greeting, ASCII art, keyword responses, sentiment detection, memory, and follow‑up flow.

---

## Author
- **Name:** Kanya  
- **Student Number:** [ST10490015]

---

## Features Across Parts 1–3
- Voice greeting (`greeting.wav`) plays on launch  
- ASCII art displayed in GUI header  
- Keyword recognition for cybersecurity topics  
- Sentiment detection with automatic tips  
- Memory feature (remembers user’s name)  
- Follow‑up flow (“tell me more”)  
- Task Assistant with JSON storage (add, view, complete, delete)  
- Quiz mini‑game with 10+ questions and feedback  
- NLP intent detection for natural phrasing  
- Activity log with timestamps and “show more”  

---

## Prerequisites
- **Visual Studio 2022**  
- **.NET 8.0**  
- **Newtonsoft.Json NuGet package**  

---

## Setup Instructions
1. Clone the repository:
   ```bash
git clone https://github.com/Kanya-12/CyberSecurityChatBot-Part3
2. Open the solution in Visual Studio 2022.
3. Install Newtonsoft.Json via NuGet:
    - Right‑click project → Manage NuGet Packages → Search “Newtonsoft.Json” → Install.
4. Place greeting.wav in the project root.
5. Build and run the project.
6. tasks.json will be auto‑created when the first task is added

