using CollageSample.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using CollageSample.ViewModels.Utils;

namespace CollageSample.ViewModels
{
    public class CollageViewModel : ViewModelBase
    {
        #region bindable properties

        #region User property
        private InstagramUser m_user;
        public InstagramUser User
        {
            get
            {
                return m_user;
            }
            private set
            {
                if (!object.Equals(m_user, value))
                {
                    m_user = value;
                    RaisePropertyChanged();
                    UpdateImages();
                }
            }
        }

        #endregion

        #region Images - most liked users images
        private List<InstagramImage> m_images;
        public List<InstagramImage> Images
        {
            get
            {
                return m_images;
            }

            private set
            {
                m_images = value;
                RaisePropertyChanged();
            }
        }

        private void UpdateImages()
        {
            if (null == User)
            {
                return;
            }

            InvokeWithProgress(GetImagesAsync());
        }

        private async System.Threading.Tasks.Task GetImagesAsync()
        {
            try
            {
                Images = await User.GetImagesAsync();
            }
            catch (System.Net.WebException wexc)
            {
                System.Diagnostics.Debug.WriteLine("{0} - {1}", wexc.Status, wexc.Message);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine("{0}", exc.Message);
            }
        }
        #endregion

        #region SendCollageCommand
        public ICommand SendCollageCommand
        {
            get;
            private set;
        }
        // TODO: really bad method:
        // 1) passing UI element into view-model isn't a good idea
        // 2) this is too complicated method
        // 3) result image is low res and very size limited
        // Needs another way to create collages.
        async Task SendCollageAsync(Windows.UI.Xaml.UIElement canvas)
        {
            var bitmap = new Windows.UI.Xaml.Media.Imaging.RenderTargetBitmap();
            await bitmap.RenderAsync(canvas);

            var fileName = string.Format("collage_{0}_{1}.jpg", User.Name, DateTime.Now.ToString("M_d_yy_h_mm_ss"));
            var file = await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync(fileName, 
                                        Windows.Storage.CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                var pixelBuffer = await bitmap.GetPixelsAsync();
                var localDpi = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().LogicalDpi;

                var randomStream = stream.AsRandomAccessStream();

                var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId, randomStream);
                encoder.SetPixelData(Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                                    Windows.Graphics.Imaging.BitmapAlphaMode.Ignore,
                                    (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight,
                                    localDpi, localDpi,
                                    pixelBuffer.ToArray());
                await encoder.FlushAsync();
            }

            Windows.ApplicationModel.Email.EmailMessage message = new Windows.ApplicationModel.Email.EmailMessage();
            message.Subject = "CollageMaker";
            message.Attachments.Add(new Windows.ApplicationModel.Email.EmailAttachment(fileName, file));
            await EmailManager.ShowComposeNewEmailAsync(message);

        }
        #endregion

        #endregion

        public CollageViewModel()
        {
            SendCollageCommand = new DelegateCommand<Windows.UI.Xaml.UIElement>((element) => 
                {
                    InvokeWithProgress(SendCollageAsync(element));
                });
        }

        public override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            User = e.Parameter as InstagramUser;
        }

    }
}
