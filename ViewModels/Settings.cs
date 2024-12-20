using System.Windows;
using System;
using System.Windows.Input;
using WpfHelpers;
using WpfHelpers.Controls;
using BaseApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
//using System.Data.Linq;
using System.IO;

namespace BaseApp.ViewModels
{

    public class Settings : ViewModelBase
    {

        SettingService ObjSettingService;
        Printer Printer;

        public ICommand OpenFileDialog { get; set; }
        public ICommand SaveCommand { get; }

        public int Id { get; set; }


        private string _ipAddress;
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                OnPropertyChanged(nameof(IpAddress));
            }
        }

        private int _port;
        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        private string _ExcelPath;
        public string ExcelPath
        {
            get
            {
                return this._ExcelPath;
            }
            set
            {
                this._ExcelPath = value;
                this.OnPropertyChanged("ExcelPath");
            }
        }


        private string pName;

        public string PName
        {
            get { return pName; }
            set { pName = value; OnPropertyChanged(nameof(PName)); }
        }


        private ObservableCollection<Settings> settingList;
        public ObservableCollection<Settings> SettingList
        {
            get { return settingList; }
            set { settingList = value; OnPropertyChanged("SettingList"); }
        }

        private Settings _selectedSetting;
        public Settings SelectedSetting
        {
            get => _selectedSetting;
            set
            {
                _selectedSetting = value;
                OnPropertyChanged(nameof(SelectedSetting));
                PopulateFieldsFromSelectedSetting();
            }
        }

        public object SelectedTemplate { get; internal set; }

        public Settings()
        {
            ObjSettingService = new SettingService();
            //Printer printer = new Printer();

            OpenFileDialog = new DelegateCommand((param) =>
            {
                using (var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "Select a Folder";
                    folderBrowserDialog.ShowNewFolderButton = true; // Allow creating new folders if necessary

                    var result = folderBrowserDialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        ExcelPath = folderBrowserDialog.SelectedPath; // Store the selected folder path
                        //ReadExcel(ExcelPath);
                    }

                }
            });


            SaveCommand = new DelegateCommand(async (param) =>
            {
                try
                {
                    // Fetch all settings from the database to validate against
                    var allSettings = await Task.Run(() => ObjSettingService.GetAll());

                    // Check for duplicate port in the current SettingList
                    if (allSettings.Any(s => s.Port == this.Port && s.Id != this.Id))
                    {
                        System.Windows.MessageBox.Show($"The port number {this.Port} is already in use. Please enter a different port.",
                                                        "Duplicate Port", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; // Stop execution if a duplicate is found
                    }

                    var newSetting = new Printer
                    {
                        Id = this.Id,
                        PName = this.PName,
                        IpAddress = this.IpAddress,
                        Port = this.Port,
                        ExcelPath = this.ExcelPath,
                    };

                    //bool success = await Task.Run(() => ObjSettingService.Save(newSetting)); // Run database operation in a background thread.
                    bool success = await ObjSettingService.Save(newSetting);

                    if (success)
                    {
                        await LoadDataAsync(); // Refresh the DataGrid
                        //System.Windows.MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Failed to save settings.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            //LoadDataAsync();
        }



        private async Task LoadDataAsync()
        {
            try
            {
                var data = await Task.Run(() => ObjSettingService.GetAll());


                SettingList = new ObservableCollection<Settings>(data);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred while loading settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //private void ReadExcel(string excelPath)
        //{

        //}

        private void PopulateFieldsFromSelectedSetting()
        {
            if (SelectedSetting != null)
            {
                Id = SelectedSetting.Id;
                PName = SelectedSetting.PName;
                IpAddress = SelectedSetting.IpAddress;
                Port = SelectedSetting.Port;
                ExcelPath = SelectedSetting.ExcelPath;
            }
            else
            {
                // Clear fields when no item is selected.

                PName = string.Empty;
                IpAddress = string.Empty;
                Port = 0;
                ExcelPath = string.Empty;
            }
        }


    }
}


