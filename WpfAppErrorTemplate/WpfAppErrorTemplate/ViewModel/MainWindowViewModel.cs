using Microsoft.VisualStudio.PlatformUI;

using System.ComponentModel;

namespace WpfAppErrorTemplate
{

    internal class MainWindowViewModel : ObservableObject, IDataErrorInfo
    {
        public MainWindowViewModel()
        {
            this.Email = "input your email";
        }

        private string eamil;
        public string Email 
        { 
            get => this.eamil; 
            set => this.SetProperty(ref this.eamil, value); 
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => this.errorMessage;
            set => this.SetProperty(ref this.errorMessage, value);
        }

        #region IDataErrorInfo

        public string Error { get;}

        public string this[string columnName] 
        { 
            get
            {
                string result = string.Empty;

                if (columnName == nameof(Email))
                {
                    if (string.IsNullOrEmpty(this.Email))
                    {
                        result = "Email is required";
                        this.ErrorMessage = result;
                    }
                    else
                    {
                        this.ErrorMessage = string.Empty;
                    }
                }

                return result;
            }
        }
        #endregion

    }
}
