using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace EnglishQuestion.MainApp.TelerikMessageBox
{
    /// <summary>
    /// Interaction logic for RadMessageBox.xaml
    /// </summary>
    public partial class RadMessageBox : RadWindow, INotifyPropertyChanged
    {
        #region DependencyProperties
        /// <summary>
        /// This is the static buttons dependency property
        /// </summary>
        public static readonly DependencyProperty ButtonsProperty =
            DependencyProperty.RegisterAttached("Buttons",
                                                typeof(MessageBoxButton),
                                                typeof(RadMessageBox),
                                                new PropertyMetadata(MessageBoxButton.OK, OnButtonsChanged));

        /// <summary>
        /// Set or get the button mode of the message box to show.
        /// The MessageBoxButton enumartion of microsoft is used.
        /// </summary>
        public MessageBoxButton Buttons
        {
            get { return ((MessageBoxButton)GetValue(ButtonsProperty)); }
            set
            {
                SetValue(ButtonsProperty, value);
                OnPropertyChanged("Buttons");
            }
        }

        /// <summary>
        /// This is the static buttons dependency property
        /// </summary>
        public static readonly DependencyProperty DialogImageProperty =
            DependencyProperty.RegisterAttached("DialogImage",
                                                typeof(MessageBoxImage),
                                                typeof(RadMessageBox),
                                                new PropertyMetadata(MessageBoxImage.Warning, OnDialogImageChanged));

        /// <summary>
        /// Set or get the button mode of the message box to show.
        /// The MessageBoxButton enumartion of microsoft is used.
        /// </summary>
        public MessageBoxImage DialogImage
        {
            get { return ((MessageBoxImage)GetValue(DialogImageProperty)); }
            set
            {
                SetValue(DialogImageProperty, value);
                OnPropertyChanged("DialogImage");
            }
        }
        #endregion

        #region StaticDependencyPropChangeHandler
        /// <summary>
        /// Event handler on changing the button mode of the message box
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void OnButtonsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// Event handler on changing the image dependency property
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void OnDialogImageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion

        #region Attributes
        /// <summary>
        /// The result of this message box
        /// </summary>
        public MessageBoxResult Result { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// The data context of the dialog is set to itself
        /// </summary>
        public RadMessageBox()
        {
            InitializeComponent();
            DataContext = this;
        }
        #endregion

        #region EventHandler
        /// <summary>
        /// Button ok click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Button yes click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Button no click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Button cancel click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            DialogResult = false;
            Close();
        }
        #endregion

        #region PropertyChanges
        /// <summary>     
        /// Occurs when a property value changes.     
        /// </summary>    
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed event handler
        /// </summary>
        /// <param name="propertyName">The name of the changed property</param>
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region StaticMethods
        /// <summary>
        /// Show a modal rad alert box
        /// This static method was implemented following the Microsoft pattern for the standard WPF MessageBox
        /// but is using the telerik RadWindow internally
        /// </summary>
        /// <param name="controlOwner">The owner window if needed otherwise null</param>
        /// <param name="message">The message to display</param>
        /// <param name="caption">The title of the message box window. (Parameter is optional)</param>
        /// <param name="button">The buttons the dialog should have - Default is ok</param>
        /// <param name="image">The image the dialog should hold - Default is Warning</param>
        /// <returns>A message box result enum with the result of the dialog</returns>
        public static MessageBoxResult Show(ContentControl controlOwner,
                                            string message,
                                            string caption = null,
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage image = MessageBoxImage.Information)
        {
            try
            {
                var dialog = new RadMessageBox() {DialogImage = image, Buttons = button};
                if (caption != null) { dialog.Header = caption; }

                dialog.txtMessage.Text = message;

                if (controlOwner != null)
                {
                    dialog.Owner = controlOwner;
                }
                else
                {
                    dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }

                dialog.ShowDialog();

                return dialog.Result;
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.ToString());
                return (MessageBoxResult.None);
            }
        }

        /// <summary>
        /// Show a modal RadMessageBox with a message and the window title as option
        /// </summary>
        /// <param name="message">The message string to show</param>
        /// <param name="caption">The title of the message box window.(Parameter is optional)</param>
        public static void Show(string message, string caption = null)
        {
            RadMessageBox.Show(null, message, caption);
        }

        /// <summary>
        /// Show a modal RadMessageBox with a message a window title and configure the buttons to display
        /// </summary>
        /// <param name="message">The message string to show</param>
        /// <param name="caption">The title of the message box window.(Parameter is optional)</param>
        /// <param name="button">The buttons the dialog should have - Default is ok</param>
        public static void Show(string message, string caption, MessageBoxButton button)
        {
            RadMessageBox.Show(null, message, caption, button);
        }

        /// <summary>
        /// Show a modal RadMessageBox with a message a window title, configure the buttons to display and set the dialogs image
        /// </summary>
        /// <param name="message">The message string to show</param>
        /// <param name="caption">The title of the message box window.(Parameter is optional)</param>
        /// <param name="button">The buttons the dialog should have - Default is ok</param>
        /// <param name="image">The image the dialog should hold - Default is Warning</param>
        public static MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage image)
        {
            return (RadMessageBox.Show(null, message, caption, button, image));
        }
        #endregion
    }
}
