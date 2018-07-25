#!/bin/bash -e

# eBayPulse Release Builder for Ubuntu
#
# This is a part of the eBayPulse project (https://github.com/kpanova/eBayPulse).
#
# MIT License
#
# Copyright (c) 2018 Ksenia Panova <mail@panovaks.ru> and
# Dmitry Bravikov <dmitry@bravikov.pro>
#
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:
#
# The above copyright notice and this permission notice shall be included in all
# copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
# SOFTWARE.

command -v dotnet || (echo "Error: dotnet-sdk does not installed."; exit 1)

BUILDER_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd $BUILDER_DIR

PROJECT_DIR="$BUILDER_DIR/../.."

PUBLISH_DIR="$PROJECT_DIR/bin/Release/netcoreapp2.0/publish"
rm -rf $PUBLISH_DIR

cd $PROJECT_DIR
dotnet publish -c Release

BUILD_DIR=$BUILDER_DIR/build
mkdir -p $BUILD_DIR
cd $BUILD_DIR

ARCHIVED_DIR=ebaypulse
rm -rf $ARCHIVED_DIR
mkdir -p $ARCHIVED_DIR

ARCHIVED_BIN_DIR=ebaypulse/bin
mkdir -p $ARCHIVED_BIN_DIR

cp -rf $PUBLISH_DIR/* $ARCHIVED_BIN_DIR
rm -rf $PUBLISH_DIR
cp -f $PROJECT_DIR/ebaypulse.service $ARCHIVED_BIN_DIR
cp -f $PROJECT_DIR/installer/ubuntu/installer.sh $ARCHIVED_DIR

VERSION=`cat $PROJECT_DIR/VERSION`
ARCHIVE_FILE="ebaypulse-$VERSION-ubuntu.tar.gz"

tar -cvzf $ARCHIVE_FILE $ARCHIVED_DIR

rm -rf $ARCHIVED_DIR

echo "Build completed."
echo $ARCHIVE_FILE
