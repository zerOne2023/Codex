using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SidebarNav.Controls
{
    /// <summary>
    /// 搜索框控件 —— 带水印、清除按钮、搜索图标
    /// </summary>
    [TemplatePart(Name = PART_TextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_ClearButton, Type = typeof(Button))]
    public class SidebarSearchBox : Control
    {
        private const string PART_TextBox = "PART_TextBox";
        private const string PART_ClearButton = "PART_ClearButton";

        private TextBox _textBox;
        private Button _clearButton;

        static SidebarSearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarSearchBox),
                new FrameworkPropertyMetadata(typeof(SidebarSearchBox)));
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string),
                typeof(SidebarSearchBox), new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(nameof(Placeholder), typeof(string),
                typeof(SidebarSearchBox), new PropertyMetadata("Search…"));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty IsEmptyProperty =
            DependencyProperty.Register(nameof(IsEmpty), typeof(bool),
                typeof(SidebarSearchBox), new PropertyMetadata(true));

        public bool IsEmpty
        {
            get => (bool)GetValue(IsEmptyProperty);
            set => SetValue(IsEmptyProperty, value);
        }

        public static readonly DependencyProperty ClearCommandProperty =
            DependencyProperty.Register(nameof(ClearCommand), typeof(ICommand),
                typeof(SidebarSearchBox), new PropertyMetadata(null));

        public ICommand ClearCommand
        {
            get => (ICommand)GetValue(ClearCommandProperty);
            set => SetValue(ClearCommandProperty, value);
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SidebarSearchBox sb)
            {
                sb.IsEmpty = string.IsNullOrEmpty(e.NewValue as string);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _textBox = GetTemplateChild(PART_TextBox) as TextBox;
            _clearButton = GetTemplateChild(PART_ClearButton) as Button;

            if (_clearButton != null)
            {
                _clearButton.Click += (s, e) =>
                {
                    Text = string.Empty;
                    _textBox?.Focus();
                    ClearCommand?.Execute(null);
                };
            }
        }

        /// <summary>聚焦搜索框</summary>
        public void FocusSearch()
        {
            _textBox?.Focus();
        }
    }
}
