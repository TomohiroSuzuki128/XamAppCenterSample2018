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
　  
　  
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/andtoid002.png?raw=true)
　  
　  
## ソースコード上に置き換え用の目印となる文字列を準備します。 ##
　  
　  
/src/Finish/XamAppCenterSample2018/Variables.cs を確認してください。
　  
　  
/src/Finish/XamAppCenterSample2018/Variables.cs
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
　  
　  
/src/Finish/Droid/appcenter-post-clone.sh
　  
　  
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
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/andtoid003.png?raw=true)
　  
　  
## ビルドを実行し、ログを確認してシェルスクリプトが正しく実行されていることを確認。 ##
　  
　  
正しく実行されていれば、以下のようにログで確認できます。
![](https://github.com/TomohiroSuzuki128/XamAppCenterSample2018/blob/develop/docs/handson/short/images/andtoid004.png?raw=true)
　  
　  
以上で、Visual Studio App Center で、秘匿情報をビルド時にインサートする手順は完了です。


