using ImageProcessor;
using System.IO;

namespace SourceEngine.Demo.Heatmaps.Compatibility
{
    public class ImageProcessorExtender
    {
        public ImageProcessorExtender()
        { }

        public void BlurImage(string imageFilepath, string outputFilepath)
        {
            using (var imageFactory = new ImageFactory())
            {
                imageFactory.Load(imageFilepath).GaussianBlur(5).Save(outputFilepath);

                //ImageProcessor.Imaging.ImageLayer over = new ImageProcessor.Imaging.ImageLayer();
                //over.Image = new Bitmap(tempPath);

                //imageFactory.Load(background)
                //    .Overlay(over)
                //    .Save(Environment.CurrentDirectory + "/temp/test2.png");

                //Debug.Log("Saved final map to {0}", path);

            }
        }
    }
}
