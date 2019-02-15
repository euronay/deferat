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

I've opted for Ubuntu 16.04 as my OS image, so let's run through how to deploy .NET Core web app onto a brand new VVM

## Add a new user and disable root access

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
Restart `ssh` service
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


## Publish app to the Server

We are going to primarily be using the instructions from Microsoft Docs to [Host ASP.NET Core on Linux](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-2.2) to deploy our app. 

First we need to copy our application to the server - you could just copy it to the server using FTP or SCP - but there is an excellent tool available called [dotnet-publish-ssh](https://github.com/albekov/dotnet-publish-ssh)

To use it you simply add a tool reference to your project's `csproj` file:
```xml
<ItemGroup>
    <DotNetCliToolReference Include="DotnetPublishSsh" Version="0.1.0" />
</ItemGroup>
```

From your build machine, run
```bash
dotnet restore
dotnet publish-ssh --ssh-host <address of your server> --ssh-user <username> --ssh-password <your password> --ssh-path <path to your site>
```


Later we will set up a CI pipeline to publish our app automatically{alert alert-info}



### Setup reverse proxy

A reverse proxy will forward requests made to our server to our app. Again, we will be following the directions on the Microsoft Docs site to [Configure a reverse proxy server](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-2.2#configure-a-reverse-proxy-server)


We follow the docs from NGINX ([here](https://www.nginx.com/resources/wiki/start/topics/tutorials/install/#official-debian-ubuntu-packages)) to install the Ubuntu release.

First we need to add the repository to our `sources.list` file. We will use nano
```bash
sudo nano /etc/apt/sources.list
``` 
And add the following lines to the bottom of the file
```text
deb http://nginx.org/packages/ubuntu/ xenial nginx
deb-src http://nginx.org/packages/ubuntu/ xenial nginx
```

Then simply run
```bash
sudo apt-get update
sudo apt-get install nginx
```

You can check that NGINX is running starting it with:
```bash
sudo service nginx start
```

If you navigate to your server's url, you should now see the default welcome page:

![](nginx-started.png)


In my case, my server already had apache2 installed and configured - I had to disable this with: 

```bash
sudo service stop apache2
```



Next we need to configure NGINX to forward request to our app. You need to modify the `/etc/nginx/sites-available/default` file and replace the contents with:

```json
server {
    listen        80;
    server_name   <your domain>;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}
```
Your will then need to link this to `etc/nginx/sites-enabled/default` with

```bash
sudo sn -l /etc/nginx/sites-available/default /etc/nginx/sites-enabled/
```

Also ensure that your default site is included in the config file for NGINX at `/etc/nginx/nginx.conf` - there should be a line like:
```text
include /etc/nginx/sites-available/default;
```

Once you have restarted the NGINX service, start your app with
```bash
dotnet <myapp>.dll
```

If you then visit your VM in the browser, you should see your wepapp running:

![](working-website.png)

## Running .NET app as a service

The final task is to set up our webapp to run as a service, so you don't have to be logged in to your server


## Further improvments

Congratulations 🎉! You have you app up and running on your VM. However, we can still go much further. Coming up next:

- Securing your site with an SSL certificate
- Setting up a CI pipeline


