using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace InitialProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application,INotifyPropertyChanged
    {
        private bool darkTheme = false;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string lang;
        public string Lang
        {
            get { return lang; }
            set
            {
                lang = value;
                OnPropertyChanged("");
            }
        }
        
        public bool DarkTheme
        {
            get {
              //  darkTheme = Properties.Settings.Default.theme;
                return darkTheme; }
            set {
                darkTheme = value;
              //  Properties.Settings.Default.theme = darkTheme;
              //  Properties.Settings.Default.Save();
            }

        }


        public ResourceDictionary ThemeDictionary
        {
            get { return Resources.MergedDictionaries[0]; }
        }


        public void ChangeTheme(Uri uri)
        {
            DarkTheme = !DarkTheme;
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        }

        public void ChangeLanguage(string currLang)
        {
            Lang= currLang;
            if (currLang.Equals("en-US"))
            {
                TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            }
            else
            {
                TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo("sr-LATN");
            }
        }

        }
    }
