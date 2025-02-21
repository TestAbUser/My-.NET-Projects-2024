using DownloadManager.PresentationLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DownloadManager
{
    public class MainWindowAdapter : WindowAdapter
    {
        private readonly IMainViewModelFactory _vmFactory;
        private bool _initialized;

        public MainWindowAdapter(Window wpfWindow,
            IMainViewModelFactory vmFactory) : base(wpfWindow)
        {

            _vmFactory = vmFactory ??
                throw new ArgumentNullException(nameof(vmFactory));
        }

        public override void Close()
        {
            EnsureInitialized();
            base.Close();
        }

        public override IWindow CreateChild(object viewModel)
        {
            EnsureInitialized();
            return base.CreateChild(viewModel); 
        }

        public override void Show()
        {
            EnsureInitialized();
            base.Show();
        }

        public override bool? ShowDialogue()
        {
            EnsureInitialized();
            return base.ShowDialogue();
        }

        private void EnsureInitialized()
        {
            if (_initialized) {return; }

            var vm = _vmFactory.Create(this);
            WpfWindow.DataContext = vm;

            _initialized=true;
        }
    }
}









