﻿using System.Windows;
using System;
using System.Windows.Input;
using WpfHelpers;
using WpfHelpers.Controls;
using BaseApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace BaseApp.ViewModels
{

    public class Settings : ViewModelBase
    {

        private readonly SettingService ObjSettingService;

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
                return _ExcelPath;
            }
            set
            {
                _ExcelPath = value;
                OnPropertyChanged("ExcelPath");
            }
        }


        private string pName;

        public string PName
        {
            get { return pName; }
            set { pName = value; OnPropertyChanged(nameof(PName)); }
        }


        private ObservableCollection<SettingsModel> settingList;
        public ObservableCollection<SettingsModel> SettingList
        {
            get { return settingList; }
            set { settingList = value; OnPropertyChanged("SettingList"); }
        }

        private SettingsModel _selectedSetting;
        public SettingsModel SelectedSetting
        {
            get => _selectedSetting;
            set
            {
                _selectedSetting = value;
                OnPropertyChanged(nameof(SelectedSetting));
                PopulateFieldsFromSelectedSetting();
            }
        }

       
        public Settings()
        {
            ObjSettingService = new SettingService();

            LoadDataAsync();

            OpenFileDialog = new DelegateCommand((param) =>
            {
                using (var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "Select a Folder";
                    folderBrowserDialog.ShowNewFolderButton = true;

                    var result = folderBrowserDialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        ExcelPath = folderBrowserDialog.SelectedPath;
                    }

                }
            });


            SaveCommand = new DelegateCommand(async (param) =>
            {
                
                try
                {
                    var allSettings = await Task.Run(() => ObjSettingService.GetAll());

                    if (allSettings.Any(s => s.Port == this.Port && s.Id != this.Id))
                    {
                        System.Windows.MessageBox.Show($"The port number {this.Port} is already in use. Please enter a different port.",
                                                        "Duplicate Port", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var newSetting = new PrinterModel
                    {
                        Id = this.Id,
                        PName = this.PName,
                        IpAddress = this.IpAddress,
                        Port = this.Port,
                        ExcelPath = this.ExcelPath,
                    };

                    bool success = await ObjSettingService.Save(newSetting);

                    if (success)
                    {
                        LoadDataAsync();
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

        }

        private void LoadDataAsync()
        {
            try
            {
                var data = ObjSettingService.GetAll().Result;
                SettingList = new ObservableCollection<SettingsModel>(data);
               
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred while loading settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


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
                PName = string.Empty;
                IpAddress = string.Empty;
                Port = 0;
                ExcelPath = string.Empty;
            }
        }

    }
}


