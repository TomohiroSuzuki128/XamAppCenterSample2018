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
  
  
# iOSの自動ビルド #
  
  
## 証明書、Provisioning Profile の作成 ##
Apple Developer Program のサイトで証明書（.cer）、Provisioning Profile（.mobileprovision） を作成し、ローカルの Mac の キーチェーンアクセス で 証明書（.p12） を作成します。
この方法についてはWeb上に情報がたくさんあるので、Webの情報を参考に行なってください。
  
  
作成した Provisioning Profile（.mobileprovision）、証明書（.p12）を保管しておきます。
  
  
## App Center で iOS の App の作成 ##
  
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

## App Center で iOS の UITest の設定 ##
