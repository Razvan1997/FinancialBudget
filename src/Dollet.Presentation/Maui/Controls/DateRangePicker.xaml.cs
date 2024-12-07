namespace Dollet.Controls;

public partial class DateRangePicker : ContentView
{
    public static readonly BindableProperty DateFromProperty =
        BindableProperty.Create(nameof(DateFrom), typeof(DateTime), typeof(DateRangePicker), default(DateTime), BindingMode.TwoWay);

    public static readonly BindableProperty DateToProperty =
        BindableProperty.Create(nameof(DateTo), typeof(DateTime), typeof(DateRangePicker), default(DateTime), BindingMode.TwoWay);

    public static readonly BindableProperty MaxDateFromProperty =
        BindableProperty.Create(nameof(MaxDateTo), typeof(DateTime), typeof(DateRangePicker), default(DateTime), BindingMode.TwoWay);

    public static readonly BindableProperty MaxDateToProperty =
        BindableProperty.Create(nameof(MaxDateTo), typeof(DateTime), typeof(DateRangePicker), default(DateTime), BindingMode.TwoWay);

    public DateTime DateFrom
    {
        get { return (DateTime)GetValue(DateFromProperty); }
        set { SetValue(DateFromProperty, value); }
    }

    public DateTime DateTo
    {
        get { return (DateTime)GetValue(DateToProperty); }
        set { SetValue(DateToProperty, value); }
    }

    public DateTime MaxDateFrom
    {
        get { return (DateTime)GetValue(MaxDateFromProperty); }
        set { SetValue(MaxDateFromProperty, value); }
    }

    public DateTime MaxDateTo
    {
        get { return (DateTime)GetValue(MaxDateToProperty); }
        set { SetValue(MaxDateToProperty, value); }
    }

    public DateRangePicker()
    {
        InitializeComponent();
    }
}