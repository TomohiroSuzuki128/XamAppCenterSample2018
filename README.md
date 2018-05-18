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
  
  
# 環境構築 #

## node.js のインストール ## 
この方法についてはWeb上に情報がたくさんあるので、Webの情報を参考に行なってください。

## App Center CLI のインストール ## 

コマンドラインから、以下のコマンドでインストール
npm install -g appcenter-cli

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

  
# Cognitive Services の Translator Text API 作成 #

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



