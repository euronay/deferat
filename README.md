## 🚀 Deferat
A simple blog engine for displaying markdown formatted posts written in .NET Core.
![](https://github.com/euronay/deferat/workflows/Build/badge.svg)

## 📘 Usage
Simply run:

```
dotnet run -p src
```

Put your posts into the `Posts` directory, author details in `Authors` and then update the `index.md` page in `Settings`. Alternatively you can set the `POSTS`, `AUTHORS` and `SETTINGS` environment variables before running the app to use you own locations.

To read how to format posts, read the [example post](/Posts/example-post/index.md)
## 🚩 Example Post
| Title        | Description                                    | Author  | Date       | Categories     | Image                                  | ImageCredit                               | Featured | Draft |
|--------------|------------------------------------------------|---------|------------|----------------|----------------------------------------|-------------------------------------------|----------|-------|
| Example Post | An example post showing you how to use Deferat | Deferat | 2019-01-01 | tests/examples | pineapple-supply-co-93822-unsplash.jpg | Photo by Pineapple Supply Co. on Unsplash | true     | false |

## 📪 It's an example post!

To use Deferat, simply write your files in markdown with the following header information. Put the `.md` file in a directory under `Posts` - the name of the directory will become the ID of the post. Place any supporting files in the directory and they will be included.

```
---
title: Example Post
description: An example post showing you how to use Deferat
author: James
date: 2019-01-01
categories: 
    - tests
    - examples
image: image.jpg
imageCredit: Image Credit
featured: true
draft: false
---

Write your post content here!
```

The following attributes are used to configure each post:

| Key | Description |
| --- | --- |
| `title` | Post title |
| `description` | Optional post description (used for metadata) |
| `author` | The id of the author (this will be linked to an [author file](Authors)) |
| `categories` | All the categories your post will appear in |
| `image` | Post header image |
| `imageCredit` | Credit for the post header image |
| `featured` | This post will appear on the home page |
| `draft` | Set this to true for your post not to appear in any lists or searches. You can preview it by nagivating directly to the post url at `https://<your server>/Posts/Read?id=<post id>` (The post id is the directory)|


### 🚩 It's markdown! ###

Your post will be formatted beautifully. This is a [link](#)</a> and some *italic* and some **bold**

This is a list
- Aliquam tincidunt mauris eu risus.
- Vestibulum auctor dapibus neque.
- Nunc dignissim risus id metus.

## 🗣️ You have headings ##

You can include code snippits:

```c#
Console.Out.WriteLine("Hello, World");
```

## 🖼️ Images:

![](pineapple-supply-co-93822-unsplash.jpg)

You can add bootstrap classes to your text{.lead}

Just add the class name after the text. The text above was followed by `{.lead}`

Emoji are replaced with lovely images: 🍒 😁


Enjoy and have fun!{.alert} {.alert-primary}  
