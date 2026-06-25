using System.Windows;

namespace CyberSecurityChatBot
{
    public partial class App : Application
    {
        /// <summary>
        /// Application startup event - plays greeting audio
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Play the greeting audio when the application starts
            AudioHelper.PlayGreeting();
        }
    }
}
