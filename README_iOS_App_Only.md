# Xamarin.iOS アプリ開発 ハンズオン テキスト #
  
  
[Android はこちら](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/master/README_Android_App_Only.md)
  
  
# はじめに #
　  
　  
Xamarin.iOS でのアプリ作成の基礎を体験できるハンズオンです。
　  
　  
Cognitive Services の Translator Text API を利用して、入力した日本語を英語に翻訳してくれるサンプルアプリを題材としています。
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/iPhone.png?raw=true)
　  
　  
# 必要環境 #
　  
- Visual Studio for Mac 最新版のインストール
- Xcode 最新版のインストール
- 有効な Azure のアカウント
- （必須ではないが確認用にあると望ましい）iOS11 以上のインストールされた iPhone の実機
　  
　  
# アプリの準備 #
　  
　  
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
　  
　  
## リポジトリを Fork またはソースをダウンロード ## 
　  
　  
https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/  
にアクセスしてリポジトリを Fork またはソースをダウンロードしてください。
　  
　  
## ソリューションを開きます。 ##
　  
　  
/src/Start/XamAppCenterSample2018.sln を開いてください。
  
  
## iOS の バンドル識別子 の設定 ## 
  
iOS プロジェクトの `Info.plist` を開き、iOS のアプリの バンドル識別子 を御自身の固有のものに変更して下さい。
- アプリケーション名 は `XamAppCenterSample2018` にして下さい。
- バンドル識別子の Organization Identifier の部分（hiro127777）は全世界で固有となるような文字列にして下さい。
  
  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/test002.png?raw=true)
  
  
# 共有コードの作成 #
  
  
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
　  
　  
　  
　  
　  
　  
お疲れ様でした。これでハンズオンは終了です。

