#!/usr/bin/env bash

# Post Build Script

# Exit immediately if a command exits with a non-zero status (failure)
set -e 

##################################################
# variables

appCenterLoginApiToken=$AppCenterLoginForAutomatedUITests # this comes from the build environment variables
appName="TomohiroSuzuki128/XamAppCenterSample2018iOS"
deviceSetName="TomohiroSuzuki128/my-devices"
ipaFileName="XamAppCenterSample2018.iOS.ipa"
sourceFileRootDir="$APPCENTER_SOURCE_DIRECTORY/src/Finish"
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


# for test
echo $APPCENTER_SOURCE_DIRECTORY
echo ""
files="$APPCENTER_SOURCE_DIRECTORY/src/Finish/UITests/*"
for filepath in $files
do
  echo $filepath
done

echo "> Build UI test projects"
find $sourceFileRootDir -regex '.*Test.*\.csproj' -exec msbuild {} \;

echo "> Run UI test command"
# Note: must put a space after each parameter/value pair
appcenter test run uitest --app $appName --devices $deviceSetName --app-path $APPCENTER_OUTPUT_DIRECTORY/$ipaFileName --test-series $testSeriesName --locale "ja_JP" --build-dir $sourceFileRootDir/$uiTestProjectName/bin/Debug --uitest-tools-dir $sourceFileRootDir/packages/Xamarin.UITest.*/tools --token $appCenterLoginApiToken 

echo ""
echo "##################################################################################################"
echo "Post Build Script complete"
echo "##################################################################################################"