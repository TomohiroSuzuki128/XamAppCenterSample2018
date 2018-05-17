using System;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Windows.Input;
using MvvmCross.Binding.BindingContext;

namespace XamAppCenterSample2018.Droid
{
    public class LinkerPleaseInclude
    {
        public void Include(TextView textView)
        {
            textView.Text = textView.Text + "";
            textView.AfterTextChanged += (s, e) => textView.Text = textView.Text + "";
            textView.Hint = textView.Hint + "";
            textView.Click += (s, e) => textView.Text = textView.Text + "";
        }

        public void Include(Button button)
        {
            button.Click += (s, e) => button.Text = button.Text + "";
        }

        public void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) => { if (command.CanExecute(null)) command.Execute(null); };
        }

        public void Include(System.ComponentModel.INotifyPropertyChanged changed)
        {
            changed.PropertyChanged += (s, e) =>
            {
                var test = e.PropertyName;
            };
        }

        public void Include(MvxTaskBasedBindingContext context)
        {
            context.Dispose();
            var context2 = new MvxTaskBasedBindingContext();
            context2.Dispose();
        }
    }
}
