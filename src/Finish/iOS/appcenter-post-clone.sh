#!/usr/bin/env bash

# Insert the iOS App Center Secret into Variables.cs file in my common project

set -e # Exit immediately if a command exits with a non-zero status (failure)

##################################################
# Setup

# 1.) The target file
CURRENT_DIRECTORY=$(cd $(dirname $0); pwd)
DIR_NAME=$(dirname ${CURRENT_DIRECTORY})
#filename="$DIR_NAME/XamAppCenterSample2018/Variables.cs"
filename="$DIR_NAME/iOS/Variables.cs"

# 2.) The text that will be replaced
stringToFind="\[ENTER YOUR API KEY\]"

# 3.) The secret it will be replaced with
AppCenterSecret=$API_Key # this is set up in the App Center build config

##################################################


echo ""
echo "**************************************************************************************************"
echo "App Center Secret Inserter"
echo "**************************************************************************************************"
echo "        Working directory:" $PWD
echo "Secret from env variables:" $AppCenterSecret
echo "              Target file:" $filename
echo "          Text to replace:" $stringToFind
echo "**************************************************************************************************"
echo ""

# for test
files="$DIR_NAME/iOS/*"
for filepath in $files; do
  echo $filepath
done

# Check if file exists first
if [ -e $filename]
then
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
echo "**************************************************************************************************"
echo "Script complete"
echo "**************************************************************************************************"