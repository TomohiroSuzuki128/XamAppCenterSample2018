# Xamarin.Android アプリ開発 ハンズオン テキスト #
  
  
[iOS はこちら](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/master/README_iOS_App_Only.md)
  
  
# はじめに #
　  
　  
Xamarin.Android でのアプリ作成の基礎を体験できるハンズオンです。
　  
　  
Cognitive Services の Translator Text API を利用して、入力した日本語を英語に翻訳してくれるサンプルアプリを題材としています。
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android.png?raw=true)
　  
　  
# 必要環境 #
　  
- Visual Studio 2017 または Visual Studio for Mac 最新版のインストール
- 有効な Azure のアカウント
- （必須ではないが確認用にあると望ましい） Android 7.0 以上の Android 実機
　  
　  
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
  
  
## Android の パッケージ名の設定 ## 

Android のアプリのパッケージ名を御自身の固有のものに変更して下さい。
- アプリケーション名 は XamAppCenterSample2018 にして下さい。

![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/test001.png?raw=true)
  
  
  
  
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
<?xml version="1.0" encoding="utf-8"?>
<resources>
    <string name="input">翻訳したい日本語</string>
    <string name="translate">英語に翻訳する</string>
    <string name="translated">翻訳された英語</string>
    <string name="app_name">XamAppCenterSample2018.Droid</string>
</resources>
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

  
  
  
## Android アプリのデバッグ ##
  
  
では、ここで Android のアプリを実機デバッグしてみましょう。

実機をお持ちの方はせっかくですから実機でデバッグしてみましょう。
お持ちでない方はシミュレータでデバッグしてみましょう。
　  
　  
### シミュレータデバッグ ###
　  
XamAppCenterSample2018.Droid > Debug > [シミュレータの機種名] に設定します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/debug002.png?raw=true)
　  
　  
「デバッグの開始」を実行します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/debug003.png?raw=true)
　  
　  
アプリが起動し、飜訳が動作すれば成功です。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/debug004.png?raw=true)
　  
　  
### 実機デバッグ ###
　  
実機をお持ちの方は、ここで Android のアプリを実機デバッグしてみましょう。
　  
　  
### 実機の開発者モードを有効にし、USBデバッグを有効にする ###
　  
実機の開発者モードを有効にし、USBデバッグを有効にしないと実機デバッグができないので変更します。
　  
　  
「システム」をタップします。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android001.png?raw=true)
　  
　  
　  
「端末情報」をタップします。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android002.png?raw=true)
　  
　  
　  
「ソフトウェア情報」をタップします。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android003.png?raw=true)
　  
　  
　  
「ビルド番号」を連打します。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android004.png?raw=true)
　  
　  
　  
開発者モードが有効になりました。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android005.png?raw=true)
　  
　  
　  
「開発者向けオプション」をタップ。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android006.png?raw=true)
　  
　  
　  
「開発者向けオプション」を ON にし、「USBデバッグ」を ON にします。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android007.png?raw=true)
　  
　  
これで実機デバッグの準備が整いました。
　  
　  
　  
### 実機デバッグ開始 ###
　  
　  
XamAppCenterSample2018.Droid > Debug > [あなたのAndroidデバイス名] に設定します。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android008.png?raw=true)
　  
　  
　  
　  
アプリが起動します。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android009.png?raw=true)
　  
　  
　  
　  
飜訳が動作すれば成功です。
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android010.png?raw=true)
　  
　  
　  
　  
お疲れ様でした。これでハンズオンは終了です。


