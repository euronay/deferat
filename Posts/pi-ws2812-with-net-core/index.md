---
title: Controlling WS2812 strip with .NET Core on Raspberry Pi
description: Controlling WS2812 neopixel strip with .Net Core GPIO
author: James
date: 2019-03-20
categories: 
    - raspberry pi
    - net core
    - rgb
    - ws2812
image: ivan-slade-761369-unsplash.jpg
imageCredit: <a style="background-color:black;color:white;text-decoration:none;padding:4px 6px;font-family:-apple-system, BlinkMacSystemFont, &quot;San Francisco&quot;, &quot;Helvetica Neue&quot;, Helvetica, Ubuntu, Roboto, Noto, &quot;Segoe UI&quot;, Arial, sans-serif;font-size:12px;font-weight:bold;line-height:1.2;display:inline-block;border-radius:3px" href="https://unsplash.com/@flowinteractive?utm_medium=referral&amp;utm_campaign=photographer-credit&amp;utm_content=creditBadge" target="_blank" rel="noopener noreferrer" title="Download free do whatever you want high-resolution photos from Ivan Slade"><span style="display:inline-block;padding:2px 3px"><svg xmlns="http://www.w3.org/2000/svg" style="height:12px;width:auto;position:relative;vertical-align:middle;top:-2px;fill:white" viewBox="0 0 32 32"><title>unsplash-logo</title><path d="M10 9V0h12v9H10zm12 5h10v18H0V14h10v9h12v-9z"></path></svg></span><span style="display:inline-block;padding:2px 3px">Ivan Slade</span></a>
featured: true
draft: true
---

Set up .net core runtime on pi 

Need binaries from [https://github.com/dotnet/core-setup]()

Choose version Linux (armhf) and download on your pi with

```shell
$ curl -o dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/release/2.2/dotnet-runtime-latest-linux-arm.tar.gz
$ sudo mkdir /opt/dotnet
$ sudo tar xzf dotnet.tar.gz -C /opt/dotnet
$ sudo ln -s /opt/dotnet/dotnet /usr/local/bin
```

You can check it's working with
```shell
$ dotnet --info
Host (useful for support):
  Version: 2.2.3
  Commit:  6b8ad509b6

.NET Core SDKs installed:
  No SDKs were found.

.NET Core runtimes installed:
  Microsoft.NETCore.App 2.2.3 [/opt/dotnet/shared/Microsoft.NETCore.App]

To install additional .NET Core runtimes or SDKs:
  https://aka.ms/dotnet-download

```


Make a new test console app on your desktop machine with `dotnet new console`. 

Program.cs
```cs
using System;

namespace hello_pi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Pi!");
        }
    }
}
```

Build it with
```shell
$ dotnet publish -r linux-arm
```

Sent it to your pi with 
```shell
$ scp -r bin/Debug/netcoreapp2.2/linux-arm/publish pi@host:~/hello-pi
```

No on your pi, open the directory and run the app!
```shell
$ cd hello-pi
$ ./hello-pi
Hello Pi!
```