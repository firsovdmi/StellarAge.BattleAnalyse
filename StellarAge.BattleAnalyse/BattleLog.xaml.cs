using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StellarAge.BattleAnalyse
{
    /// <summary>
    /// Interaction logic for BattleLog.xaml
    /// </summary>
    public partial class BattleLog : Window
    {
        public BattleLog()
        {
            InitializeComponent();
        }
        public static RenderTargetBitmap GetImage(Window view)
        {
            var size = new Size(view.ActualWidth, view.ActualHeight);
            if (size.IsEmpty)
                return null;

            var result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);

            var drawingvisual = new DrawingVisual();
            using (var context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(new Point(), size));
                context.Close();
            }

            result.Render(drawingvisual);
            return result;
        }
        public static void SaveAsPng(RenderTargetBitmap src, Stream outputStream)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));

            encoder.Save(outputStream);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var image = GetImage(this);
            //var fileStream = File.Create("D:\\Tmp\\Log.png");
            //SaveAsPng(image, fileStream);
            //fileStream.Close();

            SnapShotPNG(LogView.LogList,new Uri( "D:\\Tmp\\Log1.png"),1);
            var border = (Border)VisualTreeHelper.GetChild(LogView.LogList, 0);

            var scrl= (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
            scrl.PageDown();
            SnapShotPNG(LogView.LogList, new Uri("D:\\Tmp\\Log2.png"), 1);
        }
        void SaveToBmp(FrameworkElement visual, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, fileName, encoder);
        }

        // and so on for other encoders (if you want)


        void SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            var bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }
        public void SnapShotPNG( UIElement source, Uri destination, int zoom)
        {
            try
            {
                var actualHeight = source.RenderSize.Height;
                var actualWidth = source.RenderSize.Width;

                var renderHeight = actualHeight * zoom;
                var renderWidth = actualWidth * zoom;

                var renderTarget = new RenderTargetBitmap((int)renderWidth, (int)renderHeight, 96, 96, PixelFormats.Pbgra32);
                var sourceBrush = new VisualBrush(source);

                var drawingVisual = new DrawingVisual();
                var drawingContext = drawingVisual.RenderOpen();

                using (drawingContext)
                {
                    drawingContext.PushTransform(new ScaleTransform(zoom, zoom));
                    drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(actualWidth, actualHeight)));
                }
                renderTarget.Render(drawingVisual);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));
                using (var stream = new FileStream(destination.LocalPath, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(stream);
                }
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
