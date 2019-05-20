using System;
using System.Linq;
using Xamarin.Forms;
using Tools;

namespace Controls
{
    public class CustomProgressBar : Frame
    {
        public bool Bordered { get; set; }

        public double Progress { get; set; }

        public Color ProgressBarColor { get; set; }

        public Color TextColor { get; set; }

        public bool DisplayPercent { get; set; }

        public bool ProgressIsChanging { get; set; }

        public CustomProgressBar()
        {
            ProgressBarColor = Color.FromHex("#7e9c55");
            BackgroundColor = Color.FromHex("#abd373");
            BorderColor = Color.FromHex("#61882b");
            TextColor = Color.White;
            CornerRadius = 2;
            Padding = new Thickness(2);
        }

        public CustomProgressBar(Color progressBarColor, Color fillColor, Color borderColor, Color textColor, bool displayPercent, bool bordered, double cornerRadius = 2, int borderWidth = 2)
        {
            try
            {
                HasShadow = false;
                Bordered = bordered;
                DisplayPercent = displayPercent;
                ProgressBarColor = progressBarColor;
                BackgroundColor = fillColor;
                BorderColor = borderColor;
                TextColor = textColor;
                CornerRadius = (float)cornerRadius;

                SetProgress(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetProgress(double progress, string text = "")
        {
            try
            {
                Progress = progress;

                if (Content == null)
                {
                    var backgroundLayer = new RelativeLayout
                    {
                        BackgroundColor = Color.Transparent,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    var progressBarLayer = new Frame { BackgroundColor = this.ProgressBarColor, BorderColor = this.ProgressBarColor, CornerRadius = this.CornerRadius - 2, HasShadow = false };

                    backgroundLayer.Children.Add(progressBarLayer,
                                                Constraint.RelativeToParent((parent) => { return 0; }),
                                                Constraint.RelativeToParent((parent) => { return 0; }),
                                                Constraint.RelativeToParent((parent) => { return parent.Width * this.Progress; }),
                                                Constraint.RelativeToParent((parent) => { return parent.Height; }));

                    var progressText = new Label
                    {
                        Text = DisplayPercent ? string.IsNullOrEmpty(text) ? $"{(int)(Math.Min(Progress, 1) * 100)}%" : text : "",
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        TextColor = this.TextColor,
                        FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
                    };

                    backgroundLayer.Children.Add(progressText,
                                            Constraint.RelativeToParent((parent) => { return 0; }),
                                            Constraint.RelativeToParent((parent) => { return 0; }),
                                            Constraint.RelativeToParent((parent) => { return parent.Width; }),
                                            Constraint.RelativeToParent((parent) => { return parent.Height; }));

                    Content = backgroundLayer;

                    Padding = new Thickness(2);

                    BorderColor = Bordered ? BorderColor : Color.Transparent;
                }
                else
                {
                    if (!(Content is RelativeLayout backgroundLayer)) return;

                    var progressLayer = backgroundLayer.Children.OfType<Frame>().FirstOrDefault();

                    if (progressLayer == null) return;

                    var progressLayerConstraint = BoundsConstraint.FromExpression(() => new Rectangle(0, 0, backgroundLayer.Width * Math.Min(Progress, 1), backgroundLayer.Height), new View[0]);

                    RelativeLayout.SetBoundsConstraint(progressLayer, progressLayerConstraint);

                    var progressText = backgroundLayer.Children.OfType<Label>().FirstOrDefault();

                    if (progressText == null) return;

                    progressText.IsVisible = false;
                    progressText.IsVisible = true;
                    new ExtendedTimer(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(10), () => {
                        progressText.Text = DisplayPercent ? string.IsNullOrEmpty(text) ? $"   {(int)(Math.Min(Progress, 1) * 100)}%   " : $"   {text}   " : "";
                    }).Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
