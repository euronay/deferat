# Deferat

A simple blog engine for displaying markdown formatted posts written in .NET Core.

![](https://github.com/euronay/deferat/workflows/Build/badge.svg)

### Usage

Simply run:

```
dotnet run -p src
```

Put your posts into the `Posts` directory, author details in `Authors` and then update the `index.md` page in `Settings`. Alternatively you can set the `POSTS`, `AUTHORS` and `SETTINGS` environment variables before running the app to use you own locations.

To read how to format posts, read the [example post](/Posts/example-post/index.md)


