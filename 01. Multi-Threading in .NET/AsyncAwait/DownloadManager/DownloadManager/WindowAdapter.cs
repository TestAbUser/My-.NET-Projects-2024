using DownloadManager.PresentationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;

namespace DownloadManager
{
    public class WindowAdapter : IWindow
    {
        private readonly Window _wpfWindow;

        public WindowAdapter(Window wpfWindow)
        {
            _wpfWindow = wpfWindow ??
                throw new ArgumentNullException(nameof(wpfWindow));
        }

        public virtual void Close()
        {
            _wpfWindow.Close();
        }

        public virtual IWindow CreateChild(object viewModel)
        {
            var auw = new AddUrlWindow();
            auw.Owner = _wpfWindow;
            auw.DataContext = viewModel;
            WindowAdapter.ConfigureBehaviour(auw);

            return new WindowAdapter(auw);
        }

        public virtual void Show()
        {
            _wpfWindow.Show();
        }

        public virtual bool? ShowDialogue()
        {
            return _wpfWindow.ShowDialog();
        }

        protected Window WpfWindow => _wpfWindow;

        private static void ConfigureBehaviour(AddUrlWindow auw)
        {
            auw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            auw.CommandBindings.Add(
                new CommandBinding(PresentationCommands.Accept, 
                (sender, e) => auw.DialogResult = true));
        }
    }
}




