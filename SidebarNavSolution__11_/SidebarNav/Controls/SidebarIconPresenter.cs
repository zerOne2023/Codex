using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SidebarNav.ViewModels;

namespace SidebarNav.Controls
{
    /// <summary>
    /// 图标呈现器 —— 自动根据 IconType 显示 Path/FontIcon/Image
    /// </summary>
    public class SidebarIconPresenter : Control
    {
        static SidebarIconPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarIconPresenter),
                new FrameworkPropertyMetadata(typeof(SidebarIconPresenter)));
        }

        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register(nameof(IconType), typeof(IconType),
                typeof(SidebarIconPresenter), new PropertyMetadata(IconType.None));

        public IconType IconType
        {
            get => (IconType)GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        public static readonly DependencyProperty PathDataProperty =
            DependencyProperty.Register(nameof(PathData), typeof(Geometry),
                typeof(SidebarIconPresenter), new PropertyMetadata(null));

        public Geometry PathData
        {
            get => (Geometry)GetValue(PathDataProperty);
            set => SetValue(PathDataProperty, value);
        }

        public static readonly DependencyProperty FontIconGlyphProperty =
            DependencyProperty.Register(nameof(FontIconGlyph), typeof(string),
                typeof(SidebarIconPresenter), new PropertyMetadata(null));

        public string FontIconGlyph
        {
            get => (string)GetValue(FontIconGlyphProperty);
            set => SetValue(FontIconGlyphProperty, value);
        }

        public static readonly DependencyProperty FontIconFamilyProperty =
            DependencyProperty.Register(nameof(FontIconFamily), typeof(FontFamily),
                typeof(SidebarIconPresenter), new PropertyMetadata(new FontFamily("Segoe MDL2 Assets")));

        public FontFamily FontIconFamily
        {
            get => (FontFamily)GetValue(FontIconFamilyProperty);
            set => SetValue(FontIconFamilyProperty, value);
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(string),
                typeof(SidebarIconPresenter), new PropertyMetadata(null));

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(double),
                typeof(SidebarIconPresenter), new PropertyMetadata(20.0));

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public static readonly DependencyProperty IconBrushProperty =
            DependencyProperty.Register(nameof(IconBrush), typeof(Brush),
                typeof(SidebarIconPresenter), new PropertyMetadata(null));

        /// <summary>图标颜色（Path / FontIcon 有效）</summary>
        public Brush IconBrush
        {
            get => (Brush)GetValue(IconBrushProperty);
            set => SetValue(IconBrushProperty, value);
        }
    }
}
