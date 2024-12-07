namespace Dollet.Views;

public partial class CollectionEmptyView : ContentView
{
	public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CollectionEmptyView), defaultValue: string.Empty);

    public static readonly BindableProperty TitleSizeProperty =
        BindableProperty.Create(nameof(TitleSize), typeof(int), typeof(CollectionEmptyView), defaultValue: 20);

    public static readonly BindableProperty CaptionProperty =
        BindableProperty.Create(nameof(Caption), typeof(string), typeof(CollectionEmptyView), defaultValue: string.Empty);

    public static readonly BindableProperty CaptionSizeProperty =
        BindableProperty.Create(nameof(CaptionSize), typeof(int), typeof(CollectionEmptyView), defaultValue: 16);

    public string Title 
	{
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public int TitleSize
    {
        get { return (int)GetValue(TitleSizeProperty); }
        set { SetValue(TitleSizeProperty, value); }
    }

    public string Caption
    {
        get { return (string)GetValue(CaptionProperty); }
        set { SetValue(CaptionProperty, value); }
    }

    public int CaptionSize
    {
        get { return (int)GetValue(CaptionSizeProperty); }
        set { SetValue(CaptionSizeProperty, value); }
    }

    public CollectionEmptyView()
	{
		InitializeComponent();

        BindingContext = this;
	}
}