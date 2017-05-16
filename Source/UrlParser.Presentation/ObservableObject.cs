using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace UrlParser.Presentation
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected ObservableObject()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            Type type = typeof(T);

            MemberExpression member = propertyExpression.Body as MemberExpression;

            PropertyInfo propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
            {
                throw new InvalidOperationException("Not a property");
            }

            OnPropertyChanged(propInfo.Name);
        }
    }
}
