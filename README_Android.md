# Xamarin.Android App Center ハンズオン テキスト #
　  
　  
# はじめに #
　  
　  
App Center で Xamarin.Android アプリの自動ビルド、UIテストが試せる ハンズオンです。
　  
　  
Cognitive Services の Translator Text API を利用して、入力した日本語を英語に翻訳してくれるサンプルアプリを題材としています。
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android.png?raw=true)
　  
　  
# 必要環境 #
　  
- Visual Studio for Mac 最新版のインストール
- 有効な Github のアカウント
- 有効な Azure のアカウント
- 有効な App Center のアカウント（テストの無料試用が終了している場合、11,088円を Microsoft に支払う必要があります）
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
　  
　  
















































　  
# iOS アプリの作成 #
　  
## iOS の バンドル識別子 の設定 ## 
　  
iOS のアプリの バンドル識別子 を御自身の固有のものに変更して下さい。
- アプリケーション名 は XamAppCenterSample2018 にして下さい。
- バンドル識別子の Organization Identifier の部分は全世界で固有となるような文字列にして下さい。
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/test002.png?raw=true)
　  
　  
　  





# Android アプリの作成 #
　  


　  
　  












　  
　  
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
　  
　  
　  
お疲れ様でした。これで今回のハンズオンは終了です！！
