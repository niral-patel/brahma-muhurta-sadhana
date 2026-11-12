#!/usr/bin/env bash
# Builds windows/dist/BrahmaMuhurtaSadhana.exe: one file that is both the Windows app and its installer.
# Needs only what Windows already has (the .NET Framework C# compiler) plus Git Bash.
# The WebView2 libraries in windows/lib come from the Microsoft.Web.WebView2 NuGet package; this script
# downloads them if the folder is missing.
set -e
cd "$(dirname "$0")"
CSC="/c/Windows/Microsoft.NET/Framework64/v4.0.30319/csc.exe"
WV2_VERSION="1.0.4314.35"

if [ ! -f lib/Microsoft.Web.WebView2.Core.dll ]; then
  mkdir -p lib .pkg
  curl -sL -o .pkg/wv2.zip "https://api.nuget.org/v3-flatcontainer/microsoft.web.webview2/$WV2_VERSION/microsoft.web.webview2.$WV2_VERSION.nupkg"
  unzip -o -q .pkg/wv2.zip -d .pkg
  cp .pkg/lib/net462/Microsoft.Web.WebView2.Core.dll .pkg/lib/net462/Microsoft.Web.WebView2.WinForms.dll .pkg/runtimes/win-x64/native/WebView2Loader.dll lib/
  cp .pkg/LICENSE.txt lib/WebView2-LICENSE.txt
  rm -rf .pkg
fi

# the web app that gets embedded: everything the page loads, but not the service worker
# (inside the exe the files are already local, and a cache would only serve stale copies after an update)
RES=()
for f in index.html astronomy.js panchang-data.js cities.json manifest.webmanifest icon-192.png icon-512.png apple-touch-icon.png; do
  RES+=("-resource:..\\$f,web/$f")
done

mkdir -p dist
MSYS2_ARG_CONV_EXCL='*' "$CSC" -nologo -target:winexe -platform:x64 -optimize+ -codepage:65001 \
  -out:dist\\BrahmaMuhurtaSadhana.exe -win32icon:app.ico -win32manifest:app.manifest \
  -r:System.dll -r:System.Core.dll -r:System.Drawing.dll -r:System.Windows.Forms.dll \
  -r:lib\\Microsoft.Web.WebView2.Core.dll -r:lib\\Microsoft.Web.WebView2.WinForms.dll \
  -resource:lib\\Microsoft.Web.WebView2.Core.dll,Microsoft.Web.WebView2.Core.dll \
  -resource:lib\\Microsoft.Web.WebView2.WinForms.dll,Microsoft.Web.WebView2.WinForms.dll \
  -resource:lib\\WebView2Loader.dll,native/WebView2Loader.dll \
  "${RES[@]}" Program.cs
ls -la dist/BrahmaMuhurtaSadhana.exe | awk '{print "built dist/BrahmaMuhurtaSadhana.exe,", $5, "bytes"}'
