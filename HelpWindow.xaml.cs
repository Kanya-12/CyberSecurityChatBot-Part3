using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CyberSecurityChatBot
{
    public partial class HelpWindow : Window
    {
        private HelpResources _helpResources;
        private List<HelpResources.ResourceInfo> _currentResources;

        public HelpWindow(HelpResources helpResources)
        {
            InitializeComponent();
            _helpResources = helpResources;
            _currentResources = new List<HelpResources.ResourceInfo>();

            // Load all resources
            LoadResources(_helpResources.GetAllResources());

            // Populate category filter
            PopulateCategoryFilter();
        }

        /// <summary>
        /// Load resources into the ItemsControl
        /// </summary>
        private void LoadResources(List<HelpResources.ResourceInfo> resources)
        {
            _currentResources = resources;
            ResourcesList.ItemsSource = resources;
        }

        /// <summary>
        /// Populate the category dropdown filter
        /// </summary>
        private void PopulateCategoryFilter()
        {
            CategoryCombo.Items.Add("All Categories");
            foreach (string category in _helpResources.GetAllCategories())
            {
                CategoryCombo.Items.Add(category);
            }
            CategoryCombo.SelectedIndex = 0;
        }

        /// <summary>
        /// Handle search box text changes
        /// </summary>
        private void SearchBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FilterResources();
        }

        /// <summary>
        /// Handle category combo box selection
        /// </summary>
        private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterResources();
        }

        /// <summary>
        /// Filter resources based on search and category
        /// </summary>
        private void FilterResources()
        {
            string searchText = SearchBox.Text.Trim().ToLower();
            string selectedCategory = CategoryCombo.SelectedItem as string;

            List<HelpResources.ResourceInfo> filteredResources = new List<HelpResources.ResourceInfo>();

            foreach (var resource in _helpResources.GetAllResources())
            {
                // Check category filter
                bool categoryMatch = selectedCategory == "All Categories" || 
                                    resource.Category == selectedCategory;

                // Check search filter
                bool searchMatch = string.IsNullOrWhiteSpace(searchText) ||
                                  resource.Name.ToLower().Contains(searchText) ||
                                  resource.Description.ToLower().Contains(searchText);

                if (categoryMatch && searchMatch)
                {
                    filteredResources.Add(resource);
                }
            }

            LoadResources(filteredResources);
        }

        /// <summary>
        /// Clear all filters
        /// </summary>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
            CategoryCombo.SelectedIndex = 0;
            LoadResources(_helpResources.GetAllResources());
        }

        /// <summary>
        /// Handle hyperlink navigation
        /// </summary>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                // Open URL in default browser
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open link: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
