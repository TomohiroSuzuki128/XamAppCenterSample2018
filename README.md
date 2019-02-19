# Xamarin.iOS App Center ハンズオン テキスト 完全版 #
　  
　  
# はじめに #
　  
　  
App Center で Xamarin.iOS アプリの自動ビルド、UIテストが試せる ハンズオンです。
　  
　  
Cognitive Services の Translator Text API を利用して、入力した日本語を英語に翻訳してくれるサンプルアプリを題材としています。
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/iPhone.png?raw=true)
　  
　  
# 必要環境 #
　  
- Visual Studio for Mac 最新版のインストール
- Xcode 最新版のインストール
- 有効な Github のアカウント
- 有効な Azure のアカウント
- 有効な App Center のアカウント（テストの無料試用が終了している場合、11,088円を Microsoft に支払う必要があります）
- 有効な Apple のディベロッパー登録
- iOS11 以上のインストールされた iPhone の実機
　  
　  
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
　  
　  
## リポジトリを Fork ## 
　  
　  
https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/  
にアクセスしてリポジトリを Fork してください。
　  
　  
## ソリューションを開きます。 ##
　  
　  
/src/Start/XamAppCenterSample2018.sln を開いてください。
　  
　  
## iOS の バンドル識別子 の設定 ## 
　  
iOS プロジェクトの `Info.plist` を開き、iOS のアプリの バンドル識別子 を御自身の固有のものに変更して下さい。
- アプリケーション名 は `XamAppCenterSample2018` にして下さい。
- バンドル識別子の Organization Identifier の部分（hiro127777）は全世界で固有となるような文字列にして下さい。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/test002.png?raw=true)
　  
　  
# 共有コードの作成 #
　  
　  
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
　  
　  
　  
　  
　  
# テストプロジェクトの作成 #
　  
　  
## AppInitializer の作成 ## 
　  
　  
テスト時にアプリを初期化するクラスを作成します。
　  
　  
/UITests/AppInitializercs ファイルを開きます。
　  
　  
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
            StringAssert.Contains(string.Empty, elements.FirstOrDefault().Text);
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
            StringAssert.Contains(string.Empty, elements.FirstOrDefault().Text);
        }
    }
}
```
　 
　 
## Visual Studio App Center に App を作成する ## 
　  
　  
App Center にログインし、右上の「add new」から「add new app」を選択
　  
　  
App Name, OS, Platform を入力、選択し、「Add new app」をクリック
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/006.png?raw=true)
　  
　  
## App Center で iOS のビルドの設定 ##
　  
　  
「Build」を選択し、ソースコードをホストしたサービスを選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/024.png?raw=true)
　  
　  
「XamAppCenterSample2018」を選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/008.png?raw=true)
　  
　  
自動ビルドしたいブランチの設定アイコンを選択します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/009.png?raw=true)
　  
　  
ビルド設定を選択し、入力し、「Save & Build」を選択。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/010.png?raw=true)
　  
　  
ビルドが始まるのでしばらく待ち、成功すれば完了です。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/011.png?raw=true)
　  
　  
　  
　  
# App Center で、API Key をビルド時にインサートする # 
　  
　  
自動ビルドするときに API へのアクセスキーなどの秘匿情報などは、リポジトリにプッシュしてはいけません。でもそうすると、自動ビルド後のテストなどで、API にアクセスできないので自動テストで困ってしまいます。 よって、ビルドサーバがリポジトリから Clone した後か、ビルド前に秘匿情報をインサートする方法が便利です。
　  
今回は、ビルドサーバがリポジトリから Clone した後に API Key が自動インサートされるように設定します。
　  
　  
## Visual Studio App Center のビルド設定に秘匿情報を環境変数として登録する ## 
　  
　  
Visual Studio App Center のビルド設定を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios001.png?raw=true)
　  
　  
`Environment variables` に環境変数名とキーの値を登録します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios002.png?raw=true)
　  
　  
## ソリューションを開きます。 ##
　  
　  
/src/StartShort/XamAppCenterSample2018.sln を開いてください。
　  
　  
## ソースコード上に置き換え用の目印となる文字列を準備します。 ##
　  
　  
/src/StartShort/XamAppCenterSample2018/Variables.cs を確認してください。
　  
　  
**/src/StartShort/XamAppCenterSample2018/Variables.cs**
```csharp
using System;

namespace XamAppCenterSample2018
{
    public static class Variables
    {
        // NOTE: Replace this example key with a valid subscription key.
        public static readonly string ApiKey = "[ENTER YOUR API KEY]";
    }
}
```
　  
　  
上記の`[ENTER YOUR API KEY]`のように置き換えの目印になる文字列を設定しておきます。
　  
　  
## clone 後に自動実行されるシェルスクリプトを準備します。 ##
　  
　  
App Center には ビルドする`cspoj`と同じ階層に、`appcenter-post-clone.sh`という名前でシェルスクリプトを配置しておくと、自動認識し clone 後に自動実行してくれる機能があります。
よって、`appcenter-post-clone.sh`に`[ENTER YOUR API KEY]`を本物のキーに置き換えを行う処理を書きます。
　  
　  
**/src/Start/iOS/appcenter-post-clone.sh**
```sh
#!/usr/bin/env bash

# Insert App Center Secret into Variables.cs file in my common project

# Exit immediately if a command exits with a non-zero status (failure)
set -e 

##################################################
# variables

# (1) The target file
MyWorkingDir=$(cd $(dirname $0); pwd)
DirName=$(dirname ${MyWorkingDir})
filename="$DirName/XamAppCenterSample2018/Variables.cs"

# (2) The text that will be replaced
stringToFind="\[ENTER YOUR API KEY\]"

# (3) The secret it will be replaced with
AppCenterSecret=$API_Key # this is set up in the App Center build config

##################################################


echo ""
echo "##################################################################################################"
echo "Post clone script"
echo "  *Insert App Center Secret"
echo "##################################################################################################"
echo "        Working directory:" $DirName
echo "Secret from env variables:" $AppCenterSecret
echo "              Target file:" $filename
echo "          Text to replace:" $stringToFind
echo "##################################################################################################"
echo ""


# Check if file exists first
if [ -e $filename ]; then
    echo "Target file found"
else
    echo "Target file($filename) not found. Exiting."
    exit 1 # exit with unspecified error code. Should be obvious why we can't continue the script
fi


# Load the file
echo "Load file: $filename"
apiKeysFile=$(<$filename)


# Seach for replacement text in file
matchFound=false # flag to indicate we found a match

while IFS= read -r line; do
if [[ $line == *$stringToFind* ]]
then
# echo "Line found:" $line
    echo "Line found"
    matchFound=true

    # Edit the file and replace the found text with the Secret text
    # sed: stream editior
    #  -i: in-place edit
    #  -e: the following string is an instruction or set of instructions
    #   s: substitute pattern2 ($AppCenterSecret) for first instance of pattern1 ($stringToFind) in a line
    cat $filename | sed -i -e "s/$stringToFind/$AppCenterSecret/" $filename

    echo "App secret inserted"

    break # found the line, so break out of loop
fi
done< "$filename"

# Show error if match not found
if [ $matchFound == false ]
then
    echo "Unable to find match for:" $stringToFind
    exit 1 # exit with unspecified error code.
fi

echo ""
echo "##################################################################################################"
echo "Post clone script completed"
echo "##################################################################################################"
```
　  
　  
このスクリプトがやっていることは、
- キーを置き換えるファイルを探す。
- ファイルの中から置き換え用の目印となる文字列を探し、本物のキーに置き換える。
それだけです。

　  
　  
このスクリプトを含んだリポジトリをプッシュすると、以下のように、App Center 側で認識されます。
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios003.png?raw=true)
　  
　  
## ビルドを実行し、ログを確認してシェルスクリプトが正しく実行されていることを確認。 ##
　  
　  
正しく実行されていれば、以下のようにログで確認できます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios004.png?raw=true)
　  
　  
以上で、Visual Studio App Center で、秘匿情報をビルド時にインサートする手順は完了です。
　  
　  
　  
　  
　  
　  
# Visual Studio App Center で、自動ビルド後に iOS の自動実機UIテストを実行する #
　  
　  
次は、いよいよ自動ビルド後に iOS の自動実機UIテストを実行する設定を進めていきます。
　  
　  
　  
　  
## Visual Studio App Center のテスト 30-day trial を有効にします ##
　  
　  
「Test」 -> 「new test run」をクリック
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/016.png?raw=true)
　  
　  
「Start 30-day trial」をクリック
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/017.png?raw=true)
　  
　  
　  
　  
## Visual Studio App Center のテスト設定に実機UIテストを走らせるデバイスの組み合わせのセットを登録します ##
　  
　  
Visual Studio App Center では、1回のテストで、複数の実機の自動UIテストを走らせることができますので、テストを走らせる実機を選択して登録しておきます。
　  
　  
Visual Studio App Center のテスト設定のデバイスセット設定を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios011.png?raw=true)
　  
　  
`Set name` を設定し、テストを実行するデバイスにチェックを入れ `New device set` で保存します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios012.png?raw=true)
　  
　  
Device set が登録されていることを確認します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios013.png?raw=true)
　  
　  
　  
　  
## Visual Studio App Center にログインするキーを準備します。 ##
　  
　  
自動実行されるシェルスクリプトが自動テストを実行するときに、事前に取得しておいたキーを使って App Center にログインします。そのキーをスクリプトから利用できるように環境変数に登録しておきます。
　  
　  
「Account settings」を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios014.png?raw=true)
　  
　  
「New API token」を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios015.png?raw=true)
　  
　  
APIの利用目的の説明とアクセス権を設定し保存します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios016.png?raw=true)
　  
　  
キーが表示されるのでコピーしメモ（保管）しておきます。キーは画面を閉じると2度と表示されないのでご注意ください。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios017.png?raw=true)
　  
　  
キーが登録されていることを確認します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios018.png?raw=true)
　  
　  
次にビルド設定を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios019.png?raw=true)
　  
　  
環境変数にキーを設定して、保存します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios020.png?raw=true)
　  
　  
## 証明書と Provisioning Profile を準備します。 ##
　  
　  
App Center で iOS 実機での自動UIテストを行うには、証明書と Provisioning Profile が必要になりますので準備します。
　  
　  
Apple Developer Program のサイトで証明書（.cer）、Provisioning Profile（.mobileprovision） を作成し、ローカルの Mac の キーチェーンアクセス で 証明書（.p12） を作成します。
　  
　  
この方法についてはWeb上に情報がたくさんあるので、以下のセクションのリンクのWebの情報を参考に行なってください。
　  
　  
　  
### 証明書を準備します。 ###
　  
　  
[証明書作成方法](https://i-app-tec.com/ios/app-release.html#1)
　  
　  
### Provisioning Profile を準備します。 ###
　  
　  
[Provisioning Profile作成方法](https://i-app-tec.com/ios/app-release.html#2)
　  
　  
　  
作成した Provisioning Profile（.mobileprovision）、証明書（.p12）を保管しておきます。
　  
　  
## 作成した Provisioning Profile（.mobileprovision）、証明書（.p12）を App Center にアップロードします ##
　  
　  
ビルド設定を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios019.png?raw=true)
　  
　  
`Build type` を `Device build` に変更します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios040.png?raw=true)
　  
　  
作成した Provisioning Profile（.mobileprovision）、証明書（.p12）を アップロードし、保存します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios041.png?raw=true)  
　  
　  
## build 後に自動実行されるシェルスクリプトを準備します。 ##
　  
　  
Visual Studio App Center には ビルドする`cspoj`と同じ階層に、`appcenter-post-build.sh`という名前でシェルスクリプトを配置しておくと、自動認識し build 後に自動実行してくれる機能があります。
よって、`appcenter-post-build.sh`に、自動実機UIテストを実行する処理を書きます。
　  
ファイルはすでに準備されていますので、設定値を書き換えてください。
　  
　  
**/src/StartShort/iOS/appcenter-post-build.sh**
```sh
#!/usr/bin/env bash

# Post Build Script

# Exit immediately if a command exits with a non-zero status (failure)
set -e 

##################################################
# variables

appCenterLoginApiToken=$AppCenterLoginToken # this comes from the build environment variables
appName="TomohiroSuzuki128/XamAppCenterSample2018iOS" # 自分のアプリ名に書き換える
deviceSetName="TomohiroSuzuki128/my-devices" # 自分のデバイスセット名に書き換える
publishedAppFileName="XamAppCenterSample2018.iOS.ipa"
sourceFileRootDir="$APPCENTER_SOURCE_DIRECTORY/src/StartShort"
uiTestProjectName="UITests"
testSeriesName="all-tests"
##################################################

echo "##################################################################################################"
echo "Post Build Script"
echo "##################################################################################################"
echo "Starting Xamarin.UITest"
echo "   App Name: $appName"
echo " Device Set: $deviceSetName"
echo "Test Series: $testSeriesName"
echo "##################################################################################################"
echo ""

echo "> Build UI test projects"
find $sourceFileRootDir -regex '.*Test.*\.csproj' -exec msbuild {} \;

echo "> Run UI test command"
# Note: must put a space after each parameter/value pair
appcenter test run uitest --app $appName --devices $deviceSetName --app-path $APPCENTER_OUTPUT_DIRECTORY/$publishedAppFileName --test-series $testSeriesName --locale "ja_JP" --build-dir $sourceFileRootDir/$uiTestProjectName/bin/Debug --uitest-tools-dir $sourceFileRootDir/packages/Xamarin.UITest.*/tools --token $appCenterLoginApiToken 

echo ""
echo "##################################################################################################"
echo "Post Build Script complete"
echo "##################################################################################################"
```
　  
　  
このスクリプトがやっていることは、
- UIテストプロジェクトをビルドする。
- App Center に、自動UIテストのコマンドを発行し、実行させる。
の2つです。

　  
　  
このスクリプトを含んだリポジトリをプッシュすると、以下のように、App Center 側で認識されます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios050.png?raw=true)
　  
　  
　  
　  
## スクリプトのデバッグ方法 ##
　  
　  
このようなスクリプトを自作するときに一番困るのが、
- 「指定したファイルが見つからない」エラーが発生すること
- 環境変数の中身がよくわからないこと
です。

　  
　  
よってスクリプトを自作するときには、下記のように環境変数やディレクトリの中身をコンソールに表示させながらスクリプトを書くことで効率よくデバッグできます。
　  
　  
```sh
# for test
echo $APPCENTER_SOURCE_DIRECTORY
echo ""
files="$APPCENTER_SOURCE_DIRECTORY/src/StartShort/UITests/*"
for filepath in $files
do
  echo $filepath
done
```
　  
　  
　  
　  
## ビルドを実行し、テスト結果を確認してテストが正しく実行されていることを確認します。 ##
　  
　  
正しく実行されていれば、以下のようにテスト結果が確認できます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios051.png?raw=true)
　  
　  
また、テストコード内に以下のように、`app.Screenshot("<スクリーンショット名>")`と記述することで、スクーンショットが自動で保存されます。
　  
　  
```csharp
[Test]
public async void SucceedTranslate()
{
    await Task.Delay(2000);
    app.Screenshot("App launched");
    await Task.Delay(2000);
    app.Tap(c => c.Marked("inputText"));
    await Task.Delay(2000);
    app.EnterText("私は毎日電車に乗って会社に行きます。");
    await Task.Delay(2000);
    app.Screenshot("Japanese text entered");
    await Task.Delay(2000);
    app.DismissKeyboard();
    await Task.Delay(2000);
    app.Tap(c => c.Button("translateButton"));
    await Task.Delay(4000);
    var elements = app.Query(c => c.Marked("translatedText"));
    await Task.Delay(2000);
    app.Screenshot("Japanese text translated");
    await Task.Delay(2000);
    Assert.AreEqual("I go to the office by train every day.", elements.FirstOrDefault().Text);
}
```
　  
　  
保存されたスクリンショットは以下の手順で確認できます。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios052.png?raw=true)
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios053.png?raw=true)
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/ios054.png?raw=true)
　  
　  
これで、リポジトリにプッシュすると自動ビルドが走り、自動実機UIテストが実行されるようになりました！！
　  
　  
お疲れ様でした。これでハンズオンは終了です。

