# はじめに #

App Center で自動ビルド、UIテストが試せる iOS, Android のサンプルアプリです。

Cognitive Services の Translator Text API を利用して、入力した日本語を英語に翻訳してくれます。

&copy;2018 Tomohiro Suzuki All rights reserved.  
本コンテンツの著作権、および本コンテンツ中に出てくる商標権、団体名、ロゴ、製品、サービスなどはそれぞれ、各権利保有者に帰属します。

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



では、ここでiOSのアプリを実機デバッグしてみましょう。
iOSのアプリを実機デバッグするにはXcodeでダミーアプリを実行する必要があります。

## Xcode でのダミーアプリ実行 ##

プロビジョニングプロファイルや証明書の紐付けが自動で行われるようにXcodeでSwiftのダミーアプリを作成します。

[File]->[New]->[Project]でプロジェクトを作成します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app001.png?raw=true)



iOSのSingle View Applicationを選択し、[Next]を押します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/dammy_app002.png?raw=true)



Product Name は XamAppCenterSample2018 にして下さい。
Organization Identifier はユニークな名前になるようにしてください。
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


## iOS アプリのビルド ## 

/XamAppCenterSample2018/XamAppCenterSample2018.iOS/Info.plist ファイルを開きます。

「バンドル識別子」の文字列を先ほど Xcode で設定した、Bundle Identifier と一字一句違わないように設定します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build001.png?raw=true)



XamAppCenterSample2018.iOS > Debug > [あなたのiPhone名] に設定します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build002.png?raw=true)



「デバッグの開始」を実行します。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build003.png?raw=true)



アプリが起動すれば成功です。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/ios_build004.png?raw=true)





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

# 環境構築 #

## node.js のインストール ## 
この方法についてはWeb上に情報がたくさんあるので、Webの情報を参考に行なってください。

## App Center CLI のインストール ## 

コマンドラインから、以下のコマンドでインストール
```bash
npm install -g appcenter-cli
```

権限が無いと怒られて、パッケージのインストールに失敗する場合、下記の手順でnpmのデフォルトディレクトリの権限を変更する

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



