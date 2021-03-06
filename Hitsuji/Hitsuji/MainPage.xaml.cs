using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.TextToSpeech;

namespace Hitsuji
{
    // ＢＧＭ再生のためのインターフェースの作成
    public interface IMediaPlayer
    {
        Task PlayAsync(string title);
        void Stop();
        void PlayNext(string title);
    }

    // スマホのスリープ制御のためのインターフェースの作成
    public interface IDeviceService
    {
        void DisableSleep();
        void EnableSleep();
    }

    // メインクラス
    public partial class MainPage : ContentPage
    {
        public Image[] _images;         // イメージ配列
        public bool counting;           // 数えてます
        public int cnt=1;               // 羊の数

        public MainPage()
        {
            int i;

            // もとからある初期化
            InitializeComponent();

            // おやすみの挨拶
            CrossTextToSpeech.Current.Speak(text: "おやすみなさい", pitch: (float)0.7, speakRate: (float)0.6);

            // スマホのスリープモードをオフにする
            DependencyService.Get<IDeviceService>().DisableSleep();

            // イメージ配列の格納
            Grid grid;
            grid = g;
            _images = new Image[24];
            for (i = 0; i < 24; i++) {
                _images[i] = new Image();
                _images[i].IsVisible = false;
                grid.Children.Add(_images[i], 1, 1);
            }
            _images[0].Source = ImageSource.FromResource("Hitsuji.Image.0.PNG");
            _images[1].Source = ImageSource.FromResource("Hitsuji.Image.1.PNG");
            _images[2].Source = ImageSource.FromResource("Hitsuji.Image.2.PNG");
            _images[3].Source = ImageSource.FromResource("Hitsuji.Image.3.PNG");
            _images[4].Source = ImageSource.FromResource("Hitsuji.Image.4.PNG");
            _images[5].Source = ImageSource.FromResource("Hitsuji.Image.5.PNG");
            _images[6].Source = ImageSource.FromResource("Hitsuji.Image.6.PNG");
            _images[7].Source = ImageSource.FromResource("Hitsuji.Image.7.PNG");
            _images[8].Source = ImageSource.FromResource("Hitsuji.Image.8.PNG");
            _images[9].Source = ImageSource.FromResource("Hitsuji.Image.9.PNG");
            _images[10].Source = ImageSource.FromResource("Hitsuji.Image.10.PNG");
            _images[11].Source = ImageSource.FromResource("Hitsuji.Image.11.PNG");
            _images[12].Source = ImageSource.FromResource("Hitsuji.Image.12.PNG");
            _images[13].Source = ImageSource.FromResource("Hitsuji.Image.13.PNG");
            _images[14].Source = ImageSource.FromResource("Hitsuji.Image.14.PNG");
            _images[15].Source = ImageSource.FromResource("Hitsuji.Image.15.PNG");
            _images[16].Source = ImageSource.FromResource("Hitsuji.Image.16.PNG");
            _images[17].Source = ImageSource.FromResource("Hitsuji.Image.17.PNG");
            _images[18].Source = ImageSource.FromResource("Hitsuji.Image.18.PNG");
            _images[19].Source = ImageSource.FromResource("Hitsuji.Image.19.PNG");
            _images[20].Source = ImageSource.FromResource("Hitsuji.Image.20.PNG");
            _images[21].Source = ImageSource.FromResource("Hitsuji.Image.21.PNG");
            _images[22].Source = ImageSource.FromResource("Hitsuji.Image.s0.png");
            _images[23].Source = ImageSource.FromResource("Hitsuji.Image.s1.png");
            _images[0].IsVisible = true;

            // スタート前ＢＧＭの再生
            DependencyService.Get<IMediaPlayer>().PlayAsync("music1");
        }

        public async void OnBtnClicked(Object o, EventArgs e)
        {
            if (counting == true) { // もし数え中だったらcntをリセット
                num.Text = "最初から数えなおします";
                cnt = 0;
            } else {                // そうじゃなかったらカウント開始
                DependencyService.Get<IMediaPlayer>().PlayNext("music2");
                counting = true;
                btn.Text = "Reset";
                while (cnt<=12000) {
                    num.Text = String.Format("羊が{0:D}匹", cnt);
                    HitsujiAnime();
                    await CrossTextToSpeech.Current.Speak(text: String.Format("羊が{0:D}匹", cnt), pitch: (float)0.7, speakRate: (float)0.6);
                    await Task.Delay(2500);
                    cnt++;
                    if (cnt == 5000) {
                        DependencyService.Get<IMediaPlayer>().PlayNext("music3");
                    }
                }
                counting = false;
                cnt = 1;
                Ending();
            }
        }

        // 羊を数えるアニメーション
        async void HitsujiAnime()
        {
            int i;

            // とりあえず全部消す
            for (i = 0; i < 22; i++)
            {
                _images[i].IsVisible = false;
            }

            // パタパタパタパタ
            for (i = 0; i < 21; i++)
            {
                _images[i].IsVisible = false;
                _images[i + 1].IsVisible = true;
                await Task.Delay(150);
            }
        }

        // エンディング
        async void Ending()
        {
            int i;

            // とりあえず全部消す
            for (i = 0; i < 22; i++) {
                _images[i].IsVisible = false;
            }

            // 柵の前まで飛んで
            for (i = 0; i < 7; i++) {
                _images[i].IsVisible = false;
                _images[i + 1].IsVisible = true;
                await Task.Delay(150);
            }

            // 睡眠開始
            num.Text = "羊が寝ている";
            _images[22].IsVisible = true;
            await CrossTextToSpeech.Current.Speak(text: "羊が寝ている", pitch: (float)0.7, speakRate: (float)0.6);

            // 無限ループでグゥグゥグゥ・・・
            while (true) {
                if (counting == true) {
                    _images[22].IsVisible = false;
                    _images[23].IsVisible = false;
                    break;
                } else {
                    _images[22].IsVisible = false;
                    _images[23].IsVisible = true;
                    await Task.Delay(1000);
                    _images[22].IsVisible = true;
                    _images[23].IsVisible = false;
                    await Task.Delay(1000);
                }
            }
        }
    }
}
