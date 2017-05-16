namespace UrlParser.Presentation.Models
{
    public class UrlResultModel : ObservableObject
    {
        private string _url;
        private int? _anchorCount;
        private string _error;
        private bool _isHighlighted;

        public string Url
        {
            get { return _url; }
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged(() => Url);
                }
            }
        }

        public int? AnchorCount
        {
            get { return _anchorCount; }
            set
            {
                if (_anchorCount != value)
                {
                    _anchorCount = value;
                    OnPropertyChanged(() => AnchorCount);
                }
            }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                if (_error != value)
                {
                    _error = value;
                    OnPropertyChanged(() => Error);
                }
            }
        }

        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    OnPropertyChanged(() => IsHighlighted);
                }
            }
        }
    }
}
