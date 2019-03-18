---
title: Jenkins
description: Setting up Jenkins behind reverse proxy
author: James
date: 2019-03-14
categories: 
    - devops
    - linux
    - ubuntu
    - jenkins
    - nginx
image: alev-takil-1229205-unsplash.jpg
imageCredit: <a style="background-color:black;color:white;text-decoration:none;padding:4px 6px;font-family:-apple-system, BlinkMacSystemFont, &quot;San Francisco&quot;, &quot;Helvetica Neue&quot;, Helvetica, Ubuntu, Roboto, Noto, &quot;Segoe UI&quot;, Arial, sans-serif;font-size:12px;font-weight:bold;line-height:1.2;display:inline-block;border-radius:3px" href="https://unsplash.com/@alevtakil?utm_medium=referral&amp;utm_campaign=photographer-credit&amp;utm_content=creditBadge" target="_blank" rel="noopener noreferrer" title="Download free do whatever you want high-resolution photos from Alev Takil"><span style="display:inline-block;padding:2px 3px"><svg xmlns="http://www.w3.org/2000/svg" style="height:12px;width:auto;position:relative;vertical-align:middle;top:-2px;fill:white" viewBox="0 0 32 32"><title>unsplash-logo</title><path d="M10 9V0h12v9H10zm12 5h10v18H0V14h10v9h12v-9z"></path></svg></span><span style="display:inline-block;padding:2px 3px">Alev Takil</span></a>
featured: true
draft: true
---

I recently set this up blah blah

## NGINX Config

```
location /jenkins/ {
        proxy_pass         http://localhost:8080/jenkins/;
        proxy_set_header Host $http_host;
        proxy_set_header   X-Real-IP         $remote_addr;
        proxy_set_header   X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        proxy_redirect default;
        proxy_http_version 1.1;
        proxy_request_buffering off;
    }
```

## Set jenkins subdirectory

In `/etc/default/jenkins` change the following line to add `--prefix=/jenkins`

```
JENKINS_ARGS="--webroot=/var/cache/$NAME/war --httpPort=$HTTP_PORT --prefix=/jenkins"
```

Then run 
```bash
$ sudo service jenkins restart
```

## Granting users access to the `/var/www` directory

We need to grant our `jenkins` user access to the `/var/www` folder in order to be able to publish there. So to do so, we make `www-root` the owner fo that directory and then add our `jenkins` user to that group:

```bash
sudo chown -R www-data:www-data /var/www
sudo chmod -R g+rwX /var/www
sudo adduser jenkins www-data
```

## Allowing `jenkins` to restart our service

We also need to allow jenkins to restart the service we made in the last step. So we allow it specifically to run only the command we need.

First we add the `jenkins` user to a new group (we'll call it `appadmin`):

```bash
$ sudo groupadd appadmin
$ sudo usermod -a -G appadmin jenkins
```

The we restrict what the `appadmin` users can do by modifying the `sudoers` file
```bash
$ sudo nano /etc/sudoers
```

```
Cmnd_Alias MYAPP_CMNDS = /bin/systemctl start myapp, /bin/systemctl stop myapp
%appadmin ALL=(ALL) NOPASSWD: MYAPP_CMNDS
```
or when `jenkins` is in sudo group???
```
jenkins ALL= NOPASSWD: /usr/sbin/service deferat
```

# Set up the build on Jenkins

We set up a job with the folowing commands:

```bash
dotnet publish src -o /var/www/<app>
```

```bash
sudo service <app> restart
```

So we need to give the jenkins user access to our app folder with
```
sudo chmod -R 777 /var/www/<app>
```