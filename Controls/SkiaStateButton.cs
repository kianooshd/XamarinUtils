using System;
using System.Reflection;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using System.IO;
using SkiaSharp;

namespace Controls
{
    public class SkiaStateButton : RelativeLayout
    {
        ExtendedState extendedState;
        public ExtendedState ExtendedState
        {
            get
            {
                return extendedState;
            }
            set
            {
                extendedState = value;
                OnPropertyChanged();
            }
        }

        private bool TouchedDown { get; set; }

        public Assembly Assembly { get; set; }

        public string NormalImageSource { get; set; }

        public string NormalTappedImageSource { get; set; }

        public string CorrectImageSource { get; set; }

        public string CorrectTappedImageSource { get; set; }

        public string WrongImageSource { get; set; }

        public string WrongTappedImageSource { get; set; }

        public string DisabledImageSource { get; set; }

        SKCanvasView Image { get; set; }

        public event EventHandler Tapped;

        double xOffset;
        double yOffset;

        public SkiaStateButton()
        {
            try
            {
                Image = new SKCanvasView();

                Image.PaintSurface += Image_PaintSurface;

                Children.Add(Image,
                    Constraint.RelativeToParent((parent) => { return 0; }),
                    Constraint.RelativeToParent((parent) => { return 0; }),
                    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    Constraint.RelativeToParent((parent) => { return parent.Height; }));

                GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(Tap) });
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;
                GestureRecognizers.Add(panGestureRecognizer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (!(sender is SkiaStateButton skiaButton)) return;

            if (!skiaButton.IsEnabled) return;

            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    xOffset = e.TotalX;
                    yOffset = e.TotalY;
                    break;

                case GestureStatus.Completed:

                    if (Math.Abs(xOffset) < (0.5 * this.Width) && Math.Abs(yOffset) < (0.5 * this.Height))
                    {
                        Tap();
                    }

                    break;
            }
        }

        void Image_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            var width = e.Info.Width;
            var height = e.Info.Height;

            canvas.Clear();

            if (Assembly == null) return;

            try
            {
                var filename = "";

                if (!IsEnabled) filename = DisabledImageSource;
                else
                {
                    switch (extendedState)
                    {
                        case ExtendedState.Correct:
                            filename = TouchedDown ? CorrectTappedImageSource : CorrectImageSource;
                            break;
                        case ExtendedState.Wrong:
                            filename = TouchedDown ? WrongTappedImageSource : WrongImageSource;
                            break;
                        case ExtendedState.Normal:
                        default:
                            filename = TouchedDown ? NormalTappedImageSource : NormalImageSource;
                            break;
                    }
                }

                using (Stream stream = Assembly.GetManifestResourceStream(filename))
                using (SKManagedStream skStream = new SKManagedStream(stream))
                using (SKBitmap bitmap = SKBitmap.Decode(skStream))
                using (SKBitmap resizedbitmap = bitmap.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium))
                {
                    canvas.DrawBitmap(resizedbitmap ?? bitmap, 0, 0);
                }
            }
            catch (Exception ex)
            {
                canvas.Clear();
                throw ex;
            }
        }

        private void Tap()
        {
            if (!this.IsEnabled) return;

            RaiseTapped();

            TouchUp();
        }

        public void TouchDown()
        {
            TouchedDown = true;
            OnPropertyChanged();
        }

        public void TouchUp()
        {
            TouchedDown = false;
            OnPropertyChanged();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (Image != null) Image.InvalidateSurface();
        }

        private void RaiseTapped()
        {
            Tapped?.Invoke(this, EventArgs.Empty);
        }


    }
}
