#!/bin/bash -e

# eBayPulse Installer for Ubuntu
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

PROGRAM_NAME="eBay Pulse"

function help {
    echo -n -e "\n"\
        "$PROGRAM_NAME Installer for Ubuntu\n"\
        "\n"\
        "Usage:\n"\
        "\n"\
        "  ./installer.sh [option]\n"\
        "\n"\
        "Options:\n"\
        "\n"\
        "  --help          Display this information\n"\
        "  --install       Install $PROGRAM_NAME\n"\
        "  --update        Update $PROGRAM_NAME\n"\
        "  --remove        Remove $PROGRAM_NAME\n"\
        "  --check         Check $PROGRAM_NAME\n"\
        "\n"\
        "More informations: https://github.com/kpanova/eBayPulse\n"\
        "\n"\
        ""
}

SERVICE=ebaypulse.service
SERVICE_PATH=/etc/systemd/system/$SERVICE
EBAYPULSE_DIR="/opt/ebaypulse"
USERNAME="ebaypulse"
DOTNET_PACKAGE="dotnet-runtime-2.0.7"

INSTALLER_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd $INSTALLER_DIR

function remove {
    sudo true
    
    if [[ ! -d $EBAYPULSE_DIR ]]
    then
        echo "Error: $PROGRAM_NAME directory is not found"
        exit 1
    fi

    if [[ -f $SERVICE_PATH ]]
    then
        sudo systemctl stop ebaypulse
        sudo systemctl disable ebaypulse
        sudo rm $SERVICE_PATH
    fi
    
    sudo rm -rf $EBAYPULSE_DIR

    if id $USERNAME > /dev/null 2>&1
    then
        sudo deluser $USERNAME
    fi

    echo "$PROGRAM_NAME has been deleted"
}

function install {
    sudo true

    if [[ -d $EBAYPULSE_DIR ]]
    then
        echo "Error: $PROGRAM_NAME is already installed"
        exit 1
    fi

    # Install depedencies
    if ! dpkg -s $DOTNET_PACKAGE > /dev/null 2>&1;
    then
        UBUNTU_CODENAME=`lsb_release --codename | cut -f2`
        wget -O - https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > /tmp/microsoft.gpg
        sudo mv /tmp/microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
        SOURCE_URL="https://packages.microsoft.com/repos/microsoft-ubuntu-$UBUNTU_CODENAME-prod"
        APT_SOURCE="deb [arch=amd64] $SOURCE_URL $UBUNTU_CODENAME main"
        echo $APT_SOURCE | sudo tee /etc/apt/sources.list.d/dotnetdev.list
        sudo apt install apt-transport-https
        sudo apt update
        sudo apt install $DOTNET_PACKAGE
    fi

    # Install eBay Pulse
    sudo mkdir -p $EBAYPULSE_DIR/bin
    sudo cp -rf bin $EBAYPULSE_DIR
    sudo cp -f installer.sh $EBAYPULSE_DIR

    # Install service
    sudo cp -f $EBAYPULSE_DIR/bin/$SERVICE $SERVICE_PATH
    sudo useradd -r -s /bin/false $USERNAME
    
    # Run service
    sudo systemctl enable $SERVICE
    sudo systemctl start $SERVICE

    echo "$PROGRAM_NAME has been installed"
}

function update {
    sudo true

    remove
    install

    echo "$PROGRAM_NAME has been updated"
}

function check {
    if dpkg -s $DOTNET_PACKAGE > /dev/null 2>&1;
    then
        echo "\"$DOTNET_PACKAGE\" is installed"
    else
        echo "\"$DOTNET_PACKAGE\" is not installed"
    fi

    if [[ -d $EBAYPULSE_DIR ]]
    then
        echo "Program directory ($EBAYPULSE_DIR) exists"
    else
        echo "Program directory ($EBAYPULSE_DIR) does not exist"
    fi

    if [[ -f $SERVICE_PATH ]]
    then
        echo "Service is installed"
    else
        echo "Service is not installed"
    fi

    if id $USERNAME > /dev/null 2>&1
    then
        echo "\"$USERNAME\" user exists"
    else
        echo "\"$USERNAME\" user does not exist"
    fi
}

if [[ $# != 1 ]]
then
    echo "Error: Bad option"
    help
    exit 1
fi

if [[ $1 = "--help" ]]
then
    help
    exit 0
fi

if [[ $1 = "--install" ]]
then
    install
    exit 0
fi

if [[ $1 = "--remove" ]]
then
    remove
    exit 0
fi

if [[ $1 = "--update" ]]
then
    update
    exit 0
fi

if [[ $1 = "--check" ]]
then
    check
    exit 0
fi

echo "Error: bad option"
help
exit 1
