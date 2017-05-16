using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UrlParser.Presentation.Commands;
using UrlParser.Presentation.Models;
using UrlParser.Presentation.Services;

namespace UrlParser.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogWindowService _dialogWindowService;

        private readonly CommandBase _loadFileCommand;

        private bool _isInProgress;
        private int _progressMaximum;
        private int _progressValue;
        private string _fileName;
        private readonly ObservableCollection<UrlResultModel> _result;

        private readonly Regex AnchorRegEx = new Regex(@"<a.*?>(.*)?</a>", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public ICommand LoadFileCommand
        {
            get
            {
                return _loadFileCommand;
            }
        }

        public bool IsInProgress
        {
            get
            {
                return _isInProgress;
            }
            private set
            {
                if (_isInProgress != value)
                {
                    _isInProgress = value;
                    OnPropertyChanged(() => IsInProgress);
                }

                _isInProgress = value;
            }
        }

        public int ProgressMaximum
        {
            get
            {
                return _progressMaximum;
            }
            private set
            {
                if (_progressMaximum != value)
                {
                    _progressMaximum = value;
                    OnPropertyChanged(() => ProgressMaximum);
                }

                _progressMaximum = value;
            }
        }

        public int ProgressValue
        {
            get
            {
                return _progressValue;
            }
            private set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    OnPropertyChanged(() => ProgressValue);
                }

                _progressValue = value;
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
            private set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged(() => FileName);
                }

                _fileName = value;
            }
        }


        public ObservableCollection<UrlResultModel> Result
        {
            get
            {
                return _result;
            }
        }

        public MainViewModel(IDialogWindowService dialogWindowService)
        {
            if (dialogWindowService == null)
            {
                throw new ArgumentNullException(nameof(dialogWindowService));
            }

            _dialogWindowService = dialogWindowService;

            _loadFileCommand = new ModelCommand<object>(LoadFileAsync, CanExecuteLoadFile);

            _result = new ObservableCollection<UrlResultModel>();
        }

        private async void LoadFileAsync(object param)
        {
            var dialog = _dialogWindowService.CreateOpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                FileName = dialog.FileName;

                // TODO: Large file support
                string[] lines = File.ReadAllLines(FileName);

                StartProgress(lines.Length);

                List<Task> processingTasks = new List<Task>();

                // Load data
                foreach (string line in lines)
                {
                    processingTasks.Add(Task.Run(async () =>
                    {
                        Uri url;
                        if (Uri.TryCreate(line, UriKind.Absolute, out url))
                        {
                            var resultModel = await ParseUrlAsync(url);

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                _result.Add(resultModel);
                            });
                        }

                        IncrementProgress();
                    }));
                }

                await Task.WhenAll(processingTasks);

                // Highlight max
                if (_result.Count > 0)
                {
                    int max = _result.Max(x => x.AnchorCount.GetValueOrDefault());

                    if (max > 0)
                    {
                        foreach (var result in _result)
                        {
                            if (result.AnchorCount.GetValueOrDefault() > 0 &&
                                (result.AnchorCount / max) > .9)
                            {
                                result.IsHighlighted = true;
                            }
                        }
                    }
                }

                StopProgress();
            }
        }

        private async Task<UrlResultModel> ParseUrlAsync(Uri url)
        {
            var resultModel = new UrlResultModel()
            {
                Url = url.ToString()
            };

            using (WebClient c = new WebClient())
            {
                try
                {
                    string body = await c.DownloadStringTaskAsync(url);

                    if (!string.IsNullOrEmpty(body))
                    {
                        resultModel.AnchorCount = AnchorRegEx.Matches(body).Count;
                    }
                }
                catch (ArgumentException e)
                {
                    resultModel.Error = e.Message;
                }
                catch (WebException e)
                {
                    resultModel.Error = e.Message;
                }
            }

            return resultModel;
        }

        private void StartProgress(int maximum)
        {
            IsInProgress = true;
            ProgressMaximum = maximum;
            ProgressValue = default(int);

            _result.Clear();

            _loadFileCommand.OnCanExecuteChanged();
        }

        private void IncrementProgress()
        {
            if (!IsInProgress)
            {
                throw new InvalidOperationException();
            }
            ProgressValue++;
        }

        private void StopProgress()
        {
            IsInProgress = false;
            ProgressMaximum = default(int);
            ProgressValue = default(int);

            _loadFileCommand.OnCanExecuteChanged();
        }

        private bool CanExecuteLoadFile(object param)
        {
            return !IsInProgress;
        }
    }
}
