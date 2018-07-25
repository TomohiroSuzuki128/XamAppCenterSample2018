# はじめに #
　  
App Center で自動ビルド、UIテストが試せる iOS, Android のサンプルアプリです。
　  
Cognitive Services の Translator Text API を利用して、入力した日本語を英語に翻訳してくれます。
　  
## iOS ##
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/iPhone.png?raw=true)
　  
## Android ##
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android.png?raw=true)
　  
　  
# 必要環境 #
　  
## iOS,Android 自動ビルド ##
- Visual Studio for Mac がインストールされたMac
- Azure のアカウント
- App Center のアカウント
　  
### iOSで実機ビルドする場合 ###
- Apple Developer Program への加入必要
　  
## iOS UIテスト ##
- iOS11 以上の iPhone 実機
- Apple Developer Program への加入は不要
　  
## Android UIテスト ##
- （必須ではないがあると望ましい） Android 7.0 以上の Android 実機
　  
　  
# アプリの作成 #
　  
　  
## Cognitive Services の Translator Text API 作成 ##
　  
Azure ポータルにログインし、「新規」 -> 「translate」 で検索します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/001.png?raw=true)
　  
　  
Translator Text API を選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/002.png?raw=true)
　  
　  
「作成」をクリックします。 
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/003.png?raw=true)
　  
　  
項目を入力して「作成」をクリックします。 
価格レベルは必ず「F0」（無料）にしてください！
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/004.png?raw=true)
　  
　  
作成した Translator Text API を開いて Key をコピーし保管しておいて下さい。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/005.png?raw=true)
　  
　  
## ソリューションを開く ## 
  
https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/  
にアクセスしてソリューションを clone または zip ダウンロードしてください。
　  
　  
/src/Start/XamAppCenterSample2018.sln を開きます。
　  
　  
## API Key の記述 ##   
　  
　  
/XamAppCenterSample2018/Variables.cs ファイルを開きます。  

先ほど作成した API の Key を記述します。

注意：API Key を記述したソースをパブリックなリポジトリにコミットしないで下さい。

 ![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/api_key001.png?raw=true)   
　  
　  
## ViewModel の作成 ## 

まず、ViewModel を作成しましょう。

/XamAppCenterSample2018/ViewModels/MainViewModel.cs ファイルを作成します。

まずは、using を追加します。

```csharp
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using XamAppCenterSample2018.Services.Interfaces;
```
　  
　  
MainViewModel を MvxViewModel の派生とします。

```csharp
    public class MainViewModel : MvxViewModel
```
　  
　  
画面には、「翻訳したい日本語入力欄」「翻訳された英語表示欄」「英語に翻訳するボタン」の要素があります。  
これらを入力欄、表示欄はプロパティ、ボタンはコマンドとして実装してきます。

```csharp
        string inputText = string.Empty;
        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value);
        }

        string translatedText = string.Empty;
        public string TranslatedText
        {
            get => translatedText;
            set => SetProperty(ref translatedText, value);
        } 

        public IMvxAsyncCommand TranslateCommand { get; private set; }
```
　  
　  
コンストラクタでコマンドの処理を実装します。  
DI された Service のメソッドをコールするようにします。

```csharp
        public MainViewModel(ITranslateService translateService) : base()
        {
            TranslateCommand = new MvxAsyncCommand(async () =>
            {
                TranslatedText = await translateService.Translate(InputText);
            });
        }
```
　  
　  
これで、ViewModelは完成です。  
完成したコードは以下のようになります。

```csharp
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using XamAppCenterSample2018.Services.Interfaces;

namespace XamAppCenterSample2018.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        string inputText = string.Empty;
        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value);
        }

        string translatedText = string.Empty;
        public string TranslatedText
        {
            get => translatedText;
            set => SetProperty(ref translatedText, value);
        } 

        public IMvxAsyncCommand TranslateCommand { get; private set; }

        public MainViewModel(ITranslateService translateService) : base()
        {
            TranslateCommand = new MvxAsyncCommand(async () =>
            {
                TranslatedText = await translateService.Translate(InputText);
            });
        }

    }
}
```
　  
　  
　  
## iOS の バンドル識別子 の設定 ## 
　  
　  
iOS のアプリの バンドル識別子 を御自身の固有のものに変更して下さい。
- アプリケーション名 は XamAppCenterSample2018 にして下さい。
- バンドル識別子の Organization Identifier の部分は全世界で固有となるような文字列にして下さい。
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/test002.png?raw=true)
　  
　  
## iOS の View の作成 ## 
　  
iOS の View を作成します。  

storyborad、xib は、IDEによって更新部分以外も勝手にコードが更新され、 Git との相性が悪いので、今回はコードで UI を記述します。  

/OS/Views/MainView.cs ファイルを作成します。  
　  
　  
まずは、using を追加します。  

```csharp
using System;
using UIKit;
using Foundation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using XamAppCenterSample2018.ViewModels;
```
　  
　  
MainView を MvxViewController<MainViewModel> の派生とし、属性を設定します。

```csharp
    [Register("MainView")]
    [MvxRootPresentation(WrapInNavigationController = false)]
    public class MainView : MvxViewController<MainViewModel>
```
　  
　  
フォントサイズや UI エレメントのフィールドを定義します。

```csharp
        static readonly nfloat fontSize = 20;

        UILabel inputLabel;
        UITextView inputText;
        UIButton translateButton;
        UILabel translatedLabel;
        UITextView translatedText;
```  
　  
　  
UI エレメントを初期設定するメソッドを定義します。

```csharp
        void InitUI()
        {
        }
```  
　  
　  
InitUI の中に UI エレメントの設定値を記述していきます。  
画面には、「翻訳したい日本語のラベル」「翻訳したい日本語の入力欄」「翻訳された英語のラベル」「翻訳された英語の表示欄」「英語に翻訳するボタン」の要素があります。
　  
　  
MainView 自体の設定値です。

```csharp
            View.ContentMode = UIViewContentMode.ScaleToFill;
            View.LayoutMargins = new UIEdgeInsets(0, 16, 0, 16);
            View.Frame = new CGRect(0, 0, 375, 667);
            View.BackgroundColor = UIColor.White;
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
``` 
　  
　  
「翻訳したい日本語ラベル」inputLabel の設定値と View への追加、制約の設定です。

```csharp
            inputLabel = new UILabel
            {
                Frame = new CGRect(0, 0, 375, 20),
                Opaque = false,
                UserInteractionEnabled = false,
                ContentMode = UIViewContentMode.Left,
                Text = "翻訳したい日本語",
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 0,
                BaselineAdjustment = UIBaselineAdjustment.AlignBaselines,
                AdjustsFontSizeToFitWidth = false,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.SystemFontOfSize(fontSize),
            };
            View.AddSubview(inputLabel);

            inputLabel.HeightAnchor.ConstraintEqualTo(20).Active = true;
            inputLabel.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            inputLabel.TopAnchor.ConstraintEqualTo(View.TopAnchor, 70).Active = true;
            inputLabel.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            inputLabel.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;
```  
　  
　  
「翻訳したい日本語の入力欄」inputText の設定値と View への追加、制約の設定です。

```csharp
            inputText = new UITextView
            {
                Frame = new CGRect(0, 0, 375, 200),
                ContentMode = UIViewContentMode.ScaleToFill,
                TranslatesAutoresizingMaskIntoConstraints = false,
                KeyboardType = UIKeyboardType.Twitter,
                Font = UIFont.SystemFontOfSize(fontSize),
                AccessibilityIdentifier = "inputText",
            };

            inputText.Layer.BorderWidth = 1;
            inputText.Layer.BorderColor = UIColor.LightGray.CGColor;

            View.AddSubview(inputText);

            inputText.HeightAnchor.ConstraintEqualTo(View.HeightAnchor, 0.3f).Active = true;
            inputText.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            inputText.TopAnchor.ConstraintEqualTo(inputLabel.BottomAnchor, 5).Active = true;
            inputText.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            inputText.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;
```  
　  
　  
入力完了時にソフトキーボードを閉じるボタンの設定です。

```csharp
            var toolBar = new UIToolbar
            {
                BarStyle = UIBarStyle.Default,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };
            toolBar.HeightAnchor.ConstraintEqualTo(40).Active = true;
            toolBar.WidthAnchor.ConstraintEqualTo(View.Frame.Width).Active = true;

            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            var commitButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);

            commitButton.Clicked += (s, e) => View.EndEditing(true);
            toolBar.SetItems(new UIBarButtonItem[] { spacer, commitButton }, false);
            inputText.InputAccessoryView = toolBar;
```  
　  
　  
「英語に翻訳するボタン」translateButton の設定値と View への追加、制約の設定です。

```csharp
            translateButton = new UIButton(UIButtonType.RoundedRect)
            {
                Frame = new CGRect(0, 0, 375, 20),
                Opaque = false,
                ContentMode = UIViewContentMode.ScaleToFill,
                HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
                VerticalAlignment = UIControlContentVerticalAlignment.Center,
                LineBreakMode = UILineBreakMode.MiddleTruncation,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.SystemFontOfSize(fontSize),
                AccessibilityIdentifier = "translateButton",
            };

            translateButton.SetTitle("英語に翻訳する", UIControlState.Normal);
            View.AddSubview(translateButton);

            translateButton.HeightAnchor.ConstraintEqualTo(40f).Active = true;
            translateButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            translateButton.TopAnchor.ConstraintEqualTo(inputText.BottomAnchor, 20).Active = true;
            translateButton.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            translateButton.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;
```  
　  
　  
「翻訳された英語のラベル」translatedLabel の設定値と View への追加、制約の設定です。

```csharp
            translatedLabel = new UILabel
            {
                Frame = new CGRect(0, 0, 375, 20),
                Opaque = false,
                UserInteractionEnabled = false,
                ContentMode = UIViewContentMode.Left,
                Text = "翻訳された英語",
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 0,
                BaselineAdjustment = UIBaselineAdjustment.AlignBaselines,
                AdjustsFontSizeToFitWidth = false,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.SystemFontOfSize(fontSize),
            };
            View.AddSubview(translatedLabel);

            translatedLabel.HeightAnchor.ConstraintEqualTo(20).Active = true;
            translatedLabel.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            translatedLabel.TopAnchor.ConstraintEqualTo(translateButton.BottomAnchor, 20).Active = true;
            translatedLabel.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            translatedLabel.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;

```  
　  
　  
「翻訳された英語の表示欄」translatedText の設定値と View への追加、制約の設定です。

```csharp
            translatedText = new UITextView
            {
                Frame = new CGRect(0, 0, 375, 200),
                ContentMode = UIViewContentMode.ScaleToFill,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.SystemFontOfSize(fontSize),
                AccessibilityIdentifier = "translatedText",
                Editable = false,
            };

            translatedText.Layer.BorderWidth = 1;
            translatedText.Layer.BorderColor = UIColor.LightGray.CGColor;

            View.AddSubview(translatedText);

            translatedText.HeightAnchor.ConstraintEqualTo(View.HeightAnchor, 0.3f).Active = true;
            translatedText.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            translatedText.TopAnchor.ConstraintEqualTo(translatedLabel.BottomAnchor, 5).Active = true;
            translatedText.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            translatedText.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;

``` 
　  
　  
バインディングを設定するメソッドです。

```csharp
        void SetBinding()
        {
            var set = this.CreateBindingSet<MainView, MainViewModel>();

            set.Bind(inputText).To(vm => vm.InputText);
            set.Bind(translatedText).To(vm => vm.TranslatedText);
            set.Bind(translateButton).To(vm => vm.TranslateCommand);

            set.Apply();
        }

```  
　  
　  
ViewDidLoad で InitUI, SetBindingをコールします。

```csharp
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitUI();
            SetBinding();
        }
```  
　  
　  
これで、iOS の View は完成です。
完成したコードは以下のようになります。

```csharp
using System;
using UIKit;
using Foundation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using XamAppCenterSample2018.ViewModels;

namespace XamAppCenterSample2018.iOS.Views
{
    [Register("MainView")]
    [MvxRootPresentation(WrapInNavigationController = false)]
    public class MainView : MvxViewController<MainViewModel>
    {
		static readonly nfloat fontSize = 20;

        UILabel inputLabel;
        UITextView inputText;
        UIButton translateButton;
        UILabel translatedLabel;
        UITextView translatedText;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitUI();
            SetBinding();
        }

        void InitUI()
        {
            View.ContentMode = UIViewContentMode.ScaleToFill;
            View.LayoutMargins = new UIEdgeInsets(0, 16, 0, 16);
            View.Frame = new CGRect(0, 0, 375, 667);
            View.BackgroundColor = UIColor.White;
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            inputLabel = new UILabel
            {
                Frame = new CGRect(0, 0, 375, 20),
                Opaque = false,
                UserInteractionEnabled = false,
                ContentMode = UIViewContentMode.Left,
                Text = "翻訳したい日本語",
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 0,
                BaselineAdjustment = UIBaselineAdjustment.AlignBaselines,
                AdjustsFontSizeToFitWidth = false,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.SystemFontOfSize(fontSize),
            };
            View.AddSubview(inputLabel);

            inputLabel.HeightAnchor.ConstraintEqualTo(20).Active = true;
            inputLabel.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            inputLabel.TopAnchor.ConstraintEqualTo(View.TopAnchor, 70).Active = true;
            inputLabel.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            inputLabel.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;

            inputText = new UITextView
            {
                Frame = new CGRect(0, 0, 375, 200),
                ContentMode = UIViewContentMode.ScaleToFill,
                TranslatesAutoresizingMaskIntoConstraints = false,
                KeyboardType = UIKeyboardType.Twitter,
                Font = UIFont.SystemFontOfSize(fontSize),
                AccessibilityIdentifier = "inputText",
            };

            inputText.Layer.BorderWidth = 1;
            inputText.Layer.BorderColor = UIColor.LightGray.CGColor;

            View.AddSubview(inputText);

            inputText.HeightAnchor.ConstraintEqualTo(View.HeightAnchor, 0.3f).Active = true;
            inputText.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            inputText.TopAnchor.ConstraintEqualTo(inputLabel.BottomAnchor, 5).Active = true;
            inputText.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            inputText.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;

            var toolBar = new UIToolbar
            {
                BarStyle = UIBarStyle.Default,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };
            toolBar.HeightAnchor.ConstraintEqualTo(40).Active = true;
            toolBar.WidthAnchor.ConstraintEqualTo(View.Frame.Width).Active = true;

            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            var commitButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);

            commitButton.Clicked += (s, e) => View.EndEditing(true);
            toolBar.SetItems(new UIBarButtonItem[] { spacer, commitButton }, false);
            inputText.InputAccessoryView = toolBar;

            translateButton = new UIButton(UIButtonType.RoundedRect)
            {
                Frame = new CGRect(0, 0, 375, 20),
                Opaque = false,
                ContentMode = UIViewContentMode.ScaleToFill,
                HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
                VerticalAlignment = UIControlContentVerticalAlignment.Center,
                LineBreakMode = UILineBreakMode.MiddleTruncation,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.SystemFontOfSize(fontSize),
                AccessibilityIdentifier = "translateButton",
            };

            translateButton.SetTitle("英語に翻訳する", UIControlState.Normal);
            View.AddSubview(translateButton);

            translateButton.HeightAnchor.ConstraintEqualTo(40f).Active = true;
            translateButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            translateButton.TopAnchor.ConstraintEqualTo(inputText.BottomAnchor, 20).Active = true;
            translateButton.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            translateButton.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;

            translatedLabel = new UILabel
            {
                Frame = new CGRect(0, 0, 375, 20),
                Opaque = false,
                UserInteractionEnabled = false,
                ContentMode = UIViewContentMode.Left,
                Text = "翻訳された英語",
                TextAlignment = UITextAlignment.Left,
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 0,
                BaselineAdjustment = UIBaselineAdjustment.AlignBaselines,
                AdjustsFontSizeToFitWidth = false,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.SystemFontOfSize(fontSize),
            };
            View.AddSubview(translatedLabel);

            translatedLabel.HeightAnchor.ConstraintEqualTo(20).Active = true;
            translatedLabel.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            translatedLabel.TopAnchor.ConstraintEqualTo(translateButton.BottomAnchor, 20).Active = true;
            translatedLabel.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            translatedLabel.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;

            translatedText = new UITextView
            {
                Frame = new CGRect(0, 0, 375, 200),
                ContentMode = UIViewContentMode.ScaleToFill,
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = UIFont.SystemFontOfSize(fontSize),
                AccessibilityIdentifier = "translatedText",
                Editable = false,
            };

            translatedText.Layer.BorderWidth = 1;
            translatedText.Layer.BorderColor = UIColor.LightGray.CGColor;

            View.AddSubview(translatedText);

            translatedText.HeightAnchor.ConstraintEqualTo(View.HeightAnchor, 0.3f).Active = true;
            translatedText.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;

            translatedText.TopAnchor.ConstraintEqualTo(translatedLabel.BottomAnchor, 5).Active = true;
            translatedText.LeftAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.LeftAnchor).Active = true;
            translatedText.RightAnchor.ConstraintEqualTo(View.LayoutMarginsGuide.RightAnchor).Active = true;
        }

        void SetBinding()
        {
            var set = this.CreateBindingSet<MainView, MainViewModel>();

            set.Bind(inputText).To(vm => vm.InputText);
            set.Bind(translatedText).To(vm => vm.TranslatedText);
            set.Bind(translateButton).To(vm => vm.TranslateCommand);

            set.Apply();
        }

    }
}
``` 
　  
　  　  
　  
## iOS アプリのデバッグ ##
　  
　  
では、ここでiOSのアプリを実機デバッグしてみましょう。

実機をお持ちの方はせっかくですから実機でデバッグしてみましょう。
お持ちでない方はシミュレータでデバッグしてみましょう。
　  
　  
　  
### シミュレータデバッグ ###
　  
XamAppCenterSample2018.iOS > Debug > [シミュレータの機種名] に設定します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/debug001.png?raw=true)
　  
　  
「デバッグの開始」を実行します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build003.png?raw=true)
　  
　  
アプリが起動します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build004.png?raw=true)
　  
　  
飜訳が動作すれば成功です。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build005.png?raw=true)
　  
　  
### 実機デバッグ ###
　  
実機をお持ちの方は、ここでiOSのアプリを実機デバッグしてみましょう。
iOSのアプリを実機デバッグするにはXcodeでダミーアプリを実行する必要があります。
　  
　  
#### Xcode でのダミーアプリ実行 ####
　  
プロビジョニングプロファイルや証明書の紐付けが自動で行われるようにXcodeでSwiftのダミーアプリを作成します。

[File]->[New]->[Project]でプロジェクトを作成します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app001.png?raw=true)
　  
　  
iOSのSingle View Applicationを選択し、[Next]を押します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app002.png?raw=true)
　  
　  
Product Name は XamAppCenterSample2018 にして下さい。
Organization Identifier は先ほど決めたものと同一のものにしてください。
[Next]を押します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app003.png?raw=true)
　  
　  
<code>XamAppCenterSample2018Xcode</code>というフォルダを作成し、その中にプロジェクトを保存してください。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app004.png?raw=true)
　  
　  
Bundle Identifier が正しく設定されているのを確認して下さい。

Signingの部分が自動で修正されて、Provisioning Profile と Signing Certificate の部分にエラーのアイコンが表示されてないことを確認してください。

左上のデバッグ実行の部分にご自分のiPhoneが認識されているのを確認してください。
　  
　  
全て確認できたらデバッグ実行します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app005.png?raw=true)
　  
　  
もし、以下の表示が出た場合、[常に許可]を押します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app006.png?raw=true)
　  
　  
以下の表示が出たら、次の手順で実機の設定で開発元を信頼させます。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app007.png?raw=true)
　  
　  
実機の設定アプリを開き[プロファイルとデバイス管理]を開きます。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app008.png?raw=true)
　  
　  
デベロッパAPPに[Xcodeに設定したApple ID]が表示されていますのでタップします。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app009.png?raw=true)
　  
　  
[Xcodeに設定したApple ID]を信頼をタップして信頼させます。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app010.png?raw=true)
　  
　  
以下の表示が出た場合、ご自分の iPhoneの中 に XamAppCenterSample2018 と言う名前のアプリが既にインストールされているか確認し、インストールされている場合、アンインストールしてください。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app011.png?raw=true)
　  
　  
再度、デバッグ実行し、無事アプリが起動して真っ白な画面が表示されたら成功です。
　  
　  
これで、Xcode でのダミーアプリ実行は完了です。
　  
　  
#### iOS アプリのビルド #### 
　  
/XamAppCenterSample2018/XamAppCenterSample2018.iOS/Info.plist ファイルを開きます。
　  
「バンドル識別子」の文字列を先ほど Xcode で設定した、Bundle Identifier と一字一句違わないように設定します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build001.png?raw=true)
　  
　  
XamAppCenterSample2018.iOS > Debug > [あなたのiPhone名] に設定します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build002.png?raw=true)
　  
　  
「デバッグの開始」を実行します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build003.png?raw=true)
　  
　  
アプリが起動します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build004.png?raw=true)
　  
　  
飜訳が動作すれば成功です。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build005.png?raw=true)
　  
　  
## Android の パッケージ名の設定 ## 

Android のアプリのパッケージ名を御自身の固有のものに変更して下さい。
- アプリケーション名 は XamAppCenterSample2018 にして下さい。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/test001.png?raw=true)
　  
　  
## Android の View の作成 ## 

/Droid/Resources/layout/Main.axml を開きます。

Android の View を作成します。
Android の axml は、Git との相性も問題がないので、そのまま axml に記述します。

「翻訳したい日本語ラベル」inputTextView を追加します。

```xml
    <TextView
        android:text="@string/input"
        android:textAppearance="?android:attr/textAppearanceMedium"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:id="@+id/inputTextView" />
```
　  
　  
「翻訳したい日本語の入力欄」inputText を追加します。
また、Binding も記述します。

local:MvxBind="[View のプロパティ名] [ViewModel のプロパティ名]"
というフォーマットで記述します。

```xml
    <EditText
        android:inputType="textMultiLine"
        android:gravity="top|left"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:textSize="20sp"
        android:lines="7"
        local:MvxBind="Text InputText"
        android:id="@+id/inputText" />
```
　  
　  
「英語に翻訳するボタン」translateButton を追加します。
また、Binding も記述します。

```xml
    <Button
        android:id="@+id/translateButton"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        local:MvxBind="Click TranslateCommand"
        android:text="@string/translate" />
```
　  
　  
「翻訳された英語のラベル」translatedTextView を追加します。

```xml
    <TextView
        android:text="@string/translated"
        android:textAppearance="?android:attr/textAppearanceMedium"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:id="@+id/translatedTextView" />
```
　  
　  
「翻訳された英語の表示欄」translatedText を追加します。
また、Binding も記述します。

```xml
    <TextView
        android:inputType="textMultiLine"
        android:gravity="top|left"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:textSize="20sp"
        android:lines="7"
        local:MvxBind="Text TranslatedText"
        android:id="@+id/translatedText" />
```
　  
　  
これで、Android の View は完成です。
完成した axml は以下のようになります。

```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:local="http://schemas.android.com/apk/res-auto"
    android:id="@+id/mainLayout"
    android:orientation="vertical"
    android:layout_width="match_parent"
    android:layout_height="match_parent">
    <TextView
        android:text="@string/input"
        android:textAppearance="?android:attr/textAppearanceMedium"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:id="@+id/inputTextView" />
    <EditText
        android:inputType="textMultiLine"
        android:gravity="top|left"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:textSize="20sp"
        android:lines="7"
        local:MvxBind="Text InputText"
        android:id="@+id/inputText" />
    <Button
        android:id="@+id/translateButton"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        local:MvxBind="Click TranslateCommand"
        android:text="@string/translate" />
    <TextView
        android:text="@string/translated"
        android:textAppearance="?android:attr/textAppearanceMedium"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:id="@+id/translatedTextView" />
    <TextView
        android:inputType="textMultiLine"
        android:gravity="top|left"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:textSize="20sp"
        android:lines="7"
        local:MvxBind="Text TranslatedText"
        android:id="@+id/translatedText" />
</LinearLayout>
```
　  
　  
  ## 文字列リソースの設定 ## 

/Droid/Resources/values/Strings.xml を開きます。

画面に表示する文字列リソースを設定します。 

```xml
    <TextView
        android:text="@string/translated"
        android:textAppearance="?android:attr/textAppearanceMedium"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:id="@+id/translatedTextView" />
```
　  
　  
## Android の コードビハインド の作成 ## 

アプリとしての基本動作は View と ViewModel で完成していますが、入力後にソフトキーボードを消す動作が抜けているので、コードビハインドに記述します。
  
/Droid/Views/MainActivity.cs を開きます。 
　  
　  
まずは、using を追加します。  
  
```csharp
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.OS;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Binding;
using XamAppCenterSample2018.ViewModels;
```
　  
　  
MainActivity を MvxActivity<MainViewModel> の派生とします。
  
```csharp
    public class MainActivity : MvxActivity<MainViewModel>
```
　  
　  
UI エレメントのフィールドを定義します。

```csharp
        InputMethodManager inputMethodManager;
        LinearLayout mainLayout;
        EditText editText;
```  
　  
　  
ソフトキーボードを消すメソッドを実装します。

```csharp
        void HideSoftInput()
        {
            inputMethodManager.HideSoftInputFromWindow(mainLayout.WindowToken, HideSoftInputFlags.NotAlways);
            mainLayout.RequestFocus(); 
        }
```
　  
　  
画面の何も無いところをタッチしたときに、ソフトキーボードを消すようにします。

```csharp
        public override bool OnTouchEvent(MotionEvent e)
        {
            HideSoftInput();
            return false;
        }
```
　  
　  
ボタンや翻訳後の文章表示部分をタッチしたときに、ソフトキーボードを消すようにします。

```csharp
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);

            editText = (EditText)FindViewById(Resource.Id.inputText);
            mainLayout = (LinearLayout)FindViewById(Resource.Id.mainLayout);
            inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);

            var button = (Button)FindViewById(Resource.Id.translateButton);
            button.Click += (s, e) => HideSoftInput();

            var textView = (TextView)FindViewById(Resource.Id.translatedText);
            textView.Click += (s, e) => HideSoftInput();
        }
```
　  
　  
これで、コードビハインドは完成です。  
完成したコードは以下のようになります。

```csharp
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.OS;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Binding;
using XamAppCenterSample2018.ViewModels;

namespace XamAppCenterSample2018.Droid
{
    [Activity(Label = "XamAppCenterSample2018", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : MvxActivity<MainViewModel>
    {
        InputMethodManager inputMethodManager;
        LinearLayout mainLayout;
        EditText editText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Main);

            editText = (EditText)FindViewById(Resource.Id.inputText);
            mainLayout = (LinearLayout)FindViewById(Resource.Id.mainLayout);
            inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);

            var button = (Button)FindViewById(Resource.Id.translateButton);
            button.Click += (s, e) => HideSoftInput();

            var textView = (TextView)FindViewById(Resource.Id.translatedText);
            textView.Click += (s, e) => HideSoftInput();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            HideSoftInput();
            return false;
        }

        void HideSoftInput()
        {
            inputMethodManager.HideSoftInputFromWindow(mainLayout.WindowToken, HideSoftInputFlags.NotAlways);
            mainLayout.RequestFocus(); 
        }

    }
}
```
　  
　  
# テストプロジェクトの作成 #
　  
　  
## AppInitializer の作成 ## 
　  
　  
テスト時にアプリを初期化するクラスを作成します。
　  
　  
まずは、using を追加します。  
  
```csharp
using Xamarin.UITest;
```
　  
　  
クラスを定義します。
  
```csharp
namespace XamAppCenterSample2018.UITests
{
    public class AppInitializer
    {
    }
}
```
　  
　  
アプリのインスタンスを初期化、アプリを開始するメソッドを定義します。

```csharp
namespace XamAppCenterSample2018.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .EnableLocalScreenshots()
                    .PreferIdeSettings()
                    .InstalledApp("<あなたのアプリのパッケージ名>")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                .EnableLocalScreenshots()
                .PreferIdeSettings()
                .InstalledApp("<あなたのアプリのbundle ID>")
                .StartApp();
        }
    }
}
```
　  
　  
Android のアプリのパッケージ名は以下で確認できます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/test001.png?raw=true)
　  
　  
iOS のアプリの bundle ID は以下で確認できます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/test002.png?raw=true)
　  
　  
これで、AppInitializer は完成です。  
完成したコードは以下のようになります。
　  
　  
```csharp
using Xamarin.UITest;

namespace XamAppCenterSample2018.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .EnableLocalScreenshots()
                    .PreferIdeSettings()
                    .InstalledApp("<あなたのアプリのパッケージ名>")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                .EnableLocalScreenshots()
                .PreferIdeSettings()
                .InstalledApp("<あなたのアプリのbundle ID>")
                .StartApp();
        }
    }
}
```
　  
　  
## テストコードの作成 ## 
　  
　  
テストコードを作成します。
テストコードはiOS, Android で共用します。
　  
　  
まずは、using を追加します。  
  
```csharp
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.UITest;
```
　  
　  
クラスを定義します。
  
```csharp
namespace XamAppCenterSample2018.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
    }
}
```
　  
　  
フィールドを定義します。
  
```csharp
        IApp app;
        Platform platform;
```
　  
　  
コンストラクターを定義します。
アプリの起動時に iOS, Android を指定するために、プラットフォームを保持しておきます。
  
```csharp
        public Tests(Platform platform)
        {
            this.platform = platform;
        }
```
　  
　  
各テスト実行前にアプリを開始するメソッドを定義します。
<code>[SetUp]</code> Attribute を付加するとテストメソッドの実行前に実行されます。
  
```csharp
        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }
```
　  
　  
翻訳が成功するシナリオのテストメソッドを定義します。
- <code>[Test]</code> Attribute を付加するとテストメソッドとして扱われます。
- <code>app.Tap</code>で UI エレメントをタップします。
- <code>c.Marked("inputText")</code>でタップするUI エレメントを指定します。
- <code>Marked</code>で指定するキーは、iOS では <code>AccessibilityIdentifier</code>、Android では <code>android:id</code>で設定します。
- <code>app.DismissKeyboard()</code>で、ソフトキーボードを消します。
- <code>app.Query</code>で UI エレメントを検索します。
- <code>Assert.AreEqual</code>で、UI エレメントに表示された翻訳後のテキストが正しいか確認しています。
  
```csharp
        [Test]
        public async void SucceedTranslate()
        {
            await Task.Delay(2000);
            app.Tap(c => c.Marked("inputText"));
            await Task.Delay(2000);
            app.EnterText("私は毎日電車に乗って会社に行きます。");
            await Task.Delay(2000);
            app.DismissKeyboard();
            await Task.Delay(2000);
            app.Tap(c => c.Button("translateButton"));
            await Task.Delay(4000);
            var elements = app.Query(c => c.Marked("translatedText"));
            await Task.Delay(2000);
            Assert.AreEqual("I go to the office by train every day.", elements.FirstOrDefault().Text);
        }
```
　  
　  
翻訳したい日本語が未入力の為、翻訳が失敗するシナリオのテストメソッドを定義します。
- <code>StringAssert.Contains</code>で、UI エレメントに表示された翻訳後のテキストに指定された文字列が入っているか確認しています。
  
```csharp
        [Test]
        public async void FailTranslate()
        {
            await Task.Delay(2000);
            app.Tap(c => c.Button("translateButton"));
            await Task.Delay(4000);
            var elements = app.Query(c => c.Marked("translatedText"));
            await Task.Delay(2000);
            StringAssert.Contains("エラーコード： 400005", elements.FirstOrDefault().Text);
        }
```
　  
　  
これで、テストコードは完成です。  
完成したコードは以下のようになります。
　  
```csharp
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.UITest;

namespace XamAppCenterSample2018.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public async void SucceedTranslate()
        {
            await Task.Delay(2000);
            app.Tap(c => c.Marked("inputText"));
            await Task.Delay(2000);
            app.EnterText("私は毎日電車に乗って会社に行きます。");
            await Task.Delay(2000);
            app.DismissKeyboard();
            await Task.Delay(2000);
            app.Tap(c => c.Button("translateButton"));
            await Task.Delay(4000);
            var elements = app.Query(c => c.Marked("translatedText"));
            await Task.Delay(2000);
            Assert.AreEqual("I go to the office by train every day.", elements.FirstOrDefault().Text);
        }

        [Test]
        public async void FailTranslate()
        {
            await Task.Delay(2000);
            app.Tap(c => c.Button("translateButton"));
            await Task.Delay(4000);
            var elements = app.Query(c => c.Marked("translatedText"));
            await Task.Delay(2000);
            StringAssert.Contains("エラーコード： 400005", elements.FirstOrDefault().Text);
        }
    }
}
```
　  
　  

　  
# App Center 利用の為の環境構築 #
では、これから App Center でビルドとテストを行います。
　  
　  
## node.js のインストール ## 

以下のコマンドで node.js がインストールされている確認できます。
```bash
node -v 
```
　  
以下のように表示されればインストールされていません。
```bash
-bash: node: command not found
```
　  
　  
### Homebrew（パッケージ管理システム） のインストール ### 
　  
インストールコマンドを実行します。
```bash
/usr/bin/ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"
```
　  
インストールを実行するか確認されるので RETRUN します。
```bash
Press RETURN to continue or any other key to abort
```
　  
インストールが終わるまで待ちます。
以下のように Xcode のコマンドラインツールなどもインストールされますので結構時間がかかります。
```bash
Downloading Command Line Tools (macOS High Sierra version 10.13) for Xcode
Downloaded Command Line Tools (macOS High Sierra version 10.13) for Xcode
Installing Command Line Tools (macOS High Sierra version 10.13) for Xcode
```
　  
途中でパスワードを聞かれたら入力してください。
　  
　  
下記のように表示されればインストール終了です。
```bash
==> Installation successful!

==> Homebrew has enabled anonymous aggregate user behaviour analytics.
Read the analytics documentation (and how to opt-out) here:
  https://docs.brew.sh/Analytics.html

==> Next steps:
- Run `brew help` to get started
- Further documentation: 
    https://docs.brew.sh
```
　  
　  
### nodebrew のインストール ### 
　  
次にnodebrewをインストールします。
　  
インストールコマンドを実行します。
```bash
brew install nodebrew
```
　  
下記のように表示されればインストール終了です。
```bash
🍺  /usr/local/Cellar/nodebrew/1.0.0: 8 files, 38.4KB, built in 5 seconds
```
　  
念のため、インストールが正常に完了したか確認します。
```bash
nodebrew -v
```
　  
nodebrewのバージョン情報が表示されればインストール完了です。
```bash
nodebrew 1.0.0

Usage:
    nodebrew help                         Show this message
    nodebrew install <version>            Download and install <version> (from binary)
    nodebrew compile <version>            Download and install <version> (from source)
    nodebrew install-binary <version>     Alias of `install` (For backword compatibility)
    nodebrew uninstall <version>          Uninstall <version>
    nodebrew use <version>                Use <version>
    nodebrew list                         List installed versions
    nodebrew ls                           Alias for `list`
    nodebrew ls-remote                    List remote versions
    nodebrew ls-all                       List remote and installed versions
    nodebrew alias <key> <value>          Set alias
    nodebrew unalias <key>                Remove alias
    nodebrew clean <version> | all        Remove source file
    nodebrew selfupdate                   Update nodebrew
    nodebrew migrate-package <version>    Install global NPM packages contained in <version> to current version
    nodebrew exec <version> -- <command>  Execute <command> using specified <version>

Example:
    # install
    nodebrew install v8.9.4

    # use a specific version number
    nodebrew use v8.9.4
```
　  
　  
### node.js のインストール ### 
　  
いよいよ node.js をインストールします。
　  
インストールコマンドを実行します。
```bash
nodebrew install-binary latest
```
　  
怒られてしまいました。
```bash
Fetching: https://nodejs.org/dist/v10.7.0/node-v10.7.0-darwin-x64.tar.gz
Warning: Failed to create the file 
Warning: /Users/hiro128/.nodebrew/src/v10.7.0/node-v10.7.0-darwin-x64.tar.gz: 
Warning: No such file or directory
                                                                           0.0%
curl: (23) Failed writing body (0 != 1057)
download failed: https://nodejs.org/dist/v10.7.0/node-v10.7.0-darwin-x64.tar.gz
```
　  
ディレクトリが無いようなので、mkdirをして、
```bash
mkdir ~/.nodebrew
mkdir ~/.nodebrew/src
```
　  
リトライします。
```bash
nodebrew install-binary latest
```
　  
下記のように表示されればインストール終了です。
```bash
Fetching: https://nodejs.org/dist/v10.7.0/node-v10.7.0-darwin-x64.tar.gz
######################################################################## 100.0%
Installed successfully
```
　  
インストールされた node.js のバージョンを確認します。
```bash
nodebrew list
```
　  
インストールされたバージョンと、現在有効なバージョンが表示されます。
```bash
v10.7.0
　  
current: none
```
　  
使用する node.js バージョンを有効化します。
```bash
nodebrew use v10.7.0
```
　  
確認のためにlistを実行し、
```bash
nodebrew list
```
　  
指定したバージョンに変わっていれば成功です。
```bash
v10.7.0
　  
current: v10.7.0
```
　  
　  
### パスを通す ### 
　  
以下のコマンドでnodeコマンドへパスをbashrcへ保存します。
```bash
echo 'export PATH=$PATH:/Users/<あなたのhome>/.nodebrew/current/bin' >> ~/.bashrc
```
　  
node.js のバージョンを認します。
```bash
node -v
```
　  
うまくいっている場合は、以下のようにバージョン情報が表示されます。
```bash
node -v
v10.7.0
```
　  
ですが、以下のようなメッセージが出た場合は、うまくいってません。
```bash
node -v
-bash: node: command not found
```
　  
先ほど「.bashrc」に書き込んだパスが読み込まれていないようなので、
「.bash_profile」にコマンドを記述する必要があります。
　  
まず、.bash_profile が存在するかどうか確認します。
```bash
ls -la
```
　  
存在しない場合、ファイル作成します。
```bash
touch .bash_profile
```
　  
作成できたか確認しましょう
```bash
ls -la
```
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/bash001.png?raw=true)
　  
.bash_profileを開きます。
```bash
open ~/.bash_profile
```
　  
テキストエディタが開くので下記を記述します。
```bash
if [ -f ~/.bashrc ] ; then
. ~/.bashrc
fi
```
　  
設定を反映させます。
```bash
source ~/.bash_profile
```
　  
再度 node.js のバージョンを確認します。
```bash
node -v
```
　  
うまくいっている場合は、以下のようにバージョン情報が表示されます。
```bash
node -v
v10.7.0
```
　  
以上で、node.js のインストールが完了です。
　  
　  
## App Center CLI のインストール ## 
　  
　  
では、次に App Center CLI をインストールします。
これをインストールしないとテストのアップロードなどができません。
　  
コマンドラインから、以下のコマンドでインストール
```bash
npm install -g appcenter-cli
```
　  
　  
権限が無いと怒られて、パッケージのインストールに失敗する場合、下記の手順でnpmのデフォルトディレクトリの権限を変更します。

npm ディレクトリのパスを確認
```bash
npm config get prefix
```
　  
　  
（例）/usr/local が表示された場合、
npm ディレクトリのオーナーを自分のアカウントに変更
```bash
sudo chown -R <アカウント名> /usr/local/lib/node_modules
sudo chown -R <アカウント名> /usr/local/bin
sudo chown -R <アカウント名> /usr/local/share
```
　  
　  
インストールが終われば準備完了です。
　  
　  
# ソースコードをリポジトリにプッシュ # 
ソリューションのソースコードを、VSTS, Github, Bitbucketのいずれかにプッシュし下さい。 
　  
　  
# iOSの自動ビルドを設定する #
　  
　  
## 証明書、Provisioning Profile の作成（実機自動ビルドしたい場合のみ） ##
Apple Developer Program のサイトで証明書（.cer）、Provisioning Profile（.mobileprovision） を作成し、ローカルの Mac の キーチェーンアクセス で 証明書（.p12） を作成します。
この方法についてはWeb上に情報がたくさんあるので、Webの情報を参考に行なってください。
　  
　  
作成した Provisioning Profile（.mobileprovision）、証明書（.p12）を保管しておきます。
　  
　  
## App Center で iOS の App の作成 ##
　  
　  
ここからは、実機自動ビルド、シミュレータ自動ビルド共通の手順です。
　  
　  
App Center にログインし、右上の「add new」から「add new app」を選択
　  
　  
App Name, OS, Platform を入力、選択し、「Add new app」をクリック
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/006.png?raw=true)
　  
　  
## App Center で iOS のビルドの設定 ##
　  
　  
「Build」を選択し、ソースコードをホストしたサービスを選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/007.png?raw=true)
　  
　  
「XamAppCenterSample2018」を選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/008.png?raw=true)
　  
　  
自動ビルドしたいブランチの設定アイコンを選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/009.png?raw=true)
　  
　  
ビルド設定を選択し、入力し、「Save & Build」を選択。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/010.png?raw=true)
　  
　  
ビルドが始まるのでしばらく待ち、成功すれば完了です。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/011.png?raw=true)
　  
　  
# iOS の 自動 UITest を設定 #
　  
　  
## テストプロジェクトを一度ビルド ##
　  
　  
XamAppCenterSample2018.UITests を一度ビルドします。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/015a.png?raw=true)
　  
　  
## API Key を設定 ##
　  
　  
先ほど Azure で作成した API Key をローカルのプロジェクトに記述します。
（API Key を含むソースをプッシュしないで下さい）
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/011a.png?raw=true)

## 署名なしの ipa の作成 ##
　  
　  
iOSプロジェクトを Debug で 実機ビルドに設定します。
「単体テスト」タブを開き、「アプリのテスト」->「Add App Project」
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/012.png?raw=true)
　  
　  
iOSプロジェクトを追加します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/013.png?raw=true)
　  
　  
「テストのデバッグ」を実行します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/014.png?raw=true)
　  
　  
完了したら、Finder でiOSプロジェクトのフォルダを見てみると、署名はされていませんが、ipaファイルが生成されています。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/015.png?raw=true)
　  
　  
## App Center にファイルを転送し、テストを実行する ##
　  
　  
「Test」 -> 「new test run」をクリック
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/016.png?raw=true)
　  
　  
「Start 30-day trial」をクリック
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/017.png?raw=true)
　  
　  
iOS 11 のデバイスを選択
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/018.png?raw=true)
　  
　  
Test series, System language, Test frameworkを選択
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/019.png?raw=true)
　  
　  
画面に表示されたリファレンスを参考にコマンドを作成する。リファレンスには、--uitest-tools-dir　が指定されていないが追加で指定する。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/020.png?raw=true)

```bash
appcenter test run uitest --app <App Center のURLに表示されているアプリの名前>
 --devices <デバイスのID> --app-path <ipaのパス> --test-series "master" --locale "ja_JP"
 --build-dir <UITestがビルドされたディレクトリのパス> --uitest-tools-dir <test-cloud.exeのディレクトリのパス>
```
　  
　  
（例）
```bash
appcenter test run uitest --app "TomohiroSuzuki128/XamAppCenterSample2018iOS"
 --devices 1b6ada99
 --app-path "/Users/hiro128/Projects/XamAppCenterSample2018/src/iOS/bin/iPhone/Debug/device-builds/iphone10.2-11.3.1/XamAppCenterSample2018.iOS.ipa"
 --test-series "master" --locale "ja_JP"
 --build-dir "/Users/hiro128/Projects/XamAppCenterSample2018/src/UITests/bin/Debug/"
 --uitest-tools-dir "/Users/hiro128/Projects/XamAppCenterSample2018/src/packages/Xamarin.UITest.2.2.4/tools"
```  
　  
　  
コンソールで App Center にログインします

```bash
appcenter login
``` 
　  
　  
ブラウザに表示された認証コードをコンソールに入力します。
　  
　  
上で作成した、appcenter test run uitest コマンドを実行します。
　  
　  
テストが実行されます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/021.png?raw=true)
　  
　  
テストが成功すれば完了です。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/022.png?raw=true)
　  
　  
# Android の自動ビルドを設定する #
　  
　  
## App Center で Android の App の作成 ##
　  
　  
App Center にログインし、右上の「add new」から「add new app」を選択
　  
　  
App Name, OS, Platform を入力、選択し、「Add new app」をクリック
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/023.png?raw=true)
　  
　  
## App Center で Android のビルドの設定 ##
　  
　  
「Build」を選択し、ソースコードをホストしたサービスを選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/024.png?raw=true)
　  
　  
「XamAppCenterSample2018」を選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/008.png?raw=true)
　  
　  
自動ビルドしたいブランチの設定アイコンを選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/009.png?raw=true)
　  
　  
ビルド設定を選択し、入力し、「Save & Build」を選択。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/025.png?raw=true)
　  
　  
ビルドが始まるのでしばらく待ち、成功すれば完了です。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/026.png?raw=true)
　  
　  
# Android の 自動 UITest を設定 #
  
  
## テストプロジェクトを一度ビルド ##

XamAppCenterSample2018.UITests を一度ビルドします。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/015a.png?raw=true)

## API Key を設定 ##

先ほど Azure で作成した API Key をローカルのプロジェクトに記述します。
（iOSの時に記述していれば不要）
（API Key を含むソースをプッシュしないで下さい）
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/011a.png?raw=true)
  
  
## 署名なしの apk の作成 ##
  
  
Androidプロジェクトを Release で シミュレータビルドに設定します。
「発行のためのアーカイブ」を実行します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/027.png?raw=true)
  
  
ビルドが完了したら、Finder でAndroidプロジェクトのフォルダを見てみると、署名はされていませんが、apkファイルが生成されています。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/028.png?raw=true)
  
  
## App Center にファイルを転送し、テストを実行する ##

「Test」 -> 「new test run」をクリック
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/029.png?raw=true)


「Start 30-day trial」をクリック（表示された場合のみ）
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/017.png?raw=true)


Android 7.0 のデバイスを選択
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/030.png?raw=true)


Test series, System language, Test frameworkを選択
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/031.png?raw=true)
  
  
画面に表示されたリファレンスを参考にコマンドを作成する。リファレンスには、--uitest-tools-dir　が指定されていないが追加で指定する。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/032.png?raw=true)
  
  
```bash
appcenter test run uitest --app <App Center のURLに表示されているアプリの名前>
 --devices <デバイスのID> --app-path <apkのパス> --test-series "master" --locale "ja_JP"
 --build-dir <UITestがビルドされたディレクトリのパス> --uitest-tools-dir <test-cloud.exeのディレクトリのパス>
```

（例）
```bash
appcenter test run uitest --app "TomohiroSuzuki128/XamAppCenterSample2018Droid" --devices c8376925 --app-path "/Users/hiro128/Projects/XamAppCenterSample2018/src/Droid/bin/Release/com.hiro128777.XamAppCenterSample2018.apk" --test-series "master" --locale "ja_JP" --build-dir "/Users/hiro128/Projects/XamAppCenterSample2018/src/UITests/bin/Debug/" --uitest-tools-dir "/Users/hiro128/Projects/XamAppCenterSample2018/src/packages/Xamarin.UITest.2.2.4/tools"

```  

コンソールで App Center にログインします（まだログインしていない場合）

```bash
appcenter login
``` 

ブラウザに表示された認証コードをコンソールに入力します。

上で作成した、appcenter test run uitest コマンドを実行します。

テストが実行されます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/033.png?raw=true)

テストが成功すれば完了です。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/034.png?raw=true) 



