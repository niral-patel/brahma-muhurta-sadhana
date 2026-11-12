# Brahma Muhurta Sadhana

A guided early-morning routine (around 3:30 AM to 4:30 AM) that walks you step by step through
exercise, pranayama, meditation, Rama nama japa, puja and reading. It has how-to instructions and
shlokas for every step, a wake-up alarm, a Panchang with muhurat and festivals for your location,
a streak calendar kept year by year, and a book tracker.

**Open the app:** https://niral-patel.github.io/brahma-muhurta-sadhana/

It is a small web app with no account and no audio files. It runs the same on Windows, Mac and iPhone,
and works offline after the first visit. Labels are in English and Gujarati.

This is an extra self-help project, separate from my ML/AI portfolio.

## Install it as an app

| Device | How |
|---|---|
| Windows | Download **BrahmaMuhurtaSadhana.exe** from the [latest release](https://github.com/niral-patel/brahma-muhurta-sadhana/releases/latest), double-click it and choose **Yes** to install. See [Windows app](#windows-app). |
| iPhone | Open the link in **Safari** → **Share** → **Add to Home Screen**. It opens full screen from its icon. |
| Mac (Chrome or Edge) | Open the link → **Install** icon in the address bar, or menu → **Save and share** → **Install**. |
| Mac (Safari) | Open the link → **File** → **Add to Dock**. |

## Windows app

`BrahmaMuhurtaSadhana.exe` is one 2.5 MB file that is both the app and its installer.

- **Install:** double-click it and choose **Yes**. It copies itself to `%LocalAppData%\Programs\BrahmaMuhurtaSadhana`,
  adds Start menu and Desktop shortcuts, and opens. Choose **No** to run it once without installing.
- **Uninstall:** Windows Settings → Apps → Installed apps → Brahma Muhurta Sadhana → Uninstall. It asks whether to keep
  your saved routine, history and books.
- **Needs:** Windows 10 or 11 (64-bit). It uses the .NET Framework and the Edge WebView2 Runtime that Windows 11 already has.
- **Works offline.** The whole app is inside the exe; only the fonts come from the internet.
- **Data** is kept in `%LocalAppData%\BrahmaMuhurtaSadhana`, separate from the browser version. Use Progress → Backup to move it.
- **First run on another PC:** the file is not code-signed, so Windows SmartScreen may say "Windows protected your PC".
  Choose **More info → Run anyway**.
- **Build it yourself:** run `windows/build.sh` in Git Bash. It uses the C# compiler that ships with Windows
  (`csc.exe`, .NET Framework 4.x); no SDK is needed. Source is `windows/Program.cs`.

## The five tabs

| Tab | What it shows |
|---|---|
| **Today** | The current step with a countdown, how to do it, its shlokas, today's Panchang summary and the alarm |
| **Routine** | Every step: switch it on or off, set minutes (or rounds / malas), reorder, jump to it |
| **Panchang** | Location, month calendar with tithi and festivals, this month's festival list, tithi, nakshatra, yoga, karana, sunrise, sunset, muhurat, choghadiya and coming festivals |
| **Guide** | Suggestions, how to do each step with shlokas (Gujarati or Devanagari script), weekly meditation plan |
| **Progress** | Streak, year calendar, yearly totals, book list and tracker, backup |

## The default routine (60 minutes)

| Time | Step | Guidance on screen |
|---|---|---|
| 3:30 | Wake up and freshen (3 min) | Waking shlokas |
| 3:33 | Walk (3 min) | Countdown |
| 3:36 | Surya Namaskar (4 rounds) | 12 postures; one name of Surya per round; tap **Round done** |
| 3:42 | Hanuman dand (2 min) | 40 s work / 20 s rest |
| 3:44 | Push-ups (2 min) | 40 s work / 20 s rest |
| 3:46 | Plank (2 min) | 45 s hold / 15 s rest |
| 3:48 | Cold shower (4 min) | Bathing shloka |
| 3:52 | Kapalbhati (4 min) | Stroke counter, one per second, 45 s on / 15 s rest |
| 3:56 | Anulom Vilom (4 min) | Left/right cues; the circle grows on inhale (4) and shrinks on exhale (8) |
| 4:00 | Bhramari (2 min) | Inhale 4, hum 8 |
| 4:02 | Ujjayi (2 min) | Inhale 5, exhale 5 |
| 4:04 | Meditation (8 min) | Opening shlokas, a drone in the pitch of the day, bell every 5 minutes |
| 4:12 | Rama nama japa (1 mala) | Tap the circle for each name; counts to 108 |
| 4:18 | Puja (6 min) | Ganesha, lamp and closing shlokas |
| 4:24 | Book reading (6 min) | Prayer before reading; save the page you reached |

Bhastrika, Sheetali and Shavasana are included but switched off. **Length** on the Today tab switches
between the full routine and fixed 30 minute and 20 minute versions for short days.

## Sound

- The **wake-up alarm**.
- **Meditation**: opening bell, drone, a bell every 5 minutes, closing bell.
- One **soft bell when a timed step ends on its own** (can be switched off). A bell also marks each completed mala.
- **Back** and **Next step** are always silent. There is no voice.

## Panchang, muhurat and festivals

Calculated on the device with [astronomy-engine](https://github.com/cosinekitty/astronomy) and the same
method as my Morning Briefing app: Lahiri ayanamsa, amanta months, day counted from sunrise to sunrise.

- **Location**: use the device's live location, or choose country → state → city (about 38,000 places,
  from [GeoNames](https://www.geonames.org/), CC BY 4.0). The default is Dallas, TX.
- Shows month and Vikram Samvat, paksha, tithi, nakshatra, yoga, karana, sunrise, sunset, Brahma and
  Abhijit muhurta, Rahu kaal, Yamaganda, Gulika kaal, and day and night choghadiya.
- **Festivals and fasting days** (Ekadashi, Pradosh, Purnima, Amavasya, Sankashti and the main festivals)
  for the selected day and the next 45 days, each with a short explanation of what it is and how it is observed.
- **Ekadashi**: every Ekadashi has its own name (Kamada, Nirjala, Dev Uthi and so on) and the benefit
  traditionally linked to it. The Today tab always shows the next Ekadashi fast, how many days are left,
  and when to break the fast the next morning.
- **US holidays** (Thanksgiving, Memorial Day, the Fourth of July, daylight saving changes and others) are
  shown when the location is in the United States. Dates follow the Gujarati calendar and the local sunrise, so
  outside India a few can fall a day apart from the date in India.
- Times can differ by a minute or two from printed panchangs.

## Progress, books and backup

- A day is marked done when the routine is finished after at least 5 minutes of practice; any past day
  can be marked or unmarked by hand.
- Every year is kept. Yearly totals: days done, best streak, hours, malas of japa, pages read, books finished.
- **Books**: add and remove books, choose the one you are reading, save the page reached; pages are added
  to the day's total.
- Everything is stored in the browser on that device only. **Backup** gives a text you can paste into the
  app on another device.

## The alarm, and its limit

A web page can only ring while it is open with the screen on. So:

- On a laptop or desktop: set the alarm in the app, leave the page open and the machine plugged in.
- On iPhone: use the Clock app as the real wake-up alarm, then open this app and press **Start**.

## Care

Skip Kapalbhati and Bhastrika if you are pregnant or have high blood pressure, heart disease, a hernia
or recent abdominal surgery. Ease into cold showers if you have a heart condition. Stop any practice
that makes you dizzy and breathe normally. This app is a timer and guide, not medical advice.

The shlokas are the commonly recited versions; wording varies a little between traditions.

## Files

| File | Purpose |
|---|---|
| `index.html` | The whole app: layout, styles and logic |
| `astronomy.js` | astronomy-engine 2.1.19 (MIT), sun and moon positions |
| `panchang-data.js` | Gujarati and English names, festival rules (from my Morning Briefing app) |
| `cities.json` | Country → state → city list with coordinates and time zones (GeoNames) |
| `sw.js` | Service worker that caches the app for offline use |
| `manifest.webmanifest`, `icon-*.png`, `apple-touch-icon.png` | Install-as-app details and icons |
