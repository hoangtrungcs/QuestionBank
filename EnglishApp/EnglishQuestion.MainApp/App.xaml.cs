/*
=========================================================================================================
  Module      : Application (App.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using EnglishQuestion.AppCommon;
using EnglishQuestion.MainApp.Controls.Register;
using EnglishQuestion.MainApp.TelerikMessageBox;
using Application = System.Windows.Application;

namespace EnglishQuestion.MainApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        }

        private void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            //Exception e = (Exception)args.ExceptionObject;
            //Console.WriteLine("MyHandler caught : " + e.Message);
            //Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            RadMessageBox.Show("There are error occur in application, please contact with admin!");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                bool isRegistered = false;
                if (!File.Exists(MainApp.Properties.Settings.Default.LicenseFilePath))
                {
                    var license = new LicenseAgreement();
                    license.ShowDialog();
                    isRegistered = license.IsRegistered;
                }
                else
                {
                    string info = File.ReadAllText(MainApp.Properties.Settings.Default.LicenseFilePath);
                    isRegistered = LicenseKeyHelper.IsGenuine(info);
                    if (!isRegistered)
                    {
                        var license = new LicenseAgreement();
                        license.ShowDialog();
                        isRegistered = license.IsRegistered;
                    }
                }

                if (isRegistered)
                {
                    base.OnStartup(e);
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("There are error occur in application, please contact with admin!", ex.ToString());
            }
        }
    }
}
