/** @file NIMDeviceAPI.cs
  * @brief NIM VChat提供的音视频设备相关接口，使用前请先调用NIMVChatAPI.cs中Init
  * @copyright (c) 2015, NetEase Inc. All rights reserved
  * @author gq
  * @date 2015/12/8
  */

using System;
using System.Runtime.InteropServices;

namespace NIM
{
    public class DeviceAPI
    {
        //引用C中的方法（考虑到不同平台下的C接口引用方式差异，如[DllImport("__Internal")]，[DllImport("nimapi")]等） 
        #region NIM C SDK native methods

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_enum_device_devpath", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_vchat_enum_device_devpath(NIMDeviceType type, string json_extension, nim_vchat_enum_device_devpath_sync_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_start_device", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_vchat_start_device(NIMDeviceType type, string device_path, uint fps, string json_extension, nim_vchat_start_device_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_end_device", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_vchat_end_device(NIMDeviceType type, string json_extension);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_start_device", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_vchat_add_device_status_cb(NIMDeviceType type, nim_vchat_device_status_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_remove_device_status_cb", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_vchat_remove_device_status_cb(NIMDeviceType type);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_set_audio_data_cb", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_vchat_set_audio_data_cb(bool capture, string json_extension, nim_vchat_audio_data_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_set_video_data_cb", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_vchat_set_video_data_cb(bool capture, string json_extension, nim_vchat_video_data_cb_func cb, IntPtr user_data);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_set_audio_volumn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_vchat_set_audio_volumn(byte volumn, bool capture);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_get_audio_volumn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte nim_vchat_get_audio_volumn(bool capture);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_set_audio_input_auto_volumn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void nim_vchat_set_audio_input_auto_volumn(bool auto_volumn);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_get_audio_input_auto_volumn", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool nim_vchat_get_audio_input_auto_volumn();

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_custom_audio_data", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool nim_vchat_custom_audio_data(ulong time, IntPtr data, uint size, string json_extension);

        [DllImport(NIMGlobal.NIMNativeDLL, EntryPoint = "nim_vchat_custom_video_data", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool nim_vchat_custom_video_data(ulong time, IntPtr data, uint size, uint width, uint height, string json_extension);

        #endregion

        static nim_vchat_enum_device_devpath_sync_cb_func GetDeviceListCb = new nim_vchat_enum_device_devpath_sync_cb_func(GetDeviceListCallback);
        /// <summary>
        /// 遍历设备
        /// </summary>
        /// <param name="type">设备类型</param>
        /// <returns></returns>
        public static NIMDeviceInfoList GetDeviceList(NIMDeviceType type)
        {
            nim_vchat_enum_device_devpath(type, "", GetDeviceListCb, IntPtr.Zero);
            return device_list;
        }

        private static NIMDeviceInfoList device_list = null;
        private static void GetDeviceListCallback(bool ret, NIMDeviceType type, string json_extension, IntPtr user_data)
        {
            device_list = null;
            if (ret)
            {
                device_list = NIMDeviceInfoList.Deserialize(json_extension);
            }
        }

        static nim_vchat_start_device_cb_func StartDeviceCb = new nim_vchat_start_device_cb_func(StartDeviceCallback);
        /// <summary>
        /// 启动设备，同一NIMDeviceType下设备将不重复启动，不同的设备会先关闭前一个设备开启新设备
        /// </summary>
        /// <param name="type">设备类型</param>
        /// <param name="device_path">设备路径对应</param>
        /// <param name="fps">摄像头为采样频率（一般传电源频率取50）,其他NIMDeviceType无效（麦克风采样频率由底层控制，播放器采样频率也由底层控制）</param>
        /// <param name="handler">回调</param>
        public static void StartDevice(NIMDeviceType type, string device_path, uint fps, StartDeviceResultHandler handler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(handler);
            nim_vchat_start_device(type, device_path, fps, "", StartDeviceCb, ptr);
        }

        /// <summary>
        /// 启动设备回调
        /// </summary>
        /// <param name="type">设备类型</param>
        /// <param name="ret">是否成功</param>
        public delegate void StartDeviceResultHandler(NIMDeviceType type, bool ret);

        private static void StartDeviceCallback(NIMDeviceType type, bool ret, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<StartDeviceResultHandler>(user_data, type,ret);
        }

        /// <summary>
        /// 结束设备
        /// </summary>
        /// <param name="type">设备类型</param>
        public static void EndDevice(NIMDeviceType type)
        {
            nim_vchat_end_device(type, "");
        }

        static nim_vchat_device_status_cb_func DeviceStatusCb = new nim_vchat_device_status_cb_func(DeviceStatusCallback);
        /// <summary>
        /// 添加设备监听（摄像头和麦克风） 注意监听设备后底层会定时检查设备情况，在不需要监听后请移除
        /// </summary>
        /// <param name="type">设备类型（kNIMDeviceTypeAudioIn和kNIMDeviceTypeVideo有效）</param>
        /// <param name="handler">回调</param>
        public static void AddDeviceStatusCb(NIMDeviceType type, DeviceStatusHandler handler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(handler);
            nim_vchat_add_device_status_cb(type, DeviceStatusCb, ptr);
        }

        /// <summary>
        /// 设备状态通知
        /// </summary>
        /// <param name="type">设备类型</param>
        /// <param name="status">NIMDeviceStatus 的或值</param>
        /// <param name="device_path">重启设备时的设备路径</param>
        public delegate void DeviceStatusHandler(NIMDeviceType type, uint status, string device_path);

        private static void DeviceStatusCallback(NIMDeviceType type, uint status, string device_path, string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<DeviceStatusHandler>(user_data, device_path);
        }

        /// <summary>
        /// 移除设备监听（摄像头和麦克风）
        /// </summary>
        /// <param name="type">设备类型</param>
        public static void RemoveDeviceStatusCb(NIMDeviceType type)
        {
            nim_vchat_remove_device_status_cb(type);
        }


        static nim_vchat_audio_data_cb_func AudioDataCb = new nim_vchat_audio_data_cb_func(AudioDataCallback);
        /// <summary>
        /// 监听采集音频数据（可以不监听，通过启动设备kNIMDeviceTypeAudioOut由底层播放）
        /// </summary>
        /// <param name="handler">回调</param>
        public static void SetAudioCaptureDataCb(AudioDataHandler handler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(handler);
            nim_vchat_set_audio_data_cb(true, null, AudioDataCb, ptr);
        }

        /// <summary>
        /// 监听接收音频数据（可以不监听，通过启动设备kNIMDeviceTypeAudioOutChat由底层播放）
        /// </summary>
        /// <param name="handler">回调</param>
        public static void SetAudioReceiveDataCb(AudioDataHandler handler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(handler);
            nim_vchat_set_audio_data_cb(false, null, AudioDataCb, ptr);
        }
        /// <summary>
        /// 音频数据回调
        /// </summary>
        /// <param name="time">毫秒级时间戳</param>
        /// <param name="data">数据指针， PCM</param>
        /// <param name="size">数据长途sizeof(char)</param>
        /// <param name="rate">PCM数据的采样频率</param>
        public delegate void AudioDataHandler(ulong time, IntPtr data, uint size, int rate);
        private static void AudioDataCallback(ulong time, IntPtr data, uint size, string json_extension, IntPtr user_data)
        {
            if (user_data != IntPtr.Zero)
            {
                NIMCustomAudioDataInfo info = NIMCustomAudioDataInfo.Deserialize(json_extension);
                NimUtility.DelegateConverter.Invoke<AudioDataHandler>(user_data, time, data, size, info.SampleRate);
            }
        }

        static nim_vchat_video_data_cb_func VideoDataCb = new nim_vchat_video_data_cb_func(VideoDataCallback);
        /// <summary>
        /// 监听采集的视频数据
        /// </summary>
        /// <param name="handler">回调</param>
        public static void SetVideoCaptureDataCb(VideoDataHandler handler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(handler);
            nim_vchat_set_video_data_cb(true, null, VideoDataCb, ptr);
        }

        /// <summary>
        /// 监听接收的视频数据
        /// </summary>
        /// <param name="handler">回调</param>
        public static void SetVideoReceiveDataCb(VideoDataHandler handler)
        {
            var ptr = NimUtility.DelegateConverter.ConvertToIntPtr(handler);
            nim_vchat_set_video_data_cb(false, null, VideoDataCb, ptr);
        }

        /// <summary>
        /// 视频数据回调
        /// </summary>
        /// <param name="time">毫秒级时间戳</param>
        /// <param name="data">数据指针， ARGB</param>
        /// <param name="size">数据长途sizeof(char)</param>
        /// <param name="width">画面宽</param>
        /// <param name="rate">画面高</param>
        public delegate void VideoDataHandler(ulong time, IntPtr data, uint size, uint width, uint height);

        private static void VideoDataCallback(ulong time, IntPtr data, uint size, uint width, uint height,
            string json_extension, IntPtr user_data)
        {
            NimUtility.DelegateConverter.Invoke<VideoDataHandler>(user_data, time, data, size, width, height);
        }

        /// <summary>
        /// 音频采集音量，默认255
        /// </summary>
        public static byte audio_capture_volumn
        {
            get
            {
                return nim_vchat_get_audio_volumn(true);
            }
            set
            {
                nim_vchat_set_audio_volumn(value, true);
            }
        }

        /// <summary>
        /// 音频播放音量，默认255
        /// </summary>
        public static byte audio_play_volumn
        {
            get
            {
                return nim_vchat_get_audio_volumn(false);
            }
            set
            {
                nim_vchat_set_audio_volumn(value, false);
            }
        }

        /// <summary>
        /// 设置麦克风音量自动调节, 默认不自动调节
        /// </summary>
        public static bool audio_capture_auto_volumn
        {
            get
            {
                return nim_vchat_get_audio_input_auto_volumn();
            }
            set
            {
                nim_vchat_set_audio_input_auto_volumn(value);
            }
        }

        /// <summary>
        /// 自定义音频数据接口, 采样位深只支持16或32， kNIMDeviceSampleRate支持8000，16000，32000，44100
        /// </summary>
        /// <param name="time">时间毫秒级</param>
        /// <param name="data">音频数据pcm格式</param>
        /// <param name="size">data的数据长度 sizeof(char)</param>
        /// <param name="info">采样频和采样位深 默认如{"sample_rate":16000, "sample_bit":16}</param>
        /// <returns></returns>
        public static bool CustomAudioData(ulong time, IntPtr data, uint size, NIMCustomAudioDataInfo info)
        {
            string json_extension = info.Serialize();
            return nim_vchat_custom_audio_data(time, data, size, json_extension);
        }

        /// <summary>
        /// 自定义视频数据接口
        /// </summary>
        /// <param name="time">时间毫秒级</param>
        /// <param name="data">视频数据yuv420格式</param>
        /// <param name="size">data的数据长度 sizeof(char)</param>
        /// <param name="width">画面宽度</param>
        /// <param name="height">画面高度</param>
        /// <returns></returns>
        public static bool CustomVideoData(ulong time, IntPtr data, uint size, uint width, uint height)
        {
            return nim_vchat_custom_video_data(time, data, size, width, height, "");
        }

    }
}
