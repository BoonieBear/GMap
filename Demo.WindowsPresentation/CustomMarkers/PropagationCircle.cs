using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using Demo.WindowsPresentation.Controls;
using GMap.NET.WindowsPresentation;

namespace Demo.WindowsPresentation.CustomMarkers
{
    public class PropagationCircle : FrameworkElement
    {
        public readonly Popup Popup = new Popup();
        public readonly GMapMarker Marker;

        public PropagationCircle(GMapMarker m, Brush background)
        {
            Marker = m;
            Marker.ZIndex = 100;

            Popup.AllowsTransparency = true;
            Popup.PlacementTarget = this;
            Popup.Placement = PlacementMode.Mouse;

            SizeChanged += new SizeChangedEventHandler(PropagationCircle_SizeChanged);
            Loaded += new RoutedEventHandler(OnLoaded);

            Text = "?";

            myPen.DashCap = PenLineCap.Round;
            myPen.DashStyle = new DashStyle(new Double[] { 4.0F, 2.0F, 1.0F, 3.0F },0.0);
            myPen.Thickness = 2;
            myPen.Freeze();

            RenderTransform = scale;

            Width = Height = 10;

            Background = background;

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //UpdateVisual(true);
        }

        private void PropagationCircle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new System.Windows.Point(-e.NewSize.Width / 2, -e.NewSize.Height / 2);
            scale.CenterX = -Marker.Offset.X;
            scale.CenterY = -Marker.Offset.Y;
        }
        private ScaleTransform scale = new ScaleTransform(1, 1);
        private DrawinFx dv = null;
        public DropShadowEffect ShadowEffect;

        static readonly Typeface Font = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
        FormattedText FText;

        private Brush background = Brushes.Blue;
        public Brush Background
        {
            get
            {
                return background;
            }
            set
            {
                if (background != value)
                {
                    background = value;
                    IsChanged = true;
                }
            }
        }

        private Brush foreground = Brushes.White;
        public Brush Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                if (foreground != value)
                {
                    foreground = value;
                    IsChanged = true;

                    ForceUpdateText();
                }
            }
        }

        private Pen stroke = new Pen(Brushes.Blue, 2.0);
        public Pen Stroke
        {
            get
            {
                return stroke;
            }
            set
            {
                if (stroke != value)
                {
                    stroke = value;
                    IsChanged = true;
                }
            }
        }
        public bool IsChanged = true;
        private Pen myPen = new Pen(Brushes.Blue, 2.0);


        public double FontSize = 16;
        void ForceUpdateText()
        {
            FText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Font, FontSize, Foreground);
            IsChanged = true;
        }

        string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    ForceUpdateText();
                }
            }
        }

        Visual _child;
        public virtual Visual Child
        {
            get
            {
                return _child;
            }
            set
            {
                if (_child != value)
                {
                    if (_child != null)
                    {
                        RemoveLogicalChild(_child);
                        RemoveVisualChild(_child);
                    }

                    if (value != null)
                    {
                        AddVisualChild(value);
                        AddLogicalChild(value);
                    }

                    // cache the new child
                    _child = value;

                    InvalidateVisual();
                }
            }
        }

        public bool UpdateVisual(bool forceUpdate)
        {
            if (forceUpdate || IsChanged)
            {
                Child = Create();
                IsChanged = false;
                return true;
            }

            return false;
        }

        public void Action(string text,float radius,TimeSpan ts)
        {
            countCreate++;
            FText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Font, FontSize, Foreground);
            DoubleAnimationUsingKeyFrames myAnimation =new DoubleAnimationUsingKeyFrames();
            var keyFrames = myAnimation.KeyFrames;
            keyFrames.Add(new SplineDoubleKeyFrame(0, TimeSpan.FromSeconds(0)));
            keyFrames.Add(new SplineDoubleKeyFrame(radius, TimeSpan.FromSeconds(0.5), new KeySpline(0, 0, 1, 1)));
            keyFrames.Add(new SplineDoubleKeyFrame(0, TimeSpan.FromSeconds(0.5)));
            myAnimation.RepeatBehavior =new RepeatBehavior(ts);
            //myAnimation.Duration = ts;

            // Create a clock the for the animation.
            AnimationClock myClock = myAnimation.CreateClock();
            var square = new DrawinFx();
            using (DrawingContext dc = square.RenderOpen())
            {
                dc.DrawEllipse(null, Stroke, new Point(Width / 2, Height / 2), Width / 2 + Stroke.Thickness / 2, Height / 2 + Stroke.Thickness / 2);
                dc.DrawEllipse(Background, null, new Point(Width / 2, Height / 2), Width / 2, Height / 2);
                dc.DrawEllipse(null, myPen, new Point(Width / 2, Height / 2), null, Width / 2, myClock, Height / 2, myClock);
                
                dc.DrawText(FText, new Point(-FText.Width/2, -FText.Height));
            }
            Child = square;
        }
        int countCreate = 0;
        private DrawingVisual Create()
        {
            countCreate++;

            var square = new DrawinFx();
           
            using (DrawingContext dc = square.RenderOpen())
            {
                //dc.DrawEllipse(null, Stroke, new Point(Width / 2, Height / 2), Width / 2 + Stroke.Thickness / 2, Height / 2 + Stroke.Thickness / 2);
                //dc.DrawEllipse(Background, null, new Point(Width / 2, Height / 2), Width / 2,  Height / 2);
                dc.DrawImage(new ImageSource());
                dc.DrawText(FText, new Point(0 , 0 ));
            }

            return square;
        }

        #region Necessary Overrides -- Needed by WPF to maintain bookkeeping of our hosted visuals
        protected override int VisualChildrenCount
        {
            get
            {
                return (Child == null ? 0 : 1);
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return Child;
        }
        #endregion
    }
    public class DrawinFx : DrawingVisual
    {
        public static readonly DependencyProperty EffectProperty = DependencyProperty.Register("Effect", typeof(Effect), typeof(DrawinFx),
                                        new FrameworkPropertyMetadata(null, (FrameworkPropertyMetadataOptions.AffectsRender), new PropertyChangedCallback(OnEffectChanged)));
        public Effect Effect
        {
            get
            {
                return (Effect)GetValue(EffectProperty);
            }
            set
            {
                SetValue(EffectProperty, value);
            }
        }

        private static void OnEffectChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DrawinFx drawingVisualFx = o as DrawinFx;
            if (drawingVisualFx != null)
            {
                drawingVisualFx.setMyProtectedVisualEffect((Effect)e.NewValue);
            }
        }

        private void setMyProtectedVisualEffect(Effect effect)
        {
            VisualEffect = effect;
        }
    }
}
