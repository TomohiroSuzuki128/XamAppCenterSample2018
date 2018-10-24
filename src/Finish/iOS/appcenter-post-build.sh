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
uiTestProjectName="XamAppCenterSample2018.UITests"
testSeriesName="master"
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
files="$APPCENTER_SOURCE_DIRECTORY/*"
for filepath in $files
do
  echo $filepath
done

echo "> Run UI test command"
# Note: must put a space after each parameter/value pair
appcenter test run uitest --app $appName --devices $deviceSetName --app-path $APPCENTER_OUTPUT_DIRECTORY/$ipaFileName --test-series $testSeriesName --locale "ja_JP" --build-dir $APPCENTER_SOURCE_DIRECTORY/$uiTestProjectName/bin/Debug --uitest-tools-dir $APPCENTER_SOURCE_DIRECTORY/packages/Xamarin.UITest.*/tools --token $appCenterLoginApiToken 

echo ""
echo "**************************************************************************************************"
echo "Post Build Script complete"
echo "**************************************************************************************************"