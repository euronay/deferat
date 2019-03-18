---
title: Example Post
description: An example post showing you how to use Deferat
author: James
date: 2019-01-01
categories: 
    - tests
    - examples
image: belinda-fewings-771623-unsplash.jpg
imageCredit: <a style="background-color:black;color:white;text-decoration:none;padding:4px 6px;font-family:-apple-system, BlinkMacSystemFont, &quot;San Francisco&quot;, &quot;Helvetica Neue&quot;, Helvetica, Ubuntu, Roboto, Noto, &quot;Segoe UI&quot;, Arial, sans-serif;font-size:12px;font-weight:bold;line-height:1.2;display:inline-block;border-radius:3px" href="https://unsplash.com/@bel2000a?utm_medium=referral&amp;utm_campaign=photographer-credit&amp;utm_content=creditBadge" target="_blank" rel="noopener noreferrer" title="Download free do whatever you want high-resolution photos from Belinda Fewings"><span style="display:inline-block;padding:2px 3px"><svg xmlns="http://www.w3.org/2000/svg" style="height:12px;width:auto;position:relative;vertical-align:middle;top:-2px;fill:white" viewBox="0 0 32 32"><title>unsplash-logo</title><path d="M10 9V0h12v9H10zm12 5h10v18H0V14h10v9h12v-9z"></path></svg></span><span style="display:inline-block;padding:2px 3px">Belinda Fewings</span></a>
featured: true
---

#### It's an example post!

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
draft : false
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


### It's markdown! ###

Your post will be formatted beautifully. This is a [link](#)</a> and some *italic* and some **bold**

This is a list
- Aliquam tincidunt mauris eu risus.
- Vestibulum auctor dapibus neque.
- Nunc dignissim risus id metus.

## You have headings ##

You can include code snippits:

```c#
Console.Out.WriteLine("Hello, World");
```

Images:

![](pineapple-supply-co-93822-unsplash.jpg)

You can add bootstrap classes to your text{.lead}

Just add the class name after the text. The text above was followed by `{.lead}`

Emoji are replaced with lovely images: 🍒 😁


Enjoy and have fun!{.alert} {.alert-primary}  