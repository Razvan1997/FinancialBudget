namespace Dollet.Views;

public partial class RoundedTopElement : ContentView
{
    public static readonly BindableProperty DataProperty =
    BindableProperty.Create(nameof(Data), typeof(string), typeof(RoundedTopElement), "M 0,0 L 100,0 C 50,30 0,0 0,0 Z");

    public string Data
    {
        get => (string)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public RoundedTopElement()
    {
        InitializeComponent();

        BindingContext = this;
    }
}