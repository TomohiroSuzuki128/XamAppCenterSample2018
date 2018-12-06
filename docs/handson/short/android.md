# Xamarin.Android App Center ハンズオン テキスト#
　  
　  
# はじめに #
　  
　  
App Center で Xamarin.Android アプリの自動ビルド、UIテストが試せる ハンズオンです。
　  
　  
Cognitive Services の Translator Text API を利用して、入力した日本語を英語に翻訳してくれるサンプルアプリを題材としています。
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/images/Android.png?raw=true)
　  
　  
# 必要環境 #
　  
## Android 自動ビルド ##
- Visual Studio for Mac がインストールされたMac
- Azure のアカウント
- App Center のアカウント
　  
## Android UIテスト ##
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
　  
　  
## Visual Studio App Center に App を作成する ## 
　  
　  
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
　  
　  
　  
　  
# App Center で、API Key をビルド時にインサートする # 
　  
　  
自動ビルドするときに API へのアクセスキーなどの秘匿情報などは、リポジトリにプッシュしてはいけません。でもそうすると、自動ビルド後のテストなどで、API にアクセスできないので自動テストで困ってしまいます。 よって、ビルドサーバがリポジトリから Clone した後か、ビルド前に秘匿情報をインサートする方法が便利です。
　  
今回は、ビルドサーバがリポジトリから Clone した後に API Key が自動インサートされるように設定します。
　  
　  
## Visual Studio App Center のビルド設定に秘匿情報を環境変数として登録する ## 
　  
　  
Visual Studio App Center のビルド設定を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android001.png?raw=true)
　  
　  
`Environment variables` に環境変数名とキーの値を登録します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android002.png?raw=true)
　  
　  
## ソースコード上に置き換え用の目印となる文字列を準備します。 ##
　  
　  
/src/Finish/XamAppCenterSample2018/Variables.cs を確認してください。
　  
　  
**/src/Finish/XamAppCenterSample2018/Variables.cs**
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
　  
　  
**/src/Finish/Droid/appcenter-post-clone.sh**
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
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android003.png?raw=true)
　  
　  
## ビルドを実行し、ログを確認してシェルスクリプトが正しく実行されていることを確認。 ##
　  
　  
正しく実行されていれば、以下のようにログで確認できます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android004.png?raw=true)
　  
　  
以上で、Visual Studio App Center で、秘匿情報をビルド時にインサートする手順は完了です。
　  
　  
　  
　  
　  
　  
# Visual Studio App Center で、自動ビルド後に Android の自動実機UIテストを実行する #
　  
　  
次は、いよいよ自動ビルド後に Android の自動実機UIテストを実行する設定を進めていきます。
　  
　  
** Visual Studio App Center のテスト設定に実機UIテストを走らせるデバイスの組み合わせのセットを登録します
　  
　  
Visual Studio App Center では、1回のテストで、複数の実機の自動UIテストを走らせることができますので、テストを走らせる実機を選択して登録しておきます。
　  
　  
Visual Studio App Center のテスト設定のデバイスセット設定を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android011.png?raw=true)
　  
　  
`Set name` を設定し、テストを実行するデバイスにチェックを入れ `New device set` で保存します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android012.png?raw=true)
　  
　  
Device set が登録されていることを確認します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android013.png?raw=true)
　  
　  
　  
　  
## Visual Studio App Center にログインするキーを準備します。 ##
　  
　  
自動実行されるシェルスクリプトが自動テストを実行するときに、事前に取得しておいたキーを使って App Center にログインします。そのキーをスクリプトから利用できるように環境変数に登録しておきます。
　  
　  
「Account settings」を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android014.png?raw=true)
　  
　  
「New API token」を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android015.png?raw=true)
　  
　  
APIの利用目的の説明とアクセス権を設定し保存します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android016.png?raw=true)
　  
　  
キーが表示されるのでコピーしメモ（保管）しておきます。キーは画面を閉じると2度と表示されないのでご注意ください。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android017.png?raw=true)
　  
　  
キーが登録されていることを確認します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android018.png?raw=true)
　  
　  
次にビルド設定を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android019.png?raw=true)
　  
　  
環境変数にキーを設定して、保存します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android020.png?raw=true)
　  
　  
## keystore ファイルを準備します。 ##
　  
　  
実機での自動UIテストを行うには keystore が必要になりますので作成します。
　  
　  
Visual Studio for Mac で、Android のプロジェクトを Release に設定して、ビルド ->  発行のためのアーカイブ を押下します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android021.png?raw=true)
　  
　  
「署名と配布」を押下します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android022.png?raw=true)
　  
　  
アドホックを選択して「次へ」を押下します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android023.png?raw=true)
　  
　  
「キーの新規作成」を押下します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android024.png?raw=true)
　  
　  
項目を入力し、「OK」を押下します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android025.png?raw=true)
　  
　  
エイリアス名を右クリックし、「エイリアス情報を表示」を押下します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android026.png?raw=true)
　  
　  
keystore ファイルの場所が表示されるので、ファイルを取得します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android027.png?raw=true)
　  
　  
App Center のビルド設定を開きます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android028.png?raw=true)
　  
　  
keystore ファイルをアップします。
key alias, Key password は「新しい証明書を作成」画面で入力したものを入力します。
Keystore パスワードは Key password と同じものを入力してください。
入力したら保存します。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android029.png?raw=true)
　  
　  
　  
　  
## build 後に自動実行されるシェルスクリプトを準備します。 ##
　  
　  
Visual Studio App Center には ビルドする`cspoj`と同じ階層に、`appcenter-post-build.sh`という名前でシェルスクリプトを配置しておくと、自動認識し build 後に自動実行してくれる機能があります。
よって、`appcenter-post-build.sh`に、自動実機UIテストを実行する処理を書きます。
　  
　  
**/src/Finish/Droid/appcenter-post-build.sh**
```sh
#!/usr/bin/env bash

# Post Build Script

# Exit immediately if a command exits with a non-zero status (failure)
set -e 

##################################################
# variables

appCenterLoginApiToken=$AppCenterLoginToken # this comes from the build environment variables
appName="TomohiroSuzuki128/XamAppCenterSample2018Droid"
deviceSetName="TomohiroSuzuki128/my-devices-android"
publishedAppFileName="com.hiro128777.XamAppCenterSample2018.apk"
sourceFileRootDir="$APPCENTER_SOURCE_DIRECTORY/src/Finish"
uiTestProjectName="UITests"
testSeriesName="all-tests-android"
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
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android030.png?raw=true)
　  
　  
　  
　  
## スクリプトのデバッグ方法。 ##
　  
　  
このようなスクリプトを自作するときに一番困るのが、
- 「指定したファイルが見つからない」エラーが発生すること
- 環境変数の中身がよくわからないこと
です。
　  
　  
よってスクリプトを自作するときには、下記のように環境変数やディレクトリの中身をコンソールに表示させながらスクリプトを書くことで効率よくデバッグできます。
　  
　  
```sh
# for test
echo $APPCENTER_SOURCE_DIRECTORY
echo ""
files="$APPCENTER_SOURCE_DIRECTORY/src/Finish/UITests/*"
for filepath in $files
do
  echo $filepath
done
```
　  
　  
　  
　  
## ビルドを実行し、テスト結果を確認してテストが正しく実行されていることを確認します。 ##
　  
　  
正しく実行されていれば、以下のようにテスト結果が確認できます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android031.png?raw=true)
　  
　  
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
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android032.png?raw=true)
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android033.png?raw=true)
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/android034.png?raw=true)
　  
　  
これで、リポジトリにプッシュすると自動ビルドが走り、自動実機UIテストが実行されるようになりました！！
　  
　  
お疲れ様でした。これでハンズオンは終了です。


