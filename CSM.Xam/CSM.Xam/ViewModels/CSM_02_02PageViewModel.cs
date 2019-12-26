using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CSM.Xam.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Navigation;

namespace CSM.Xam.ViewModels
{
    public class CSM_02_02PageViewModel : ViewModelBase
    {
        public CSM_02_02PageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            Title = "Chỉnh sửa hình ảnh mặt hàng";          
        }
        #region Bindprop

        #region ImagePathBindProp
        private string _ImagePathBindProp = string.Empty;
        public string ImagePathBindProp
        {
            get { return _ImagePathBindProp; }
            set { SetProperty(ref _ImagePathBindProp, value); }
        }
        #endregion

        #endregion

        #region Command

        #region SaveCommand

        public DelegateCommand<object> SaveCommand { get; private set; }
        private async void OnSave(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(ImagePathBindProp))
            {
                await PageDialogService.DisplayAlertAsync("Lỗi", "Bạn chưa chọn ảnh.", "Đóng");
                return;
            }

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day

                NavigationParameters navigaParam = new NavigationParameters();
                navigaParam.Add(Keys.IMAGE, ImagePathBindProp);
                await NavigationService.GoBackAsync(navigaParam);

            }
            catch (Exception e)
            {
                await ShowError(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitSaveCommand()
        {
            SaveCommand = new DelegateCommand<object>(OnSave);
            SaveCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region TakeImageCommand

        public DelegateCommand<object> TakeImageCommand { get; private set; }
        private async void OnTakeImage(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await PageDialogService.DisplayAlertAsync("Lỗi", "Điện thoại không hổ trợ máy ảnh", "Đóng");
                return;
            }

            IsBusy = true;

            var fileName = $"{Guid.NewGuid().ToString()}.jpg";
            var filePath = "";

            try
            {
                // Thuc hien cong viec tai day
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                    storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (storageStatus == PermissionStatus.Granted && cameraStatus == PermissionStatus.Granted)
                {
                    try
                    {

                        var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                        {
                            Directory = "my_images",
                            Name = fileName,
                            PhotoSize = PhotoSize.Full,
                            CompressionQuality = 92,
                            DefaultCamera = CameraDevice.Front,
                            SaveToAlbum = true,
                        });

                        if (file != null)
                        {
                            filePath = file.Path;
                        }
                    }
                    catch (Exception ex)
                    {
                        await ShowError(ex);
                    }
                }
                else if (storageStatus != PermissionStatus.Unknown || cameraStatus != PermissionStatus.Unknown)
                {
                    await PageDialogService.DisplayAlertAsync("Yêu cầu bị từ chối", "Không thể chụp ảnh.", "Đóng");
                }

                if (string.IsNullOrEmpty(filePath) == false)
                {
                    ImagePathBindProp = filePath;
                }
                else
                {
                    ImagePathBindProp = "";
                }
            }
            catch (Exception e)
            {
                await ShowError(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitTakeImageCommand()
        {
            TakeImageCommand = new DelegateCommand<object>(OnTakeImage);
            TakeImageCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #region SelectImageCommand

        public DelegateCommand<object> SelectImageCommand { get; private set; }
        private async void OnSelectImage(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await PageDialogService.DisplayAlertAsync("Lỗi", "Điện thoại không hổ trợ máy ảnh", "Đóng");
                return;
            }

            var filePath = "";

            IsBusy = true;

            try
            {
                // Thuc hien cong viec tai day
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                    storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (storageStatus == PermissionStatus.Granted && cameraStatus == PermissionStatus.Granted)
                {
                    try
                    {

                        var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                        {

                            PhotoSize = PhotoSize.Full,
                            CompressionQuality = 92
                        });

                        if (file != null)
                        {
                            filePath = file.Path;
                        }
                    }
                    catch (Exception ex)
                    {
                        await ShowError(ex);
                    }
                }
                else if (storageStatus != PermissionStatus.Unknown || cameraStatus != PermissionStatus.Unknown)
                {
                    await PageDialogService.DisplayAlertAsync("Yêu cầu bị từ chối", "Không thể chụp ảnh.", "Đóng");
                }

                if (string.IsNullOrEmpty(filePath) == false)
                {
                    ImagePathBindProp = filePath;
                }
                else
                {
                    ImagePathBindProp = "";
                }
            }
            catch (Exception e)
            {
                await ShowError(e);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitSelectImageCommand()
        {
            SelectImageCommand = new DelegateCommand<object>(OnSelectImage);
            SelectImageCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        #endregion
    }
}
