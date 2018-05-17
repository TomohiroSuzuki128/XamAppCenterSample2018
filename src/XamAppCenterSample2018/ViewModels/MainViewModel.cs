using MvvmCross.Commands;
using MvvmCross.ViewModels;
using XamAppCenterSample2018.Services.Interfaces;

namespace XamAppCenterSample2018.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        string inputText = string.Empty;
        public string InputText
        {
            get { return inputText; }
            set
            {
                inputText = value;
                RaisePropertyChanged();
            }
        }

        string translatedText = string.Empty;
        public string TranslatedText
        {
            get { return translatedText; }
            set
            {
                translatedText = value;
                RaisePropertyChanged();
            }
        }

        public IMvxAsyncCommand TranslateCommand { get; private set; }

        public MainViewModel(ITranslateService translateService) : base()
        {
            TranslateCommand = new MvxAsyncCommand(async () =>
            {
                TranslatedText = await translateService.Translate(InputText);
            });
        }

    }
}
