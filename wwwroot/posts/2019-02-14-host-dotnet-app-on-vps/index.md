---
title: Hosting a .NET Core app on an Ubuntu server
author: James
date: 2019-02-14
categories: 
    - dotnet
    - devops
    - vps
    - linux
image: jason-blackeye-131049-unsplash.jpg
featured: true
---

Small Linux VMs are super cheap option to host .NET Core webapps. I am actually hosting this blog on a VM from [INIZ](https://iniz.com/)

I've opted for Ubuntu 16.04 as my OS image, so let's run through how to set up a .NET Core web app from scratch

## Add a new user and disable root Access

```bash
adduser james
usermod -aG sudo james
```
Install nano and then edit **sshd_config**

```bash
sudo nano /etc/ssh/ssdh_config
```
Change **PermitRootLogin** to no and add an **AllowUsers** entry
```text
PermitRootLogin no
AllowUsers james
```
Restart ssh service
```bash
sudo systemctl restart sshd
```


## Install .NET SDK

Following the linux install instructions from the [.NET website](https://dotnet.microsoft.com/download), install the .NET Core SDK (you can just install the runtime if you are deploying published apps - but I prefer to be able to build on the server if I need)

In my case, I had to run the following. Firstly to install the Microsoft package sources:

```bash
wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```

And then install the .NET SDK

```bash
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-2.2
```

Make sure it's installed by running
```bash
dotnet --version
```


## TODO

- Cloning repo and setting up as a remote
- Open web app to outside world
- [Host on Linux with Nginx](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-2.2)
- Setup Domain
- LetsEncrypt

<i class="fab fa-docker" /> Docker 

