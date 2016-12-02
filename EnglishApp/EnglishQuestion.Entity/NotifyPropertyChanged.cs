/*
=========================================================================================================
  Module      : Notify Property Changed (NotifyPropertyChanged.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EnglishQuestion.Entity
{
    /// <summary>
    /// Notify Property Changed
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> m_properties = new Dictionary<string, object>();

        /// <summary>
        /// Gets the value of a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T Get<T>([CallerMemberName] string name = null)
        {
            object value = null;
            return m_properties.TryGetValue(name, out value) ? (value == null ? default(T) : (T)value) : default(T);
        }

        /// <summary>
        /// Sets the value of a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        protected void Set<T>(T value, [CallerMemberName] string name = null)
        {
            if (Equals(value, Get<T>(name))) return;
            m_properties[name] = value;
            OnPropertyChanged(name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}