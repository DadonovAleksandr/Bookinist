using Bookinist.DAL.Entityes;
using Bookinist.Infrastructure.Commands;
using Bookinist.Interfaces;
using Bookinist.Model.AppSettings.AppConfig;
using Bookinist.Service.UserDialogService;
using Bookinist.ViewModels.Base;
using ProjectVersionInfo;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Bookinist.ViewModels.MainWindowVm
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private readonly IAppConfig _appConfig;
        private readonly IUserDialogService _userDialogService;
        private readonly IRepository<Book> _booksRepository;

        /* ------------------------------------------------------------------------------------------------------------ */
        public MainWindowViewModel(IUserDialogService userDialogService,
            IRepository<Book> booksRepository)
        {
            _log.Debug($"Вызов конструктора {GetType().Name}");
            _appConfig = AppConfig.GetConfigFromDefaultPath();
            _userDialogService = userDialogService;
            
            var prjVersion = new ProjectVersion(Assembly.GetExecutingAssembly());
            Title = $"{AppConst.Get().AppDesciption} {prjVersion.Version}";

            _booksRepository = booksRepository;
            var books = _booksRepository.Items.Take(10).ToArray();

            #region Commands
            Exit = new RelayCommand(OnExitExecuted, CanExitExecute);
            #endregion

        }

        /// <summary>
        /// Действия выполняемые при закрытии основной формы
        /// </summary>
        public void OnExit()
        {
            //_projectConfigurationRepository?.Save();
        }
        /* ------------------------------------------------------------------------------------------------------------ */
        #region Commands

        #region Exit
        public ICommand Exit { get; }
        private void OnExitExecuted(object p) => Application.Current.Shutdown();
        private bool CanExitExecute(object p) => true;
        #endregion

        #endregion

        /* ------------------------------------------------------------------------------------------------------------ */

        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title { get => Get<string>(); set => Set(value); }

    }
}