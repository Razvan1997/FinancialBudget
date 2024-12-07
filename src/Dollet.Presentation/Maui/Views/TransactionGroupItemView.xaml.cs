namespace Dollet.Views
{
    public partial class TransactionGroupItemView : ContentView
    {
        public TransactionGroupItemView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(string), typeof(TransactionGroupItemView));

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(nameof(Color), typeof(Color), typeof(TransactionGroupItemView));

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty CategoryProperty =
            BindableProperty.Create(nameof(Category), typeof(string), typeof(TransactionGroupItemView));

        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }

        public static readonly BindableProperty PercentProperty =
            BindableProperty.Create(nameof(Percent), typeof(double), typeof(TransactionGroupItemView));

        public double Percent
        {
            get => (double)GetValue(PercentProperty);
            set => SetValue(PercentProperty, value);
        }

        public static readonly BindableProperty AmountProperty =
            BindableProperty.Create(nameof(Amount), typeof(double), typeof(TransactionGroupItemView));

        public double Amount
        {
            get => (double)GetValue(AmountProperty);
            set => SetValue(AmountProperty, value);
        }
    }
}